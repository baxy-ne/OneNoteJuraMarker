using Microsoft.Office.Interop.OneNote;
using OneNoteJuraMarker.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace OneNoteJuraMarker.OneNoteLogic;

public class OneNoteProgram : IOneNoteProgram
{
    private readonly XNamespace _ns = "http://schemas.microsoft.com/office/onenote/2013/onenote";
    private readonly List<string> _prefixes = new() { "§§", "§", "$", "Art." };
    private readonly List<string> _abbreviations = new() { "Abs.", "S.", "HS", "Nr.", "Var.", "Alt.", "  ", "lit.", "Gr.", "1.", "2.", "3." };
    private readonly List<string> _legalCodes = new() { "BGB", "GG", "VwVfG", "VwGO" };
    private readonly List<string> _romanNumerals = new() { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
    public void ProcessOneNotePages()
    {
        var onenoteApp = new Application();
        try
        {
            var notebookDoc = GetNotebookHierarchy(onenoteApp);

            foreach (var page in notebookDoc.Descendants(_ns + "Page"))
            {
                var pageId = page.Attribute("ID")?.Value;
                if (string.IsNullOrEmpty(pageId)) continue;

                ProcessPage(onenoteApp, pageId);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            Marshal.ReleaseComObject(onenoteApp);
        }
    }

    public XDocument GetNotebookHierarchy(Application app)
    {
        app.GetHierarchy("", HierarchyScope.hsPages, out string xml);
        return XDocument.Parse(xml);
    }

    public void ProcessPage(Application app, string pageId)
    {
        app.GetPageContent(pageId, out string content);
        var pageDoc = XDocument.Parse(content);
        bool modified = false;

        foreach (var textElement in pageDoc.Descendants(_ns + "T"))
        {
            string original = textElement.Value;
            if (original.Contains("<span style=\"color:#00B0F0\">")) continue;

            string plainText = StripHtml(original);
            string formatted = FormatLegalReferences(plainText);

            if (formatted != original)
            {
                textElement.ReplaceNodes(new XCData(formatted));
                modified = true;
            }
        }

        if (modified)
            UpdatePage(app, pageDoc);
    }

    public string StripHtml(string input) =>
        Regex.Replace(input, "<.*?>", string.Empty);

    public string FormatLegalReferences(string text)
    {
        var tokens = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var result = new List<string>();
        int i = 0;

        while (i < tokens.Length)
        {
            string token = tokens[i];
            if (_prefixes.Contains(token))
            {
                string block = token;
                i++;
                while (i < tokens.Length && IsLegalElement(tokens[i]))
                    block += " " + tokens[i++];
                result.Add($"<span style=\"color:#00B0F0\"><b>{token}</b> {block[token.Length..].Trim()}</span>");
            }
            else
            {
                result.Add(token);
                i++;
            }
        }

        return string.Join(" ", result);
    }

    public bool IsLegalElement(string token)
    {
        return int.TryParse(token, out _) ||
               _romanNumerals.Contains(token.ToUpper()) ||
               (_abbreviations.Contains(token) || _legalCodes.Contains(token)) ||
               (token.Length == 2 && char.IsLetter(token[0]) && token[1] == ')') ||
               token is "-" or "." or ",";
    }

    public void UpdatePage(Application app, XDocument doc)
    {
        doc.Declaration = null;
        using var sw = new StringWriter();
        doc.Save(sw, SaveOptions.DisableFormatting);
        string updatedXml = sw.ToString();

        File.WriteAllText("debug.xml", updatedXml);

        app.UpdatePageContent(updatedXml);
    }
}

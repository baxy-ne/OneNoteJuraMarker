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
    [STAThread]
    public void ProcessOneNotePages()
    {
        var onenoteApp = new Microsoft.Office.Interop.OneNote.Application();
        try
        {
            XNamespace ns = "http://schemas.microsoft.com/office/onenote/2013/onenote";
            onenoteApp.GetHierarchy("", HierarchyScope.hsPages, out string notebookXml);
            XDocument notebookDoc = XDocument.Parse(notebookXml);

            foreach (var page in notebookDoc.Descendants(ns + "Page"))
            {
                string pageId = page.Attribute("ID")?.Value;
                if (string.IsNullOrEmpty(pageId))
                    continue;

                onenoteApp.GetPageContent(pageId, out string pageContent);
                XDocument pageDoc = XDocument.Parse(pageContent);
                bool modified = false;

                foreach (var textElement in pageDoc.Descendants(ns + "T"))
                {
                    // Skip elements that already have our legal formatting
                    if (textElement.Value.Contains("<span style=\"color:#00B0F0\">"))
                    {
                        continue;
                    }

                    // Remove existing non-legal HTML tags
                    string plainText = RemoveHtmlTags(textElement.Value);
                    // Apply legal formatting
                    string modifiedText = ApplyLegalFormatting(plainText);

                    if (modifiedText != textElement.Value)
                    {
                        textElement.ReplaceNodes(new XCData(modifiedText));
                        modified = true;
                    }
                }

                if (modified)
                {
                    SaveUpdatedPage(onenoteApp, pageDoc);
                }
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

    string RemoveHtmlTags(string input)
    {
        // This regex removes all tags (<...>)
        return Regex.Replace(input, "<.*?>", string.Empty);
    }

    string ApplyLegalFormatting(string text)
    {
        // Define the fixed prefixes.
        var prefixes = new List<string> { "§§", "§", "$", "Art." };

        // Define sets of legal elements for simple checks.
        var abbreviations = new List<string> { "Abs.", "S.", "HS", "Nr.", "Var.", "Alt.", "  ", "lit.", "Gr.", "1.", "2.", "3." };
        var legalCodes = new List<string> { "BGB", "GG", "VwVfG", "VwGO" };
        var romanNumerals = new List<string> { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

        // Split the text into tokens by whitespace.
        string[] tokens = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var resultTokens = new List<string>();

        int i = 0;
        while (i < tokens.Length)
        {
            string token = tokens[i];

            // If the token is a fixed prefix, start building a legal reference block.
            if (prefixes.Contains(token))
            {
                // Begin the block with the prefix.
                string legalBlock = token;
                i++; // Move past the prefix.

                // Gather all subsequent tokens that qualify as legal elements.
                while (i < tokens.Length && IsLegalElement(tokens[i], abbreviations, romanNumerals, legalCodes))
                {
                    legalBlock += " " + tokens[i];
                    i++;
                }
                // Format the entire block:
                // The prefix is made bold and the whole block is wrapped in a blue-colored span.
                // We assume the prefix is the first token in the block.>
                string formattedBlock = $"<span style=\"color:#00B0F0\"><b>{token}</b> {legalBlock.Substring(token.Length).Trim()}</span>";
                resultTokens.Add(formattedBlock);
            }
            else
            {
                // If the token is not part of a legal reference, add it as-is.
                resultTokens.Add(token);
                i++;
            }
        }

        // Rebuild the string from the tokens.
        return string.Join(" ", resultTokens);
    }

    /// <summary>
    /// Checks if a token qualifies as a legal element.
    /// </summary>
    private bool IsLegalElement(string token, List<string> abbreviations, List<string> romanNumerals, List<string> legalCodes)
    {
        // Check if the token is numeric.
        if (int.TryParse(token, out _))
            return true;

        // Check if the token is one of the defined Roman numerals.
        if (romanNumerals.Contains(token.ToUpper()))
            return true;

        // Check if the token is a letter followed by a closing parenthesis, e.g., "a)"
        if (token.Length == 2 && char.IsLetter(token[0]) && token[1] == ')')
            return true;

        // Check if the token is one of the known abbreviations.
        if (abbreviations.Contains(token))
            return true;

        // Check if the token is one of the legal codes.
        if (legalCodes.Contains(token))
            return true;

        // Check if the token is one of the special characters.
        if (token == "-" || token == "." || token == ",")
            return true;

        // Otherwise, not considered a legal element.
        return false;
    }


    private void SaveUpdatedPage(Microsoft.Office.Interop.OneNote.Application onenoteApp, XDocument pageDoc)
    {
        // Remove XML declaration before updating
        pageDoc.Declaration = null;
        using (StringWriter sw = new StringWriter())
        {
            pageDoc.Save(sw, SaveOptions.DisableFormatting);
            string validatedXml = sw.ToString();
            // Optionally write to debug file
            File.WriteAllText("debug.xml", validatedXml);
            onenoteApp.UpdatePageContent(validatedXml);
        }
    }
}

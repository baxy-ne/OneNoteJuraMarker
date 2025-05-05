using Microsoft.Office.Interop.OneNote;
using System.Xml.Linq;

namespace OneNoteJuraMarker.Interfaces;

public interface IOneNoteProgram
{
    string FormatLegalReferences(string text);
    XDocument GetNotebookHierarchy(Application app);
    bool IsLegalElement(string token);
    void ProcessOneNotePages();
    void ProcessPage(Application app, string pageId);
    string StripHtml(string input);
    void UpdatePage(Application app, XDocument doc);
}
using Microsoft.Office.Interop.OneNote;
using OneNoteJuraMarker.Interfaces;
using OneNoteJuraMarker.Models;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OneNoteJuraMarker.OneNoteLogic;


public class OneNoteParser : IOneNoteParser
{
    public List<NotebookModel> LoadNotebooksFromXml()
    {
        var onenoteApp = new Application();
        XNamespace one = "http://schemas.microsoft.com/office/onenote/2013/onenote";
        onenoteApp.GetHierarchy("", HierarchyScope.hsPages, out string notebookXml);

        var doc = XDocument.Parse(notebookXml);
        var notebooks = new List<NotebookModel>();

        foreach (var nb in doc.Descendants(one + "Notebook"))
        {
            var notebook = new NotebookModel
            {
                Name = (string)nb.Attribute("name"),
                Path = (string)nb.Attribute("path"),
                ID = (string)nb.Attribute("ID")
            };

            foreach (var section in nb.Descendants(one + "Section"))
            {
                var sectionModel = new SectionModel
                {
                    Name = (string)section.Attribute("name"),
                    Path = (string)section.Attribute("path")
                };

                foreach (var page in section.Descendants(one + "Page"))
                {
                    var pageId = (string)page.Attribute("ID");
                    string pageContentXml = "";

                    onenoteApp.GetPageContent(pageId, out pageContentXml, PageInfo.piAll, XMLSchema.xs2013);
                    var pageModel = new PageModel
                    {
                        Name = (string)page.Attribute("name"),
                        DateTime = (string)page.Attribute("dateTime"),
                        PageXML = pageContentXml
                    };

                    sectionModel.Pages.Add(pageModel);
                }

                notebook.Sections.Add(sectionModel);
            }

            notebooks.Add(notebook);
        }

        return notebooks;
    }

}

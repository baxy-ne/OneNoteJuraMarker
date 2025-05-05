using OneNoteJuraMarker.Models;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OneNoteJuraMarker.OneNoteLogic;


public class OneNoteParser
{
    public static List<NotebookModel> LoadNotebooksFromXml(string xmlPath)
    {
        XNamespace one = "http://schemas.microsoft.com/office/onenote/2013/onenote";

        var doc = XDocument.Load(xmlPath);
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
                    var pageModel = new PageModel
                    {
                        Name = (string)page.Attribute("name"),
                        DateTime = (string)page.Attribute("dateTime")
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

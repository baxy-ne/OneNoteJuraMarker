using OneNoteJuraMarker.Models;
using System.Collections.Generic;

namespace OneNoteJuraMarker.Interfaces;

public interface IOneNoteParser
{
    List<NotebookModel> LoadNotebooksFromXml();
}
using System.Collections.Generic;

namespace OneNoteJuraMarker.Models;

public class NotebookModel
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string ID { get; set; }

    public List<SectionModel> Sections { get; set; } = new();
}

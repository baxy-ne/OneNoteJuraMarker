using System.Collections.Generic;

namespace OneNoteJuraMarker.Models;

public class NotebookModel
{
    public required string Name { get; set; }
    public required string Path { get; set; }
    public required string ID { get; set; }

    public List<SectionModel> Sections { get; set; } = new();
}

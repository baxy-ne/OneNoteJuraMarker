using System.Collections.Generic;

namespace OneNoteJuraMarker.Models;

public class SectionModel
{
    public required string Name { get; set; }
    public required string Path { get; set; }

    public List<PageModel> Pages { get; set; } = new();
}

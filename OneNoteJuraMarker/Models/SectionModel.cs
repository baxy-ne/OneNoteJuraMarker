using System.Collections.Generic;

namespace OneNoteJuraMarker.Models;

public class SectionModel
{
    public string Name { get; set; }
    public string Path { get; set; }

    public List<PageModel> Pages { get; set; } = new();
}

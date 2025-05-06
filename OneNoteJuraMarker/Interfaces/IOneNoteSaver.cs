using Microsoft.UI.Xaml.Documents;

namespace OneNoteJuraMarker.Interfaces
{
    public interface IOneNoteSaver
    {
        Paragraph DoTheStuff(string xmlPageCode);
    }
}
using OneNoteJuraMarker.Interfaces;
using OneNoteJuraMarker.ViewModels;

namespace OneNoteJuraMarker.Utils;

public class YourOneNoteHelper(MainViewModel mainViewModel) : IYourOneNoteHelper
{
    public string GetPageContent(string pageName)
    {
        var code = string.Empty;
        foreach (var notebook in mainViewModel.Notebooks)
        {
            foreach (var sec in notebook.Sections)
            {
                foreach (var page in sec.Pages)
                {
                    if (page.Name == pageName)
                    {
                        code = page.PageXML;
                    }
                }
            }
        }

        return code;
    }
}

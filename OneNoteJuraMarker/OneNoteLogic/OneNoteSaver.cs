using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Text;
using System;
using System.Collections.Generic;
using Windows.UI;
using OneNoteJuraMarker.Interfaces;
using System.Diagnostics;

namespace OneNoteJuraMarker.OneNoteLogic;

public class OneNoteSaver : IOneNoteSaver
{
    private readonly List<string> _prefixes = new() { "§§", "§", "$", "Art." };
    private readonly List<string> _abbreviations = new() { "Abs.", "S.", "HS", "Nr.", "Var.", "Alt.", "lit.", "Gr.", "1.", "2.", "3." };
    private readonly List<string> _legalCodes = new() { "BGB", "GG", "VwVfG", "VwGO" };
    private readonly List<string> _romanNumerals = new() { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

    public Paragraph DoTheStuff(string xmlPageCode)
    {
        var paragraph = new Paragraph();

        var words = xmlPageCode.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;

        while (i < words.Length)
        {
            string word = words[i];

            bool alreadyFormatted = word.Contains("#00B0F0") || word.Contains("<b>") || word.Contains("color:");

            if (_prefixes.Contains(word) && !alreadyFormatted)
            {
                string block = word;
                i++;

                while (i < words.Length && IsLegalElement(words[i]))
                {
                    block += " " + words[i];
                    i++;
                }

                var run = new Run
                {
                    Text = block + " ",
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 176, 240)) // #00B0F0
                };

                paragraph.Inlines.Add(run);
            }
            else
            {
                paragraph.Inlines.Add(new Run { Text = word + " " });
                i++;
            }
        }
        Debug.WriteLine(paragraph);
        return paragraph;
    }

    private bool IsLegalElement(string token)
    {
        return int.TryParse(token, out _) ||
               _romanNumerals.Contains(token.ToUpper()) ||
               _abbreviations.Contains(token) ||
               _legalCodes.Contains(token) ||
               (token.Length == 2 && char.IsLetter(token[0]) && token[1] == ')') ||
               token is "-" or "." or ",";
    }
}

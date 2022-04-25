using System.Linq;
using System.Collections.Generic;

public enum Language
{
    None,
    Ukrainian,
    English
}

public static class LanguageDetection
{
    public static Dictionary<Language, string> none = new Dictionary<Language, string>() {
        { Language.None, "" }
    };
    public static Dictionary<Language, string> ukrainian = new Dictionary<Language, string>() { 
        { Language.Ukrainian, " àáâã´äåºæçè³¿éêëìíîïğñòóôõö÷øùüşÿ" } 
    };
    public static Dictionary<Language, string> english = new Dictionary<Language, string>() {
        { Language.English, " abcdefghijklmnopqrstuvwxyz" } 
    };

    public static Dictionary<Language, string> DetectLanguage(string message)
    {
        foreach(char symbol in message)
        {
            string letter = symbol.ToString().ToLower();

            if (letter == " ")
                continue;

            string ukrainianAlphabet = ukrainian.First().Value;
            if (ukrainianAlphabet.Contains(letter))
                return ukrainian;

            string englishAlphabet = english.First().Value;
            if (englishAlphabet.Contains(letter))
                return english;
        }

        return none;
    }

    public static int GetCharPosition(Dictionary<Language, string> lang, char symbol)
    {
        int index = 0;

        string alphabet = lang.First().Value;
        symbol = symbol.ToString().ToLower()[0];

        foreach (char _symbol in alphabet)
        {
            if (_symbol == symbol)
                return index;

            index++;
        }

        return -1;
    }
}
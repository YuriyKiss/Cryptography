using System.Linq;
using System.Collections.Generic;

public class CaesarsCipher : IEncryption
{
    public CaesarsCipher() { }

    public string Validate(string message, string[] options)
    {
        if (message == "")
        {
            return "Message should not be empty";
        }

        if (options[0] == "")
        {
            return "Please enter offset";
        }

        Dictionary<Language, string> lang = LanguageDetection.DetectLanguage(message);

        Language language = lang.First().Key;
        int languageLength = lang.First().Value.Length;

        if (language == Language.None)
        {
            return "Language not recognised";
        }

        int offset = int.Parse(options[0]);
        if (offset <= 0 || offset >= languageLength)
        {
            return $"Offset is out of bounds, set it between 1 and {languageLength - 1} for {language}";
        }

        return "";
    }

    public string Encrypt(string message, string[] options)
    {
        string response = "";

        int offset = int.Parse(options[0]);
        Dictionary<Language, string> lang = LanguageDetection.DetectLanguage(message);

        string alphabet = lang.First().Value;
        int languageLength = lang.First().Value.Length;

        foreach (char symbol in message) 
        {
            int charIndex = LanguageDetection.GetCharPosition(lang, symbol);

            if (charIndex != -1)
            {
                int newOffset = (charIndex + offset + languageLength) % languageLength;

                if (symbol == alphabet[charIndex])
                {
                    response += alphabet[newOffset];
                }
                else
                {
                    string uppercase = alphabet[newOffset].ToString().ToUpper();

                    response += uppercase;
                }
            }
            else
            {
                response += symbol;
            }
        }

        return response;
    }
    
    public string Decrypt(string message, string[] options)
    {
        options[0] = "-" + options[0];

        return Encrypt(message, options);
    }
}

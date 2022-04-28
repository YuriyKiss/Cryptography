using System;
using System.Linq;
using System.Collections.Generic;

public class TritemiusEncryption : IEncryption
{
    public TritemiusEncryption() { }

    public string Validate(string message, string[] options)
    {
        if (message.Length < 1)
        {
            return "Message should not be empty";
        }

        foreach (string option in options)
        {
            if (option.Length < 1)
            {
                return "Key is incomplete";
            }
        }

        if (options.Length == 1)
        {
            KeyValuePair<Language, string> messageLanguage = LanguageDetection.DetectLanguage(message).First();
            KeyValuePair<Language, string> keyLanguage = LanguageDetection.DetectLanguage(options[0]).First();

            if (messageLanguage.Key != keyLanguage.Key)
            {
                return "Message and key should be same language";
            }
        }

        return "";
    }
    
    public string Encrypt(string message, string[] options)
    {
        string result = "";

        KeyValuePair<Language, string> language = LanguageDetection.DetectLanguage(message).First();
        for (int i = 0; i < message.Length; ++i)
        {
            // keep symbol the same if it's special
            if (!language.Value.Contains(message[i]))
            {
                result += message[i];
            }
            else
            {
                result += language.Value[Helper.Mod(
                    Helper.GetCharIndex(language.Value, message[i]) + GetValidKey(message[i], i, options, language),
                    language.Value.Length)];
            }
        }
        return result;
    }

    public string Decrypt(string message, string[] options)
    {
        string result = "";

        KeyValuePair<Language, string> language = LanguageDetection.DetectLanguage(message).First();
        for (int i = 0; i < message.Length; ++i)
        {
            if (!language.Value.Contains(message[i]))
            {
                result += message[i];
            }
            else
            {
                result += language.Value[
                    Helper.Mod(
                    Helper.GetCharIndex(language.Value, message[i]) +
                    language.Value.Length -
                    Helper.Mod(
                        GetValidKey(message[i], i, options, language),
                        language.Value.Length
                        ),
                    language.Value.Length)
                    ]; ;
            }
        }

        return result;
    }

    public int GetValidKey(char symbol, int symbolPosition, string[] options, KeyValuePair<Language, string> language)
    {
        if (options.Length == 1)
        {
            char keySymbol = options[0][Helper.Mod(symbolPosition, options[0].Length)];

            return symbolPosition +
                    Helper.GetCharIndex(language.Value, keySymbol);

        }
        else if (options.Length == 2)
        {
            return int.Parse(options[0]) * symbolPosition + int.Parse(options[1]);
        }
        else if (options.Length == 3)
        {
            return Convert.ToInt32(int.Parse(options[0]) * Math.Pow(symbolPosition, 2) +
                int.Parse(options[1]) * symbolPosition +
                int.Parse(options[2]));
        }
        else
        {
            throw new Exception("Invalid key, cannot convert to vector or motto");
        }

    }
}

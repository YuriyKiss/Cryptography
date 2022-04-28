using System.Linq;
using System.Collections.Generic;

public class VigenereEncryption : IEncryption
{
    public VigenereEncryption() { }

    public string Validate(string message, string[] options)
    {
        if (message.Length < 1)
        {
            return "Message should not be empty";
        }

        if (options[0].Length < 1)
        {
            return "Key should not be empty";
        }

        KeyValuePair<Language, string> messageLanguage = LanguageDetection.DetectLanguage(message).First();
        KeyValuePair<Language, string> keyLanguage = LanguageDetection.DetectLanguage(options[0]).First();

        if (messageLanguage.Key != keyLanguage.Key)
        {
            return "Message and key should be same language";
        }

        return "";
    }

    public string Encrypt(string message, string[] options)
    {
        return Vigenere(message, options[0], true);
    }

    public string Decrypt(string message, string[] options)
    {
        return Vigenere(message, options[0], false);
    }

    public string Vigenere(string message, string key, bool flag)
    {
        string result = "";

        KeyValuePair<Language, string> language = LanguageDetection.DetectLanguage(message).First();
        for (int i = 0; i < message.Length; ++i)
        {
            int messageIndex = Helper.Mod(i, message.Length);
            int keyIndex = Helper.Mod(i, key.Length);

            int textCharIndex = Helper.GetCharIndex(language.Value, message[messageIndex]);
            int keywordCharIndex = Helper.GetCharIndex(language.Value, key[keyIndex]);

            if (textCharIndex < 0)
            {
                result += message[messageIndex];
            }
            else
            {
                int index = textCharIndex + ((flag ? 1 : -1) * keywordCharIndex);
                index = Helper.Mod(index, language.Value.Length);

                result += language.Value[index];
            }
        }

        return result;
    }
}

using System;
using System.Text;

public class XOREncryption : IEncryption
{
    private string lastGeneratedKey;

    public XOREncryption() { }

    public string Validate(string message, string[] options)
    {
        if (message == "")
        {
            return "Message should not be empty";
        }

        if (options[0].Length < 0 && lastGeneratedKey == null)
        {
            return "There is no key entered and generated";
        }

        return "";
    }

    public string Encrypt(string message, string[] options)
    {
        string key = options[0];

        if (key.Length < 1)
        {
            key = GenerateRandomKey(message.Length);
        }

        string result = "";
        for (int i = 0; i < message.Length; ++i)
        {
            result += (char)GetValidKey(message[i], i, key);
        }
        return result;
    }

    private string GenerateRandomKey(int length)
    {
        Random random = new Random();
        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < length; ++i)
            builder.Append((char)random.Next(1, char.MaxValue));

        string key = builder.ToString();
        lastGeneratedKey = key;

        return key;
    }

    private int GetValidKey(char symbol, int symbolPosition, string key)
    {
        char keySymbol = key[Helper.Mod(symbolPosition, key.Length)];
        int val = symbol ^ keySymbol;

        return val;
    }

    public string Decrypt(string message, string[] options)
    {
        if (options[0].Length < 1)
            options[0] = lastGeneratedKey;

        return Encrypt(message, options);
    }
}

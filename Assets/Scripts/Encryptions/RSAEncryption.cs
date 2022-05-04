using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

public struct Key
{
    public int Exp { get; set; }
    public int N { get; set; }
}

public class RSAEncryption : IEncryption
{
    public string Validate(string message, string[] options)
    {
        if (options.Length == 3)
        {
            if (options[0].Length < 1 || options[1].Length < 1 || options[2].Length < 1)
            {
                return "p, q or e entered wrong";
            }

            int p = int.Parse(options[0]);
            int q = int.Parse(options[1]);
            int e = int.Parse(options[2]);

            if (!Helper.IsPrime(p))
            {
                return "p is not prime";
            }

            if (!Helper.IsPrime(q))
            {
                return "q is not prime.";
            }
            int fi = (p - 1) * (q - 1);
            if (!Helper.Coprime(e, fi))
            {
                return "e is not coprime with fi";
            }
            if (!(e > 1 && e < fi))
            {
                return "e should be { 1 < e < fi }";
            }
        }
        else if (options.Length == 7)
        {
            if (message.Length < 1)
            {
                return "Message should not be empty";
            }

            if (options[3].Length < 1 && options[4].Length < 1 && options[5].Length < 1 && options[6].Length < 1)
            {
                return "One of Exp or N is empty";
            }

            if (options[4] != options[6])
            {
                return "N should be the for both keys";
            }
        }

        return "";
    }

    public string Encrypt(string message, string[] options)
    {
        Dictionary<Language, string> lang = LanguageDetection.DetectLanguage(message);
        string alphabet = lang.First().Value;

        string convertedMessage = ConvertSymbols(message, alphabet);

        int Exp = int.Parse(options[3]);
        int N = int.Parse(options[4]);

        BigInteger m = BigInteger.Parse(convertedMessage);
        BigInteger exp = Exp;

        return BigInteger.ModPow(m, exp, N).ToString();
    }

    public string Decrypt(string message, string[] options)
    {
        int Exp = int.Parse(options[5]);
        int N = int.Parse(options[6]);

        BigInteger c = BigInteger.Parse(message);
        BigInteger d = Exp;

        return BigInteger.ModPow(c, d, N).ToString();
    }

    public static string ConvertSymbols(string text, string alphabet)
    {
        int digitsCount = (int)Math.Floor(Math.Log10(alphabet.Length) + 1);

        string result = "";

        foreach (char symbol in text)
        {
            int num = alphabet.IndexOf(symbol);
            result += num.ToString("D" + digitsCount);
        }

        return result;
    }

    public (Key open, Key closed) GetKeys(int p, int q, int e)
    {
        int fi = (p - 1) * (q - 1);

        int n = p * q;

        int d = Helper.ModInverse(e, fi);

        Key open = new Key { Exp = e, N = n };
        Key closed = new Key { Exp = d, N = n };

        return (open, closed);
    }
}
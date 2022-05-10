using System;
using System.Text;
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

        List<string> stringBuilder = new List<string>();
        int blockSize = GetBlockSize(N, alphabet.Length);
        for (var i = 0; i < convertedMessage.Length; i += blockSize)
        {
            string temp = string.Join(string.Empty, convertedMessage.Skip(i).Take(blockSize));
            BigInteger block = BigInteger.Parse(AddZerosEncrypt(temp, blockSize));
            var temp2 = BigInteger.ModPow(block, Exp, N).ToString();
            stringBuilder.Add(temp2);
        }

        return string.Join(" ", stringBuilder.ToArray());
    }

    public string Decrypt(string message, string[] options)
    {
        string alphabet = " abcdefghijklmnopqrstuvwxyz";

        int Exp = int.Parse(options[5]);
        int N = int.Parse(options[6]);

        StringBuilder result = new StringBuilder();
        var splittedText = message.Split(' ');
        foreach (string block in splittedText)
        {
            BigInteger numBlock = BigInteger.Parse(block);
            string decryptedText = BigInteger.ModPow(numBlock, Exp, N).ToString();
            result.Append(AddZerosDecrypt(decryptedText, GetBlockSize(N, alphabet.Length)));
        }

        string almostCompleteResult = result.ToString();
        string completeResult = string.Empty;
        for (int i = 0; i < almostCompleteResult.Length; i += 2)
        {
            int number = int.Parse(almostCompleteResult.Substring(i, 2));
            completeResult += alphabet[number];
        }
        return completeResult;
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

    private int GetBlockSize(int N, int size)
    {
        int i = 1;
        while (i <= 100)
        {
            int lowerBound = int.Parse(string.Concat(Enumerable.Repeat(size, i)));
            int upperBound = int.Parse(string.Concat(Enumerable.Repeat(size, i + 1)));
            if (N < upperBound && N > lowerBound)
            {
                return lowerBound.ToString().Length;
            }
            i++;
        }

        return 0;
    }

    private string AddZerosEncrypt(string text, int blockSize)
    {
        while (text.Length % blockSize != 0)
        {
            text += "0";
        }
        return text;
    }

    private string AddZerosDecrypt(string text, int blockSize)
    {
        while (text.Length % blockSize != 0)
        {
            text = text.Insert(0, "0");
        }
        return text;
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
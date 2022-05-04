using System;
using System.IO;
using System.Drawing;

public static class Helper
{
    public static int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    public static int GetCharIndex(string alphabet, char c)
    {
        for (int i = 0; i < alphabet.Length; ++i)
        {
            if (alphabet[i] == c)
            {
                return i;
            }
        }

        return -1;
    }

    public static string ProcessImageToString(string path)
    {
        Image image = new Bitmap(path);
        System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Png;

        using (MemoryStream ms = new MemoryStream())
        {
            // Convert Image to byte[]
            image.Save(ms, format);
            byte[] imageBytes = ms.ToArray();

            // Convert byte[] to Base64 String
            return Convert.ToBase64String(imageBytes);
        }
    }

    public static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        var boundary = (int)Math.Floor(Math.Sqrt(number));

        for (int i = 3; i <= boundary; i += 2)
            if (number % i == 0)
                return false;

        return true;
    }

    public static int GCD(int value1, int value2)
    {
        while (value1 != 0 && value2 != 0)
        {
            if (value1 > value2)
                value1 %= value2;
            else
                value2 %= value1;
        }
        return Math.Max(value1, value2);
    }

    public static bool Coprime(int value1, int value2)
    {
        return GCD(value1, value2) == 1;
    }

    public static int ModInverse(int a, int n)
    {
        int i = n,
            v = 0,
            d = 1;
        while (a > 0)
        {
            int t = i / a,
                x = a;

            a = i % x;
            i = x;
            x = d;
            d = v - t * x;
            v = x;
        }
        v %= n;
        if (v < 0)
        {
            v = (v + n) % n;
        }
        return v;
    }
}

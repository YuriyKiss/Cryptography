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
}

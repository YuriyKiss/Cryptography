using UnityEngine;
using System.Collections.Generic;

public static class FrequencyDistribution
{
    public static string Distribute(string encryption, string decryption)
    {
        string result = "Decrypted message\n";
        Dictionary<char, int> encryptionDistribution = GetDistribution(encryption);
        result += FormatDistribution(encryptionDistribution, encryption.Length);

        result += "Encrypted message\n";
        Dictionary<char, int> decryptionDistribution = GetDistribution(decryption);
        result += FormatDistribution(decryptionDistribution, decryption.Length);

        return result;
    }

    private static Dictionary<char, int> GetDistribution(string message)
    {
        Dictionary<char, int> distribution = new Dictionary<char, int>();

        foreach (char symbol in message)
        {
            if (distribution.ContainsKey(symbol))
            {
                distribution[symbol] += 1;
            }
            else
            {
                distribution[symbol] = 1;
            }
        }

        return distribution;
    }

    private static string FormatDistribution(Dictionary<char, int> distribution, int length)
    {
        string result = "";

        foreach(KeyValuePair<char, int> pair in distribution)
        {
            float percentage = (float)pair.Value / (float)length * 100f;
            result += $"Letter \'{pair.Key}\' - {Mathf.Round(percentage * 100f) / 100f}%\n";
        }

        return result;
    }
}
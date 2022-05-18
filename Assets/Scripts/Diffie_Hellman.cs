using UnityEngine;
using TMPro;
using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

public class Diffie_Hellman : MonoBehaviour
{
    public TMP_InputField p;
    public TMP_InputField g;

    public TMP_InputField a;
    public TMP_InputField b;

    public TMP_InputField A;
    public TMP_InputField B;

    public TMP_InputField K;

    public void CalculateDiffieHellman()
    {
        List<int> primes = GeneratePrimesSieveOfEratosthenes(1000);

        p.text = primes[UnityEngine.Random.Range(0, 999)].ToString();
        g.text = primes[UnityEngine.Random.Range(0, 999)].ToString();

        a.text = Mathf.Round(UnityEngine.Random.Range(0, Mathf.Pow(2, 16))).ToString();
        b.text = Mathf.Round(UnityEngine.Random.Range(0, Mathf.Pow(2, 16))).ToString();

        A.text = BigInteger.ModPow(BigInteger.Parse(g.text), BigInteger.Parse(a.text), BigInteger.Parse(p.text)).ToString();
        B.text = BigInteger.ModPow(BigInteger.Parse(g.text), BigInteger.Parse(b.text), BigInteger.Parse(p.text)).ToString();

        BigInteger K_A = BigInteger.ModPow(BigInteger.Parse(A.text), BigInteger.Parse(b.text), BigInteger.Parse(p.text));
        BigInteger K_B = BigInteger.ModPow(BigInteger.Parse(B.text), BigInteger.Parse(a.text), BigInteger.Parse(p.text));

        if (K_A != K_B)
        {
            print("Something went wrong");
            return;
        }

        K.text = K_A.ToString();
    }

    public static int ApproximateNthPrime(int nn)
    {
        double n = (double)nn;
        double p;
        if (nn >= 7022)
        {
            p = n * Math.Log(n) + n * (Math.Log(Math.Log(n)) - 0.9385);
        }
        else if (nn >= 6)
        {
            p = n * Math.Log(n) + n * Math.Log(Math.Log(n));
        }
        else if (nn > 0)
        {
            p = new int[] { 2, 3, 5, 7, 11 }[nn - 1];
        }
        else
        {
            p = 0;
        }
        return (int)p;
    }

    public static BitArray SieveOfEratosthenes(int limit)
    {
        BitArray bits = new BitArray(limit + 1, true);
        bits[0] = false;
        bits[1] = false;
        for (int i = 0; i * i <= limit; i++)
        {
            if (bits[i])
            {
                for (int j = i * i; j <= limit; j += i)
                {
                    bits[j] = false;
                }
            }
        }
        return bits;
    }

    public static List<int> GeneratePrimesSieveOfEratosthenes(int n)
    {
        int limit = ApproximateNthPrime(n);
        BitArray bits = SieveOfEratosthenes(limit);
        List<int> primes = new List<int>();
        for (int i = 0, found = 0; i < limit && found < n; i++)
        {
            if (bits[i])
            {
                primes.Add(i);
                found++;
            }
        }
        return primes;
    }
}

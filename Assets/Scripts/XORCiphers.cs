using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class XORCiphers
{
    public Dictionary<char, string> russianLetters;
    public Dictionary<char, string> englishLetters;
    public XORCiphers() 
    {

        russianLetters = new Dictionary<char, string>()
        {
              { 'а', "000000" },
              { 'б', "000001" },
              { 'в', "000010" },
              { 'г', "000011" },
              { 'д', "000100" },
              { 'е', "000101" },
              { 'ё', "000110" },
              { 'ж', "000111" },
              { 'з', "001000" },
              { 'и', "001001" },
              { 'й', "001010" },
              { 'к', "001011" },
              { 'л', "001100" },
              { 'м', "001101" },
              { 'н', "001110" },
              { 'о', "001111" },
              { 'п', "010000" },
              { 'р', "010001" },
              { 'с', "010010" },
              { 'т', "010011" },
              { 'у', "010100" },
              { 'ф', "010101" },
              { 'х', "010110" },
              { 'ц', "010111" },
              { 'ч', "011000" },
              { 'ш', "011001" },
              { 'щ', "011010" },
              { 'ъ', "011011" },
              { 'ы', "011100" },
              { 'ь', "011101" },
              { 'э', "011110" },
              { 'ю', "011111" },
              { 'я', "100000" },
              { '0', "100001" },
              { '1', "100010" },
              { '2', "100011" },
              { '3', "100100" },
              { '4', "100101" },
              { '5', "100110" },
              { '6', "100111" },
              { '7', "101000" },
              { '8', "101001" },
              { '9', "101010" }
        };
        englishLetters = new Dictionary<char, string>()
        {
              { 'a', "000000" },
              { 'b', "000001" },
              { 'c', "000010" },
              { 'd', "000011" },
              { 'e', "000100" },
              { 'f', "000101" },
              { 'g', "000110" },
              { 'h', "000111" },
              { 'i', "001000" },
              { 'j', "001001" },
              { 'k', "001010" },
              { 'l', "001011" },
              { 'm', "001100" },
              { 'n', "001101" },
              { 'o', "001110" },
              { 'p', "001111" },
              { 'q', "010000" },
              { 'r', "010001" },
              { 's', "010010" },
              { 't', "010011" },
              { 'u', "010100" },
              { 'v', "010101" },
              { 'w', "010110" },
              { 'x', "010111" },
              { 'y', "011000" },
              { 'z', "011001" },
              { '0', "011010" },
              { '1', "011011" },
              { '2', "011100" },
              { '3', "011101" },
              { '4', "011110" },
              { '5', "011111" },
              { '6', "100000" },
              { '7', "100001" },
              { '8', "100010" },
              { '9', "100011" }
        };
    }
    public string GetBinary(string text, Cipher.Languages language)
    {
        if(text == "")
        {
            throw new System.Exception("Please insert text");
        }
        Cipher.Languages verifyLanguage = Cipher.CheckLanguage(text);
        if (language != verifyLanguage && verifyLanguage!=Cipher.Languages.NotDecided)
        {
            throw new System.Exception("Language mismatch");
        }
        string binaryOutput = "";
        foreach(char character in text)
        {
            if (!char.IsLetter(character) && !char.IsDigit(character))
            {
                throw new System.Exception("Please remove symbols from the text");
            }
            switch (language)
            {
                case Cipher.Languages.English:
                    binaryOutput += englishLetters[character];
                    break;
                case Cipher.Languages.Russian:
                    binaryOutput += russianLetters[character];
                    break;
            }
        }
        return binaryOutput;
    }
    public string GetTextFromBinary(string binary, Cipher.Languages language)
    {
        string textOutput = "";
        if (!CheckIfBinary(binary))
        {
            throw new System.Exception("Not binary");
        }
        if (binary.Length % 6 != 0)
        {
            throw new System.Exception("Incorrect format");
        }
        for (int i = 0; i < binary.Length-5; i+=6) 
        {
            int end = i + 6;
            string substring = binary[i..end];
            switch (language)
            {
                case Cipher.Languages.English:
                    foreach(var letter in englishLetters)
                    {
                        if (letter.Value == substring)
                        {
                            Debug.Log(letter.Key);
                            textOutput += letter.Key.ToString();
                        }
                    }
                    break;
                case Cipher.Languages.Russian:
                    foreach (var letter in russianLetters)
                    {
                        Debug.Log($"{letter.Key},{letter.Value}");
                        if (letter.Value == substring)
                        {
                            textOutput += letter.Key.ToString();
                        }
                    }
                    break;
            }
        }
        return textOutput;
    }
    public bool CheckIfBinary(string text)
    {
        foreach (char character in text)
        {
            if (character!='1' && character!='0') 
            {
                return false;
            }
        }
        return true;
    }
    public string XOR(string binary1, string binary2)
    {
        if(binary1=="" ||binary2 == "")
        {
            throw new Exception("One of the strings is empty");
        }
        if(!CheckIfBinary(binary1) && !CheckIfBinary(binary2))
        {
            throw new Exception("One of the strings is not binary");
        }
        string xorResult = "";
        int bin1Length= binary1.Length;
        int bin2Length= binary2.Length;
        
        switch (bin1Length>bin2Length)
        {
            case true:
                for (int i = 0; i < bin1Length; i++)
                {
                    if (binary1[i] != binary2[i%bin2Length])
                    {
                        xorResult += '1';
                    }
                    else
                    {
                        xorResult += '0';
                    }
                }
                break;
            case false:
                for (int i = 0; i < bin1Length; i++)
                {
                    if (binary1[i] != binary2[i])
                    {
                        xorResult += '1';
                    }
                    else
                    {
                        xorResult += '0';
                    }
                }
                break;
        }
        
        return xorResult;
    }
    public List<string> BinaryIntoBlocks(string text)
    {
        List<string> result = new List<string>();
        for (int i = 0; i < text.Length; i += 6)
        {
            result.Add(text[i..(i + 6)]); ;
        }
        return result;
    }
    public string CascadeCiphering(string text, string key, string gamma)
    {
        if (text == "" || key == "" || gamma == "")
        {
            throw new Exception("One of the strings is empty");
        }
        if (!CheckIfBinary(text) && !CheckIfBinary(key) && !CheckIfBinary(key))
        {
            throw new Exception("One of the strings is not binary");
        }
        string cipheredText = "";
        List<string> blocks = BinaryIntoBlocks(text);
        string iv = XOR(gamma, key);
        foreach (string block in blocks)
        {
            string nextBlock = XOR(block, iv);
            cipheredText += nextBlock;
            iv = nextBlock;
        }
        return cipheredText;
    }
    

    public string CascadeDeciphering(string text, string key, string gamma)
    {
        if (text == "" || key == "")
        {
            throw new Exception("One of the strings is empty");
        }
        if (!CheckIfBinary(text) && !CheckIfBinary(key))
        {
            throw new Exception("One of the strings is not binary");
        }
        string decipheredText = "";
        List<string> blocks = BinaryIntoBlocks(text);
        string iv = XOR(gamma,key);
        foreach (string block in blocks)
        {
            string nextBlock = XOR(block, iv);
            decipheredText += nextBlock;
            iv = nextBlock;
        }
        return decipheredText;
    }
    public string GeneratePerfectGamma(int length)
    {
        if (length % 2 != 0)
        {
            throw new System.Exception("Length must be even to allow equal numbers of 1s and 0s.");
        }

        System.Random random = new System.Random();

        while (true)
        {
            double[] data = new double[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = NextGaussian(random, 0, 1);
            }

            int[] binaryData = data.Select(x => x > 0 ? 1 : 0).ToArray();
            Debug.Log(string.Concat(binaryData));
            if (binaryData.Count(x => x == 1) ==  binaryData.Count(x => x == 0))
            {
                Debug.Log("Success!");
                return string.Concat(binaryData);
            }
        }
    }
    public static double NextGaussian(System.Random random, double mean, double stdDev)
    {
        // Box-Muller transform
        double u1 = 1.0 - random.NextDouble();
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }
    public string ReverseString(string input)
    {
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
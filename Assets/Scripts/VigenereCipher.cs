using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class VigenereCipher : Cipher
{
    private CaesarCipher caesarObject;
    private float englishTextIndexOfCoincidence = (float)0.0644;
    private float englishAlphabetIndexOfCoincidence = (float)0.0385;
    private float englishAlphabetWithNumbersIndexOfCoincidence = (float)0.0277;
    private float russianTextIndexOfCoincidence = (float)0.0553;
    private float russianAlphabetIndexOfCoincidence = (float)0.0303;
    private float russianAlphabetWithNumbersIndexOfCoincidence = (float)0.0232;
    public VigenereCipher() : base()
    {
        caesarObject = new CaesarCipher();
        keyType = typeof(string);
    }

    public override string CipherText(string text, string keyString, Languages language)
    {
        string key = keyString.ToLower();
        if(CheckLanguage(key)!= language)
        {
            throw new Exception("Please enter the key in the same language without symbols");
        }
        if (text == "")
        {
            throw new Exception("Please enter text");
        }
        string formatedText = text.ToLower();
        Languages textLanguage = language;
        if (textLanguage == Languages.NotDecided)
        {
            textLanguage = CheckLanguage(formatedText);
            if (textLanguage == Languages.NotDecided)
            {
                throw new System.Exception("Please specify the language");
            }
        }
        string cipheredText = "";
        for (int i = 0;i<formatedText.Length;i++)
        {
            char c = formatedText[i];
            if (char.IsLetterOrDigit(c))
            {
                int shiftedIndex = -1;
                int keyLetterIndex = -1;
                switch (textLanguage)
                {
                    case Languages.English:
                        if (CheckLanguage(c.ToString()) != Languages.English && char.IsLetter(c))
                        {
                            throw new Exception("Language inconsistency");
                        }
                        shiftedIndex = englishSymbolsWithNumbers.IndexOf(c);
                        keyLetterIndex = englishSymbolsWithNumbers.IndexOf(key[i%key.Length]);
                        if ((shiftedIndex + keyLetterIndex) % englishSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex + keyLetterIndex) % englishSymbolsWithNumbers.Count + englishSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex + keyLetterIndex) % englishSymbolsWithNumbers.Count;
                        }
                        cipheredText += englishSymbolsWithNumbers[(int)shiftedIndex];
                        break;
                    case Languages.Russian:
                        if (CheckLanguage(c.ToString()) != Languages.Russian && char.IsLetter(c))
                        {
                            throw new Exception("Language inconsistency");
                        }
                        shiftedIndex = russianSymbolsWithNumbers.IndexOf(c);
                        keyLetterIndex = russianSymbolsWithNumbers.IndexOf(key[i % key.Length]);
                        if ((shiftedIndex + keyLetterIndex) % russianSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex + keyLetterIndex) % russianSymbolsWithNumbers.Count + russianSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex + keyLetterIndex) % russianSymbolsWithNumbers.Count;
                        }
                        cipheredText += russianSymbolsWithNumbers[(int)shiftedIndex];
                        break;
                    case Languages.NotDecided:
                        throw new System.Exception("Language undecided");
                        break;
                }
            }
            else
            {
                throw new System.Exception("Symbol Detected");
            }
        }
        return cipheredText;
    }

    public override string DecipherText(string text, string keyString, Languages language)
    {
        string key = keyString.ToLower();
        if (CheckLanguage(key) != language)
        {
            throw new Exception("Please enter the key in the same language without symbols");
        }
        if (text == "")
        {
            throw new Exception("Please enter text");
        }
        string formatedText = text.ToLower();
        Languages textLanguage = language;
        if (textLanguage == Languages.NotDecided)
        {
            textLanguage = CheckLanguage(formatedText);
            if (textLanguage == Languages.NotDecided)
            {
                throw new System.Exception("Please specify the language");
            }
        }
        string cipheredText = "";
        for (int i = 0; i < formatedText.Length; i++)
        {
            char c = formatedText[i];
            if (char.IsLetterOrDigit(c))
            {
                int shiftedIndex = -1;
                int keyLetterIndex = -1;
                switch (textLanguage)
                {
                    case Languages.English:
                        if (CheckLanguage(c.ToString()) != Languages.English && char.IsLetter(c))
                        {
                            throw new Exception("Language inconsistency");
                        }
                        shiftedIndex = englishSymbolsWithNumbers.IndexOf(c);
                        keyLetterIndex = englishSymbolsWithNumbers.IndexOf(key[i % key.Length]);
                        if ((shiftedIndex - keyLetterIndex) % englishSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex - keyLetterIndex) % englishSymbolsWithNumbers.Count + englishSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex - keyLetterIndex) % englishSymbolsWithNumbers.Count;
                        }
                        cipheredText += englishSymbolsWithNumbers[(int)shiftedIndex];
                        break;
                    case Languages.Russian:
                        if (CheckLanguage(c.ToString()) != Languages.Russian && char.IsLetter(c))
                        {
                            throw new Exception("Language inconsistency");
                        }
                        shiftedIndex = russianSymbolsWithNumbers.IndexOf(c);
                        keyLetterIndex = russianSymbolsWithNumbers.IndexOf(key[i % key.Length]);
                        if ((shiftedIndex - keyLetterIndex) % russianSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex - keyLetterIndex) % russianSymbolsWithNumbers.Count + russianSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex - keyLetterIndex) % russianSymbolsWithNumbers.Count;
                        }
                        cipheredText += russianSymbolsWithNumbers[(int)shiftedIndex];
                        break;
                    case Languages.NotDecided:
                        throw new System.Exception("Language undecided");
                        break;
                }
            }
            else
            {
                throw new System.Exception("Symbol Detected");
            }
        }
        return cipheredText;
    }

    public override string GetProbableKey(string text, Languages language)
    {
        if (text == "")
        {
            throw new Exception("Please enter text");
        }
        Languages textLanguage = language;
        if (textLanguage == Languages.NotDecided)
        {
            textLanguage = CheckLanguage(text);
            if (textLanguage == Languages.NotDecided)
            {
                throw new System.Exception("Please specify the language");
            }
        }
        int maxKeyLength = 20;
        Dictionary<int, float> probableKeyLengths = new Dictionary<int, float>();
        
        for (int keyLength = 1; keyLength <= maxKeyLength; keyLength++)
        {
            List<string> textGroups = new List<string>();
            for(int i = 0; i< text.Length; i ++)
            {
                string textGroup="";
                for (int k =i;k<text.Length;k+= keyLength)
                    textGroup += text[k];
                textGroups.Add(textGroup);
            }
            List<float> iocValues = new List<float>();
            foreach (string textGroup in textGroups)
            {
                if(textGroup!=null&& textGroup!="")
                    iocValues.Add(CalculateIOC(textGroup,language));
            }
            float sumIOC = 0;
            foreach(float IOC in iocValues)
            {
                if (IOC != float.NaN)
                    sumIOC += IOC;
            }
            float averageIOC = sumIOC / iocValues.Count;
            probableKeyLengths.Add(keyLength, averageIOC);
        }
        int mostProbableKeyLen = -1;
        float smallestDiff = float.MaxValue;
        foreach (var keyLen in probableKeyLengths)
        {
            switch (language)
            {
                case Languages.Russian:
                    if (Math.Abs(keyLen.Value - russianTextIndexOfCoincidence )< smallestDiff)
                    {
                        mostProbableKeyLen = keyLen.Key;
                        smallestDiff = Math.Abs(keyLen.Value - russianTextIndexOfCoincidence);
                    }
                        break;
                case Languages.English:
                    if (Math.Abs(keyLen.Value - englishTextIndexOfCoincidence)< smallestDiff)
                    {
                        mostProbableKeyLen = keyLen.Key;
                        smallestDiff = Math.Abs(keyLen.Value - russianTextIndexOfCoincidence);
                    }
                    break;
            }
            
        }
        Debug.Log($"Key len: {mostProbableKeyLen}");
        return FormKey(text, language, mostProbableKeyLen);
    }
    public float CalculateIOC(string text, Languages language)
    {
        Languages textLanguage = language;
        if (textLanguage == Languages.NotDecided)
        {
            textLanguage = CheckLanguage(text);
            if (textLanguage == Languages.NotDecided)
            {
                throw new System.Exception("Please specify the language");
            }
        }
        Dictionary<char, int> letterCounts = new Dictionary<char, int>();
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                if (!letterCounts.ContainsKey(c))
                {
                    letterCounts.Add(c, 0);
                }
                letterCounts[c]++;
            }
        }
        int textLength = text.Length;
        float numerator = 0;
        foreach(var letter in letterCounts)
        {
            numerator += letter.Value*(letter.Value-1);
        }
        float denominator = textLength*(textLength-1);
        if(denominator==0)
        {
            return 0;
        }
        return ((float)numerator/denominator);
    }
    public string FormKey(string text, Languages language, int keyLength)
    {
        List<string> textGroups = new List<string>();
        for (int i = 0; i < keyLength; i++)
        {
            string textGroup = "";
            for (int k = i; k < text.Length; k += keyLength)
                textGroup += text[k];
            textGroups.Add(textGroup);
        }
        string finalKey = "";
        foreach (var group in textGroups) 
        {
            int keyLetter = int.Parse(caesarObject.GetProbableKey(group, language));
            switch (language)
            {
                case Languages.Russian:
                    if (keyLetter < 0)
                    {
                        keyLetter += russianSymbolsWithNumbers.Count;
                    }
                    finalKey += russianSymbolsWithNumbers[keyLetter];
                    break;
                case Languages.English:
                    if (keyLetter< 0)
                    {
                        keyLetter += englishSymbolsWithNumbers.Count;
                    }

                    finalKey += englishSymbolsWithNumbers[keyLetter];
                    break;
            }
        }
        Debug.Log(finalKey);
        return finalKey;
    }
}

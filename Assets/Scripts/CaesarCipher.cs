using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.VisualScripting.Icons;

public class CaesarCipher:Cipher
{

    public CaesarCipher():base()
    {
        keyType = typeof(int);
    }
    public override string CipherText(string text, string keyString, Languages language)
    {
        BigInteger key = BigInteger.Parse(keyString);
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
        foreach(char c in formatedText)
        {
            if(char.IsLetterOrDigit(c)){
                BigInteger shiftedIndex;
                switch (textLanguage)
                {
                    case Languages.English:
                        if (CheckLanguage(c.ToString()) != Languages.English && char.IsLetter(c))
                        {
                            throw new Exception("Language inconsistency");
                        }
                        shiftedIndex = englishSymbolsWithNumbers.IndexOf(c);
                        if ((shiftedIndex + key) % englishSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex + key) % englishSymbolsWithNumbers.Count + englishSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex + key) % englishSymbolsWithNumbers.Count;
                        }
                        cipheredText += englishSymbolsWithNumbers[(int)shiftedIndex];
                        break;
                    case Languages.Russian:
                        if (CheckLanguage(c.ToString()) != Languages.Russian && char.IsLetter(c))
                        {
                            throw new Exception("Language inconsistency");
                        }
                        shiftedIndex = russianSymbolsWithNumbers.IndexOf(c);
                        if ((shiftedIndex + key) % russianSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex + key) % russianSymbolsWithNumbers.Count + russianSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex + key) % russianSymbolsWithNumbers.Count;
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
        BigInteger key = BigInteger.Parse(keyString);
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
        foreach (char c in formatedText)
        {
            if (char.IsLetterOrDigit(c))
            {
                BigInteger shiftedIndex;
                switch (textLanguage)
                {
                    case Languages.English:
                        if(CheckLanguage(c.ToString())!= Languages.English&&char.IsLetter(c))
                        {
                            throw new Exception("Language inconsistency");
                        }
                        shiftedIndex = englishSymbolsWithNumbers.IndexOf(c);
                        if ((shiftedIndex - key) % englishSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex - key) % englishSymbolsWithNumbers.Count + englishSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex - key) % englishSymbolsWithNumbers.Count;
                        }
                        cipheredText += englishSymbolsWithNumbers[(int)shiftedIndex];
                        break;
                    case Languages.Russian:
                        if (CheckLanguage(c.ToString()) != Languages.Russian && char.IsLetter(c))
                        {
                            throw new Exception("Language inconsistency");
                        }
                        shiftedIndex = russianSymbolsWithNumbers.IndexOf(c);
                        if ((shiftedIndex - key) % russianSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex - key) % russianSymbolsWithNumbers.Count + russianSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex - key) % russianSymbolsWithNumbers.Count;
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
    public Languages CheckLanguage(string text)
    {
        Languages language = Languages.NotDecided;
        foreach(char c in text)
        {
            if (!char.IsLetterOrDigit(c))
            {
                throw new System.Exception("Non-letter detected");
            }
            switch (language)
            {    
                case Languages.English:
                    if (char.IsLetter(c))
                    {
                        if (Regex.IsMatch(c.ToString(), @"\p{IsCyrillic}"))
                        {
                            throw new System.Exception("Language inconsistency");
                        }
                    }
                    break;
                case Languages.Russian:
                    if (char.IsLetter(c))
                    {
                        if (Regex.IsMatch(c.ToString(), @"\p{IsBasicLatin}"))
                        {
                            throw new System.Exception("Language inconsistency");
                        }
                    }
                    break;
                case Languages.NotDecided:
                    if (char.IsLetter(c))
                    {
                        if (Regex.IsMatch(c.ToString(), @"\p{IsCyrillic}"))
                        {
                            language = Languages.Russian;
                        }
                        else if(Regex.IsMatch(c.ToString(), @"\p{IsBasicLatin}"))
                        {
                            language = Languages.English;
                        }
                    }
                    break;
            }
        }
        return language;
    }
    public override string GetProbableKey(string text,Languages language)
    {
        if (text == "")
        {
            throw new Exception("Please enter text");
        }
        int possibleKey = 0;
        Languages textLanguage = language;
        if (textLanguage == Languages.NotDecided)
        {
            textLanguage = CheckLanguage(text);
            if (textLanguage == Languages.NotDecided)
            {
                throw new System.Exception("Please specify the language");
            }
        }
        Debug.Log(textLanguage.ToString());
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
        int highestCount = -1;
        char mostFrequentLetter = '_';
        foreach (var letter in letterCounts)
        {
            if (highestCount < letter.Value)
            {
                highestCount = letter.Value;
                mostFrequentLetter = letter.Key;
            }
        }
        Debug.Log(mostFrequentLetter);
        switch (textLanguage)
        {
            case Languages.Russian:
                Debug.Log($"MostFrequentLetter = {mostFrequentLetter}, key:{GetKeyBetweenLetters(mostFrequentLetter, mostFrequentRussianSymbol, language)}");
                possibleKey = -GetKeyBetweenLetters(mostFrequentLetter,mostFrequentRussianSymbol,language);
                break;
            case Languages.English:
                possibleKey = -GetKeyBetweenLetters(mostFrequentLetter,mostFrequentEnglishSymbol, language);
                break;
        }
        return possibleKey.ToString();
    }
}

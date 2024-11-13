using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;

public class CaesarCipher
{
    private List<char> russianSymbolsWithNumbers;
    private List<char> englishSymbolsWithNumbers;
    private List<char> russianSymbolsWithoutNumbers;
    private List<char> englishSymbolsWithoutNumbers;
    private Dictionary<char, double> russianFrequencies;
    private Dictionary<char, double> englishFrequencies;
    char mostFrequentRussianSymbol = 'о';
    char mostFrequentEnglishSymbol = 'e';

    public enum Languages
    {
        Russian, English, NotDecided
    }

    public List<char> RussianSymbolsWithNumbers { get => russianSymbolsWithNumbers; private set => russianSymbolsWithNumbers = value; }
    public List<char> EnglishSymbolsWithNumbers { get => englishSymbolsWithNumbers; private set => englishSymbolsWithNumbers = value; }
    public List<char> EnglishSymbolsWithoutNumbers { get => englishSymbolsWithoutNumbers; set => englishSymbolsWithoutNumbers = value; }
    public List<char> RussianSymbolsWithoutNumbers { get => russianSymbolsWithoutNumbers; set => russianSymbolsWithoutNumbers = value; }

    public CaesarCipher()
    {
        RussianSymbolsWithNumbers = new List<char> {
            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 
            'й', 'к', 'л', 'м', 'н','о','п','р','с','т', 'у', 
            'ф','х', 'ц', 'ч', 'ш', 'щ','ъ','ы','ь','э','ю','я',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
        EnglishSymbolsWithNumbers = new List<char> {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
        EnglishSymbolsWithoutNumbers = new List<char>
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        RussianSymbolsWithoutNumbers = new List<char>
        {
            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и',
            'й', 'к', 'л', 'м', 'н','о','п','р','с','т', 'у',
            'ф','х', 'ц', 'ч', 'ш', 'щ','ъ','ы','ь','э','ю','я'
        };
        russianFrequencies = new Dictionary<char, double>
        {
                { 'а', 0.0801 },
                { 'б', 0.0159 },
                { 'в', 0.0454 },
                { 'г', 0.0170 },
                { 'д', 0.0298 },
                { 'е', 0.0845 },
                { 'ё', 0.0004 },
                { 'ж', 0.0094 },
                { 'з', 0.0165 },
                { 'и', 0.0735 },
                { 'й', 0.0121 },
                { 'к', 0.0349 },
                { 'л', 0.0440 },
                { 'м', 0.0321 },
                { 'н', 0.0670 },
                { 'о', 0.1097 },
                { 'п', 0.0281 },
                { 'р', 0.0473 },
                { 'с', 0.0547 },
                { 'т', 0.0626 },
                { 'у', 0.0262 },
                { 'ф', 0.0026 },
                { 'х', 0.0097 },
                { 'ц', 0.0048 },
                { 'ч', 0.0144 },
                { 'ш', 0.0073 },
                { 'щ', 0.0036 },
                { 'ъ', 0.0004 },
                { 'ы', 0.0190 },
                { 'ь', 0.0174 },
                { 'э', 0.0032 },
                { 'ю', 0.0064 },
                { 'я', 0.0201 } };
        englishFrequencies = new Dictionary<char, double>
            {
                { 'a', 0.08167 },
                { 'b', 0.01492 },
                { 'c', 0.02782 },
                { 'd', 0.04253 },
                { 'e', 0.12702 },
                { 'f', 0.02228 },
                { 'g', 0.02015 },
                { 'h', 0.06094 },
                { 'i', 0.06966 },
                { 'j', 0.00153 },
                { 'k', 0.00772 },
                { 'l', 0.04025 },
                { 'm', 0.02406 },
                { 'n', 0.06749 },
                { 'o', 0.07507 },
                { 'p', 0.01929 },
                { 'q', 0.00095 },
                { 'r', 0.05987 },
                { 's', 0.06327 },
                { 't', 0.09056 },
                { 'u', 0.02758 },
                { 'v', 0.00978 },
                { 'w', 0.02360 },
                { 'x', 0.00150 },
                { 'y', 0.01974 },
                { 'z', 0.00074 }
            };
    }
    public string CipherText(string text, int key)
    {
        string formatedText = RemoveWhitespace(text.ToLower());
        Languages textLanguage = CheckLanguage(text);

        string cipheredText = "";
        foreach(char c in formatedText)
        {
            if(char.IsLetterOrDigit(c)){
            int shiftedIndex;
                switch (textLanguage)
                {
                    case Languages.English:
                        shiftedIndex = englishSymbolsWithNumbers.IndexOf(c);
                        if ((shiftedIndex + key) % englishSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex + key) % englishSymbolsWithNumbers.Count + englishSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex + key) % englishSymbolsWithNumbers.Count;
                        }
                        cipheredText += englishSymbolsWithNumbers[shiftedIndex];
                        break;
                    case Languages.Russian:
                        shiftedIndex = russianSymbolsWithNumbers.IndexOf(c);
                        if ((shiftedIndex + key) % russianSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex + key) % russianSymbolsWithNumbers.Count + russianSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex + key) % russianSymbolsWithNumbers.Count;
                        }
                        cipheredText += russianSymbolsWithNumbers[shiftedIndex];
                        break;
                    case Languages.NotDecided:
                        throw new System.Exception("Language undecided");
                        break;
                }
            }
            else
            {
                cipheredText += c;
            }
        }
        return cipheredText;
    }
    public string DecipherText(string text, int key)
    {
        string formatedText = RemoveWhitespace(text.ToLower());
        Languages textLanguage = CheckLanguage(text);

        string cipheredText = "";
        foreach (char c in formatedText)
        {
            if (char.IsLetterOrDigit(c))
            {
                int shiftedIndex;
                switch (textLanguage)
                {
                    case Languages.English:
                        shiftedIndex = englishSymbolsWithNumbers.IndexOf(c);
                        if ((shiftedIndex - key) % englishSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex - key) % englishSymbolsWithNumbers.Count + englishSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex - key) % englishSymbolsWithNumbers.Count;
                        }
                        cipheredText += englishSymbolsWithNumbers[shiftedIndex];
                        break;
                    case Languages.Russian:
                        shiftedIndex = russianSymbolsWithNumbers.IndexOf(c);
                        if ((shiftedIndex - key) % russianSymbolsWithNumbers.Count < 0)
                        {
                            shiftedIndex = (shiftedIndex - key) % russianSymbolsWithNumbers.Count + russianSymbolsWithNumbers.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex - key) % russianSymbolsWithNumbers.Count;
                        }
                        cipheredText += russianSymbolsWithNumbers[shiftedIndex];
                        break;
                    case Languages.NotDecided:
                        throw new System.Exception("Language undecided");
                        break;
                }
            }
            else
            {
                cipheredText += c;
            }
        }
        return cipheredText;
    }
    public Languages CheckLanguage(string text)
    {
        Languages language = Languages.NotDecided;
        foreach(char c in text)
        {
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
    public int GetKeyWithStatsSimplified(string text)
    {
        int possibleKey = 0;
        Languages language = CheckLanguage(text);
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
        switch (language)
        {
            case Languages.Russian:
                possibleKey = GetKeyBetweenLetters(mostFrequentLetter,mostFrequentRussianSymbol,language);
                break;
            case Languages.English:
                possibleKey = GetKeyBetweenLetters(mostFrequentLetter,mostFrequentEnglishSymbol, language);
                break;
        }
        return possibleKey;
    }
    public int GetKeyWithStats(string text)
    {
        Languages language = CheckLanguage(text);
        int totalLetterCount = text.Length;
        Dictionary<char,int> letterCounts = new Dictionary<char,int>();
        foreach (char c in text)
        {
            if(char.IsLetter(c))
            {
                if (!letterCounts.ContainsKey(c))
                {
                    letterCounts.Add(c, 0);
                }
                letterCounts[c]++;
            }
        }
        Dictionary<char,double> cipheredFrequencies = new Dictionary<char,double>();
        foreach (var letter in letterCounts) 
        {
            cipheredFrequencies.Add(letter.Key, letter.Value/totalLetterCount);
        }
        Dictionary<int,int> probableKeys = new Dictionary<int,int>();
        switch (language)
        {
            case Languages.English:
                
                foreach(var letter in cipheredFrequencies)
                {
                    int probableKey = 0;
                    double closestDiff = double.MaxValue;
                    foreach (var baseLetter in englishFrequencies)
                    {
                        if (Math.Abs(letter.Value - baseLetter.Value) < closestDiff)
                        {
                            closestDiff = Math.Abs(letter.Value - baseLetter.Value);
                            probableKey = GetShortestKeyBetweenLetters(letter.Key,baseLetter.Key, language);
                        }
                    }
                    if (!probableKeys.ContainsKey(probableKey))
                    {
                        probableKeys.Add(probableKey, 0);
                    }
                    probableKeys[probableKey]++;
                }
                break;
            case Languages.Russian:
                foreach (var letter in cipheredFrequencies)
                {
                    int probableKey = 0;
                    double closestDiff = double.MaxValue;
                    foreach (var baseLetter in russianFrequencies)
                    {
                        if (Math.Abs(letter.Value - baseLetter.Value) < closestDiff)
                        {
                            closestDiff = Math.Abs(letter.Value - baseLetter.Value);
                            probableKey = GetShortestKeyBetweenLetters(letter.Key, baseLetter.Key,language);
                        }
                    }
                    if (!probableKeys.ContainsKey(probableKey))
                    {
                        probableKeys.Add(probableKey, 0);
                    }
                    probableKeys[probableKey]++;
                }
                break;
        }
        int highestProbability = -1;
        int likelyKey = -1;
        foreach (var key in probableKeys)
        {
            if (highestProbability<key.Value)
            {
                highestProbability = key.Value; likelyKey = key.Key;
            }
        }
        return likelyKey;
    }
    public int GetShortestKeyBetweenLetters(char letter1,char letter2, Languages language)
    {
        switch (language)
        {
            case Languages.English:
                char newLetter1Eng = letter1;
                char newLetter2Eng = letter2;
                int forwardDistEng = 0;
                int backwardDistEng = 0;
                //Moving forward through the array
                while (newLetter1Eng != newLetter2Eng)
                {
                    if (englishSymbolsWithoutNumbers.IndexOf(newLetter1Eng)+1 != englishSymbolsWithoutNumbers.Count)
                    {
                        forwardDistEng++;
                        newLetter1Eng = englishSymbolsWithoutNumbers[englishSymbolsWithoutNumbers.IndexOf(newLetter1Eng) + 1];
                    }
                    else
                    {
                        forwardDistEng++;
                        newLetter1Eng = englishSymbolsWithoutNumbers[0];
                    }
                }
                //Moving backwards
                while (newLetter1Eng != newLetter2Eng)
                {
                    if (englishSymbolsWithoutNumbers.IndexOf(newLetter1Eng) - 1 != -1)
                    {
                        backwardDistEng++;
                        newLetter1Eng = englishSymbolsWithoutNumbers[englishSymbolsWithoutNumbers.IndexOf(newLetter1Eng) - 1];
                    }
                    else
                    {
                        backwardDistEng++;
                        newLetter1Eng = englishSymbolsWithoutNumbers[englishSymbolsWithoutNumbers.Count-1];
                    }
                }
                int shortestDistEng = Math.Min(forwardDistEng, backwardDistEng);
                if(shortestDistEng == backwardDistEng)
                {
                    shortestDistEng = -shortestDistEng;
                }
                return shortestDistEng;
                break;
            case Languages.Russian:
                int forwardDistRus = 0;
                int backwardDistRus = 0;
                char newLetter1Rus = letter1;
                char newLetter2Rus = letter2;
                //Moving forward through the array
                while (newLetter1Rus != newLetter2Rus)
                {
                    if (englishSymbolsWithoutNumbers.IndexOf(newLetter1Rus) + 1 != englishSymbolsWithoutNumbers.Count)
                    {
                        forwardDistRus++;
                        newLetter1Rus = englishSymbolsWithoutNumbers[englishSymbolsWithoutNumbers.IndexOf(newLetter1Rus) + 1];
                    }
                    else
                    {
                        forwardDistRus++;
                        newLetter1Rus = englishSymbolsWithoutNumbers[0];
                    }
                }
                //Moving backwards
                while (newLetter1Rus != newLetter2Rus)
                {
                    if (englishSymbolsWithoutNumbers.IndexOf(newLetter1Rus) - 1 != -1)
                    {
                        backwardDistRus++;
                        newLetter1Rus = englishSymbolsWithoutNumbers[englishSymbolsWithoutNumbers.IndexOf(newLetter1Rus) - 1];
                    }
                    else
                    {
                        backwardDistRus++;
                        newLetter1Rus = englishSymbolsWithoutNumbers[englishSymbolsWithoutNumbers.Count - 1];
                    }
                }
                int shortestDistRus = Math.Min(forwardDistRus, backwardDistRus);
                if (shortestDistRus == backwardDistRus)
                {
                    shortestDistRus = -shortestDistRus;
                }
                return shortestDistRus;
                break;
        }
        return -1;
    }
    public int GetKeyBetweenLetters(char letter1, char letter2, Languages language)
    {
        int firstKey = 0;
        switch (language)
        {
            case Languages.English:
                firstKey = englishSymbolsWithNumbers.IndexOf(letter2)- englishSymbolsWithNumbers.IndexOf(letter1);
                break;
            case Languages.Russian:
                firstKey = russianSymbolsWithNumbers.IndexOf(letter2) - russianSymbolsWithNumbers.IndexOf(letter1);
                break;
        }
        return firstKey;
    }
    static public string RemoveWhitespace(string str)
    {
        return Regex.Replace(str, @"\s+", "");
    }
}

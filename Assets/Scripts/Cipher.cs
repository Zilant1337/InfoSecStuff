using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class Cipher
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

    public Cipher()
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
    public abstract string CipherText(string text, string keyString, Languages language);
    public abstract string DecipherText(string text, string keyString, Languages language);
    public Languages CheckLanguage(string text)
    {
        Languages language = Languages.NotDecided;
        foreach (char c in text)
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
                        else if (Regex.IsMatch(c.ToString(), @"\p{IsBasicLatin}"))
                        {
                            language = Languages.English;
                        }
                    }
                    break;
            }
        }
        return language;
    }
    public int GetProbableKey(string text, Languages language)
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
                possibleKey = GetKeyBetweenLetters(mostFrequentLetter, mostFrequentRussianSymbol, language);
                break;
            case Languages.English:
                possibleKey = GetKeyBetweenLetters(mostFrequentLetter, mostFrequentEnglishSymbol, language);
                break;
        }
        return possibleKey;
    }

    public int GetKeyBetweenLetters(char letter1, char letter2, Languages language)
    {
        int firstKey = 0;
        switch (language)
        {
            case Languages.English:
                firstKey = englishSymbolsWithNumbers.IndexOf(letter2) - englishSymbolsWithNumbers.IndexOf(letter1);
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

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;

public class CaesarCipher
{
    private List<char> russianSymbols;
    private List<char> englishSymbols;
    public enum Languages
    {
        Russian, English, NotDecided
    }

    public List<char> RussianSymbols { get => russianSymbols; private set => russianSymbols = value; }
    public List<char> EnglishSymbols { get => englishSymbols; private set => englishSymbols = value; }
    public CaesarCipher()
    {
        RussianSymbols = new List<char> {
            'à', 'á', 'â', 'ã', 'ä', 'å', '¸', 'æ', 'ç', 'è', 'é', 'ê', 'ë', 'ì', 'í', 'î', 'ï', 'ð', 'ñ', 'ò', 'ó', 'ô', 'õ', 'ö', '÷', 'ø', 'ù', 'ú', 'û', 'ü', 'ý', 'þ', 'ÿ',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
        EnglishSymbols = new List<char> {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
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
                        shiftedIndex = englishSymbols.IndexOf(c);
                        if (key + shiftedIndex < 0)
                        {
                            shiftedIndex = (shiftedIndex + key) % englishSymbols.Count + englishSymbols.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex + key) % englishSymbols.Count;
                        }
                        cipheredText += englishSymbols[shiftedIndex];
                        break;
                    case Languages.Russian:
                        shiftedIndex = englishSymbols.IndexOf(c);
                        if (key + shiftedIndex < 0)
                        {
                            shiftedIndex = (shiftedIndex + key) % russianSymbols.Count + russianSymbols.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex + key) % russianSymbols.Count;
                        }
                        cipheredText += russianSymbols[shiftedIndex];
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
                        shiftedIndex = englishSymbols.IndexOf(c);
                        if (shiftedIndex - key < 0)
                        {
                            shiftedIndex = (shiftedIndex - key) % englishSymbols.Count + englishSymbols.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex - key) % englishSymbols.Count;
                        }
                        cipheredText += englishSymbols[shiftedIndex];
                        break;
                    case Languages.Russian:
                        shiftedIndex = englishSymbols.IndexOf(c);
                        if (shiftedIndex - key < 0)
                        {
                            shiftedIndex = (shiftedIndex - key) % russianSymbols.Count + russianSymbols.Count;
                        }
                        else
                        {
                            shiftedIndex = (shiftedIndex - key) % russianSymbols.Count;
                        }
                        cipheredText += russianSymbols[shiftedIndex];
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
                    if (!char.IsLetterOrDigit(c))
                    {
                        if (Regex.IsMatch(c.ToString(), @"\p{IsCyrillic}"))
                        {
                            throw new System.Exception("Language inconsistency");
                        }
                    }
                    break;
                case Languages.Russian:
                    if (!char.IsLetterOrDigit(c))
                    {
                        if (Regex.IsMatch(c.ToString(), @"\p{IsBasicLatin}"))
                        {
                            throw new System.Exception("Language inconsistency");
                        }
                    }
                    break;
                case Languages.NotDecided:
                    if (!char.IsLetterOrDigit(c))
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
    static string RemoveWhitespace(string str)
    {
        return Regex.Replace(str, @"\s+", "");
    }
}

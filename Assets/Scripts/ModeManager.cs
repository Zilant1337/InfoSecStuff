using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using AnotherFileBrowser.Windows;
using System.IO;
using UnityEngine.Networking;
using System.Numerics;

public class ModeManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject basicCipherMode;
    [SerializeField]
    private GameObject fileCipherMode;
    [SerializeField]
    private GameObject crackingMode;
    [SerializeField]
    private GameObject currentMode;
    [SerializeField]
    private TMP_Dropdown languageDropDown;
    [SerializeField]
    private GameObject Errorscreen;

    [SerializeField]
    private TMP_Text errortext;
    public CaesarCipher cipherObject;
    public CaesarCipher.Languages language;

    public void SwitchMode(GameObject newMode)
    {
        currentMode?.SetActive(false);
        if(newMode!=currentMode)
        {
            currentMode = newMode;
            currentMode.SetActive(true);
        }
    }

    void Start()
    {
        language = CaesarCipher.Languages.NotDecided;
        currentMode = null;
        cipherObject = new CaesarCipher();
    }
    public void CipherBasicInput()
    {
        string text = basicCipherMode.GetComponent<BasicCipherScript>().TextField.text;
        string keyString = basicCipherMode.GetComponent<BasicCipherScript>().KeyField.text;
        BigInteger key = 0;
        try
        {
            key = BigInteger.Parse(keyString);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
        try
        {
            basicCipherMode.GetComponent<BasicCipherScript>().OutputField.text = cipherObject.CipherText(text, key, language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
    }
    public void DecipherBasicInput()
    {
        string text = basicCipherMode.GetComponent<BasicCipherScript>().TextField.text;
        string keyString = basicCipherMode.GetComponent<BasicCipherScript>().KeyField.text;
        BigInteger key = 0;
        try
        {
            key = BigInteger.Parse(keyString);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
        try
        {
            basicCipherMode.GetComponent<BasicCipherScript>().OutputField.text = cipherObject.DecipherText(text, key, language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
    }
    public void CipherFileInput()
    {
        string text = fileCipherMode.GetComponent<FileCipherScript>().TextField.text;
        string keyString = fileCipherMode.GetComponent<FileCipherScript>().KeyField.text;
        BigInteger key = 0;
        try
        {
            key = BigInteger.Parse(keyString);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
        }
        try
        {
            fileCipherMode.GetComponent<FileCipherScript>().OutputField.text = cipherObject.CipherText(text, key,language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
    }
    public void DecipherFileInput()
    {
        string text = fileCipherMode.GetComponent<FileCipherScript>().TextField.text;
        string keyString = fileCipherMode.GetComponent<FileCipherScript>().KeyField.text;
        BigInteger key = 0;
        try
        {
            key = BigInteger.Parse(keyString);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
        try
        {
            fileCipherMode.GetComponent<FileCipherScript>().OutputField.text = cipherObject.DecipherText(text, key, language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
    }
    public void GetTextFromFile(TMP_InputField inputField)
    {
        string fileText = "";
        BrowserProperties browserProperties = new BrowserProperties();
        browserProperties.filter = "txt files (*.txt)|*.txt";
        browserProperties.filterIndex = 0;
        new FileBrowser().OpenFileBrowser(browserProperties, path =>
        {
            fileText=GetText(path);
        });
        Debug.Log(fileText);
        try
        {
            string formatedText = fileText.ToLower();
            if (cipherObject.CheckLanguage(formatedText) ==CaesarCipher.Languages.Russian|| cipherObject.CheckLanguage(fileText) == CaesarCipher.Languages.English)
            {
                inputField.text = formatedText;
            }
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
    }
    public void SaveTextToFile(TMP_InputField inputField)
    {
        string selectedPath="";
        BrowserProperties browserProperties = new BrowserProperties();
        browserProperties.filter = "txt files (*.txt)|*.txt";
        browserProperties.filterIndex = 0;
        new FileBrowser().OpenFileBrowser(browserProperties, path =>
        {
            selectedPath=path;
        });
        using (StreamWriter outputFile = new StreamWriter(selectedPath, false))
        {
            outputFile.Write(inputField.text);
        }
    }
    public void CrackCode()
    {
        string text = crackingMode.GetComponent<CrackingScript>().TextField.text;
        int possibleKey = cipherObject.GetKeyWithStatsSimplified(text, language);
        try
        {
            
            crackingMode.GetComponent<CrackingScript>().KeyField.text = possibleKey.ToString();
            crackingMode.GetComponent<CrackingScript>().OutputField.text = cipherObject.CipherText(text, possibleKey, language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
        //This is the long and most likely broken version
        /*int likelyKey = cipherObject.GetKeyWithStats(crackingMode.GetComponent<CrackingScript>().TextField.text);
        crackingMode.GetComponent<CrackingScript>().KeyField.text = likelyKey.ToString();
        crackingMode.GetComponent<CrackingScript>().TextField.text = cipherObject.CipherText(crackingMode.GetComponent<CrackingScript>().TextField.text,likelyKey);*/
    }
    private string GetText(string path)
    {
        var streamReader =new StreamReader(path);
        var text = streamReader.ReadToEnd();
        streamReader.Close();
        return text;
    }
    public void ChangeLanguage()
    {
        string dropdownText = languageDropDown.options[languageDropDown.value].text;
        switch (dropdownText)
        {
            case "English":
                language = CaesarCipher.Languages.English;
                Debug.Log("Switched to english");
                break;
            case "Russian":
                language = CaesarCipher.Languages.Russian;
                Debug.Log("Switched to russian");
                break;
            case "Autodetect":
                language = CaesarCipher.Languages.NotDecided;
                Debug.Log("Switched to autodetect");
                break;
        }
    }
    public void DisplayError(string text)
    {
        errortext.text = text;
        Errorscreen.SetActive(true);
    }
    public void CloseErrorScreen()
    {
        Errorscreen.SetActive(false);
    }
}

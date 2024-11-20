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
        language = CaesarCipher.Languages.Russian;
        currentMode = null;
        cipherObject = new CaesarCipher();
    }
    public void CipherBasicInput()
    {
        string text = basicCipherMode.GetComponent<ModeScript>().TextField.text;
        string keyString = basicCipherMode.GetComponent<ModeScript>().KeyField.text;
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
            basicCipherMode.GetComponent<ModeScript>().OutputField.text = cipherObject.CipherText(text, key, language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
    }
    public void DecipherBasicInput()
    {
        string text = basicCipherMode.GetComponent<ModeScript>().TextField.text;
        string keyString = basicCipherMode.GetComponent<ModeScript>().KeyField.text;
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
            basicCipherMode.GetComponent<ModeScript>().OutputField.text = cipherObject.DecipherText(text, key, language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
    }
    public void CipherFileInput()
    {
        string text = fileCipherMode.GetComponent<ModeScript>().TextField.text;
        string keyString = fileCipherMode.GetComponent<ModeScript>().KeyField.text;
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
            fileCipherMode.GetComponent<ModeScript>().OutputField.text = cipherObject.CipherText(text, key,language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
    }
    public void DecipherFileInput()
    {
        string text = fileCipherMode.GetComponent<ModeScript>().TextField.text;
        string keyString = fileCipherMode.GetComponent<ModeScript>().KeyField.text;
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
            fileCipherMode.GetComponent<ModeScript>().OutputField.text = cipherObject.DecipherText(text, key, language);
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
        try
        {
            string formatedText = fileText.ToLower();
            if (cipherObject.CheckLanguage(formatedText) ==CaesarCipher.Languages.Russian|| cipherObject.CheckLanguage(fileText) == CaesarCipher.Languages.English)
            {
                inputField.text = formatedText;
            }
            ClearAllFieldsInCurrentMode();
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
    }
    public void SaveTextToFile(TMP_InputField inputField)
    {
        try
        {
            if (inputField.text == "")
            {
                throw new Exception("Please enter text");
            }
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
            return;
        }
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
        string text = crackingMode.GetComponent<ModeScript>().TextField.text;
        try
        {
            if (text == "")
            {
                throw new Exception("Please enter text");
            }
        }
        catch (Exception e) 
        {
            DisplayError(e.Message);
            return;
        }
        int possibleKey = cipherObject.GetKeyWithStatsSimplified(text, language);
        try
        {
            crackingMode.GetComponent<ModeScript>().OutputField.text = cipherObject.CipherText(text, possibleKey, language);
            crackingMode.GetComponent<ModeScript>().KeyField.text = possibleKey.ToString();
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
        ClearAllFieldsInCurrentMode();
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
    public void ClearAllFieldsInCurrentMode()
    {
        currentMode.GetComponent<ModeScript>().KeyField.text = "";
        currentMode.GetComponent<ModeScript>().OutputField.text = "";
    }
}

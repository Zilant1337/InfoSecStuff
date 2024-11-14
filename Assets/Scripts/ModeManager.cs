using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using AnotherFileBrowser.Windows;
using System.IO;
using UnityEngine.Networking;

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
    public CaesarCipher cipherObject;

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
        currentMode = null;
        cipherObject = new CaesarCipher();
    }
    public void CipherBasicInput()
    {
        string text = basicCipherMode.GetComponent<BasicCipherScript>().TextField.text;
        string keyString = basicCipherMode.GetComponent<BasicCipherScript>().KeyField.text;
        int key = 0;
        try
        {
            key = int.Parse(keyString);
        }
        catch
        {
            Debug.Log("Key is not a valid number, try again");
        }
        try
        {
            basicCipherMode.GetComponent<BasicCipherScript>().OutputField.text = cipherObject.CipherText(text, key);
        }
        catch
        {
            Debug.Log("Ciphering unsuccessful");
        }
    }
    public void DecipherBasicInput()
    {
        string text = basicCipherMode.GetComponent<BasicCipherScript>().TextField.text;
        string keyString = basicCipherMode.GetComponent<BasicCipherScript>().KeyField.text;
        int key = 0;
        try
        {
            key = int.Parse(keyString);
        }
        catch
        {
            Debug.Log("Key is not a valid number, try again");
        }
        try
        {
            basicCipherMode.GetComponent<BasicCipherScript>().OutputField.text = cipherObject.DecipherText(text, key);
        }
        catch
        {
            Debug.Log("Deciphering unsuccessful");
        }
    }
    public void CipherFileInput()
    {
        string text = fileCipherMode.GetComponent<FileCipherScript>().TextField.text;
        string keyString = fileCipherMode.GetComponent<FileCipherScript>().KeyField.text;
        int key = 0;
        try
        {
            key = int.Parse(keyString);
        }
        catch
        {
            Debug.Log("Key is not a valid number, try again");
        }
        try
        {
            fileCipherMode.GetComponent<FileCipherScript>().OutputField.text = cipherObject.CipherText(text, key);
        }
        catch
        {
            Debug.Log("Ciphering unsuccessful");
        }
    }
    public void DecipherFileInput()
    {
        string text = fileCipherMode.GetComponent<FileCipherScript>().TextField.text;
        string keyString = fileCipherMode.GetComponent<FileCipherScript>().KeyField.text;
        int key = 0;
        try
        {
            key = int.Parse(keyString);
        }
        catch
        {
            Debug.Log("Key is not a valid number, try again");
        }
        try
        {
            fileCipherMode.GetComponent<FileCipherScript>().OutputField.text = cipherObject.DecipherText(text, key);
        }
        catch
        {
            Debug.Log("Deciphering unsuccessful");
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
        catch
        {
            Debug.Log("Error reading file, probably mixed languages");
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
        int possibleKey = cipherObject.GetKeyWithStatsSimplified(text);
        crackingMode.GetComponent<CrackingScript>().KeyField.text = possibleKey.ToString();
        crackingMode.GetComponent<CrackingScript>().OutputField.text = cipherObject.CipherText(text, possibleKey);
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

}

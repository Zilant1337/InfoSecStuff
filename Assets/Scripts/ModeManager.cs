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
using UnityEngine.SceneManagement;

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
    private TMP_Dropdown cipherDropDown;
    [SerializeField]
    private GameObject Errorscreen;

    [SerializeField]
    private TMP_Text errortext;
    public Cipher currentCipher;
    public CaesarCipher caesarCipher;
    public VigenereCipher vigenereCipher;
    public Cipher.Languages language;

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
        caesarCipher = new CaesarCipher();
        vigenereCipher = new VigenereCipher();
        currentCipher = caesarCipher;
    }
    public void CipherBasicInput()
    {
        string text = basicCipherMode.GetComponent<ModeScript>().TextField.text;
        string keyString = basicCipherMode.GetComponent<ModeScript>().KeyField.text;
        try
        {
            basicCipherMode.GetComponent<ModeScript>().OutputField.text = currentCipher.CipherText(text, keyString, language);
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
        try
        {
            basicCipherMode.GetComponent<ModeScript>().OutputField.text = currentCipher.DecipherText(text, keyString, language);
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
        try
        {
            fileCipherMode.GetComponent<ModeScript>().OutputField.text = currentCipher.CipherText(text, keyString, language);
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

        try
        {
            fileCipherMode.GetComponent<ModeScript>().OutputField.text = currentCipher.DecipherText(text, keyString, language);
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
            if (Cipher.CheckLanguage(formatedText) ==CaesarCipher.Languages.Russian|| Cipher.CheckLanguage(fileText) == CaesarCipher.Languages.English)
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
        
        try
        {
            string possibleKey = currentCipher.GetProbableKey(text, language);
            crackingMode.GetComponent<ModeScript>().OutputField.text = currentCipher.DecipherText(text, possibleKey.ToString(), language);
            crackingMode.GetComponent<ModeScript>().KeyField.text = possibleKey;
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
    public void ChangeCipher()
    {
        ClearAllFieldsInCurrentMode();
        string dropdownText = cipherDropDown.options[cipherDropDown.value].text;
        switch (dropdownText)
        {
            case "Caesar":
                currentCipher = caesarCipher;
                Debug.Log("Switched to caesar");
                break;
            case "Vigenere":
                currentCipher = vigenereCipher;
                Debug.Log("Switched to vigenere");
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
        if (currentMode != null){
        currentMode.GetComponent<ModeScript>().KeyField.text = "";
            currentMode.GetComponent<ModeScript>().OutputField.text = "";
        }
    }
    public void SwitchToGammas()
    {
        SceneManager.LoadScene("GammasScene",LoadSceneMode.Single);
    }
}

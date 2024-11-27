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

public class GammaModeManagerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject basicCipherMode;
    [SerializeField]
    private GameObject gammaGenerationMode;
    [SerializeField]
    private GameObject linkageCipherMode;
    [SerializeField]
    private GameObject currentMode;
    [SerializeField]
    private TMP_Dropdown languageDropDown;
    [SerializeField]
    private GameObject Errorscreen;

    [SerializeField]
    private TMP_Text errortext;

    public XORCiphers xorCipher;
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
        xorCipher = new XORCiphers();
    }
    public void BasicXORCipher()
    {
        try
        {
            string text = basicCipherMode.GetComponent<BasicCipherMode>().inputTextField.text.ToLower();
            string key = basicCipherMode.GetComponent<BasicCipherMode>().keyField.text.ToLower();
            if(text == "")
            {
                throw new Exception("Please input text");
            }
            if (key == "")
            {
                throw new Exception("Please input key");
            }
            string binaryText = xorCipher.GetBinary(text,language);
            string binaryKey = xorCipher.GetBinary(key,language);
            basicCipherMode.GetComponent<BasicCipherMode>().binaryInputTextField.text = binaryText;
            basicCipherMode.GetComponent<BasicCipherMode>().keyBinaryField.text = binaryKey;
            basicCipherMode.GetComponent<BasicCipherMode>().gammaField.text = xorCipher.XOR(binaryText,binaryKey);
            
        }
        catch (Exception e) 
        {
            DisplayError(e.Message);
        }
    }
    public void BasicXORDecipher()
    {
        try
        {
            string gamma = basicCipherMode.GetComponent<BasicCipherMode>().gammaField.text;
            string binaryKey = basicCipherMode.GetComponent<BasicCipherMode>().keyBinaryField.text;
            if (gamma == ""||binaryKey== "")
            {
                throw new Exception("Please cipher the text first");
            }

            basicCipherMode.GetComponent<BasicCipherMode>().binaryDecipheredField.text = xorCipher.XOR(gamma, binaryKey);
            basicCipherMode.GetComponent<BasicCipherMode>().decipheredTextField.text = xorCipher.GetTextFromBinary(basicCipherMode.GetComponent<BasicCipherMode>().binaryDecipheredField.text, language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
        }
    }
    public void DublicateAsBinary(TMP_InputField from, TMP_InputField to)
    {
        if(from.text!="")
            to.text =xorCipher.GetBinary(from.text,language); 
    }
   
    public void ChangeLanguage()
    {
        ClearAllFieldsInCurrentMode();
        string dropdownText = languageDropDown.options[languageDropDown.value].text;
        switch (dropdownText)
        {
            case "English":
                language = Cipher.Languages.English;
                Debug.Log("Switched to english");
                break;
            case "Russian":
                language = Cipher.Languages.Russian;
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
        if (currentMode != null){
        currentMode.GetComponent<ModeScript>().KeyField.text = "";
            currentMode.GetComponent<ModeScript>().OutputField.text = "";
        }
    }
    public void SwitchToNormal()
    {
        SceneManager.LoadScene("VigenereCaesarScene", LoadSceneMode.Single);
    }
}

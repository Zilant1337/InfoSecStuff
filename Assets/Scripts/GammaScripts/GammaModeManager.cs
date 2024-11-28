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
    public void GammaGenerationCipher()
    {
        try
        {
            string text = gammaGenerationMode.GetComponent<GammaGenerationMode>().inputTextField.text.ToLower();
            string key = gammaGenerationMode.GetComponent<GammaGenerationMode>().keyBinaryField.text;
            if (text == "")
            {
                throw new Exception("Please input text");
            }
            if (key == "")
            {
                throw new Exception("Please generate key");
            }
            string binaryText = xorCipher.GetBinary(text, language);
            gammaGenerationMode.GetComponent<GammaGenerationMode>().binaryInputTextField.text = binaryText;
            gammaGenerationMode.GetComponent<GammaGenerationMode>().gammaField.text = xorCipher.XOR(binaryText, key);

        }
        catch (Exception e)
        {
            DisplayError(e.Message);
        }
    }
    public void GammaGenerationDecipher()
    {
        try
        {
            string gamma = gammaGenerationMode.GetComponent<GammaGenerationMode>().gammaField.text;
            string binaryKey = gammaGenerationMode.GetComponent<GammaGenerationMode>().keyBinaryField.text;
            if (gamma == "" || binaryKey == "")
            {
                throw new Exception("Please cipher the text first");
            }

            gammaGenerationMode.GetComponent<GammaGenerationMode>().binaryDecipheredField.text = xorCipher.XOR(gamma, binaryKey);
            gammaGenerationMode.GetComponent<GammaGenerationMode>().decipheredTextField.text = xorCipher.GetTextFromBinary(gammaGenerationMode.GetComponent<GammaGenerationMode>().binaryDecipheredField.text, language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
        }
    }
    public void GammaGenerationGenerate()
    {
        try
        {
            string text = gammaGenerationMode.GetComponent<GammaGenerationMode>().binaryInputTextField.text;
            if(text == "")
            {
                throw new Exception("Please input text");
            }
            if (Cipher.CheckLanguage(text.ToLower()) != language && Cipher.CheckLanguage(text.ToLower()) != Cipher.Languages.NotDecided)
            {
                throw new Exception("Language mismatch");
            }
            string gamma = xorCipher.GeneratePerfectGamma(text.Length);
            gammaGenerationMode.GetComponent<GammaGenerationMode>().keyBinaryField.text = gamma;
        }
        catch(Exception e)
        {
            DisplayError(e.Message);
        }
    }
    public void LinkageCipherCipher()
    {
        try
        {
            string text = linkageCipherMode.GetComponent<LinkageCipherMode>().inputTextField.text.ToLower();
            string key = linkageCipherMode.GetComponent<LinkageCipherMode>().keyField.text;
            string gamma = linkageCipherMode.GetComponent<LinkageCipherMode>().gammaField.text;
            if (text == "")
            {
                throw new Exception("Please input text");
            }
            if (key == "")
            {
                throw new Exception("Please input key");
            }
            if(gamma == "")
            {
                throw new Exception("Please generate gamma");
            }
            string binaryText = xorCipher.GetBinary(text, language);
            string binaryKey = xorCipher.GetBinary(key, language);
            linkageCipherMode.GetComponent<LinkageCipherMode>().binaryInputTextField.text = binaryText;
            linkageCipherMode.GetComponent<LinkageCipherMode>().keyBinaryField.text = binaryKey;
            linkageCipherMode.GetComponent<LinkageCipherMode>().IV2Field.text = xorCipher.XOR(gamma, binaryKey);
            string cipheredBinary = xorCipher.CascadeCiphering(binaryText, binaryKey, gamma);
            linkageCipherMode.GetComponent<LinkageCipherMode>().binaryCipheredField.text = xorCipher.CascadeCiphering(binaryText,binaryKey,gamma);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
        }
    }
    public void LinkageCipherDecipher()
    {
        try
        {
            string gamma = linkageCipherMode.GetComponent<LinkageCipherMode>().gammaField.text;
            string binaryKey = linkageCipherMode.GetComponent<LinkageCipherMode>().keyBinaryField.text;
            string cipheredBinary = linkageCipherMode.GetComponent<LinkageCipherMode>().binaryCipheredField.text;
            if (gamma == "" || binaryKey == "" || cipheredBinary == "")
            {
                throw new Exception("Please cipher the text first");
            }

            string decipheredBinary = xorCipher.CascadeDeciphering(cipheredBinary, binaryKey, gamma);
            linkageCipherMode.GetComponent<LinkageCipherMode>().binaryDecipheredField.text = decipheredBinary;
            linkageCipherMode.GetComponent<LinkageCipherMode>().decipheredTextField.text = xorCipher.GetTextFromBinary(decipheredBinary, language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
        }
    }
    public void LinkageCipherGenerate()
    {
        try
        {
            string text = linkageCipherMode.GetComponent<LinkageCipherMode>().binaryInputTextField.text;
            if (text == "")
            {
                throw new Exception("Please input text");
            }
            if (Cipher.CheckLanguage(text.ToLower()) != language && Cipher.CheckLanguage(text.ToLower()) != Cipher.Languages.NotDecided)
            {
                throw new Exception("Language mismatch");
            }
            string gamma = xorCipher.GeneratePerfectGamma(text.Length);
            linkageCipherMode.GetComponent<LinkageCipherMode>().gammaField.text = gamma;
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
        }
    }
    public void DublicateAsBinary(TMP_InputField from, TMP_InputField to)
    {
        try
        {
            if (from.text != "")
                to.text = xorCipher.GetBinary(from.text.ToLower(), language);
        }
        catch (Exception e)
        {
            DisplayError(e.Message);
        }
    }

    public void ChangeLanguage()
    {
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
    public void SwitchToNormal()
    {
        SceneManager.LoadScene("VigenereCaesarScene", LoadSceneMode.Single);
    }   
}

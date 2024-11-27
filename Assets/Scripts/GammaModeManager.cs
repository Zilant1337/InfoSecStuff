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
    private TMP_Dropdown cipherDropDown;
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

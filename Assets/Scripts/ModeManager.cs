using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        currentMode = newMode;
        currentMode.SetActive(true);
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
            Debug.Log("Ciphru vvedi eblan");
        }
        try
        {
            basicCipherMode.GetComponent<BasicCipherScript>().TextField.text = cipherObject.CipherText(text, key);
        }
        catch
        {
            Debug.Log("Error when ciphering");
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
            Debug.Log("Ciphru vvedi eblan");
        }
        try
        {
            basicCipherMode.GetComponent<BasicCipherScript>().TextField.text = cipherObject.DecipherText(text, key);
        }
        catch
        {
            Debug.Log("Error when deciphering");
        }
    }
}

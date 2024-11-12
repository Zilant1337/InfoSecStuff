using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}

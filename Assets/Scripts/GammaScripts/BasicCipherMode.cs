using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasicCipherMode : MonoBehaviour
{
    [SerializeField]
    private GammaModeManagerScript m_GammaModeManagerScript;
    [SerializeField]
    public TMP_InputField inputTextField;
    [SerializeField]
    public TMP_InputField binaryInputTextField;
    [SerializeField]
    public TMP_InputField keyField;
    [SerializeField]
    public TMP_InputField keyBinaryField;
    [SerializeField]
    public TMP_InputField gammaField;
    [SerializeField]
    public TMP_InputField decipheredTextField;
    [SerializeField]
    public TMP_InputField binaryDecipheredField;

    public void DublicateTextAsBinary()
    {
        m_GammaModeManagerScript.DublicateAsBinary(inputTextField, binaryInputTextField);
    }
    public void DublicateKeyAsBinary()
    {
        m_GammaModeManagerScript.DublicateAsBinary(keyField, keyBinaryField);
    }
}

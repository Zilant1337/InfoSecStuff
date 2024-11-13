using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicCipherScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField keyField;
    [SerializeField]
    private TMP_InputField textField;

    public TMP_InputField KeyField { get => keyField; set => keyField = value; }
    public TMP_InputField TextField { get => textField; set => textField = value; }
}

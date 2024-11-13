using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FileCipherScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField keyField;
    [SerializeField]
    private TMP_InputField textField;

    public TMP_InputField KeyField { get => keyField; set => keyField = value; }
    public TMP_InputField TextField { get => textField; set => textField = value; }
}

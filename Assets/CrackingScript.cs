using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrackingScript : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField keyField;
    [SerializeField]
    private TMP_InputField textField;
    [SerializeField]
    private TMP_InputField outputField;

    public TMP_InputField KeyField { get => keyField; set => keyField = value; }
    public TMP_InputField TextField { get => textField; set => textField = value; }
    public TMP_InputField OutputField { get => outputField; set => outputField = value; }

}

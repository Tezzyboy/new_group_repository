using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class UIWelcomeScript : MonoBehaviour
{
    public TextMeshProUGUI output;
    public TMP_InputField userName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject level1button;
    public GameObject level2button;
    public GameObject level3button;


    void Start()
    {
        level1button.SetActive(false);
        level2button.SetActive(false);
        level3button.SetActive(false);
    }

    // Update is called once per frame


    void ButtonDemo()
    {
        output.text = "Welcome " + userName.text;
        level1button.SetActive(true);
        level2button.SetActive(true);
        level3button.SetActive(true);
    }
}

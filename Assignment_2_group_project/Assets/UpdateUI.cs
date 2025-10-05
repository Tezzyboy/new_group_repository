using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;


public class UpdateUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject ObjectPrefab;
    [SerializeField] private int totalobjects = 3;
    
    private TextMeshProUGUI UIText;
    private string ObjectID;

    private void Awake()
    {
        UIText = GetComponent<TextMeshProUGUI>();
        ObjectID = ObjectPrefab.GetComponent<solarscript>().ID; // Going to look for the object script, and then for the 'ID' component

    }

    private void LateUpdate()
    {
        int collected = PlayerPrefs.GetInt(ObjectID, 0);
        UIText.text = $"{collected}/{totalobjects}";
        //UIText.text = PlayerPrefs.GetInt(ObjectID).ToString();
    }// playerprefs is a good way easily access the public ID 
}

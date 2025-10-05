using UnityEngine;
using TMPro;
using System.Collections;


public class Level1Menutext : MonoBehaviour

{
    public TextMeshProUGUI levelText;
    public float displayDuration = 8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (levelText != null)
        {
            StartCoroutine(HideTextAfterDelay(displayDuration));
        }

    }

    //coroutine to handle the time delay
    IEnumerator HideTextAfterDelay(float delay)
    {
        // wait for 8 seconds
        yield return new WaitForSeconds(delay);

        //Turn off the text object
        levelText.gameObject.SetActive(false);
    }
}

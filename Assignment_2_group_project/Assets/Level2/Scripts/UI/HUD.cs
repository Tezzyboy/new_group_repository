using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD I;
    [SerializeField] private TextMeshProUGUI partsText;

    void Awake() => I = this;

    public void SetParts(int cur, int req)
    {
        if (partsText) partsText.text = $"{cur}/{req}";
    }
}
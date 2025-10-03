using UnityEngine;
using System.Collections;

public class TurbineAssembler : MonoBehaviour
{
    [Header("PartRender")]
    public SpriteRenderer mastRenderer;
    public SpriteRenderer nacelleRenderer;
    public SpriteRenderer bladeRenderer;

    [Header("IncompleteAlpha")]
    [Range(0f,1f)] public float incompleteAlpha = 0.35f;

    [Header("Windzone & Rotor")]
    public GameObject windZone;   
    public Spin rotorSpin;   
    public float targetRPM = 120f;
    public float rampTime = 1.2f; 

    // === 新增：能量点位刷新器 ===
    [Header("Energy Spawner")]
    public EnergySpawner energySpawner;   // 在 Inspector 里把 EnergySpawner 拖进来
    public bool startSpawnerOnBuilt = true; // 组装完成后自动开刷

    bool mastDone, nacelleDone, bladeDone;

    void Start()
    {
        SetAlpha(mastRenderer,    incompleteAlpha);
        SetAlpha(nacelleRenderer, incompleteAlpha);
        SetAlpha(bladeRenderer,   incompleteAlpha);

        if (windZone) windZone.SetActive(false);
        if (rotorSpin) { rotorSpin.enabled = false; rotorSpin.rpm = 0f; }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = PartInventory.I;
        if (inv == null) return;

        if (!mastDone    && inv.TryUse(PartType.Mast))    { mastDone    = true; SetAlpha(mastRenderer,    1f); }
        if (!nacelleDone && inv.TryUse(PartType.Nacelle)) { nacelleDone = true; SetAlpha(nacelleRenderer, 1f); }
        if (!bladeDone   && inv.TryUse(PartType.Blade))   { bladeDone   = true; SetAlpha(bladeRenderer,   1f); }

        if (mastDone && nacelleDone && bladeDone)
        {
            // 风区开启
            if (windZone && !windZone.activeSelf) windZone.SetActive(true);

            // 叶片平滑加速
            if (rotorSpin && !rotorSpin.enabled)  StartCoroutine(RampUpSpin());

            // === 新增：启动能量点位刷新 ===
            if (startSpawnerOnBuilt && energySpawner)
                energySpawner.Begin();

            // （可选）如果你把开刷逻辑放在 GameManager 里，也可以通知它：
            // GameManager.I?.OnTurbineBuilt();

            enabled = false;  // 完成后不再重复判定
        }
    }

    IEnumerator RampUpSpin()
    {
        rotorSpin.enabled = true;
        float t = 0f;
        float start = rotorSpin.rpm;
        while (t < rampTime)
        {
            t += Time.deltaTime;
            rotorSpin.rpm = Mathf.Lerp(start, targetRPM, t / rampTime);
            yield return null;
        }
        rotorSpin.rpm = targetRPM;
    }

    static void SetAlpha(SpriteRenderer r, float a)
    {
        if (!r) return;
        var c = r.color; c.a = a; r.color = c;
    }
}
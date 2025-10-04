using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("References")]
    public Transform player;
    public Transform defaultSpawn;

    [Header("Energy / CO2")]
    public int   energy        = 0;     // 当前能量
    public int   targetEnergy  = 100;   // 目标能量（进度条上限/通关阈值）
    public float co2           = 100f;  // 初始 CO2
    public float co2PerEnergy  = 1f;    // 每 +1 能量，CO2 减多少。要“+5能量 -5CO2”就设 1

    [Header("Spawner (optional)")]
    public EnergySpawner energySpawner;
    public bool startSpawnerOnTurbineBuilt = true;

    Vector3 _respawn;

    void Awake()
    {
        I = this;
        _respawn = defaultSpawn ? defaultSpawn.position : Vector3.zero;
    }

    void Start()
    {
        // 定位玩家
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        if (player)
        {
            player.position = _respawn;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb) { rb.linearVelocity = Vector2.zero; rb.angularVelocity = 0f; }
        }

        // 初始化 HUD 数值（避免显示 New Text）
        HUD.I?.SetEnergy(energy, targetEnergy);  // 比如 "Energy 0/100"
        HUD.I?.SetCO2(co2);                      // 比如 "CO2 100"
        // 碎片的 "Parts x/y" 由 PartInventory 在 Awake/Start 或 Add() 时调用 HUD.I.SetParts()
    }

    public void SetCheckpoint(Vector3 pos) => _respawn = pos;

    public void Respawn(Transform who)
    {
        who.position = _respawn;
        var rb = who.GetComponent<Rigidbody2D>();
        if (rb) { rb.linearVelocity = Vector2.zero; rb.angularVelocity = 0f; }
    }

    
    public void AddEnergy(int amount)
    {
        if (amount <= 0) return;

        energy = Mathf.Clamp(energy + amount, 0, targetEnergy);
        co2 = Mathf.Max(0, co2 - amount);

        HUD.I?.SetEnergy(energy, targetEnergy);
        HUD.I?.SetCO2(co2);

        if (energy >= targetEnergy)
            OnEnergyGoalReached();
    }

    public void ResetEnergyAndCO2(int energyValue = 0, float co2Value = 100f)
    {
        energy = Mathf.Max(0, energyValue);
        co2    = Mathf.Clamp(co2Value, 0f, 100f);
        HUD.I?.SetEnergy(energy, targetEnergy);
        HUD.I?.SetCO2(co2);
    }

    public void OnTurbineBuilt()
    {
        if (startSpawnerOnTurbineBuilt && energySpawner)
            energySpawner.Begin();
    }

    void OnEnergyGoalReached()
    {
        Debug.Log("[GM] Energy goal reached! 关卡完成。");
        // TODO: 在这里触发通关流程
    }
}
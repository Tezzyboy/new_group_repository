using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("References")]
    public Transform player;        
    public Transform defaultSpawn;  

  
    [Header("Energy / CO₂")]
    public int energy = 0;
    public int targetEnergy = 20;     // 达标阈值（通关）
    public float co2 = 100f;          // 初始CO₂
    public float co2PerEnergy = 2f;   // 每1能量降低多少CO₂

    [Header("HUD (可选, 可留空)")]
    public TMP_Text energyText;       // 显示 "Energy x/target"
    public TMP_Text co2Text;          // 显示 "CO₂ y"

    [Header("Spawner 控制(可选)")]
    public EnergySpawner energySpawner;  // 指向你的点位刷新器
    public bool startSpawnerOnTurbineBuilt = true; // 风车完成后是否自动开刷

    // ====== 原有 ======
    Vector3 _respawn;

    void Awake(){
        I = this;
        _respawn = defaultSpawn ? defaultSpawn.position : Vector3.zero;
    }

    void Start()  
    {
        if (!player) {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        if (player && defaultSpawn){
            player.position = defaultSpawn.position;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb){ rb.linearVelocity = Vector2.zero; rb.angularVelocity = 0f; }
        }

        UpdateEnergyHUD();
    }


    public void SetCheckpoint(Vector3 pos){ _respawn = pos; }

    public void Respawn(Transform who){
        who.position = _respawn;
        var rb = who.GetComponent<Rigidbody2D>();
        if (rb){ rb.linearVelocity = Vector2.zero; rb.angularVelocity = 0f; }
    }

    
    public void AddEnergy(int amount)
    {
        if (amount <= 0) return;
        energy += amount;
        co2 = Mathf.Max(0f, co2 - amount * co2PerEnergy);
        UpdateEnergyHUD();

        if (energy >= targetEnergy)
        {
            OnEnergyGoalReached();
        }
    }

    public void ResetEnergyAndCO2(int energyValue = 0, float co2Value = -1f)
    {
        energy = Mathf.Max(0, energyValue);
        co2 = (co2Value < 0f) ? 100f : co2Value;
        UpdateEnergyHUD();
    }

    void UpdateEnergyHUD()
    {
        if (energyText) energyText.text = $"Energy {energy}/{targetEnergy}";
        if (co2Text)    co2Text.text    = $"CO₂ {Mathf.RoundToInt(co2)}";
    }

    void OnEnergyGoalReached()
    {
        Debug.Log("[GM] Energy goal reached! 关卡完成。");
        // TODO: 在这里做通关/开门/加载下个关卡等
        // e.g. SceneManager.LoadScene("LevelSelect");
    }

   
    public void OnTurbineBuilt()
    {
        if (startSpawnerOnTurbineBuilt && energySpawner)
            energySpawner.Begin();
    }
}
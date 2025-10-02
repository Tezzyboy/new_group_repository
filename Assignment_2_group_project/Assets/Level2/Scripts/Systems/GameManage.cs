using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("References")]
    public Transform player;        
    public Transform defaultSpawn;  

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
            // Debug.Log($"Spawned at {player.position}");
        }
    }

    public void SetCheckpoint(Vector3 pos){ _respawn = pos; }

    public void Respawn(Transform who){
        who.position = _respawn;
        var rb = who.GetComponent<Rigidbody2D>();
        if (rb){ rb.linearVelocity = Vector2.zero; rb.angularVelocity = 0f; }
    }
}
using UnityEngine;

public class collectsolar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Object solobject;

    private void Awake()
    {   //Store a reference to solarscript 
        solobject = GetComponent<solarscript>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Newplayer"))
        {
            Destroy(gameObject);
        }
    }

}

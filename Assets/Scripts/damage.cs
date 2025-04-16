using UnityEngine;

public class damage : MonoBehaviour
{
    public playerHealth pHealth;
    public float Damage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pHealth.health -= Damage;
        }
    }


}

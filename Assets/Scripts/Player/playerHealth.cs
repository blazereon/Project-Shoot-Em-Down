using UnityEngine;
using UnityEngine.UI;


public class playerHealth : MonoBehaviour
{
    public GameObject player;
    public float health;
    public float maxHealth;
    public Image healthBar;
    public Transform respawnPoint;
    public bool respawned = false;
    

    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);



        if(health <= 0)
        {
            player.transform.position = respawnPoint.position;
            respawned = true;
        }

        if(respawned == true)
        {
            health = 10;
            respawned = false;
        }
    }
}

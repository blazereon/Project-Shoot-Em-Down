using UnityEngine;

public class damage : MonoBehaviour
{
    public Player pHealth;
    public int Damage = 1;

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
            EventSystem.Current.AttackPlayer(Damage);
        }
    }


}

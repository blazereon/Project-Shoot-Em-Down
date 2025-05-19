using Unity.VisualScripting;
using UnityEngine;

public class DashAttackBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Player dash attack invoked");
            EventSystem.Current.AttackEnemy(collision.gameObject, DamageType.Melee, 20, 0, false);
        }
    }
}

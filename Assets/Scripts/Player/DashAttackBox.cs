using Unity.VisualScripting;
using UnityEngine;

public class DashAttackBox : MonoBehaviour
{
    public Player player;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Player dash attack invoked");
            EventSystem.Current.AttackEnemy(collision.gameObject, DamageType.Melee, 20, 0, false);

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (player.DashAbility.UpgradeTier >= 3)
            {
                EventSystem.Current.ApplyEffect(collision.gameObject, new FragileMark(enemy, 5f));
            }
        }
    }
}

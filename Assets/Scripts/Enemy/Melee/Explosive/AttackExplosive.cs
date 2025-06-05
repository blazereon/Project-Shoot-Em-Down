using System.Collections;
using UnityEngine;

public class AttackExplosive : BaseExplosive
{
    public override void EnterState(ManagerExplosive enemy)
    {
        enemy.StartCoroutine(StartSelfDestruct(enemy));
    }

    public override void UpdateState(ManagerExplosive enemy)
    {
        // No exit since we want the countdown to be unstoppable no matter what
        // switch to stun
        /*
        if (enemy.IsStunned)
        {
            enemy.prevState = this;
            enemy.SwitchState(enemy.stunState);
        }*/
    }

    public override void FixedUpdateState(ManagerExplosive enemy)
    {
        
    }

    IEnumerator StartSelfDestruct(ManagerExplosive enemy)
    {
        Debug.Log("START SELF DESTRUCT SEQUENCE");
        yield return new WaitForSeconds(enemy.explosionTimer);
        Debug.Log("SELF DESTRUCT INITIATED");

        Vector2 _directionToTarget = EventSystem.Current.PlayerLocation - (Vector2)enemy.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, _directionToTarget, enemy.explosionRadius, LayerMask.GetMask("Wall", "Ground", "Player"));

        if (hit.collider == null)
        {
            // do nothing
        }
        else if (hit.collider.tag == "Player")
        {
            EventSystem.Current.AttackPlayer(enemy.AttackDamage);
        }

        enemy.TakeDamage(enemy.gameObject, DamageType.Melee, 9999, 0, false);
    }
}

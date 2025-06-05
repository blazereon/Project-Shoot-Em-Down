using System.Collections;
using UnityEngine;

public class AttackShieldMelee : BaseShieldMelee
{
    public bool _canAttack = true;
    private float _attackDistance = 1.3f;
    public override void EnterState(ManagerShieldMelee enemy)
    {
        _canAttack = true;
    }

    public override void UpdateState(ManagerShieldMelee enemy)
    {
        if (Vector2.Distance(enemy.transform.position, EventSystem.Current.PlayerLocation) > _attackDistance)
        {
            enemy.StopAllCoroutines();
            enemy.SwitchState(enemy.chaseState);
        }
        if (_canAttack == true)
        {
            enemy.StartCoroutine(HandleMultiHit(enemy));
            _canAttack = false;
            enemy.StartCoroutine(HandleAttackCooldown());
        }

        // switch to stun
        if (enemy.IsStunned)
        {
            enemy.prevState = this;
            enemy.SwitchState(enemy.stunState);
        }
    }

    public override void FixedUpdateState(ManagerShieldMelee enemy)
    {
        enemy.enemyRb.linearVelocityX = 0;
    }

    IEnumerator HandleAttackCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        _canAttack = true;
    }

    IEnumerator HandleMultiHit(ManagerShieldMelee enemy)
    {
        for (int i = 0; i < 2; i++)
        {
            EventSystem.Current.AttackPlayer(enemy.AttackDamage);
            yield return new WaitForSeconds(0.5F);
        }
    }
}

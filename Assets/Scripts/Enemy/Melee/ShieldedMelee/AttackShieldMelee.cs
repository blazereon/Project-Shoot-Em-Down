using System.Collections;
using UnityEngine;

public class AttackShieldMelee : BaseShieldMelee
{
    public bool _canAttack = true;
    private float _attackDistance = 1.3f;
    public override void EnterState(ManagerShieldMelee genericEnemy)
    {
        _canAttack = true;
    }

    public override void UpdateState(ManagerShieldMelee genericEnemy)
    {
        if (Vector2.Distance(genericEnemy.transform.position, EventSystem.Current.PlayerLocation) > _attackDistance)
        {
            genericEnemy.StopAllCoroutines();
            genericEnemy.SwitchState(genericEnemy.chaseState);
        }
        if (_canAttack == true)
        {
            genericEnemy.StartCoroutine(HandleMultiHit(genericEnemy));
            _canAttack = false;
            genericEnemy.StartCoroutine(HandleAttackCooldown());
        }
    }

    public override void FixedUpdateState(ManagerShieldMelee genericEnemy)
    {
        genericEnemy.enemyRb.linearVelocityX = 0;
    }

    IEnumerator HandleAttackCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        _canAttack = true;
    }

    IEnumerator HandleMultiHit(ManagerShieldMelee genericEnemy)
    {
        for (int i = 0; i < 2; i++)
        {
            EventSystem.Current.AttackPlayer(genericEnemy.AttackDamage);
            yield return new WaitForSeconds(0.5F);
        }
    }
}

using System.Collections;
using UnityEngine;
public class AttackGenericEnemy: BaseGenericEnemy
{
    public bool _canAttack = true;
    private float _attackDistance = 1.3f;
    public override void EnterState(ManagerGenericEnemy genericEnemy)
    {
        _canAttack = true;
    }

    public override void UpdateState(ManagerGenericEnemy genericEnemy)
    {
        if (Vector2.Distance(genericEnemy.transform.position, EventSystem.Current.PlayerLocation) > _attackDistance)
        {
            genericEnemy.StopAllCoroutines();
            genericEnemy.SwitchState(genericEnemy.chaseState);
        }
        if (_canAttack == true){
            EventSystem.Current.AttackPlayer(1);
            Debug.Log("Enemy Attack Invoked on Player");
            _canAttack = false;
            genericEnemy.StartCoroutine(HandleAttackCooldown());
        }
    }

    public override void FixedUpdateState(ManagerGenericEnemy genericEnemy)
    {
        genericEnemy.GenericEnemyRb.linearVelocityX = 0;
    }

    IEnumerator HandleAttackCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        _canAttack = true;
    }
}
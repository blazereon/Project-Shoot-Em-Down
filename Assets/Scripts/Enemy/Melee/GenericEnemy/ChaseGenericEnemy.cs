using System.Collections;
using UnityEngine;

public class ChaseGenericEnemy : BaseGenericEnemy
{
    bool isPlayerInSight = false;

    private int _rayNumber = 20;
    private float _chaseDistance = 5f;

    private LayerMask _layerMask;
    public override void EnterState(ManagerGenericEnemy genericEnemy)
    {
        _layerMask = LayerMask.GetMask("Wall", "Player");
        isPlayerInSight = true;
        genericEnemy.StartCoroutine(CheckPlayer());
    }

    public override void UpdateState(ManagerGenericEnemy genericEnemy)
    {
        if (!isPlayerInSight){
            genericEnemy.SwitchState(genericEnemy.wanderState);
        }

        for (int i = 0; i < _rayNumber; i++)
        {
            Vector3 _rayDirection = Quaternion.Euler(0, 0, 0 + (i * (120 / _rayNumber))) * Vector2.right;
            RaycastHit2D _hit = Physics2D.Raycast(genericEnemy.transform.position, _rayDirection, _chaseDistance, _layerMask);
            Debug.DrawRay(genericEnemy.transform.position, _rayDirection * _chaseDistance);

            if (_hit.collider == null) return;

            if (_hit.collider.tag == "Player")
            {
                isPlayerInSight = true;
            }
        }

    }

    public override void FixedUpdateState(ManagerGenericEnemy genericEnemy)
    {
        if (genericEnemy.facing == Enemy.EnemyFacing.Left)
        {
            genericEnemy.GenericEnemyRb.linearVelocityX = Vector2.left.x * genericEnemy.ChasingSpeed * Time.fixedDeltaTime;
        }
        else if (genericEnemy.facing == Enemy.EnemyFacing.Right)
        {
            genericEnemy.GenericEnemyRb.linearVelocityX = Vector2.right.x * genericEnemy.ChasingSpeed * Time.fixedDeltaTime;
        }
    }

    IEnumerator CheckPlayer()
    {
        while(isPlayerInSight)
        {
            isPlayerInSight = false;
            yield return new WaitForSeconds(0.5f);
        }

        isPlayerInSight = false;
    }


}
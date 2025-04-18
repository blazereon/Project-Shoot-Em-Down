using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ChaseGenericEnemy : BaseGenericEnemy
{
    private bool _isPlayerInSight = false;
    private bool _isChaseMode = false;

    private int _rayNumber = 200;
    private int _rayMaxAngle = 180;
    private float _chaseDistance = 5f;
    private float _attackDistance = 1.3f;

    private LayerMask _layerMask;
    public override void EnterState(ManagerGenericEnemy genericEnemy)
    {
        _layerMask = LayerMask.GetMask("Wall", "Player");
        _isPlayerInSight = true;
        _isChaseMode = true;
        genericEnemy.StartCoroutine(CheckPlayer(genericEnemy));
    }

    public override void UpdateState(ManagerGenericEnemy genericEnemy)
    {

        if (!_isChaseMode){
            Debug.Log("End of chase mode");
            genericEnemy.SwitchState(genericEnemy.wanderState);
        }

        _isPlayerInSight = CheckIfPlayerInSight(genericEnemy);

        Debug.Log("Enemy-Player Distance: " + Vector2.Distance(genericEnemy.transform.position, EventSystem.Current.PlayerLocation));

        if (_isPlayerInSight && (Vector2.Distance(genericEnemy.transform.position, EventSystem.Current.PlayerLocation) <= _attackDistance))
        {
            genericEnemy.SwitchState(genericEnemy.attackState);    
        }

        Debug.Log("Player In Sight: " +  _isPlayerInSight);

        
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

    public bool CheckIfPlayerInSight(ManagerGenericEnemy genericEnemy)
    {

        RaycastHit2D hitLeft = Physics2D.Raycast(genericEnemy.transform.position, Vector2.left, _chaseDistance, _layerMask);
        if (hitLeft.collider != null)
        {
            Debug.DrawRay(genericEnemy.transform.position, Vector2.left * _chaseDistance);
            if (hitLeft.collider.tag == "Player") return true;
        }

        RaycastHit2D hitRight = Physics2D.Raycast(genericEnemy.transform.position, Vector2.right, _chaseDistance, _layerMask);
        if (hitRight.collider != null)
        {
            Debug.DrawRay(genericEnemy.transform.position, Vector2.right * _chaseDistance);
            if (hitRight.collider.tag == "Player") return true;
        }

        return false;
    }

    IEnumerator CheckPlayer(ManagerGenericEnemy genericEnemy)
    {
        while(_isChaseMode)
        {
            yield return new WaitForSeconds(1.5f);
            if (!_isPlayerInSight)
            {
                _isChaseMode = false;
                break;
            }
            var _ploc = EventSystem.Current.PlayerLocation;
            if (_ploc.x > genericEnemy.transform.position.x)
            {
                genericEnemy.facing = Enemy.EnemyFacing.Right;
            }
            else if (_ploc.x < genericEnemy.transform.position.x)
            {
                genericEnemy.facing = Enemy.EnemyFacing.Left;
            }
            _isChaseMode = true;
        }
    }


}
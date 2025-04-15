using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.Tilemaps;
using UnityEngine;


public enum Facing
{
    left,
    right
}

public enum EnemyState
{
    Wandering,
    Chasing,

    Attacking
}
public class GenericAhhEnemy : Enemy
{

    public Rigidbody2D Rb;
    public int RayNumber = 4;
    public float DetectionRange = 1f;
    public float DetectionAngle = 90f;

    public float WallDistanceLimit = 4;
    public float RayMaxAngle = 120f;

    public float wanderSpeed;
    public float ChasingSpeed;

    private LayerMask _layerMask;
    private Facing _facing;
    private EnemyState _enemyState;

    private float _wanderSpeedInstance;
    private float _chasingSpeedInstance;

    private float _detectRangeInstance;
    

    void Start()
    {
        _layerMask = LayerMask.GetMask("Wall", "Player");
        _detectRangeInstance = DetectionRange;
        _wanderSpeedInstance = wanderSpeed;
        _chasingSpeedInstance = ChasingSpeed;
    }
    

    void Update()
    {  
        switch(_enemyState)
        {
            case EnemyState.Wandering:
                Wander();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
        }

    }

    void FixedUpdate()
    {
        if (_enemyState == EnemyState.Wandering)
        {
             Rb.linearVelocityX = _wanderSpeedInstance * Time.fixedDeltaTime;
        }
        else if (_enemyState == EnemyState.Chasing)
        {
            Rb.linearVelocityX = _chasingSpeedInstance * Time.fixedDeltaTime;
        }
        else if (_enemyState == EnemyState.Attacking)
        {
            Rb.linearVelocityX = 0;
        }
    }

    void Wander()
    {
        for (int i = 0; i < RayNumber; i++){
            RaycastHit2D hit = Physics2D.Raycast(transform.position,  Quaternion.Euler(0,0, DetectionAngle + (i * (RayMaxAngle / RayNumber))) * Vector2.right, _detectRangeInstance, _layerMask);
    
            Debug.DrawRay(transform.position, Quaternion.Euler(0,0, DetectionAngle - (i * (RayMaxAngle / RayNumber))) * Vector2.right * _detectRangeInstance, Color.red);

            if (hit.collider == null)
            {
                continue;
            }

            if (hit.collider.tag == "Wall")
            {
                if (hit.distance <= WallDistanceLimit)
                {
                    Debug.Log("wall distance limit reached");
                    //flip enemy
                    Flip();
                    return;
                }
            }

            if( hit.collider.tag == "Player"){
                Debug.DrawRay(transform.position, Quaternion.Euler(0,0, DetectionAngle - (i * (RayMaxAngle / RayNumber))) * Vector2.right * _detectRangeInstance, Color.green);
                Debug.Log("Player detected");
                
                //Trigger Chase State
                _enemyState = EnemyState.Chasing;
            }
        }
    }

    void Chase()
    {

    }

    void Flip()
    {
        _facing = _facing == Facing.left ? Facing.right : Facing.left;
        UpdateFlip();
    }

    void Flip(Facing face)
    {
        _facing = face;
        UpdateFlip();
    }

    void UpdateFlip()
    {
        if (_facing == Facing.right)
        {
            _detectRangeInstance = -DetectionRange;
            _wanderSpeedInstance = -_wanderSpeedInstance;
        } else if (_facing == Facing.left)
        {
            _detectRangeInstance = Math.Abs(DetectionRange);
            _wanderSpeedInstance = Math.Abs(_wanderSpeedInstance);
        }
    }
    
}
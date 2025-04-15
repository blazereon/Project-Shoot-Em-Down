using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 100;
    public int AttackDamage;

    [Space(10)]
    [Header("Player Detection Settings")]
    public int rayNumber = 36;
    public float rayMaxAngle = 360;
    public float detectionRange = 5;

    public void TakeDamage(GameObject pObject, int damage)
    {
        if (pObject == gameObject){
            Health -= damage;
            if (Health <= 0) Destroy(this.gameObject);
            Debug.Log("HP: " + Health);
        }
    }

    public void PlayerDetection()
    {
        bool _isPlayerDetected = false;

        for (int i = 0; i < rayNumber; i++)
        {
            Vector3 _rayDirection = Quaternion.Euler(0, 0, i * (rayMaxAngle / rayNumber)) * transform.right;

            // Cast a raycast only if no players are detected, could be more optimized this way
            if (!_isPlayerDetected)
            {
                RaycastHit2D _hit = Physics2D.Raycast(transform.position, _rayDirection, detectionRange);
                Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.green);

                if (_hit)
                {
                    // code
                    if (_hit.collider.name == "Player")
                    {
                        Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.red);

                        _isPlayerDetected = true;

                    }
                }
            }
        }

        if (_isPlayerDetected)
        {
            // Chasing logic here
        }
        else
        {
            // Wandering logic here
        }
    }

}
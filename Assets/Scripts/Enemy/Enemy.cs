using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health = 100;
    public int AttackDamage;

    [Space(10)]
    [Header("Player Detection Settings")]
    public int rayNumber = 36;
    public float rayMaxAngle = 360f;
    public float rayStartAngle = 0f;
    public float detectionRange = 5f;

    [HideInInspector]
    public bool isPlayerDetected;

    public void TakeDamage(GameObject pObject, int damage)
    {
        if (pObject == gameObject){
            Health -= damage;
            if (Health <= 0) Destroy(this.gameObject);
            Debug.Log("HP: " + Health);
        }
    }

    public bool PlayerDetection()
    {
        bool _isPlayerDetected = false;

        float _currentAngle = rayStartAngle;

        Debug.Log("SCANNING");

        for (int i = 0; i < rayNumber; i++)
        {
            Vector3 _rayDirection = Quaternion.Euler(0, 0, _currentAngle + (i * (rayMaxAngle / rayNumber))) * transform.right * transform.localScale.x;

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

                        Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.yellow);

                        _isPlayerDetected = true;
                        Debug.Log("HIT!");
                    }
                }
                else
                {
                    _isPlayerDetected = false;
                }
            }
        }

        return _isPlayerDetected;
    }

}
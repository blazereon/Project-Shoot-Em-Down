using System.Collections;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public int Health = 100;
    public int AttackDamage;
    public int PneumaAmount;

    [Space(10)]
    [Header("Player Detection Settings")]
    public int rayNumber = 36;
    public float rayMaxAngle = 360f;
    public float rayStartAngle = 0f;
    public float detectionRange = 5f;
    public float delayScanTime = 0.5f;

    [HideInInspector]
    public bool isPlayerDetected;
    [HideInInspector]
    public bool[] hitRays;
    [HideInInspector]
    public enum EnemyFacing
    {
        Left,
        Right
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        isPlayerDetected = false;
        
    }

    public void TakeDamage(GameObject pObject, int damage)
    {
        if (pObject == gameObject){
            Health -= damage;
            if (Health <= 0)
            {
                EventSystem.Current.SendPlayerPneuma(PneumaAmount);
                Destroy(this.gameObject);
            }
            Debug.Log("HP: " + Health);
        }
    }

    public bool PlayerDetection(Vector3 scale)
    {
        bool _isPlayerDetected = false;

        float _currentAngle = rayStartAngle;

        Debug.Log("SCANNING");

        for (int i = 0; i < rayNumber; i++)
        {
            Vector3 _rayDirection = Quaternion.Euler(0, 0, _currentAngle + (i * (rayMaxAngle / rayNumber))) * transform.right * scale.x;

            // Cast a raycast only if no players are detected, could be more optimized this way
            if (!_isPlayerDetected)
            {
                RaycastHit2D _hit = Physics2D.Raycast(transform.position, _rayDirection, detectionRange, LayerMask.GetMask("Wall", "Player"));
                Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.green);

                if (_hit)
                {
                    // code
                    if (_hit.collider.tag == "Player")
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

    public bool PlayerDetection(EnemyFacing faceDirection)
    {
        bool _isPlayerDetected = false;

        float _currentAngle = rayStartAngle;

        Debug.Log("SCANNING");

        for (int i = 0; i < rayNumber; i++)
        {
            Vector3 _rayDirection = Quaternion.Euler(0, 0, _currentAngle + (i * (rayMaxAngle / rayNumber))) * transform.right * (faceDirection == EnemyFacing.Left ? -1 : 1);

            // Cast a raycast only if no players are detected, could be more optimized this way
            if (!_isPlayerDetected)
            {
                RaycastHit2D _hit = Physics2D.Raycast(transform.position, _rayDirection, detectionRange, LayerMask.GetMask("Wall", "Player"));
                Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.green);

                if (_hit)
                {
                    // code
                    if (_hit.collider.tag == "Player")
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

    public IEnumerator LockOnPlayer()
    {
        Vector3 _lastPlayerPosition = player.transform.position;

        yield return new WaitForSeconds(delayScanTime);     // Delay the position to give chance for the player to get out of lock

        Vector2 _directionToPlayer = _lastPlayerPosition - transform.position;
        float _enemyToPlayerAngle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg;

        Debug.Log("player pos: " + _lastPlayerPosition + " " + player.transform.position);

        for (int i = 0; i < rayNumber; i++)
        {
            float angleOffset = (i - rayNumber / 2f) * (rayMaxAngle / rayNumber);
            float currentAngle = _enemyToPlayerAngle + angleOffset;

            Vector3 _rayDirection = Quaternion.Euler(0, 0, currentAngle) * transform.right * transform.localScale.x;
            RaycastHit2D _hit = Physics2D.Raycast(transform.position, _rayDirection, detectionRange);

            Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.black);
            if (_hit)
            {
                // code
                if (_hit.collider.name == "Player")
                {
                    Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.red);

                    hitRays[i] = true;
                }
                else
                {
                    hitRays[i] = false;
                }
            }
            else
            {
                hitRays[i] = false;
            }
        }

        if (!hitRays.Contains(true))
        {
            isPlayerDetected = false;
        }

    }

}
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

// Basic Movement of a grounded-ranged enemy. Moves towards the player, and has maxEngagementRange and minMoveDistance
// Attacks inside the maxEngagementRange and stops moving if the distance between player is greater than maxChaseDistance
// This means, this enemy attacks while moving
public class RangedGrounded : Enemy {

    [Space(10)]
    [Header("Enemy Unique Settings")]
    public GameObject player;

    public float speed;
    public float maxEngagementRange;    // attack inside this range
    public float stopChaseDistance;     // stop moving inside this range
    
    public float wanderInterval = 0.5f;

    [HideInInspector]
    public float playerDistance;
    [HideInInspector]
    public Vector2 scale;
    [HideInInspector]
    public int direction;

    void Awake()
    {
        EventSystem.Current.OnAttackEnemy += TakeDamage;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        scale = transform.localScale;

        direction = Random.value > 0.5f ? 1 : -1;

        // isPlayerDetected = false;
    }

    private void Update()
    {
        
        if(!isPlayerDetected)
        {
            isPlayerDetected = ScanPlayer();
        }


        if (isPlayerDetected)
        {
            ChasePlayer();
            LockScanning();
        }
        else
        {
            // Wander();
        }

    }

    public bool ScanPlayer()
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

    public void LockScanning()
    {
        Debug.Log("LOCKED");
        Vector2 _directionToPlayer = player.transform.position - transform.position;
        float _enemyToPlayerAngle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < rayNumber; i++)
        {
            float angleOffset = (i - rayNumber / 2f) * (rayMaxAngle / rayNumber);
            float currentAngle = _enemyToPlayerAngle + angleOffset;

            Vector3 _rayDirection = Quaternion.Euler(0, 0, currentAngle) * transform.right * transform.localScale.x;
            RaycastHit2D _hit = Physics2D.Raycast(transform.position, _rayDirection, detectionRange);

            Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.black);
            Debug.Log(_rayDirection);
            if (_hit)
            {
                // code
                if (_hit.collider.name == "Player")
                {

                    Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.red);

                }
            }
        }
        
    }

    public void ChasePlayer()
    {

        playerDistance = Vector2.Distance(player.transform.position, transform.position);

        // Face and move towards the player
        if (player.transform.position.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
            
            // Check if the enemy can engage the player
            if (maxEngagementRange >= playerDistance)
            {
                // Attack Mechanic here
                // Debug.Log("Go ATTACK!");
            }

            // Check if the enemy would move towards the player
            if (stopChaseDistance <= playerDistance)
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
            
            if (maxEngagementRange >= playerDistance)
            {
                // Attack Mechanic here
                // Debug.Log("Go ATTACK!");
            }

            if (stopChaseDistance <= playerDistance)
            {
                transform.Translate(speed * Time.deltaTime * -1, 0, 0);
            }

            transform.localScale = scale;
        }

        if (playerDistance >= detectionRange)
        {
            isPlayerDetected = false;
            return;
        }
    }

    public void Wander()
    {
        float _timer = 0;
        
        _timer += Time.deltaTime * 100;
        // Debug.Log("is wandering " + _timer);

        if (_timer >= wanderInterval)
        {
            _timer = 0f;

            direction = Random.value > 0.5f ? 1 : -1;
            // Debug.Log("Direction chosen: " + direction);
        }

        scale.x = Mathf.Abs(scale.x) * direction;
        transform.Translate(speed * Time.deltaTime * direction, 0, 0);
        transform.localScale = scale;
    }

    void OnDestroy()
    {
        EventSystem.Current.OnAttackEnemy -= TakeDamage;
    }
}
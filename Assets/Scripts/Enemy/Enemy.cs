using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : Entity
{

    public GameObject player;

    public EnemyStatUI EnemyStatUIInstance;
    public int Health = 100;
    public int MaxHealth = 100;
    public int MeleeResistancePercentage;
    public int RangeResistancePercentage;
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
    public bool hasPlayerDetected = false;
    [HideInInspector]
    public bool[] hitRays;
    [HideInInspector]
    public enum EnemyFacing
    {
        Left,
        Right
    }

    public bool isFiringBurst { get; set; } = false;
    public bool isWeakSpotActive { get; set; } = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        isPlayerDetected = false;

    }

    // public void TakeDamageEffect(GameObject pObject, DamageType type, int damage, int violencePercentage, bool weakSpotHit)
    // {
    //     TakeDamage(pObject, type,damage,violencePercentage,weakSpotHit);
    // }
    public void TakeDamage(GameObject pObject, DamageType type, int damage, int violencePercentage, bool weakSpotHit)
    {
        Debug.Log("New damage system invoked " + pObject + " " + type + " " + damage + " " + violencePercentage + " " + weakSpotHit);

        if (!weakSpotHit)
        {
            AudioManager.instance.RandomSFX(AudioManager.instance.enemyTakeDmg);
        }
        
        if (pObject != this.gameObject) return;
        float _rawDamage = 0;
        float _rawViolence;
        float _rawResPercent;

        int _pneuma, _aggression;

        switch (type)
        {
            case DamageType.Melee:
                _rawResPercent = (float)MeleeResistancePercentage / 100;
                _rawViolence = _rawResPercent * ((float)violencePercentage / 100);
                Debug.Log("Raw Violence: " + _rawViolence);
                _rawDamage = damage - (damage * (_rawResPercent - _rawViolence));
                Debug.Log("Raw damage: " + _rawDamage);

                //Calculate amount of pneuma and aggression based on damage dealt
                _pneuma = (int)(Mathf.Min(Health, _rawDamage) * 0.20f);
                _aggression = (int)(Mathf.Min(Health, _rawDamage) * 0.10f);

                Health -= (int)_rawDamage;
                UpdateUIData();
                break;

            case DamageType.Range:
                _rawResPercent = (float)RangeResistancePercentage / 100;
                _rawViolence = _rawResPercent * ((float)violencePercentage / 100);
                Debug.Log("Raw Violence: " + _rawViolence);
                _rawDamage = damage - (damage * (_rawResPercent - _rawViolence));
                Debug.Log("Raw damage: " + _rawDamage);
                Health -= (int)_rawDamage;

                //Calculate amount of pneuma and aggression based on damage dealt
                _pneuma = (int)(Mathf.Min(Health, _rawDamage) * 0.20f);
                _aggression = (int)(Mathf.Min(Health, _rawDamage) * 0.10f);

                Debug.Log("PN Range Hurt " + isWeakSpotActive + weakSpotHit);
                if (isWeakSpotActive && weakSpotHit)
                {
                    Debug.Log("Weak Spot hit, sending orbs!");

                    OrbManager.Current.GetOrb(OrbType.Pneuma, 5, transform.position);
                    OrbManager.Current.GetOrb(OrbType.Aggression, 5, transform.position);
                }
                UpdateUIData();
                break;

            case DamageType.Suicide:
                _pneuma = 0;
                _aggression = 0;
                Health -= (int)_rawDamage + 9999;
                break;

            default:
                Debug.Log("Invalid Damage Type");
                return;
        }

        if (_pneuma > 0 || _aggression > 0)
        {
            OrbManager.Current.GetOrb(OrbType.Pneuma, _pneuma, transform.position);
            OrbManager.Current.GetOrb(OrbType.Aggression, _aggression, transform.position);
        }
        

        if (Health <= 0)
        {
            AudioManager.instance.PlayFX(AudioManager.instance.enemyDeath, false);
            EventSystem.Current.EnemyKill();
            Destroy(this.gameObject);
        }

        //if the damage is not a consumption of mark, all marks will be consumed preventing stackoverflow error
        if (!isConsumingMark) ConsumeMark();

    }

    public void TakeDamageOld(GameObject pObject, int damage)
    {
        if (pObject == gameObject)
        {
            AudioManager.instance.RandomSFX(AudioManager.instance.enemyTakeDmg);
            Health -= damage;
            if (Health <= 0)
            {
                AudioManager.instance.RandomSFX(AudioManager.instance.enemyDeath);
                EventSystem.Current.EnemyKill();
                Destroy(this.gameObject);
            }
            Debug.Log("HP: " + Health);
        }
    }

    public bool PlayerDetection(Vector3 scale)
    {
        bool _isPlayerDetected = hasPlayerDetected;
        float _currentAngle = rayStartAngle;

        Debug.Log("SCANNING");

        if (!_isPlayerDetected)
        {
            for (int i = 0; i < rayNumber; i++)
            {
                Vector3 _rayDirection = Quaternion.Euler(0, 0, (_currentAngle - (rayMaxAngle / 2)) + (i * (rayMaxAngle / (rayNumber - 1)))) * (transform.localScale.x > 0 ? Vector3.right : Vector3.left);

                // Cast a raycast only if no players are detected, could be more optimized this way
                RaycastHit2D _hit = Physics2D.Raycast(transform.position, _rayDirection, detectionRange, LayerMask.GetMask("Wall", "Player"));
                Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.green);

                if (_hit)
                {
                    if (_hit.collider.tag == "Player")
                    {

                        Debug.DrawRay(transform.position, _rayDirection * detectionRange, Color.yellow);

                        _isPlayerDetected = true;
                        hasPlayerDetected = true;

                        break;
                    }
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

    public void InstantiateProjectile(int attackDmg, float projectileSpd, Vector2 trajectory, GameObject projectile)
    {
        AudioManager.instance.RandomSFX(AudioManager.instance.enemyAttackRanged);
        GameObject _spawnProjectile = GameObject.Instantiate(projectile, transform.position, transform.rotation);

        Projectile _projectileScript = _spawnProjectile.GetComponent<Projectile>();

        _projectileScript.ProjectileCurrentProperties.AttackDamage = attackDmg;
        _projectileScript.ProjectileCurrentProperties.ProjectileSpeed = projectileSpd;
        _projectileScript.ProjectileCurrentProperties.Trajectory = trajectory;

        _projectileScript.ProjectileCurrentProperties.DestroyOnly = LayerMask.GetMask("Ground", "Wall");
        _projectileScript.ProjectileCurrentProperties.FiredBy = ProjectileOwner.Enemy;
        _projectileScript.ProjectileCurrentProperties.Destination = LayerDestinations.Player;
    }

    public IEnumerator SingleFileBurst(bool startAttack, int attackDmg, float projectileSpd, int burstCount, float atkSpd, Vector3 playerPos, GameObject projectile)
    {
        isFiringBurst = true;

        if (EventSystem.Current.PlayerLocation == null || !startAttack)
        {
            isFiringBurst = false;
            yield break;
        }

        for (int i = 0; i < burstCount; i++)
        {
            if (EventSystem.Current.PlayerLocation == null || !startAttack)
            {
                isFiringBurst = false;
                yield break;
            }

            Vector2 _projectileTrajectory = (playerPos - transform.position).normalized;
            InstantiateProjectile(attackDmg, projectileSpd, _projectileTrajectory, projectile);

            yield return new WaitForSeconds(atkSpd);
        }

        isFiringBurst = false;
    }

    public IEnumerator TrackingBurst(bool startAttack, int attackDmg, float projectileSpd, int burstCount, float atkSpd, Vector3 enemyPos, GameObject projectile)
    {
        isFiringBurst = true;

        if (EventSystem.Current.PlayerLocation == null || !startAttack)
        {
            Debug.Log("STOPPING ATTACK");
            yield break;
        }

        for (int i = 0; i < burstCount; i++)
        {
            if (EventSystem.Current.PlayerLocation == null || !startAttack)
            {
                yield break;
            }

            Vector3 _playerPos = EventSystem.Current.PlayerLocation;
            Vector2 _projectileTrajectory = (_playerPos - enemyPos).normalized;
            InstantiateProjectile(attackDmg, projectileSpd, _projectileTrajectory, projectile);

            yield return new WaitForSeconds(atkSpd);
        }

        isFiringBurst = false;
    }

    public IEnumerator TwirlBurst(bool startAttack, int attackDmg, float projectileSpd, int burstCount, float startAngle, float burstStep, float atkSpd, Vector3 enemyTransform, GameObject projectile)
    {
        isFiringBurst = true;
        Vector2 _projectileTrajectory;

        for (int i = 0; i < burstCount; i++)
        {
            if (EventSystem.Current.PlayerLocation == null || !startAttack)
            {
                yield break;
            }

            _projectileTrajectory = (Quaternion.Euler(0, 0, startAngle + (i * burstStep)) * enemyTransform);
            // InstantiateProjectile(enemy, _projectileTrajectory);
            InstantiateProjectile(attackDmg, projectileSpd, _projectileTrajectory, projectile);

            yield return new WaitForSeconds(atkSpd);
        }

        isFiringBurst = false;
    }

    public override void UpdateUIData()
    {
        EnemyStatUIInstance.UpdateValues(new EnemyStatProps
        {
            Health = this.Health,
            MaxHealth = this.MaxHealth,
            Effects = this.CurrentEffect
        });
    }
}
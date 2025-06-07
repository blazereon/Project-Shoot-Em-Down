using System.Collections.Generic;
using UnityEngine;

public class HitDetect : MonoBehaviour
{
    public bool weakSpotActive = false;

    private bool _playerDetected = false;
    private bool _weakHitDetected = false;
    private bool _instaKill = false;
    private int _damage;
    private int _violence;
    private Enemy _enemy;
    private GameObject _parent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        _parent = this.transform.parent.gameObject;
        EventSystem.Current.OnApplyEffect += ReceiveEffect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool PlayerDetected()
    {
        return _playerDetected;
    }

    public bool WeakSpotHitDetected()
    {
        AudioManager.instance.RandomSFX(AudioManager.instance.enemyTakeDmgWeakSpot);
        // Since ReceiveEffect takes place first, if instakill is detected, then dont bother continuing
        if (!_instaKill)
        {
            bool _wasHit = _weakHitDetected;

            _weakHitDetected = false;

            if (_enemy != null)
            {
                _enemy.isWeakSpotActive = true;
            }

            // Forward the damage to parent whose subscribed to TakeDamage event
            _enemy?.TakeDamage(_parent, DamageType.Range, _damage, _violence, true);

            // Unnecessary for now but just in case we need it
            return _wasHit;
        }
        else
        {
            _enemy?.TakeDamage(_parent, DamageType.Range, 99999999, 100, true);
            return true;
        }
        
    }

    void ReceiveEffect(GameObject pObject, Effect effect)
    {
        if (pObject != this.gameObject) return;
        effect.EntityHolder = _enemy;
        

        if (weakSpotActive)
        {
            switch (effect)
            {
                case Instakill:
                    _instaKill = true;
                    return;
            }
        }
        effect.OnEffectStart();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collision on weakspot detected!");
        if (collider.tag == "Player")
        {
            _playerDetected = true;
        }
        if (collider.tag == "PlayerProjectile" && weakSpotActive)
        {
            // Access the projectile here
            Projectile _projectile = collider.gameObject.GetComponent<Projectile>();

            _weakHitDetected = true;

            _damage = _projectile.ProjectileCurrentProperties.AttackDamage;
            _violence = 0;

            WeakSpotHitDetected();
        }
        else if (collider.tag == "PlayerProjectile")
        {

        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("Exited");
        if (collider.tag == "Player")
        {
            _playerDetected = false;
        }
    }

    private void OnDestroy()
    {
        EventSystem.Current.OnApplyEffect -= ReceiveEffect;
    }
}

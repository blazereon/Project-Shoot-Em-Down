using UnityEngine;

public class HitDetect : MonoBehaviour
{
    public bool weakSpotActive = false;

    private bool _playerDetected = false;
    private bool _weakHitDetected = false;
    private int _damage;
    private int _violence;
    private Enemy _enemy;
    private GameObject _parent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        _parent = this.transform.parent.gameObject;
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

    void OnTriggerEnter2D(Collider2D collider)
    {
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
}

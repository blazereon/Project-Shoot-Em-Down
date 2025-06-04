using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NUnit.Framework;
using UnityEditor.PackageManager;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileProps ProjectileCurrentProperties;
    public Rigidbody2D rb;
    public List<Effect> EffectsList = new List<Effect>();
    private HashSet<int> objectHitID = new HashSet<int>();

    private void Awake()
    {
        EventSystem.Current.OnModifyProjectile += ModifyProjectile;
        EventSystem.Current.OnSimpleDeflectProjectile += SimpleDeflect;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Debug.Log("Instantiated!");
        // destroyOnly = new List<LayerMask>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 _playerVec3 = EventSystem.Current.PlayerLocation;
        Vector2 _bulletPrevPos = transform.position;
        float _distance = Vector2.Distance(EventSystem.Current.PlayerLocation, transform.position);

        if (_distance > 50)
        {
            Destroy(gameObject);
        }

        // Backup Collision via ray casting
        RaycastHit2D _ray = Physics2D.Raycast(transform.position, rb.linearVelocity.normalized, transform.localScale.x + 0.5f);

        // Debugging
        Vector2 rayEndPoint = (Vector2)transform.position + rb.linearVelocity.normalized * (transform.localScale.x + 0.5f);
        // Debug.DrawRay(transform.position, rayEndPoint - (Vector2)transform.position, Color.red);

        if (_ray.collider == null) return;
        if (_ray.collider)
        {
            int _objID = _ray.collider.GetInstanceID();
            if (!objectHitID.Contains(_objID))
            {
                objectHitID.Add(_objID);
                DetectHit(_ray.collider);
            }
            else
            {

            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = ProjectileCurrentProperties.Trajectory * ProjectileCurrentProperties.ProjectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        // DetectHit(collision);
    }


    private void DetectHit(Collider2D collision)
    {
        int _playerLayer = LayerMask.NameToLayer(LayerDestinations.Player.ToString());
        int _enemyLayer = LayerMask.NameToLayer(LayerDestinations.Enemy.ToString());
        int _nonCollideEnemy = LayerMask.NameToLayer("NonCollideEnemy");

        Debug.Log("Collided to layer: " + collision.gameObject.layer);

        Debug.Log("hit rec: " + collision.gameObject.layer + " " + ProjectileCurrentProperties.FiredBy + " " + _enemyLayer);

        if ((collision.gameObject.layer == _playerLayer && ProjectileCurrentProperties.FiredBy == ProjectileOwner.Enemy))
        {
            Debug.Log("Player hit!");
            EventSystem.Current.AttackPlayer(ProjectileCurrentProperties.AttackDamage);

            if (!ProjectileCurrentProperties.isPiercing)
            {
                Destroy(gameObject);
            }

        }
        else if ((collision.gameObject.layer == _enemyLayer || collision.gameObject.layer == _nonCollideEnemy) && ProjectileCurrentProperties.FiredBy == ProjectileOwner.Player)
        {
            var _enemyInstance = collision.gameObject.GetComponent<Entity>();
            foreach (Effect effect in EffectsList)
            {
                var _clonedEffect = effect.Clone();
                _clonedEffect.EntityHolder = _enemyInstance;
                EventSystem.Current.ApplyEffect(collision.gameObject, _clonedEffect);
            }
            // EventSystem.Current.AttackEnemy(collision.gameObject, ProjectileCurrentProperties.AttackDamage);
            if (collision.gameObject.tag == "WeakSpot")
            {
                Debug.Log("Enemy hit! On weak spot");
                EventSystem.Current.AttackEnemy(collision.gameObject, DamageType.Range, ProjectileCurrentProperties.AttackDamage, 0, true);
            }
            else
            {
                Debug.Log("Enemy hit! On Non weak spot");
                EventSystem.Current.AttackEnemy(collision.gameObject, DamageType.Range, ProjectileCurrentProperties.AttackDamage, 0, false);
            }

            if (!ProjectileCurrentProperties.isPiercing)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (((1 << collision.gameObject.layer) & ProjectileCurrentProperties.DestroyOnly.value) != 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ModifyProjectile(GameObject sentGameObject, ProjectileProps props)
    {
        //if the sent game object is not this game object itself. do not run
        if (sentGameObject != this.gameObject)
        {
            return;
        }
        //do not forget to handle sprite logic
        ProjectileCurrentProperties = props;
    }

    private void SimpleDeflect(GameObject sentGameObject, float speed)
    {
        
        //if the sent game object is not this game object itself. do not run
        if (sentGameObject != this.gameObject)
        {
            return;
        }
        Debug.Log("Deflect!!");
        //do not forget to handle sprite logic
        ProjectileCurrentProperties.FiredBy = ProjectileOwner.Player;
        ProjectileCurrentProperties.Destination = LayerDestinations.Enemy;
        ProjectileCurrentProperties.Trajectory.x *= -1;
        ProjectileCurrentProperties.ProjectileSpeed = speed;
    }

    private void OnDestroy()
    {
        EventSystem.Current.OnModifyProjectile -= ModifyProjectile;
        EventSystem.Current.OnSimpleDeflectProjectile -= SimpleDeflect;
    }
}

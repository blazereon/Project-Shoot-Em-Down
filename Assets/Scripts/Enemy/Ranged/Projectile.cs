using System.Collections.Generic;
using System.Runtime.Serialization;
using NUnit.Framework;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int atkDmg;
    public float speed;
    public Vector2 trajectory;
    public Rigidbody2D rb;

    public LayerMask destroyOnly;

    public enum projectileOwner
    {
        Player,
        Enemy
    }
    public projectileOwner firedBy;

    public enum layerDestinations
    {
        Player,
        Enemy
    }
    public layerDestinations destination;

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
        float _distance = Vector2.Distance(EventSystem.Current.PlayerLocation, transform.position);

        if (_distance > 50)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = trajectory * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int playerLayer = LayerMask.NameToLayer(layerDestinations.Player.ToString());
        int enemyLayer = LayerMask.NameToLayer(layerDestinations.Enemy.ToString());

        Debug.Log("hit rec: " + collision.gameObject.layer + " " + firedBy + " " + enemyLayer);

        if ((collision.gameObject.layer == playerLayer && firedBy == projectileOwner.Enemy))
        {
            Debug.Log("Player hit!");
            EventSystem.Current.AttackPlayer(atkDmg);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == enemyLayer && firedBy == projectileOwner.Player)
        {
            Debug.Log("Enemy hit!");
            EventSystem.Current.AttackEnemy(collision.gameObject, atkDmg);
            Destroy(gameObject);
        }
        else
        {
            if (((1 << collision.gameObject.layer) & destroyOnly.value) != 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

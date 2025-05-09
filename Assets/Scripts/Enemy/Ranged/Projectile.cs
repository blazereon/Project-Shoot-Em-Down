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

    public List<LayerMask> destroyOnly;
    public enum layerDestinations
    {
        Player,
        Enemy
    }
    public layerDestinations destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // destroyOnly = new List<LayerMask>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 _playerVec3 = EventSystem.Current.PlayerLocation;
        float _distance = Vector2.Distance(EventSystem.Current.PlayerLocation, transform.position);

        if (_distance > 10)
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

        if (collision.gameObject.layer == LayerMask.NameToLayer(layerDestinations.Player.ToString()))
        {
            EventSystem.Current.AttackPlayer(atkDmg);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer(layerDestinations.Enemy.ToString()))
        {
            EventSystem.Current.AttackEnemy(collision.gameObject, atkDmg);
            Destroy(gameObject);
        }

        foreach (LayerMask layerMask in destroyOnly)
        {
            Destroy(gameObject);
        }
    }
}

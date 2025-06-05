using System.Collections;
using UnityEngine;
public enum OrbType
{
    Aggression,
    Pneuma
}

public class Orb : MonoBehaviour
{
    public OrbManager orbManagerInstance;
    private float OrbMovementSpeed = 2f;
    private Rigidbody2D Rb;
    private CircleCollider2D _col;
    public OrbType Type;
    public int value;
    public float IdleDuration = 2f;
    private bool _isIdle = true;

    void Awake()
    {
        Rb = gameObject.GetComponent<Rigidbody2D>();
        _col = gameObject.GetComponent<CircleCollider2D>();
    }
    void Update()
    {
        if (_isIdle) return;
        Physics2D.IgnoreCollision(_col, EventSystem.Current.PlayerCollider, false);
        transform.position = Vector2.Lerp(transform.position, EventSystem.Current.PlayerLocation, OrbMovementSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        
    }

    public void OnInstantiateOrb(Vector2 position)
    {
        Physics2D.IgnoreCollision(_col, EventSystem.Current.PlayerCollider, true);
        transform.position = position;
        Rb.AddForce(new Vector2(Random.Range(-1f, 1f) * 5, Random.Range(-1f, 1f) * 5), ForceMode2D.Impulse);
        _isIdle = true;
        StartCoroutine(IdleCoroutine());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Send player values
            orbManagerInstance.ReturnOrb(gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Physics2D.IgnoreCollision(_col, collision.collider);
        }
    }

    IEnumerator IdleCoroutine()
    {
        _isIdle = true;
        _col.isTrigger = false;
        Rb.gravityScale = 1;
        yield return new WaitForSeconds(IdleDuration);
        _col.isTrigger = true;
        Rb.linearVelocity = Vector2.zero;
        Rb.gravityScale = 0;
        _isIdle = false;
    }
}
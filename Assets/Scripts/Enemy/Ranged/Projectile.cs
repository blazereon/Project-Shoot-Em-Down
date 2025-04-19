using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int atkDmg;
    public float speed;
    public Vector2 trajectory;
    public Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventSystem.Current.AttackPlayer(atkDmg);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

public class Deflector : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    public bool CanDeflect = true;
    public float deflectCooldown = 2f; // Deflect cooldown
    private float activeDuration = 1f; // Deflect duration

    void Start()
    {
    /*  
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        gameObject.SetActive(false);
    */    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanDeflect)
        {
            StartCoroutine(ActivateDeflector());
        }
    }

    private IEnumerator ActivateDeflector()
    {
        CanDeflect = false;
        gameObject.SetActive(true);
        boxCollider.enabled = true;

        yield return new WaitForSeconds(activeDuration); // deflector visible for a short time

        gameObject.SetActive(false);
        boxCollider.enabled = false;

        yield return new WaitForSeconds(deflectCooldown); // wait before you can deflect again

        CanDeflect = true;
    }

    void OnTriggerEnter2D(Collider2D other)
{
    // Make sure this only affects objects tagged as "Bullet"
    // if (!other.CompareTag("genericProjectile(Clone)")) return;

    // Calculate reflection
    UnityEngine.Vector2 incoming = (other.transform.position - transform.position).normalized;
    UnityEngine.Vector2 normal = transform.up; // Adjust if needed depending on your deflector's facing
    UnityEngine.Vector2 reflected = UnityEngine.Vector2.Reflect(incoming, normal);

    // Visual debug
    UnityEngine.Debug.DrawRay(other.transform.position, reflected, Color.magenta, 1f);

    // Apply reflection to the bullet's Rigidbody2D
    Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        float speed = rb.linearVelocity.magnitude;

        // Optional: check your bullet state if needed
        Projectile bullet = other.GetComponent<Projectile>();
        if (bullet == null) // || !bullet.IsUpdatingTravel())
        {
            rb.linearVelocity = reflected.normalized * speed;
        }
    }

    
}

}

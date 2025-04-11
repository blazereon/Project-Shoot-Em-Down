using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    InputAction moveAction;
    InputAction jumpAction;


    Rigidbody2D rb;
    CapsuleCollider2D col;


    Vector2 moveValue;

    private bool isGrounded = true;
    


    public float speed = 500f;
    public float jumpForce = 5f;
    public float landAcceleration = 2f;
    public float landStart = 3f;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {

        if (jumpAction.IsPressed() && isGrounded) {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        float moveRate = speed * Time.fixedDeltaTime * moveValue.x;
        rb.linearVelocityX = moveRate;

        if (rb.linearVelocityY < landStart && !isGrounded) {
            rb.linearVelocityY -= landAcceleration * Time.fixedDeltaTime;
        }

    }
}

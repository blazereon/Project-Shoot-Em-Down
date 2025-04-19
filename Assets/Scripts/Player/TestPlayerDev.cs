using System.Collections;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerDev : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    InputAction moveAction;
    InputAction jumpAction;
    InputAction dashAction;
    InputAction attackAction;

    Rigidbody2D rb;
    CapsuleCollider2D col;

    Vector2 moveValue;
    private bool canDash = true;
    private bool isDashing = false;
    private bool isAttacking = false;

    private Facing faceState = Facing.right;

    private float _currentSpeed;
    private int _momentum;

    public int Health;

    public float Speed = 500f;
    public float JumpForce = 5f;
    public float LandAcceleration = 2f;
    public float LandStart = 3f;
    public float DashForce;
    public float DashDuration;
    public float DashRecovery;
    public int MaxMomentum;

    public AttackBox attackBox;
    public GroundBox groundBox;


    public enum MoveState {
        Idle,
        Dash,
        Jump,
        Land
    }

    void Awake()
    {
        EventSystem.Current.OnAttackPlayer += TakeDamage;
    }


    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        dashAction = InputSystem.actions.FindAction("Dash");
        attackAction = InputSystem.actions.FindAction("Attack");

        _currentSpeed = Speed;
        
        rb = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {
        // Debug.Log(groundBox.isGrounded);
        EventSystem.Current.PlayerLocation = transform.position;
        
        moveValue = moveAction.ReadValue<Vector2>();

        if (dashAction.IsPressed() && canDash) {
            StartCoroutine(Dash());
            StartCoroutine(Momentum());
        }

        if (attackAction.IsPressed() && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        if (moveValue.x < 0) {
            faceState = Facing.left;
        } else if (moveValue.x > 0) {
            faceState = Facing.right;
        }

    }

    void FixedUpdate()
    {

        if (jumpAction.IsPressed() && groundBox.isGrounded) {

            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }

        if (!isDashing){

            if (rb.linearVelocityY < LandStart && !groundBox.isGrounded) {
                rb.linearVelocityY -= LandAcceleration * Time.fixedDeltaTime;
            }
            
            float moveRate = _currentSpeed * Time.fixedDeltaTime * moveValue.x;
            rb.linearVelocityX = moveRate;
        }

    }

    private void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log("Player Received Damage, Current HP: "+ Health);
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        rb.linearVelocityY = 0;
        if (faceState == Facing.left) {
            rb.linearVelocityX = -DashForce;
        } else if (faceState == Facing.right) {
            rb.linearVelocityX = DashForce;
        }
        rb.gravityScale = 0;
        yield return new WaitForSeconds(DashDuration);
        rb.linearVelocityX = 0;
        rb.gravityScale = 1;
        isDashing = false;
        yield return new WaitForSeconds(DashRecovery);
        canDash = true;
    }

    IEnumerator Attack()
    {
        Debug.Log("Attack invoked");
        isAttacking = true;
        attackBox.Attack(faceState);
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
    }

    IEnumerator Momentum()
    {
        yield return new WaitForSeconds(1f);

    }

    void OnDestroy()
    {
        EventSystem.Current.OnAttackPlayer -= TakeDamage;
    }
}

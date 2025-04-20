using UnityEngine;

public class ManagerRangedGrounded : Enemy
{
    BaseRangedGrounded currentState;

    public WanderRangedGrounded wanderState = new WanderRangedGrounded();
    public ChaseRangedGrounded chaseState = new ChaseRangedGrounded();
    public AttackRangedGrounded attackState = new AttackRangedGrounded();

    [Space(10)]
    [Header("MOVEMENT")]
    public Rigidbody2D enemyRb;
    public float wanderSpeed;
    public float chaseSpeed;
    public float wallDistanceLimit;
    public float startEngagementRange;

    [Space(10)]
    [Header("ATTACK")]
    public GameObject projectile;
    public enum shootType{
        Single,
        SingleFileBurst,
        TrackingBurst,
        AOEBurst
    }
    public shootType shootMode;

    [Space(5)]
    [Tooltip("For all burst mode, number of projectiles to be launch per burst")]
    public int burstCount;

    [Space(5)]
    [Tooltip("For single file and tracking Burst mode, time interval between bullets in the bullets")]
    public float projectileInterval;

    [Space(5)]
    [Tooltip("For AOE burst mode, max spread angle of the burst")]
    public float burstSpread;

    [Space(5)]
    public int attackDmg;
    public float attackSpd;
    public float projectileSpd;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public bool hasProjectileParent;
    [HideInInspector]
    public GameObject[] projectileParents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = wanderState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(BaseRangedGrounded state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void Flip()
    {
        Vector2 objectScale = transform.localScale;
        objectScale.x *= -1;

        transform.localScale = objectScale;

    }

    public void SpriteFlip()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if ( spriteRenderer.flipX == true) // if facing left
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
}


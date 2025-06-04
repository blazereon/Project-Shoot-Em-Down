using UnityEngine;

public class ManagerShieldRanged : Enemy
{
    BaseShieldRanged currentState;

    public WanderShieldRanged wanderState = new WanderShieldRanged();
    public ChaseShieldRanged chaseState = new ChaseShieldRanged();
    public AttackShieldRanged attackState = new AttackShieldRanged();

    public BoxCollider2D weakSpotBox;
    public HitDetect hitDetect;

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

    public float attackSpd;
    public float projectileSpd;

    public enum shootType
    {
        Single,
        SingleFileBurst,
        TrackingBurst,
        AOEBurst,
        TwirlBurst
    }
    public shootType shootMode;

    [Tooltip("For all burst mode, number of projectiles to be launch per burst")]
    public int burstCount;

    [Tooltip("For single file and tracking Burst mode, time interval between bullets burst")]
    public float projectileInterval;

    [Tooltip("For AOE burst mode, max spread angle of the burst")]
    public float burstSpread;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public bool hasProjectileParent;
    [HideInInspector]
    public GameObject[] projectileParents;
    [HideInInspector]
    public Collider2D enemyCollider;
    [HideInInspector]
    public int attackDmg;

    private void Awake()
    {
        EventSystem.Current.OnDamageEnemy += TakeDamage;
        EventSystem.Current.OnApplyEffect += ReceiveEffect;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weakSpotBox = transform.Find("weakSpot").GetComponent<BoxCollider2D>();
        hitDetect = transform.Find("weakSpot").GetComponent<HitDetect>();
        enemyCollider = GetComponent<Collider2D>();

        if (weakSpotBox == null || hitDetect == null)
        {
            Debug.LogError("weakSpotBox child or its components are missing. Make sure to add a BoxCollider2D Component and the HitDetect.cs");
        }

        attackDmg = AttackDamage;

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

    public void SwitchState(BaseShieldRanged state)
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

    void OnDestroy()
    {
        EventSystem.Current.OnDamageEnemy -= TakeDamage;
        EventSystem.Current.OnApplyEffect -= ReceiveEffect;
    }
}

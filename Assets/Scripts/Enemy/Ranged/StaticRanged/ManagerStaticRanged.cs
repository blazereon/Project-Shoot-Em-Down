using UnityEngine;

public class ManagerStaticRanged : Enemy
{
    BaseStaticRanged currentState;

    public ScanStaticRanged scanState = new ScanStaticRanged();
    public AttackStaticRanged attackState = new AttackStaticRanged();

    public BoxCollider2D scanBox;
    public HitDetect hitDetect;

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
    public float burstInterval;

    [Tooltip("For AOE burst mode, max spread angle of the burst")]
    public float burstSpread;

    [HideInInspector]
    public Collider2D enemyCollider;
    [HideInInspector]
    public int attackDmg;

    private void Awake()
    {
        EventSystem.Current.OnDamageEnemy += TakeDamage;
    }

    void Start()
    {
        scanBox = transform.Find("ScanBox").GetComponent<BoxCollider2D>();
        hitDetect = transform.Find("ScanBox").GetComponent<HitDetect>();
        enemyCollider = GetComponent<Collider2D>();

        if (scanBox == null || hitDetect == null)
        {
            Debug.LogError("ScanBox child or its components are missing. Make sure to add a BoxCollider2D Component and the HitDetect.cs");
        }

        attackDmg = AttackDamage;

        currentState = scanState;
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

    public void SwitchState(BaseStaticRanged state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void OnDestroy()
    {
        EventSystem.Current.OnDamageEnemy -= TakeDamage;
    }
}

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
    public enum shootType
    {
        Single,
        SingleFileBurst,
        TrackingBurst,
        AOEBurst,
        TwirlBurst
    }
    public shootType shootMode;

    [Space(5)]
    [Tooltip("For all burst mode, number of projectiles to be launch per burst")]
    public int burstCount;

    [Space(5)]
    [Tooltip("For single file and tracking Burst mode, time interval between bullets burst")]
    public float projectileInterval;

    [Space(5)]
    [Tooltip("For AOE burst mode, max spread angle of the burst")]
    public float burstSpread;

    [Space(5)]
    public int attackDmg;
    public float attackSpd;
    public float projectileSpd;

    [HideInInspector]
    public Collider2D enemyCollider;

    private void Awake()
    {
        EventSystem.Current.OnAttackEnemy += TakeDamage;
    }

    void Start()
    {
        scanBox = transform.Find("ScanBox").GetComponent<BoxCollider2D>();
        hitDetect = transform.Find("ScanBox").GetComponent<HitDetect>();

        enemyCollider = GetComponent<Collider2D>();

        if (scanBox == null || hitDetect == null)
        {
            Debug.LogError("ScanBox child is missing. Make sure to add a BoxCollider2D Component and the HitDetect.cs");
        }

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
        EventSystem.Current.OnAttackEnemy -= TakeDamage;
    }
}

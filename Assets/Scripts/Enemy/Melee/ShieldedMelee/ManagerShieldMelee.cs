using UnityEngine;

public class ManagerShieldMelee : Enemy
{
    BaseShieldMelee currentState;

    public WanderShieldMelee wanderState = new WanderShieldMelee();
    public ChaseShieldMelee chaseState = new ChaseShieldMelee();
    public AttackShieldMelee attackState = new AttackShieldMelee();


    public BoxCollider2D weakSpotBox;
    public HitDetect hitDetect;


    public EnemyFacing facing = EnemyFacing.Left;
    public float wanderSpeed;
    public float chaseSpeed;


    //Generic Enemy Variables

    public Rigidbody2D enemyRb;

    public float wallDistanceLimit;


    public Collider2D enemyCollider {  get; private set; }

    void Awake()
    {
        EventSystem.Current.OnDamageEnemy += TakeDamage;
        EventSystem.Current.OnApplyEffect += ReceiveEffect;
        enemyRb = GetComponent<Rigidbody2D>();
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
        else
        {
            hitDetect.weakSpotActive = true;
        }

        currentState = wanderState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(BaseShieldMelee state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void Flip()
    {
        facing = facing == EnemyFacing.Left ? EnemyFacing.Right : EnemyFacing.Left;
        UpdateFlip();
    }

    public void Flip(EnemyFacing face)
    {
        facing = face;
        UpdateFlip();
    }

    public void ScaleFlip()
    {
        Vector2 objectScale = transform.localScale;
        objectScale.x *= -1;

        transform.localScale = objectScale;

    }

    void UpdateFlip()
    {
        if (facing == EnemyFacing.Left)

        {

        }
        else if (facing == EnemyFacing.Right)
        {

        }
    }

    void OnDestroy()
    {
        EventSystem.Current.OnDamageEnemy -= TakeDamage;
        EventSystem.Current.OnApplyEffect -= ReceiveEffect;
    }
}

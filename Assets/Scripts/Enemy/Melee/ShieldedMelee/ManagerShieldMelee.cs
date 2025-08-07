using UnityEngine;

public class ManagerShieldMelee : Enemy
{
    BaseShieldMelee currentState;
    public BaseShieldMelee prevState { get; set; }

    public WanderShieldMelee wanderState = new WanderShieldMelee();
    public ChaseShieldMelee chaseState = new ChaseShieldMelee();
    public AttackShieldMelee attackState = new AttackShieldMelee();
    public StunShieldedMelee stunState = new StunShieldedMelee();


    public BoxCollider2D weakSpotBox;
    public HitDetect hitDetect;


    public EnemyFacing facing = EnemyFacing.Left;
    public float wanderSpeed;
    public float chaseSpeed;


    //Generic Enemy Variables

    public Rigidbody2D enemyRb;

    public float wallDistanceLimit;


    public CapsuleCollider2D enemyCollider { get; private set; }

    public GameObject _shieldObject { get; private set; }
    public Transform _shieldTransform { get; private set; }

    //Set the Sprite Animator
    public Animator MainAnimator;
    public Animator AttackAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.Current.OnDamageEnemy += TakeDamage;
        EventSystem.Current.OnApplyEffect += ReceiveEffect;
        EventSystem.Current.OnReleaseStunningField += DetectStunningField;
        enemyRb = GetComponent<Rigidbody2D>();

        // Main Animator
        MainAnimator = GetComponent<Animator>();

        // Attack Animator
        //AttackAnimator = transform.Find("Hammer").GetComponent<Animator>();

        weakSpotBox = transform.Find("weakSpot").GetComponent<BoxCollider2D>();
        hitDetect = transform.Find("weakSpot").GetComponent<HitDetect>();
        enemyCollider = GetComponent<CapsuleCollider2D>();

        if (enemyCollider == null)
        {
            Debug.LogError("Shielded Melee collider doesn't use or have Capsule Collider 2D, please set the appropriate collider for this enemy");
        }

        if (weakSpotBox == null || hitDetect == null)
        {
            Debug.LogError("weakSpotBox child or its components are missing. Make sure to add a BoxCollider2D Component and the HitDetect.cs");
        }
        else
        {
            hitDetect.weakSpotActive = true;
        }

        _shieldObject = FindChildByTag(gameObject, "Shield");

        if (_shieldObject == null)
        {
            Debug.LogError("Shield child not found! Make sure the shield is tagged as 'Shield'");
        }
        else
        {
            _shieldTransform = _shieldObject.transform;
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
        EventSystem.Current.OnReleaseStunningField -= DetectStunningField;
    }

    private string currentMainAnimation = "";
    private string currentAttackAnimation = "";

    public void PlayMainAnimation(string newAnimation)
    {
        if (newAnimation == currentMainAnimation) return;
        MainAnimator.Play(newAnimation);
        currentMainAnimation = newAnimation;
    }
/*
    public void PlayAttackAnimation(string newAnimation)
    {
        if (newAnimation == currentAttackAnimation) return;
        AttackAnimator.Play(newAnimation);
        currentAttackAnimation = newAnimation;
    }
*/
    
}

using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class ManagerExplosive : Enemy
{
    BaseExplosive currentState;
    public BaseExplosive prevState {  get; set; }

    public WanderExplosive wanderState = new WanderExplosive();
    public ChaseExplosive chaseState = new ChaseExplosive();
    public AttackExplosive attackState = new AttackExplosive();
    public StunExplosive stunState = new StunExplosive();


    public EnemyFacing facing = EnemyFacing.Left;
    public float WanderSpeed;
    public float ChasingSpeed;
    public float startSelfDetructDistance;


    public Rigidbody2D enemyRb;
    public float WallDistanceLimit;


    public float explosionTimer;
    public float explosionRadius;

    void Awake()
    {
        EventSystem.Current.OnDamageEnemy += TakeDamage;
        EventSystem.Current.OnApplyEffect += ReceiveEffect;
        enemyRb = GetComponent<Rigidbody2D>();
    }

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

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void SwitchState(BaseExplosive state)
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

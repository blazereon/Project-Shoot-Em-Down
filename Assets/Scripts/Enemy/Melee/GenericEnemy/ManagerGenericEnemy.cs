using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ManagerGenericEnemy : Enemy
{

    BaseGenericEnemy currentState;
    public BaseGenericEnemy prevState { get; set; }

    public WanderGenericEnemy wanderState = new WanderGenericEnemy();
    public ChaseGenericEnemy chaseState = new ChaseGenericEnemy();
    public AttackGenericEnemy attackState = new AttackGenericEnemy();
    public StunGenericEnemy stunState = new StunGenericEnemy();


    public EnemyFacing facing = EnemyFacing.Left;
    public float WanderSpeed;
    public float ChasingSpeed;


    //Generic Enemy Variables

    public Rigidbody2D GenericEnemyRb;

    public float WallDistanceLimit;

    //Set the Sprite Animator
    public Animator MainAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.Current.OnDamageEnemy += TakeDamage;
        EventSystem.Current.OnApplyEffect += ReceiveEffect;

        GenericEnemyRb = GetComponent<Rigidbody2D>();
        // Main Animator
        MainAnimator = GetComponent<Animator>();

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

    public void SwitchState(BaseGenericEnemy state)
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

    private string currentMainAnimation = "";

    public void PlayMainAnimation(string newAnimation)
    {
        if (newAnimation == currentMainAnimation) return;
        MainAnimator.Play(newAnimation);
        currentMainAnimation = newAnimation;
    }
    
    public void ScaleFlip()
    {
        Vector2 objectScale = transform.localScale;
        objectScale.x *= -1;

        transform.localScale = objectScale;

    }
}

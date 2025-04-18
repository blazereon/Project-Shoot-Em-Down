using UnityEngine;

public class ManagerRangedGrounded : Enemy
{
    BaseRangedGrounded currentState;

    public WanderRangedGrounded wanderState = new WanderRangedGrounded();
    public ChaseRangedGrounded chaseState = new ChaseRangedGrounded();
    public AttackRangedGrounded attackState = new AttackRangedGrounded();

    public Rigidbody2D enemyRb;
    public float wanderSpeed;
    public float chaseSpeed;
    public float wallDistanceLimit;

    public float startEngagementRange;

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

}


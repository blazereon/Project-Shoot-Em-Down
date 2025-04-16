using UnityEngine;

public class GenericEnemyManager : Enemy
{

    GenericEnemyBaseState currentState;

    GenericEnemyWanderState wanderState = new GenericEnemyWanderState();
    GenericEnemyChaseState chaseState = new GenericEnemyChaseState();
    GenericEnemyAttackState attackState = new GenericEnemyAttackState();

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

    public void SwitchState(GenericEnemyBaseState state)
    {
        currentState = state;
    }
}

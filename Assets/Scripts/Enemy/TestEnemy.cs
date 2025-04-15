using UnityEngine;

public class TestEnemy : Enemy 
{

    void Awake()
    {
        EventSystem.Current.OnAttackEnemy += TakeDamage;
    }

    void OnDestroy()
    {
        EventSystem.Current.OnAttackEnemy -= TakeDamage;
    }
}
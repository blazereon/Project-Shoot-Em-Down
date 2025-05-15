using UnityEngine;

public class TestEnemy : Enemy 
{

    void Awake()
    {
        EventSystem.Current.OnDamageEnemy += TakeDamage;
    }

    void OnDestroy()
    {
        EventSystem.Current.OnDamageEnemy -= TakeDamage;
    }
}
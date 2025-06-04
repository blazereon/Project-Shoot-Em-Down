using UnityEditor;
using UnityEngine;

public class AetherMark : Effect
{
    public Keen KeenInstance;
    Enemy EnemyInstance;
    Coroutine durationCouroutine;

    public AetherMark(Enemy enemy, float duration)
    {
        EnemyInstance = enemy;
        Duration = duration;
    }
    public override void OnEffectStart()
    {
        if (EnemyInstance == null)
        {
            EnemyInstance = (Enemy)EntityHolder;
        }
        KeenInstance.OnConsumeMark += ConsumeMark;
        Debug.Log("Aether mark applied");
        EnemyInstance.AddEffect(this);
        durationCouroutine = CoroutineHandler.Instance.StartCoroutine(EffectExpiration());
    }


    public override void OnEffectEnd()
    {
        ConsumeMark();
        KeenInstance.OnConsumeMark -= ConsumeMark;
        CoroutineHandler.Instance.StopCoroutine(durationCouroutine);
        EnemyInstance.RemoveEffect(this);
        Debug.Log("Aether mark end");
    }

    public void ConsumeMark()
    {
        var _layerMask = LayerMask.GetMask("Enemy", "NonCollideEnemy", "Projectile");
        Collider2D[] hits = Physics2D.OverlapCircleAll(EnemyInstance.transform.position, 3, _layerMask);

        if (hits == null)
        {
            return;
        }

        foreach (var hit in hits)
        {
            if (hit.gameObject.tag == "Enemy")
            {
                EventSystem.Current.AttackEnemy(hit.gameObject, DamageType.Melee, 40, 0, false);
            }
        }

        Debug.Log("Aether mark consumed");
    }

    public override Effect Clone()
    {
        var _clonedAetherMark = new AetherMark(null, Duration);
        _clonedAetherMark.KeenInstance = KeenInstance;
        return _clonedAetherMark;
    }
}
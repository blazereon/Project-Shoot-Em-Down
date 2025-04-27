using UnityEngine;

public enum WeaponType
{
    Ranged,
    Melee
}

public class Weapon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public WeaponType Type;
    [SerializeField] public BaseProjectile Projectile;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerWeapon()
    {

    }
}

using UnityEngine;

public enum DamageType
{
    Range,
    Melee
}

public enum Facing
{
    left,
    right
}

public class Entity : MonoBehaviour
{
    public int MeleeResistance;
    public int RangeResistance;

}

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerDebug
{
    public BasePlayerState playerState;
    public PlayerStats playerStats;
    public List<Effect> EffectsList;
}
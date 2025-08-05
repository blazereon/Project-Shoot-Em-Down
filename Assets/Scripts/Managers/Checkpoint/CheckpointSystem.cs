using System;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    public static CheckpointSystem Current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Action<GameObject> OnRegisterCheckPoint;
    public Action<PlayerStats> OnSpawnCheckpoint;

    public Transform CheckpointTransform;
    void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterCheckpoint(GameObject checkpoint)
    {
        OnRegisterCheckPoint?.Invoke(checkpoint);
    }

    public void SpawnStart(PlayerStats stats)
    {

    }

    public void SpawnOnCheckpoint(PlayerStats stats)
    {
        OnSpawnCheckpoint?.Invoke(stats);
    }

    public void GetCheckpointLocation()
    {

    }
}

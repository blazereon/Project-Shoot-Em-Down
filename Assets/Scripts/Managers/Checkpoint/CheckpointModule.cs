using System;
using System.Numerics;
using Unity.Cinemachine;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CheckpointModule : MonoBehaviour
{
    public enum Type
    {
        Spawn,
        Checkpoint
    }

    [SerializeField]
    private Type _checkpointType;
    public Type CheckpointType
    {
        get
        {
            return _checkpointType;
        }
        set
        {
            _checkpointType = value;
        }
    }

    [NonSerialized] public bool IsPlayerCheckpoint = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is create
    void Awake()
    {
        CheckpointSystem.Current.OnRegisterCheckPoint += UnregisterCheckpoint;
        CheckpointSystem.Current.OnSpawnCheckpoint += SpawnPlayerCheckpoint;
    }
    void Start()
    {

        if (_checkpointType == Type.Spawn)
        {
            RegisterCheckpoint();
            SpawnPlayerCheckpoint(GameManager.Current.PlayerSavedStats);
        }
    }
    private void InstantiatePlayer(PlayerStats CurrentStats)
    {
        GameObject pObject = Instantiate(GameManager.Current.PlayerObject, this.transform);
        Player player = pObject.GetComponent<Player>();

        if (GameManager.Current.IsPlayerNew)
        {
            GameManager.Current.PlayerSavedStats = player.PlayerBaseStats;
            GameManager.Current.IsPlayerNew = false;
        }
        else
        {
            player.PlayerCurrentStats = GameManager.Current.PlayerSavedStats;
        }
        CameraController.Current.TrackTarget(pObject);

        //Retain upgraded abilities
        for (int i = 0; i < player.PlayerCurrentStats.KeenAbility.AbilityData.UpgradeTier; i++)
        {
            player.KeenAbility.UpgradeComponent();
        }
        for (int i = 0; i < player.PlayerCurrentStats.DashAbility.AbilityData.UpgradeTier; i++)
        {
            player.DashAbility.UpgradeComponent();
        }
        for (int i = 0; i < player.PlayerCurrentStats.DestructiveBoltAbility.AbilityData.UpgradeTier; i++)
        {
            player.DestructiveBoltAbility.UpgradeComponent();
        }
    }

    private void RegisterCheckpoint()
    {
        //Register checkpoint to the system
        IsPlayerCheckpoint = true;
        CheckpointSystem.Current.RegisterCheckpoint(this.gameObject);
        CheckpointSystem.Current.CheckpointTransform = this.transform;
    }

    private void UnregisterCheckpoint(GameObject gameObject)
    {
        if (gameObject == this.gameObject) return;
        IsPlayerCheckpoint = false;
    }

    private void SpawnPlayerCheckpoint(PlayerStats stats)
    {
        if (!IsPlayerCheckpoint) return;
        InstantiatePlayer(stats);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RegisterCheckpoint();
        }
    }
}

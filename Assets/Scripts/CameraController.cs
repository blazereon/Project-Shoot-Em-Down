using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Current;
    public CinemachineCamera CinemachineCameraInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }
    }
    public void TrackTarget(GameObject player)
    {
        CinemachineCameraInstance.Follow = player.transform;
    }
}

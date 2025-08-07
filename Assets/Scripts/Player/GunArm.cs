using System;
using UnityEngine;

public class GunArm : MonoBehaviour
{
    public Animator ArmAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateArm(bool isIdle, int tier)
    {
        String baseString = "Gun_Arm";
        if (isIdle) baseString += "_Idle";
        baseString += "_v" + tier.ToString();
        ArmAnimator.Play(baseString);
    }
}

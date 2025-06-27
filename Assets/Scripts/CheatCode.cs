using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CheatCode : MonoBehaviour
{
    private string currentString = "";
    private float cheatStringReset = 1.5f;
    private float timer;

    [SerializeField]
    private List<CheatCodeInstance> cheatCodeList = new List<CheatCodeInstance>();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            timer = cheatStringReset;
            foreach (char c in Input.inputString)
            {
                currentString += c;
                Debug.Log("Key was: " + c + " String was: " + currentString);
                CheckCheat(currentString);
            }
        }
        else
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    currentString = "";
                    Debug.Log("Key Cheat string timeout");
                }
            }
        }
    }

    void CheckCheat(string _codeInput)
    {
        foreach (CheatCodeInstance cheat in cheatCodeList)
        {
            if (_codeInput == cheat.code)
            {
                Debug.Log("Cheat call invoked");
                cheat.cheatEvent?.Invoke();
                currentString = "";
            }
        }
    }

    public void MaxHealth()
    {
        Debug.Log("Full Health Cheat Invoked");
    }

    public void MaxAggro()
    {
        Debug.Log("Full Aggro Cheat Invoked");
    }

    public void DashUp1()
    {
        Debug.Log("Dash +1 Upgrade Cheat Invoked");
    }

    public void DashMinus1()
    {
        Debug.Log("Dash -1 Upgrade Cheat Invoked");
    }

    public void RangeUp1()
    {
        Debug.Log("Range +1 Upgrade Cheat Invoked");
    }

    public void RangedMinus1()
    {
        Debug.Log("Range -1 Upgrade Cheat Invoked");
    }

    public void MeleeUp1()
    {
        Debug.Log("Melee +1 Upgrade Cheat Invoked");
    }

    public void MeleeMinus1()
    {
        Debug.Log("Melee -1 Upgrade Cheat Invoked");
    }
}

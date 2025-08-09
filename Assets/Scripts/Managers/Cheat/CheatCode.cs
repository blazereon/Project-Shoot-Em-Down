using System.Collections.Generic;
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
            if (_codeInput.Contains(cheat.code, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Cheat call invoked");
                cheat.cheatEvent?.Invoke();
                currentString = "";
            }
        }
    }

    public void AddHealth()
    {
        CheatEventSystem.Current.InvokePlayerCheat(new PlayerStatDelta { deltaHealth = 1 });
    }

    public void MaxHealth()
    {
        CheatEventSystem.Current.InvokePlayerCheat(new PlayerStatDelta { Health = 10, MaxHealth = 10 });
    }

    public void MaxAggro()
    {
        CheatEventSystem.Current.InvokePlayerCheat(new PlayerStatDelta { MaxAggression = 100, Aggression = 100 });
    }

    public void DashUp1()
    {
        CheatEventSystem.Current.InvokeUpgradeAbilityCheat(CompAbilityType.Dash);
        Debug.Log("Dash +1 Upgrade Cheat Invoked");
    }

    public void DashMinus1()
    {
        //irreversible yet
        Debug.Log("Dash -1 Upgrade Cheat Invoked");
    }

    public void RangeUp1()
    {
        CheatEventSystem.Current.InvokeUpgradeAbilityCheat(CompAbilityType.DestructiveBolt);
        Debug.Log("Range +1 Upgrade Cheat Invoked");
    }

    public void RangedMinus1()
    {
        //irreversible yet
        Debug.Log("Range -1 Upgrade Cheat Invoked");
    }

    public void MeleeUp1()
    {
        CheatEventSystem.Current.InvokeUpgradeAbilityCheat(CompAbilityType.Keen);
        Debug.Log("Melee +1 Upgrade Cheat Invoked");
    }

    public void MeleeMinus1()
    {
        //irreversible yet
        Debug.Log("Melee -1 Upgrade Cheat Invoked");
    }

    public void AddSP()
    {
        CheatEventSystem.Current.InvokePlayerCheat(new PlayerStatDelta { deltaSkillPoint = 1 });
    }
}

using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.AI;

public class TreeGroup : MonoBehaviour
{
    public Tier Tier1, Tier2, Tier3;

    [Range(0.0f, 3.0f)]
    [SerializeField]
    private float _fillAmount;
    public float FillAmount
    {
        get
        {
            return _fillAmount;
        }
        set
        {
            _fillAmount = value;

            //adjustment logic
            SetOverallFillAmount();
        }
    }

    void OnValidate()
    {
        SetOverallFillAmount();
    }

    void SetOverallFillAmount()
    {
        //Temporarily locks all tiers except tier 1
        Tier1.Lock = true;
        Tier2.Lock = true;
        Tier3.Lock = true;

        Tier1.FillAmount = 1 - _fillAmount;
        Tier2.FillAmount = 2 - _fillAmount;
        Tier3.FillAmount = 3 - _fillAmount;

        if (_fillAmount < 1)
        {
            Tier1.Lock = false;
        }
        else if (_fillAmount < 2)
        {
            Tier2.Lock = false;
        }
        else if (_fillAmount < 3)
        {
            Tier3.Lock = false;
        }
    }
}

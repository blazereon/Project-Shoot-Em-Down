using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

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
        Tier1.FillAmount = 1 - _fillAmount;
        Tier2.FillAmount = 2 - _fillAmount;
        Tier3.FillAmount = 3 - _fillAmount;
    }
}

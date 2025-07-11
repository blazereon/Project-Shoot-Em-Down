using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XpMeter : MonoBehaviour
{
    public Image XpMeterBar;
    public TextMeshProUGUI XpTextTMP;
    public TextMeshProUGUI SkillPointTMP;

    public void UpdateValue(int xp, int maxXp, int sp)
    {
        float XpPercentage = (float)xp / maxXp;
        XpMeterBar.fillAmount = XpPercentage;
        XpTextTMP.text = String.Format("{0}/{1}", xp, maxXp);
        SkillPointTMP.text = String.Format("SP: {0}", sp);        
    }

}

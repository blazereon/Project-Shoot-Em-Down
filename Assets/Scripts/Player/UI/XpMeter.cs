using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XpMeter : MonoBehaviour
{
    public Image XpMeterBar;
    public TextMeshProUGUI XpTextTMP;
    public TextMeshProUGUI SkillPointTMP;

    public float visibleDuration = 3f; // seconds to stay visible after gaining XP
    private float hideTimer = 0f;

    private bool isVisible = false;

    void Update()
    {
        // Auto-hide after timer runs out
        if (isVisible && hideTimer > 0f)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f)
            {
                Hide();
            }
        }
    }

    public void UpdateValue(int xp, int maxXp, int sp)
    {
        float XpPercentage = (float)xp / maxXp;
        XpMeterBar.fillAmount = XpPercentage;
        XpTextTMP.text = $"{xp}/{maxXp}";
        SkillPointTMP.text = $"SP: {sp}";

        // Show the UI if XP is gained or full
        if (xp > 0 || xp >= maxXp)
        {
            Show();

            // Only set the timer if not already full XP
            if (xp < maxXp)
            {
                hideTimer = visibleDuration;
            }
        }
    }

    private void Show()
    {
        if (!isVisible)
        {
            gameObject.SetActive(true);
            isVisible = true;
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        isVisible = false;
    }
}

using TMPro;
using UnityEngine;

public class ChainMeter : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI ChainMeterTMP;

    [Header("Shake Settings")]
    public float shakeIntensity = 5f;

    private RectTransform rectTransform;
    private Vector3 originalPosition;

    private void Start()
    {
        rectTransform = ChainMeterTMP.GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void UpdateValue(PlayerStats playerStats)
    {
        if (playerStats.ChainTimer > 0)
        {
            // Update UI
            ChainMeterTMP.gameObject.SetActive(true);
            ChainMeterTMP.text = "x" + playerStats.Chain;

            // Calculate normalized time (1 → 0)
            float t = Mathf.Clamp01(playerStats.ChainTimer / playerStats.ChainDuration);

            // Lerp font size from 20 (end) to 150 (start)
            ChainMeterTMP.fontSize = Mathf.Lerp(20, 250, t);

            Vector2 shakeOffset = Random.insideUnitCircle * shakeIntensity * t;
            rectTransform.anchoredPosition = originalPosition + (Vector3)shakeOffset;

            // Decrease timer
            playerStats.ChainTimer -= Time.deltaTime;
        }
        if(playerStats.Chain == 0 || playerStats.ChainTimer == 0 || ChainMeterTMP.text == "x0"){
            ChainMeterTMP.text = "";
            ChainMeterTMP.gameObject.SetActive(false);
        }
    }

}
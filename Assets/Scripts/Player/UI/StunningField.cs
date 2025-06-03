using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class StunningField : MonoBehaviour
{
    public GameObject StunningFieldObject;
    public float StunningEffectDuration = 0.5f;
    public float FadeInPercent = 0.8f;
    public float FadeOutPercent = 0.2f;
    private float _effectTimer;
    Image StunningFieldImage;
    RectTransform StunningFieldRectTransform;

    Color initColor, targetColor;

    public float ExpansionSpeed = 0.2f;

    private void Awake()
    {
        StunningFieldImage = StunningFieldObject.GetComponent<Image>();
        StunningFieldRectTransform = StunningFieldObject.GetComponent<RectTransform>();
        EventSystem.Current.OnReleaseStunningField += InvokeStunningField;
        initColor = StunningFieldImage.color;
        targetColor = new Color(initColor.r, initColor.g, initColor.b, 0f);
    }
    public void InvokeStunningField()
    {
        _effectTimer = 0;
        StunningFieldImage.color = initColor;
        StunningFieldRectTransform.localScale = Vector3.zero;
        Debug.Log("Invoked stunning field");
        StunningFieldRectTransform.position = Camera.main.WorldToScreenPoint(EventSystem.Current.PlayerLocation);
        CoroutineHandler.Instance.StartCoroutine(StunningFieldCoroutine());
    }

    IEnumerator StunningFieldCoroutine()
    {
        Color opaqueColor = Color.white;
        //expanding effect
        while (_effectTimer < StunningEffectDuration)
        {
            Debug.Log("Stun Effect Timer: " + _effectTimer);
            StunningFieldRectTransform.localScale += new Vector3(ExpansionSpeed, ExpansionSpeed, ExpansionSpeed);
            yield return new WaitForSeconds(0.01f);
            _effectTimer += 0.01f;

            if ((_effectTimer / StunningEffectDuration) >= FadeInPercent)
            {
                //Fade Out
                Debug.Log("Fades out");
                float _lerpValue = (StunningEffectDuration - (StunningEffectDuration * FadeInPercent) - (StunningEffectDuration - _effectTimer)) / (StunningEffectDuration * FadeOutPercent);
                StunningFieldImage.color = Color.Lerp(opaqueColor, targetColor, _lerpValue);
            }
            else
            {
                //Fade In
                float _lerpValue = _effectTimer / (StunningEffectDuration * FadeInPercent);
                Debug.Log("Fades in: " + _lerpValue);

                StunningFieldImage.color = Color.Lerp(initColor, opaqueColor, _lerpValue);
            }
        }
    }
    private void OnDestroy()
    {
        EventSystem.Current.OnReleaseStunningField -= InvokeStunningField;
    }
}
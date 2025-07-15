using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tier : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is create

    public Image Icon;
    public Image Frame;
    public Image Line;
    public Image Inactive;

    public Image InactiveIcon;
    public Image InactiveFrame;
    public Image InactiveLine;

    public float HoldTime;
    public bool IsUnlocked = false;
    float _holdTimer;


    [SerializeField]
    private bool _IsPressed;
    public bool IsPressed
    {
        get
        {
            return _IsPressed;
        }
        set
        {
            _IsPressed = value;
        }
    }


    [SerializeField]
    private Sprite _iconSprite;
    public Sprite IconSprite
    {
        get
        {
            return _iconSprite;
        }
        set
        {
            _iconSprite = value;
            Icon.sprite = _iconSprite;
            InactiveIcon.sprite = _iconSprite;
        }
    }
    [SerializeField]
    private Sprite _frameSprite;
    public Sprite FrameSprite
    {
        get
        {
            return _frameSprite;
        }
        set
        {
            _frameSprite = value;
            Frame.sprite = _frameSprite;
            InactiveFrame.sprite = _frameSprite;
        }
    }
    [SerializeField]
    private Sprite _lineSprite;
    public Sprite LineSprite
    {
        get
        {
            return _lineSprite;
        }
        set
        {
            _lineSprite = value;
            Line.sprite = _lineSprite;
            InactiveLine.sprite = _lineSprite;
        }
    }
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _fillAmount;
    public float FillAmount
    {
        get
        {
            return _fillAmount;
        }
        set
        {
            _fillAmount = Mathf.Clamp01(value);
            Inactive.fillAmount = _fillAmount;
        }
    }

    void OnValidate()
    {
        if (_iconSprite != null)
        {
            Icon.sprite = _iconSprite;
            InactiveIcon.sprite = _iconSprite;
        }

        if (_frameSprite != null)
        {
            Frame.sprite = _frameSprite;
            InactiveFrame.sprite = _frameSprite;
        }

        if (_lineSprite != null)
        {
            Line.sprite = _lineSprite;
            InactiveLine.sprite = _lineSprite;
        }
        Inactive.fillAmount = _fillAmount;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!IsUnlocked)
        {
            IncrementFillAmount();
        }
        
    }

    public void IncrementFillAmount()
    {
        if (IsPressed)
        {

            _holdTimer += Time.deltaTime;
            if (_holdTimer >= HoldTime)
            {
                IsUnlocked = true;
                Inactive.fillAmount = 0;
            }
            Inactive.fillAmount = 1 - ((float)_holdTimer / HoldTime);
            return;
        }
        else
        {
            _holdTimer = 0;
            Inactive.fillAmount = 1;
            return;
        }
    }
}

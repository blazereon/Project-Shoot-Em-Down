using UnityEngine;
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
        
    }
}

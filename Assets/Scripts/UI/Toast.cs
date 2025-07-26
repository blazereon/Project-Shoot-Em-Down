using System;
using System.Collections;
using System.Xml.Schema;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    public struct ToastMessage
    {
        public String Title;
        public String Message;
        public float DisplayTime;
    }

    public enum State
    {
        FadeIn,
        Display,
        FadeOut,
        None
    }
    public float FadeTime = 4f;
    public float DisplayTime = 2f;
    private float _fadeTimer, _displayTimer = 0;
    private Coroutine DisplayTimeCoroutineInstance;

    public TextMeshProUGUI TitleTMP;
    public TextMeshProUGUI MessageTMP;
    public Image Frame;

    private Color _defaultColor;

    private ToastMessage _currentMessage;
    public ToastMessage CurrentMessage
    {
        get
        {
            return _currentMessage;
        }
        set
        {
            _currentMessage = value;
            TitleTMP.text = _currentMessage.Title;
            MessageTMP.text = _currentMessage.Message;
            _currentState = State.FadeIn;
        }
    }

    private State _currentState;
    public State CurrentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
        }
    }

    public void Start()
    {
        _currentState = State.None;
        _defaultColor = Frame.color;
        _defaultColor.a = 0;
        Frame.color = _defaultColor;
    }

    void Update()
    {
        switch (_currentState)
        {
            case State.Display:
                Display();
                break;
            case State.FadeIn:
                FadeIn();
                break;
            case State.FadeOut:
                FadeOut();
                break;
            case State.None:
                None();
                break;
        }
    }

    private void FadeIn()
    {
        _displayTimer = 0;
        _fadeTimer += Time.deltaTime;
        Frame.color = Color.Lerp(_defaultColor, new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, 1), _fadeTimer / FadeTime);
        TitleTMP.alpha = _fadeTimer / FadeTime;
        MessageTMP.alpha = _fadeTimer / FadeTime;
        if (_fadeTimer >= FadeTime)
        {
            _fadeTimer = 0;
            _currentState = State.Display;
        }
    }

    private void FadeOut()
    {
        _displayTimer = 0;
        _fadeTimer += Time.deltaTime;
        TitleTMP.alpha = 1 - _fadeTimer / FadeTime;
        MessageTMP.alpha = 1 - _fadeTimer / FadeTime;
        Frame.color = Color.Lerp(new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, 1), _defaultColor, _fadeTimer / FadeTime);
        if (_fadeTimer >= FadeTime)
        {
            _fadeTimer = 0;
            _currentState = State.None;
        }
    }

    private void Display()
    {
        Frame.color = new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, 1);
        TitleTMP.alpha = 1;
        MessageTMP.alpha = 1;
        _displayTimer += Time.deltaTime;

        if (_displayTimer > _currentMessage.DisplayTime)
        {
            // _displayTimer = 0;
            _currentState = State.FadeOut;
        }
    }


    private void None()
    {
        Frame.color = _defaultColor;
        TitleTMP.alpha = 0;
        MessageTMP.alpha = 0;
    }
}

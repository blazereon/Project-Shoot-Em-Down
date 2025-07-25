using System;
using System.Collections;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    public enum State
    {
        FadeIn,
        Display,
        FadeOut,
        None
    }
    public float FadeTime = 4f;
    public float DisplayTime = 2f;
    private float _fadeTimer = 0;

    public TextMeshProUGUI TitleTMP;
    public TextMeshProUGUI MessageTMP;
    public Image Frame;

    private Color _defaultColor;
    private string _title;
    public string Title
    {
        get
        {
            return _title;
        }
        set
        {
            _title = value;
            TitleTMP.text = _title;
        }
    }

    private string _message;
    public string Message
    {
        get
        {
            return _message;
        }
        set
        {
            _message = value;
            MessageTMP.text = _message;
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

        _currentState = State.FadeIn;
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
        _fadeTimer += Time.deltaTime;
        TitleTMP.alpha = _fadeTimer / FadeTime;
        MessageTMP.alpha = _fadeTimer / FadeTime;
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
    }

   

    private void None()
    {
        Frame.color = _defaultColor;
        TitleTMP.alpha = 0;
        MessageTMP.alpha = 0;
    }
}

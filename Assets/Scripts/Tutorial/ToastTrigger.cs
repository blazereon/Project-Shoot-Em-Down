using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class ToastTrigger : MonoBehaviour
{
    public enum Type
    {
        Push,
        Force
    }

    private bool IsTriggered = false;
    public Type TriggerType;
    public bool SingleUse;
    [SerializeField] public Toast.ToastMessage TriggerToastMessage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        if (IsTriggered && SingleUse) return;

        switch (TriggerType)
        {
            case Type.Push:
                UIManager.Current.PushToast(TriggerToastMessage);
                break;
            case Type.Force:
                UIManager.Current.ForceToast(TriggerToastMessage);
                break;
        }

        IsTriggered = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag != "Player") return;

    }

    void Update()
    {
        
    }
}

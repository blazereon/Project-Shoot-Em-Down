using System;
using JetBrains.Annotations;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public bool IsTriggered;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player") return;
        IsTriggered = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player") return;
        IsTriggered = false;
    }
}

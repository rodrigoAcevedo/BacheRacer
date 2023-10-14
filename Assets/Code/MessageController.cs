using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MessageType
{
    Principal,
    Secondary,
    Notification
}

public class MessageController : MonoBehaviour
{
    [SerializeField]
    private Text PrincipalMessageText;
    private void OnEnable()
    {
        Events.OnMessage.Subscribe(ShowMessage);
    }

    private void OnDisable()
    {
        Events.OnMessage.Unsubscribe(ShowMessage);
    }

    private void ShowMessage(MessageType type, string msg)
    {
        switch (type)
        {
            case MessageType.Principal:
                ShowPrincipalMessage(msg);
                break;
        }
    }

    private void ShowPrincipalMessage(string msg)
    {
        PrincipalMessageText.gameObject.SetActive(true);
        PrincipalMessageText.text = msg;
    }
}

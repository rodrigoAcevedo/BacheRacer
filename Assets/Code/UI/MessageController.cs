using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                StartCoroutine(ShowPrincipalMessage(msg));
                break;
        }
    }

    private IEnumerator ShowPrincipalMessage(string msg)
    {
        PrincipalMessageText.gameObject.SetActive(true);
        PrincipalMessageText.text = msg;
        
        yield return new WaitForSeconds(3f);
        
        PrincipalMessageText.gameObject.SetActive(false);
        PrincipalMessageText.text = String.Empty;
        yield return null;
    }
    
}

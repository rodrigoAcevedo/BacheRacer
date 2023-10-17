using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Health;

    [SerializeField] private Button NitroButton;
    [SerializeField] private TextMeshProUGUI NitroTimer;
    // Start is called before the first frame update

    private void OnEnable()
    {
        NitroButton.onClick.AddListener(OnNitroButtonClicked);
        Events.OnUpdatePlayerHealth.Subscribe(OnUpdatePlayerHealth);
    }

    private void OnDisable()
    {
        NitroButton.onClick.RemoveListener(OnNitroButtonClicked);
        Events.OnUpdatePlayerHealth.Unsubscribe(OnUpdatePlayerHealth);
    }

    private void OnUpdatePlayerHealth(string value)
    {
        Health.text = $"Health: {value}";
    }

    void Start()
    {
        Health.text = $"Health: {UserDataUtility.GetPlayerHealth()}";
        NitroButton.gameObject.SetActive(InventoryUtility.Nitro);
    }

    private void OnNitroButtonClicked()
    {
        Events.OnNitroActivated.Dispatch(true);
        PlayfabManager.Instance.SubtractCurrency("NO", 1);
        NitroButton.gameObject.SetActive(false);
        
        // Check if consumed the Nitro and see how much time is to be ready next one
    }
}

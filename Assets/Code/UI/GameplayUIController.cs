using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Health;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Events.OnUpdatePlayerHealth.Subscribe(OnUpdatePlayerHealth);
    }

    private void OnDisable()
    {
        Events.OnUpdatePlayerHealth.Unsubscribe(OnUpdatePlayerHealth);
    }

    private void OnUpdatePlayerHealth(string value)
    {
        Health.text = $"Health: {value}";
    }

    void Start()
    {
        Health.text = $"Health: {UserDataUtility.GetPlayerHealth()}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

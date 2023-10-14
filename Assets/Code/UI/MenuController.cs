using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button PlayButton;

    private void OnEnable()
    {
        PlayButton.onClick.AddListener(PlayGame);
    }

    private void OnDisable()
    {
        PlayButton.onClick.RemoveListener(PlayGame);
    }

    private void PlayGame()
    {
        
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KilometersDone;
    [SerializeField] private TextMeshProUGUI PlayerHealth;
    [SerializeField] private Button PlayButton;

    private void OnEnable()
    {
        PlayButton.onClick.AddListener(PlayGame);
    }

    private void OnDisable()
    {
        PlayButton.onClick.RemoveListener(PlayGame);
    }

    private void Start()
    {
        KilometersDone.text = $"Km: {GameManager.Instance.TotalKilometersRan}";
        PlayerHealth.text = $"Health: {GameManager.Instance.LastKnownPlayerHealth}";
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(sceneName: "Intermission");
    }
}

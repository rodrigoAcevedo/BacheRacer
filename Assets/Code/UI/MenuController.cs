using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KilometersDone;
    [SerializeField] private TextMeshProUGUI PlayerHealth;
    [SerializeField] private TextMeshProUGUI CoinsAmount;
    [SerializeField] private TextMeshProUGUI DiamondsAmount;
    [SerializeField] private Button PlayButton;

    private void OnEnable()
    {
        PlayButton.onClick.AddListener(PlayGame);
        Events.OnUpdateMenuStats.Subscribe(OnUpdateMenuStats);
    }

    private void OnDisable()
    {
        PlayButton.onClick.RemoveListener(PlayGame);
        Events.OnUpdateMenuStats.Unsubscribe(OnUpdateMenuStats);
    }

    private void Start()
    {
        OnUpdateMenuStats();
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(sceneName: "Intermission");
    }

    private void OnUpdateMenuStats()
    {
        KilometersDone.text = $"Km: {GameManager.Instance.TotalKilometersRan}";
        PlayerHealth.text = $"Health: {GameManager.Instance.LastKnownPlayerHealth}";
        CoinsAmount.text = $"CN: {GameManager.Instance.CoinsAmount}";
        DiamondsAmount.text = $"DM: {GameManager.Instance.DiamondsAmount}";

        if (GameManager.Instance.LastKnownPlayerHealth > 0)
        {
            PlayButton.gameObject.SetActive(true);
        }
    }
}

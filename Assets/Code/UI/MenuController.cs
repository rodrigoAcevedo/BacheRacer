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
    
    [SerializeField] private Button BuyNitroButton;
    [SerializeField] private TextMeshProUGUI BuyNitroTimer;
    
    [SerializeField] private Button PlayButton;

    private void OnEnable()
    {
        PlayButton.onClick.AddListener(PlayGame);
        BuyNitroButton.onClick.AddListener(BuyNitro);
        Events.OnUpdateMenuStats.Subscribe(OnUpdateMenuStats);
        Events.OnCurrencyTimedUpdated.Subscribe(OnCurrencyTimedUpdated);
    }

    private void OnDisable()
    {
        PlayButton.onClick.RemoveListener(PlayGame);
        BuyNitroButton.onClick.RemoveListener(BuyNitro);
        Events.OnUpdateMenuStats.Unsubscribe(OnUpdateMenuStats);
        Events.OnCurrencyTimedUpdated.Unsubscribe(OnCurrencyTimedUpdated);

    }

    private void PlayGame()
    {
        SceneManager.LoadScene(sceneName: "Intermission");
    }

    private void BuyNitro()
    {
        // TODO: In a future this could be a bundle or something similar.
        PlayfabManager.Instance.BuyItem("NO", 1, "DM", 1);
    }

    private void OnUpdateMenuStats()
    {
        KilometersDone.text = $"Km: {GameManager.Instance.TotalKilometersRan}";
        PlayerHealth.text = $"Health: {GameManager.Instance.LastKnownPlayerHealth}";
        CoinsAmount.text = $"CN: {InventoryUtility.Coins}";
        DiamondsAmount.text = $"DM: {InventoryUtility.Diamonds}";
        BuyNitroButton.gameObject.SetActive(!InventoryUtility.Nitro);
        BuyNitroTimer.gameObject.SetActive(!InventoryUtility.Nitro);

        if (!InventoryUtility.Nitro)
        {
            PlayfabManager.Instance.GetTimeToRecharge("NO");
        }

        if (GameManager.Instance.LastKnownPlayerHealth > 0)
        {
            PlayButton.gameObject.SetActive(true);
        }
    }

    private void OnCurrencyTimedUpdated()
    {
        
    }
}

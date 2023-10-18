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

    [SerializeField] private Button BuyHealthButton;
    
    [SerializeField] private Button PlayButton;

    private float SecondsLeftToRefreshNitro = 3600f;

    private void OnEnable()
    {
        PlayButton.onClick.AddListener(PlayGame);
        BuyNitroButton.onClick.AddListener(BuyNitro);
        BuyHealthButton.onClick.AddListener(BuyHealth);
        Events.OnUpdateMenuStats.Subscribe(OnUpdateMenuStats);
        Events.OnCurrencyTimedUpdated.Subscribe(OnCurrencyTimedUpdated);
    }

    private void OnDisable()
    {
        PlayButton.onClick.RemoveListener(PlayGame);
        BuyNitroButton.onClick.RemoveListener(BuyNitro);
        BuyHealthButton.onClick.AddListener(BuyNitro);
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
        PlayfabManager.Instance.BuyItem(GameConstants.Currencies.NITRO, 1, GameConstants.Currencies.DIAMONDS, 1);
    }

    private void BuyHealth()
    {
        UserDataUtility.RestorePlayerHealth();
    }

    private void OnUpdateMenuStats()
    {
        KilometersDone.text = $"Km: {GameManager.Instance.TotalKilometersRan}";
        PlayerHealth.text = $"Health: {GameManager.Instance.LastKnownPlayerHealth}";
        CoinsAmount.text = $"CN: {InventoryUtility.Coins}";
        DiamondsAmount.text = $"DM: {InventoryUtility.Diamonds}";
        BuyNitroButton.gameObject.SetActive(!InventoryUtility.Nitro);
        BuyNitroTimer.gameObject.SetActive(!InventoryUtility.Nitro);
        BuyHealthButton.gameObject.SetActive(GameManager.Instance.LastKnownPlayerHealth <= 0);

        if (!InventoryUtility.Nitro)
        {
            PlayfabManager.Instance.GetTimeToRecharge(GameConstants.Currencies.NITRO);
        }

        if (GameManager.Instance.LastKnownPlayerHealth > 0)
        {
            PlayButton.gameObject.SetActive(true);
        }
    }

    private void OnCurrencyTimedUpdated()
    {
        SecondsLeftToRefreshNitro = InventoryUtility.currencyData[GameConstants.Currencies.NITRO].SecondsToRecharge;
    }

    private void Update()
    {
        if (!InventoryUtility.Nitro)
        {
            SecondsLeftToRefreshNitro -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(SecondsLeftToRefreshNitro);
            BuyNitroTimer.text = time.ToString("mm':'ss");

            if (SecondsLeftToRefreshNitro <= 0)
            {
                BuyNitroButton.gameObject.SetActive(false);
                BuyNitroTimer.gameObject.SetActive(false);
                // With this we should update the inventory as we received the nitro.
                // It's quite expensive we would make an specific call for Nitro
                PlayfabManager.Instance.GetUserInventory();
            }
        }
    }
}

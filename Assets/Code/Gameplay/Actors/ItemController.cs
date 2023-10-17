using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private string currencyID;
    [SerializeField] private int amount;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameConstants.Tags.PLAYER))
        {
            PlayfabManager.Instance.AddCurrency(currencyID, amount);
        }
    }
}

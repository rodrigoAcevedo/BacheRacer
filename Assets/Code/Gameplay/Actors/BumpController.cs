using System;
using UnityEngine;

public class BumpController : MonoBehaviour
{
    [SerializeField]
    private float damage = 5f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameConstants.Tags.PLAYER))
        {
            Events.OnDamage.Dispatch(damage);
        }
    }
}

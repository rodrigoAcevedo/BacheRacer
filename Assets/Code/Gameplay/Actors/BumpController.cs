using System;
using UnityEngine;

public class BumpController : MonoBehaviour
{
    [SerializeField]
    private float damage = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameConstants.Tags.PLAYER))
        {
            Events.OnDamage.Dispatch(damage);
        }
    }
}

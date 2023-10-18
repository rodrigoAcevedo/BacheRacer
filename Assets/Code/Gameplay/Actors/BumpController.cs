using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BumpController : RoadItem
{
    [SerializeField]
    private float damage = 5f;

    [SerializeField] private List<Sprite> spriteVariants;

    private void Awake()
    {
        int index = Random.Range(0, 3);
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteVariants[index];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameConstants.Tags.PLAYER))
        {
            Events.OnDamage.Dispatch(damage);
        }
    }

}

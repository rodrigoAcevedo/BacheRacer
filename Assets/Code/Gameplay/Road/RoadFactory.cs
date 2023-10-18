using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadFactory : MonoBehaviour
{
    [SerializeField] private List<RoadItem> roadItems;

    private Dictionary<int, RoadItem> roadItemsDictionary;

    private void Awake()
    {
        roadItemsDictionary = new Dictionary<int, RoadItem>();

        foreach (RoadItem road in roadItems)
        {
            int type = (int) road.type;
            roadItemsDictionary.Add(type, road);
        }
    }

    public RoadItem CreateNewItem(int type, Transform transform)
    {
        RoadItem newItem;
        if (roadItemsDictionary.TryGetValue(type, out newItem))
        {
            return Instantiate(newItem, transform);
        }
        else
        {
            throw new System.Exception("There is no item of type = " + type);
        }
    }
}
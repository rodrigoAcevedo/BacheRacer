using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GameObject RoadPrefab;
    [SerializeField] private GameObject Player;

    [SerializeField] private Transform playerStart;
    // Start is called before the first frame update
    void Start()
    {
        Transform parent = transform.parent;
        
        GameObject playerInstance = Instantiate(Player, parent);
        playerInstance.transform.position = playerStart.position;
        
        Vector3 firstRoadPos = new Vector3(0, 0, 1);
        Instantiate(RoadPrefab, firstRoadPos, Quaternion.identity, parent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

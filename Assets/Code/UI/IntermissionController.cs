using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntermissionController : MonoBehaviour
{
    [SerializeField]
    private float transitionTime = 5f;
    [SerializeField]
    private TextMeshProUGUI CurrentKilometer;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(TransitionToGameplay), transitionTime);
        CurrentKilometer.text = $"Km: {GameManager.Instance.TotalKilometersRan}";
    }

    private void TransitionToGameplay()
    {
        SceneManager.LoadScene(sceneName: "Game");
    }
}

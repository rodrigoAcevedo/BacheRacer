using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    public Text FpsText;
    private float DeltaTime;

    void Update () {
        DeltaTime += (Time.deltaTime - DeltaTime) * 0.1f;
        float fps = 1.0f / DeltaTime;
        FpsText.text = Mathf.Ceil (fps).ToString ();
    }
}

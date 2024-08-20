using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string newScene;

    void OnTriggerEnter(Collider other)
    {
        SceneSwitching.SwitchTo(newScene);

    }
}

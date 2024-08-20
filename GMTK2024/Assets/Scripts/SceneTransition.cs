using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string newScene;
    public MusicOrchestrator orchestrator;

    void OnTriggerEnter(Collider other)
    {
        SceneSwitching.SwitchTo(newScene);
        orchestrator.transitionTrack2();
    }
}

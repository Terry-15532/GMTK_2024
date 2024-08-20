using Unity.Cinemachine;
using UnityEditor.UI;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public CinemachineCamera startMenuCamera;
    public MusicOrchestrator orchestrator;

    public void startLevel() {
        startMenuCamera.enabled = false;
        FindFirstObjectByType<MusicOrchestrator>().transitionTrack1();
        Destroy(this.gameObject);
    }
}

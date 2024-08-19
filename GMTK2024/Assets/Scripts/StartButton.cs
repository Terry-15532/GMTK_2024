using Unity.Cinemachine;
using UnityEditor.UI;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public CinemachineCamera startMenuCamera;

    public void startLevel() {
        startMenuCamera.enabled = false;
        Destroy(this.gameObject);
    }
}

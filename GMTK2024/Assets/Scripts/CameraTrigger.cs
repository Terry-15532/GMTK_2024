using Unity.Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    //CinemachineBrain brain;
    public CinemachineCamera cam;

    private void OnTriggerEnter(Collider other)
    {
        cam.Prioritize();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

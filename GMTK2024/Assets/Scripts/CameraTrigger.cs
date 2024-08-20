using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraTrigger : MonoBehaviour{
	//CinemachineBrain brain;
	public CinemachineCamera cam;

	// public void Reset(){
	// 	cam.enabled = false;
	// }
	//
	// public void Start(){
	// 	Stage.instance.resetStage.AddListener(Reset);
	// }

	private void OnTriggerEnter(Collider other){
		cam.enabled = true;
	}

	public void OnTriggerExit(Collider other){
		if (other.transform.position.x < transform.position.x){
			cam.enabled = false;
		}
	}
}
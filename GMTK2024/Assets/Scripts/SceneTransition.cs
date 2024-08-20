using UnityEngine;

public class SceneTransition : MonoBehaviour{
	public string newScene;
	// public MusicOrchestrator orchestrator;

	public int track;

	void OnTriggerEnter(Collider other){
		SceneSwitching.SwitchTo(newScene);
		// orchestrator.transitionTrack2();
		if (track == 2){
			MusicOrchestrator.instance.transitionTrack2();
		}
	}
}
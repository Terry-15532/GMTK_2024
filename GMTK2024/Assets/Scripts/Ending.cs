using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour{
	// public Animator animator;

	public CustomUIElement image;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	// void Start()
	// {
	//     
	// }
	//
	// // Update is called once per frame
	// void Update()
	// {
	//     
	// }

	void OnTriggerEnter(Collider collider){
		// animator.Play("FadeIn");
		image.SetActive(true);
		image.SetAttrAni(0, 1, 1.5f, ColorAttr.a);
		// Tools.CallDelayed(() => { image.SetAttrAni(1, 0, 3f, ColorAttr.a); }, 3f);
		Tools.CallDelayed(() => {
			// image.SetAttrAni(1, 0, 3f, ColorAttr.a);
			StartGate.started = false;
			SceneSwitching.SwitchTo("Level_1");
		}, 3f);
		// StartCoroutine(endingWait());
	}

	// IEnumerator endingWait(){
	// 	yield return new WaitForSeconds(6f);
	// 	StartGate.started = false;
	// 	SceneManager.LoadScene("Level_1");
	// }
}
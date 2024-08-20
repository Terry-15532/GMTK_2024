using System;
using UnityEngine;
using static Tools;

[RequireComponent(typeof(Outline))]
public class StartGate : CustomElement{
	public static bool started = false;

	public Outline outline;

	public Vector3 endingPos, endingScale, characterTargetPos;

	public GameObject checkPoint0;

	public void Start(){
		// Character.instance.enabled = false;
		outline = GetComponent<Outline>();
		checkPoint0.SetActive(false);
		outline.enabled = false;
	}

	public void OnMouseEnter(){
		outline.enabled = true;
	}

	public void OnMouseDown(){
		StartGame();
	}

	public void OnMouseExit(){
		outline.enabled = false;
	}

	public void StartGame(){
		checkPoint0.SetActive(true);
		SetPositionAni(endingPos, 1f, scaled: false);
		SetScaleAni(endingScale, 1f, scaled: false);
		var v = characterTargetPos - Character.instance.transform.position;
		// v.y = 0;
		Character.instance.rb.constraints = RigidbodyConstraints.FreezeRotation;
		Character.instance.rb.linearVelocity = v / 5f;
		Debug.Log(v);
		Character.instance.animator.SetBool(Character.running, true);
		CallDelayed(() => {
			Character.instance.rb.linearVelocity = Vector3.zero;
			// Character.instance.enabled = true;
			Character.instance.animator.SetBool(Character.running, false);
			Character.instance.wallGrounded = true;
			Character.instance.rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
			started = true;
		}, 5);
	}
}
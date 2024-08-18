using System;
using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour{
	public Rigidbody rb;
	public ShadowChecker checker;
	public float speed = 5;
	public bool canJump = true;
	public static Character instance;
	public bool jumpKeyDown;

	public void Awake(){
		instance = this;
		rb = GetComponent<Rigidbody>();
		checker = GetComponent<ShadowChecker>();
	}

	public void OnCollisionStay(Collision other){
		canJump = true;
	}

	public void Update(){
		jumpKeyDown = (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0));
	}

	public void FixedUpdate(){
		var v = rb.linearVelocity;
		var top = checker.HitTop(out float topDist);
		var bottom = checker.HitBottom(out float bottomDist);
		var left = checker.HitLeft(out float leftDist);
		var right = checker.HitRight(out float rightDist);


		var finalV = new Vector3(Input.GetAxis("Horizontal") * speed, v.y, 0);

		finalV.y += -9.8f * Time.fixedDeltaTime;

		if (canJump && jumpKeyDown){
			Debug.Log("AAA");
			finalV.y = 5;
			canJump = false;
		}

		if (bottom && finalV.y <= 0){
			canJump = true;
			finalV.y = top ? 5 : 0;
			transform.position += new Vector3(0, bottomDist, 0);
		}

		if (top && finalV.y > 0){
			finalV.y = 0;
			transform.position -= new Vector3(0, topDist, 0);
		}

		if (left && finalV.x < 0){
			canJump = true;
			finalV = new Vector3(0, finalV.y > 0 ? finalV.y : -1, 0);
			transform.position += new Vector3(leftDist, 0, 0);
		}

		if (right && finalV.x > 0){
			canJump = true;
			finalV = new Vector3(0, finalV.y >= 0 ? finalV.y : -1, 0);
			transform.position -= new Vector3(rightDist, 0, 0);
		}

		rb.linearVelocity = finalV;
		Debug.Log(finalV + bottom.ToString());
	}
}
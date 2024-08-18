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
		jumpKeyDown = Input.GetKeyDown(KeyCode.Space);
	}

	public void FixedUpdate(){
		var top = checker.HitTop(out float topDist);
		var bottom = checker.HitBottom(out float bottomDist);
		var left = checker.HitLeft(out float leftDist);
		var right = checker.HitRight(out float rightDist);


		var finalV = new Vector3(Input.GetAxis("Horizontal") * speed, rb.linearVelocity.y, 0);

		finalV.y += -9.8f * Time.fixedDeltaTime; //改为手动设置重力，否则即使设置速度y分量为零仍然会向下动

		if (canJump && jumpKeyDown){
			finalV.y = 5;
			canJump = false;
		}

		if (bottom && finalV.y <= 0){
			canJump = true;
			finalV.y = top ? 3 : 0;
			transform.position += new Vector3(0, bottomDist, 0);
		}

		else if (top && finalV.y > 0){
			finalV.y = 0;
			transform.position -= new Vector3(0, topDist, 0);
		}

		if (left && finalV.x < 0){
			canJump = true;
			finalV.x = 0;
			transform.position += new Vector3(leftDist, 0, 0);
		}

		if (right && finalV.x > 0){
			canJump = true;
			finalV.x = 0;
			transform.position -= new Vector3(rightDist, 0, 0);
		}

		rb.linearVelocity = finalV;
	}
}
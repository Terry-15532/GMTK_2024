using System;
using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour{
	public Rigidbody rb;
	public ShadowChecker checker;


	public void Update(){
		var v = rb.linearVelocity;
		var top = checker.HitTop();
		var bottom = checker.HitBottom();
		var left = checker.HitLeft();
		var right = checker.HitRight();

		if (bottom && v.y < 0){
			rb.linearVelocity = new Vector3(v.x, top ? 1 : 0, v.z);
		}
		else if (top && v.y > 0){
			//使用else确保不会卡住，当上下判定点都有阴影时会往上走
			rb.linearVelocity = new Vector3(v.x, 0, v.z);
		}

		if (left && v.x < 0){
			rb.linearVelocity = new Vector3(0, v.y, v.z);
		}

		else if (right && v.x > 0){
			rb.linearVelocity = new Vector3(0, v.y, v.z);
		}
	}
}
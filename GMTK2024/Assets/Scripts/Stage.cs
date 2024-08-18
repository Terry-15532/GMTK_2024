using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Stage : MonoBehaviour{
	public static int wallLayer;
	public static int platformLayer;

	public Light currLight;

	public int index;

	public static Stage instance;
	public UnityEvent updateShadow = new();

	public Material shadowColliderMat;

	public void Awake(){
		instance = this;
		wallLayer = 1 << LayerMask.NameToLayer("Wall");
		platformLayer = 1 << LayerMask.NameToLayer("Platform");
	}

	// public void Update(){
	// 	var h = Input.GetAxis("Horizontal");
	// 	var v = Input.GetAxis("Vertical");
	// 	if (h + v != 0){
	// 		currLight.transform.position += new Vector3(h, v, 0);
	// 		updateShadow.Invoke();
	// 	}
	// }
}
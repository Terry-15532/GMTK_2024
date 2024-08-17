using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Stage : MonoBehaviour{
	public Light light;

	public int index;

	public static Stage instance;
	public UnityEvent updateShadow = new();

	public Material shadowColliderMat;

	public void Awake(){
		instance = this;
	}

	public void Update(){
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");
		if (h + v != 0){
			light.transform.position += new Vector3(h, v, 0);
			updateShadow.Invoke();
		}
	}
}
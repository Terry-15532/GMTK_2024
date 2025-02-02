using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Unity.Cinemachine;

public class Stage : MonoBehaviour{
	public static int wallLayer;
	public static int platformLayer;

	[Header("初始灯光组")] public Transform[] currLights;

	public int index;

	[Header("最大缩放次数")] public int maxScalingOperation;

	[HideInInspector] public int scalingOperationLeft;

	public static bool canReset = true;

	[HideInInspector]
	public float platformZ{
		get{ return Character.instance.transform.position.z; }
	}

	public static Stage instance;
	[FormerlySerializedAs("updateShadow")] public UnityEvent updateShadowTrigger = new();
	public UnityEvent showOutline = new(), resetStage = new();

	// public Material shadowColliderMat;

	public void Awake(){
		instance = this;
		wallLayer = 1 << LayerMask.NameToLayer("Wall");
		platformLayer = 1 << LayerMask.NameToLayer("Platform");
		scalingOperationLeft = maxScalingOperation;
		// platformZ = Character.instance.transform.position.z;
	}

	private void Start(){
		showOutline.Invoke();
	}

	// public void Reset(){
	// 	resetStage.Invoke();
	// }

	// public void ShowOutline(){
	// 	showOutline.Invoke();
	// }

	public void Update(){
		if (Input.GetKeyDown(KeyCode.O)){
			showOutline.Invoke();
		}

		if (Input.GetKeyDown(KeyCode.R)){
			if (canReset){
				resetStage.Invoke();
			}
		}
	}
}
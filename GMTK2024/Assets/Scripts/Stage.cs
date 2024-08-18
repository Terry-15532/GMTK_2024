using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Stage : MonoBehaviour{
	public static int wallLayer;
	public static int platformLayer;

	public Transform currLight;

	public int index;

	public int maxScalingOperation, scalingOperationLeft;

	public static Stage instance;
	[FormerlySerializedAs("updateShadow")] public UnityEvent updateShadowTrigger = new();
	public UnityEvent showOutline = new(), resetStage = new();

	// public Material shadowColliderMat;

	public void Awake(){
		instance = this;
		wallLayer = 1 << LayerMask.NameToLayer("Wall");
		platformLayer = 1 << LayerMask.NameToLayer("Platform");
		scalingOperationLeft = maxScalingOperation;
	}

    private void Start()
    {
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
			resetStage.Invoke();
		}
	}
}
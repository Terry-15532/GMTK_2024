using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Stage : MonoBehaviour{
	public static int wallLayer;
	public static int platformLayer;

	[Header("初始灯光组")]
	public Transform[] currLights;

	public int index;

	[Header("最大缩放次数")]
	public int maxScalingOperation;
	
	[HideInInspector]
	public int scalingOperationLeft;

	[Header("平台所在面的Z坐标，用于计算角色阴影模式")]
	public float platformZ;

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
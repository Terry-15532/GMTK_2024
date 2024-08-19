using System;
using Unity.VisualScripting;
using UnityEngine;

[Tooltip("设置灯光的触发器")]
[RequireComponent(typeof(BoxCollider))]
public class SetLightTrigger : MonoBehaviour{
	[Header("玩家进入此Trigger时激活的灯光")] public Transform[] lightSourceRight;
	[Header("玩家从左侧退出此Trigger时激活的灯光")] public Transform[] lightSourceLeft;

	[Header("是否是该关卡的第一盏灯")] public bool isFirstLight;

	[HideInInspector] public BoxCollider c;

	public void Reset(){
		Stage.instance.currLights = lightSourceRight;
	}

	public void Start(){
		c = GetComponent<BoxCollider>();
		c.isTrigger = true;
		if (isFirstLight){
			Stage.instance.resetStage.AddListener(Reset);
		}
	}

	public void OnTriggerEnter(Collider other){
		if (other.name == "Character"){
			Stage.instance.currLights = lightSourceRight;
		}
	}

	public void OnTriggerExit(Collider other){
		if (other.transform.position.x < transform.position.x){
			Stage.instance.currLights = lightSourceLeft;
		}
	}
}
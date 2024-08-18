using System;
using Unity.VisualScripting;
using UnityEngine;

[Tooltip("设置灯光的触发器")]
[RequireComponent(typeof(BoxCollider))]
public class SetLightTrigger : MonoBehaviour{
	
	[Header("玩家进入此Trigger时激活的灯光")]
	public Transform lightSource;

	[HideInInspector]
	public BoxCollider c;

	public void Start(){
		c = GetComponent<BoxCollider>();
		c.isTrigger = true;
	}

	public void OnTriggerEnter(Collider other){
		if (other.name == "Character"){
			Stage.instance.currLight = lightSource;
		}
	}
}
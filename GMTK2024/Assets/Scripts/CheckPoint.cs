using System;
using UnityEngine;


public class CheckPoint : MonoBehaviour{
	// [Header("")] public Stage activeStage;

	[Header("此关卡的最大缩放次数")] public int MaxScalingCount;

	public void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "Character"){
			Character.instance.initPos = Character.instance.transform.position;
			Stage.instance.maxScalingOperation = MaxScalingCount;
			Stage.instance.scalingOperationLeft = MaxScalingCount;
		}
	}
}
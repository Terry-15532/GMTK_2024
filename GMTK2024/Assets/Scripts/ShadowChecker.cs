using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager;


public class ShadowChecker : MonoBehaviour{
	public Transform top, right, bottom, left;

	public bool HitTop(){
		return HitsShadow(top.position);
	}

	public bool HitRight(){
		return HitsShadow(right.position);
	}

	public bool HitBottom(){
		return HitsShadow(bottom.position);
	}

	public bool HitLeft(){
		return HitsShadow(left.position);
	}

	public bool HitsShadow(Vector3 pos){
		var cameraPos = GameInfo.mainCamera.transform.position;
		Ray ray = new Ray(cameraPos, (cameraPos - pos).normalized);
		if (Physics.Raycast(ray, out RaycastHit hit, 100, Stage.wallLayer)){
			ray.direction = Stage.instance.currLight.transform.position - hit.point;
			if (Physics.Raycast(ray, 1000, Stage.platformLayer)){
				return true;
			}
			else{
				return false;
			}
		}
		else{
			return false;
		}
	}
}
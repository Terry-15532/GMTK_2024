using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager;


public class ShadowChecker : MonoBehaviour{
	public Transform[] top, right, bottom, left; //监测点

	public float maxDist = 0.5f; //最大Dist(监测点与影子边缘的距离)，防止出现底部监测点在角色水平移动时进到影子里导致瞬移的情况
	public float angleBuffer = 2f;

	// public bool HitTop(out float dist){
	// 	foreach (var t in top){
	// 		if (HitsShadow(t.position)){
	// 			var pos = t.position;
	// 			do{
	// 				pos.y += Time.fixedDeltaTime;
	// 			} while (pos.y < t.position.y + 10 && !HitsShadow(t.position));
	//
	// 			dist = pos.y - t.position.y;
	// 			return true;
	// 		}
	// 	}
	// 	dist = 0;
	// 	return false;
	// }
	//
	// public bool HitRight(){
	// 	foreach (var t in right){
	// 		if (HitsShadow(t.position)){
	// 			return true;
	// 		}
	// 	}
	//
	// 	return false;
	// }
	//
	// public bool HitBottom(){
	// 	foreach (var t in bottom){
	// 		if (HitsShadow(t.position)){
	// 			return true;
	// 		}
	// 	}
	//
	// 	return false;
	// }
	//
	// public bool HitLeft(){
	// 	foreach (var t in left){
	// 		if (HitsShadow(t.position)){
	// 			return true;
	// 		}
	// 	}
	//
	// 	return false;
	// }
	public bool HitTop(out float dist){
		//out的Dist用于在Character里面设置位置，防止出现抖动
		foreach (var t in top){
			if (HitsShadow(t.position)){
				if (HitsShadow(new Vector3(t.position.x, t.position.y - maxDist, t.position.z))){
					dist = 0;
					return true;
				}

				float low = t.position.y;
				float high = t.position.y - maxDist;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					float mid = (low + high) / 2;
					var pos = t.position;
					pos.y = mid;
					if (HitsShadow(pos)){
						low = mid;
					}
					else{
						high = mid;
					}
				}

				dist = t.position.y - low;
				return true;
			}
		}

		dist = 0;
		return false;
	}

	public bool HitRight(out float dist){
		foreach (var t in right){
			if (HitsShadow(t.position)){
				if (HitsShadow(new Vector3(t.position.x - maxDist, t.position.y, t.position.z))){
					dist = 0;
					return true;
				}

				float low = t.position.x;
				float high = t.position.x - maxDist;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					float mid = (low + high) / 2;
					var pos = t.position;
					pos.x = mid;
					if (HitsShadow(pos)){
						low = mid;
					}
					else{
						high = mid;
					}
				}

				dist = t.position.x - low;
				return true;
			}
		}

		dist = 0;
		return false;
	}

	public bool HitBottom(out float dist){
		foreach (var t in bottom){
			if (HitsShadow(t.position)){
				if (HitsShadow(new Vector3(t.position.x, t.position.y + maxDist, t.position.z))){
					dist = 0;
					return true;
				}

				float low = t.position.y;
				float high = t.position.y + maxDist;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					float mid = (low + high) / 2;
					var pos = t.position;
					pos.y = mid;
					if (HitsShadow(pos)){
						low = mid;
					}
					else{
						high = mid;
					}
				}

				dist = low - t.position.y;
				return true;
			}
		}

		dist = 0;
		return false;
	}

	public bool HitLeft(out float dist){
		foreach (var t in left){
			if (HitsShadow(t.position)){
				if (HitsShadow(new Vector3(t.position.x + maxDist, t.position.y, t.position.z))){
					dist = 0;
					return true;
				}

				float low = t.position.x;
				float high = t.position.x + maxDist;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					float mid = (low + high) / 2;
					var pos = t.position;
					pos.x = mid;
					if (HitsShadow(pos)){
						low = mid;
					}
					else{
						high = mid;
					}
				}

				dist = low - t.position.x;
				return true;
			}
		}

		dist = 0;
		return false;
	}


	public bool HitsShadow(Vector3 pos){
		var cameraPos = GameInfo.mainCamera.transform.position;
		Ray ray = new Ray(cameraPos, (pos - cameraPos).normalized);
		// Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 0.01f);


		if (Physics.Raycast(ray, out RaycastHit hit, 100, Stage.wallLayer)){
			foreach (var l in Stage.instance.currLights){
				ray.direction = l.position - hit.point;
				ray.origin = hit.point;
				// Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 0.01f);
				// Vector3 normal = Vector3.forward;
				Vector3 normal = l.transform.forward;

				float angle = Vector3.Angle(-ray.direction, normal);
				float light_angle = l.GetComponent<Light>().innerSpotAngle / 2;
				if ((Physics.Raycast(ray, 1000, Stage.platformLayer) && InLight(ray, l))){
					return true;
				}
			}
		}

		return false;
	}

	public bool InLight(Ray ray, Transform l){
		Vector3 normal = l.transform.forward;
		float angle = Vector3.Angle(-ray.direction, normal);
		float light_angle = l.GetComponent<Light>().innerSpotAngle / 2;
		return Mathf.Abs(angle) + angleBuffer <= light_angle;
	}

	public bool InAnyLight(Ray ray){
		foreach (var l in Stage.instance.currLights){
			if (InLight(ray, l)){
				return true;
			}
		}

		return false;
	}
}
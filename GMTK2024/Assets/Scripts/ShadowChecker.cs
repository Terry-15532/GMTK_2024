using UnityEngine;


public class ShadowChecker : MonoBehaviour{
	public Transform[] top, right, bottom, left; //监测点

	public float maxDist = 0.5f; //最大Dist(监测点与影子边缘的距离)，防止出现底部监测点在角色水平移动时进到影子里导致瞬移的情况
	public float angleBuffer = 0.5f;

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
			if (HitsShadow(t.position, out Transform light)){
				if (HitsShadow(new Vector3(t.position.x, t.position.y - maxDist, t.position.z), out _, light)){
					dist = 0;
					return true;
				}

				float low = t.position.y;
				float high = t.position.y - maxDist;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					float mid = (low + high) / 2;
					var pos = t.position;
					pos.y = mid;
					if (HitsShadow(pos, out _, light)){
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
			if (HitsShadow(t.position, out Transform light)){
				if (HitsShadow(new Vector3(t.position.x - maxDist, t.position.y, t.position.z), out _, light)){
					dist = 0;
					return true;
				}

				float low = t.position.x;
				float high = t.position.x - maxDist;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					float mid = (low + high) / 2;
					var pos = t.position;
					pos.x = mid;
					if (HitsShadow(pos, out _)){
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
			if (HitsShadow(t.position, out Transform light)){
				Character.instance.inLight = light;

				if (HitsShadow(new Vector3(t.position.x, t.position.y + maxDist, t.position.z), out _, light)){
					dist = 0;
					return true;
				}

				float low = t.position.y;
				float high = t.position.y + maxDist;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					float mid = (low + high) / 2;
					var pos = t.position;
					pos.y = mid;
					if (HitsShadow(pos, out _, light)){
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
			if (HitsShadow(t.position, out Transform light)){
				if (HitsShadow(new Vector3(t.position.x + maxDist, t.position.y, t.position.z), out _, light)){
					dist = 0;
					return true;
				}

				float low = t.position.x;
				float high = t.position.x + maxDist;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					float mid = (low + high) / 2;
					var pos = t.position;
					pos.x = mid;
					if (HitsShadow(pos, out _, light)){
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


	public bool HitsShadow(Vector3 pos, out Transform light, Transform inLight = null){
		var cameraPos = GameInfo.mainCamera.transform.position;
		Ray ray = new Ray(cameraPos, (pos - cameraPos).normalized);
		// Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 0.01f);


		if (Physics.Raycast(ray, out RaycastHit hit, 100, Stage.wallLayer)){
			if (inLight != null){
				ray.direction = inLight.position - hit.point;
				ray.origin = hit.point;
				if (InLight(ray, inLight) && Physics.Raycast(ray, out hit, 1000, Stage.platformLayer)){
					light = inLight;
					return true;
				}
			}
			else{
				ray.origin = hit.point;
				foreach (var l in Stage.instance.currLights){
					ray.direction = l.position - hit.point;
					if (InLight(ray, l) && Physics.Raycast(ray, out var hit2, 1000, Stage.platformLayer)){
						Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);
						Debug.DrawLine(ray.origin, ray.origin + new Vector3(0, 5, 0), Color.green);
						Debug.DrawLine(hit2.point, hit2.point + new Vector3(0, 5, 0), Color.magenta);
						light = l;
						return true;
					}
				}
			}
		}

		light = null;
		return false;
	}

	public bool InLight(Ray ray, Transform l){
		Vector3 normal = l.transform.forward;
		float angle = Vector3.Angle(-ray.direction, normal);
		float light_angle = l.GetComponent<Light>().spotAngle / 2;
		return Mathf.Abs(angle) - angleBuffer <= light_angle;
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
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager;


public class ShadowChecker : MonoBehaviour{
	public Transform[] top, right, bottom, left;

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
		foreach (var t in top){
			if (HitsShadow(t.position)){
				if (HitsShadow(new Vector3(t.position.x, t.position.y - 0.1f, t.position.z))){
					dist = 0;
					return true;
				}

				float low = t.position.y;
				float high = t.position.y - 0.1f;
				float mid = low;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					mid = (low + high) / 2;
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
				if (HitsShadow(new Vector3(t.position.x - 0.1f, t.position.y, t.position.z))){
					dist = 0;
					return true;
				}

				float low = t.position.x;
				float high = t.position.x - 0.1f;
				float mid = low;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					mid = (low + high) / 2;
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
				if (HitsShadow(new Vector3(t.position.x, t.position.y + 0.1f, t.position.z))){
					dist = 0;
					return true;
				}

				float low = t.position.y;
				float high = t.position.y + 0.1f;
				float mid = high;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					mid = (low + high) / 2;
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
				if (HitsShadow(new Vector3(t.position.x + 0.1f, t.position.y, t.position.z))){
					dist = 0;
					return true;
				}

				float low = t.position.x;
				float high = t.position.x + 0.1f;
				float mid = high;
				while (Mathf.Abs(high - low) > Time.deltaTime / 10){
					mid = (low + high) / 2;
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
			ray.direction = Stage.instance.currLight.transform.position - hit.point;
			ray.origin = hit.point;
			// Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 0.01f);
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
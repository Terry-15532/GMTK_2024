using UnityEngine;

[Tooltip("设置灯光的触发器")]
[RequireComponent(typeof(BoxCollider))]
public class SetLightTrigger : MonoBehaviour{
	[Header("玩家进入此Trigger时激活的灯光")] public Transform[] lightSourceRight;
	[HideInInspector] public Transform[] lightSourceLeft;

	// [Header("是否是该关卡的第一盏灯")] public bool isFirstLight;

	// [HideInInspector] public BoxCollider c;

	// public void Reset(){
	// 	Stage.instance.currLights = lightSourceRight;
	// }

	public void Start(){
		// c = GetComponent<BoxCollider>();
		// c.isTrigger = true;
		// if (isFirstLight){
		// 	Stage.instance.resetStage.AddListener(Reset);
		// }

		foreach (var l in lightSourceRight){
			l.GetComponent<Light>().enabled = false;
			// l.GetComponent<Light>().shadows = LightShadows.None;
		}
	}

	public void OnTriggerEnter(Collider other){
		if (other.name == "Character"){
			if (other.transform.position.x < transform.position.x){
				lightSourceLeft = Stage.instance.currLights;
			}

			Stage.instance.currLights = lightSourceRight;
			foreach (var l in lightSourceLeft){
				l.GetComponent<Light>().enabled = false;
				// l.GetComponent<Light>().shadows = LightShadows.None;
			}

			foreach (var l in lightSourceRight){
				l.GetComponent<Light>().enabled = true;
				// l.GetComponent<Light>().shadows = LightShadows.Hard;
			}
		}
	}

	public void OnTriggerExit(Collider other){
		if (other.transform.position.x < transform.position.x){
			Stage.instance.currLights = lightSourceLeft;
			foreach (var l in lightSourceLeft){
				l.GetComponent<Light>().enabled = true;
				// l.GetComponent<Light>().shadows = LightShadows.Hard;
			}

			foreach (var l in lightSourceRight){
				l.GetComponent<Light>().enabled = false;
				// l.GetComponent<Light>().shadows = LightShadows.None;
			}
		}
	}
}
using UnityEngine;
using System;
using UnityEngine.UIElements;

[RequireComponent(typeof(Outline))]
public class MovableLamp : MonoBehaviour{
	[Header("最大Z坐标")] public float maxZ;
	[Header("最小Z坐标")] public float minZ;
	[Header("滚轮移动Z坐标的速度")] public float scrollSpeed;
	[HideInInspector] public Vector3 initPos;
	[HideInInspector] public bool isDragging;

	[HideInInspector] public Outline outline;


	public void ResetPos(){
		transform.position = initPos;
	}

	public void Start(){
		initPos = transform.position;
		Stage.instance.resetStage.AddListener(ResetPos);
		Stage.instance.showOutline.AddListener(ShowOutline);
		outline = GetComponent<Outline>();
	}
	
	public void ShowOutline(){
		outline.enabled = true;
		Tools.CallDelayed(() => { outline.enabled = false; }, 1);
	}

	public void OnMouseDown(){
		isDragging = !isDragging;
	}

	public void Update(){
		if (isDragging){
			Ray ray = GameInfo.mainCamera.ScreenPointToRay(Input.mousePosition);
			var pos = ray.GetPoint((transform.position - GameInfo.mainCamera.transform.position).magnitude);
			var z = Mathf.Clamp(transform.position.z + Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, minZ, maxZ);
			transform.position = new Vector3(pos.x, pos.y, z);
		}
	}

	public void OnMouseEnter(){
		outline.enabled = true;
	}

	public void OnMouseExit(){
		outline.enabled = false;
	}
}
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
	public Character character;
	public bool mouseOn = false;


	public void ResetPos(){
		transform.position = initPos;
	}

	public void Start(){
		outline = GetComponent<Outline>();
		outline.enabled = false;
		initPos = transform.position;
		Stage.instance.resetStage.AddListener(ResetPos);
		Stage.instance.showOutline.AddListener(ShowOutline);
		
	}
	
	public void ShowOutline(){
		outline.enabled = true;
		Tools.CallDelayed(() => { outline.enabled = false; }, 1);
	}

	public void OnMouseDown(){
		if (character.canMoveLight) {
			isDragging = !isDragging;
		}
	}

	public void Update(){
		if (isDragging){
			character.movingLight = true;
			Ray ray = GameInfo.mainCamera.ScreenPointToRay(Input.mousePosition);
			var pos = ray.GetPoint((transform.position - GameInfo.mainCamera.transform.position).magnitude);
			var z = Mathf.Clamp(transform.position.z + Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, minZ, maxZ);
			transform.position = new Vector3(pos.x, pos.y, z);
		} else {
			character.movingLight = false;
		}
	}

	public void OnMouseEnter(){
		mouseOn = true;
		Debug.Log(mouseOn);
		if (character.canMoveLight) {
			outline.enabled = true;
		}
	}

	public void OnMouseExit(){
		mouseOn = false;
		Debug.Log(mouseOn);
		outline.enabled = false;
	}


}
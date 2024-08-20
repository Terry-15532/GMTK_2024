using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class ScalableObject : CustomElement{
	[HideInInspector] public Vector3 initScale;

	[Header("单次缩放比例")] public float scaleRatio = 2;
	[Header("最大缩放比例")] public float maxScaleRatio = 2f;
	[Header("最小缩放比例")] public float minScaleRatio = 0.5f;

	[Header("缩放动画时长")] public float aniDuration = 0.5f;

	[Header("初始大小")] public float currScale;

	[Header("始终显示描边")] public bool showOutlineAlways = true;

	[HideInInspector] public Outline outline; //这是QuickOutline插件，显示描边用的
	[HideInInspector] public bool mouseOver = false;

	public void Start(){
		outline = GetComponent<Outline>();
		outline.OutlineColor = Color.white;
		outline.enabled = false;
		mouseOver = false;
		initScale = transform.localScale;
		currScale = 1;
		Stage.instance.showOutline.AddListener(ShowOutline);
		Stage.instance.resetStage.AddListener(ResetScale);
	}

	public void ResetScale(){
		StopAllCoroutines();
		currScale = 1;
		transform.localScale = initScale;
	}

	public void Update(){
		if (showOutlineAlways && !outline.enabled){
			outline.enabled = true;
		}

		if (mouseOver){
			if (currScale < maxScaleRatio && Input.GetKeyDown(KeyCode.Mouse0)){
				// Debug.Log("Larger");
				if (Stage.instance.scalingOperationLeft > 0){
					currScale *= scaleRatio;
					Stage.instance.scalingOperationLeft--;
					UpdateScale();
				}
			}
			else if (currScale > minScaleRatio && Input.GetKeyDown(KeyCode.Mouse1)){
				// Debug.Log("Smaller");
				if (Stage.instance.scalingOperationLeft > 0){
					currScale /= scaleRatio;
					Stage.instance.scalingOperationLeft--;
					UpdateScale();
				}
			}
		}
	}

	public void OnMouseEnter(){
		outline.enabled = true;
		mouseOver = true;
	}

	public void OnMouseExit(){
		outline.enabled = false;
		mouseOver = false;
	}

	public void ShowOutline(){
		outline.enabled = true;
		Tools.CallDelayed(() => { outline.enabled = false; }, 1);
	}

	public void UpdateScale(){
		var s = initScale * currScale;
		s.z = initScale.z;
		SetScaleAni(s, aniDuration, scaled: false);
	}
}
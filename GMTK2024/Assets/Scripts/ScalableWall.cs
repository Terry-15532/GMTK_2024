// using System;
// using UnityEngine;
//
// public class ScalableWall : CustomElement{
// 	public bool isLarger = false;
// 	public Vector3 initScale;
// 	public float scaleRatio = 2;
// 	public float aniDuration;
//
// 	public Outline outline; //Outline是QuickOutline插件，显示描边用的
//
// 	public override void Awake(){
// 		base.Awake();
// 		initScale = transform.localScale;
// 		outline = GetComponent<Outline>();
// 		outline.OutlineColor = Color.white;
// 		outline.enabled = false;
// 	}
//
//
//
// 	public void OnMouseUpAsButton(){
// 		outline.OutlineColor = Color.white;
// 		isLarger = !isLarger;
// 		if (isLarger){
// 			SetScaleAni(initScale * scaleRatio, aniDuration, scaled: false); //这是以前不知道有DoTween，在CustomElement里面自己写的一个平滑设置大小的函数。最后一个scaled参数是指是否受TimeScale影响
// 		}
// 		else{
// 			SetScaleAni(initScale, aniDuration, scaled: false);
// 		}
// 	}
//
// 	public void OnMouseExit(){
// 		outline.enabled = false;
// 	}
// }
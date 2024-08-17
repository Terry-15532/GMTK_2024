using System;
using TMPro;
using UnityEngine;
using static GameInfo;
using static Tools;

public enum MsgBoxType{
	msg,
	input
}

public class MsgBox : CustomUIElement{
	public MsgBoxType type;
	public TMP_InputField inputField;
	public CustomUIElement background, box;
	public TextMeshProUGUI msg;
	public Action<bool, string> buttonPressed;
	public bool canBeClosed, hidden = false;
	public static bool msgShowing = false;

	public static MsgBox Create(string s, MsgBoxType t, Action<bool, string> buttonPressedAction = null, string defaultContent = "", string placeHolder = ""){
		if (!msgShowing){
			msgShowing = true;
			var msgBox = Instantiate(Resources.Load<MsgBox>("Prefabs/MsgBox"), canvasRectTransform);
			var v = msgBox.position;
			v.z = 0;
			msgBox.position = v;
			msgBox.type = t;
			msgBox.msg.text = s;
			msgBox.buttonPressed = buttonPressedAction;
			msgBox.inputField.text = defaultContent;
			msgBox.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = placeHolder;
			msgBox.Show();
			return msgBox;
		}
		else return null;
	}

	public void Show(){
		// var v = position;
		// v.z = 0;
		// position = v;
		position = new Vector2(0, 0);
		hidden = false;
		scale = new Vector2(1, 1);
		background.SetActive(true);
		box.SetActive(true);
		background.SetAttrAni(0, 0.5f, 0.4f, ColorAttr.a);
		box.SetAttrAni(0, 1, 0.2f, ColorAttr.a);
		box.position = new Vector2(-2500, 0);
		box.SetPositionAni(new Vector2(0, 0), 0.2f);
		inputField.Select();
		if (type == MsgBoxType.msg){
			inputField.gameObject.SetActive(false);
			msg.rectTransform.anchoredPosition = new Vector2(0, 0);
		}
	}

	public void Hide(bool confirm){
		var v = transform.position;
		v.z = 0;
		transform.position = v;
		if (!hidden){
			hidden = true;
			background.SetAttrAni(0, 0.4f, ColorAttr.a, true);
			box.SetAttrAni(0, 0.2f, ColorAttr.a, true);
			box.SetPositionAni(new Vector2((confirm ? 1 : -1) * 2500, 0), 0.2f, true);
			CallDelayedAsync(() => msgShowing = false, 0.1f);
			CallDelayedAsync(() => {
				try{
					Destroy(this.gameObject);
				}
				catch{ }
			}, 0.5f);
		}
	}

	public void Confirm(){
		canBeClosed = true;
		buttonPressed?.Invoke(true, inputField.text);
		if (canBeClosed) Hide(true);
	}

	public void Update(){
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Z)){
			Cancel();
		}

		if (Input.GetKeyDown(KeyCode.Return)){
			Confirm();
		}
	}

	public void Cancel(){
		canBeClosed = true;
		buttonPressed?.Invoke(false, inputField.text);
		if (canBeClosed){
			Hide(false);
		}
	}
}
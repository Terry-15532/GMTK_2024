using System;
using UnityEngine;


[Serializable]
public class Settings : MonoBehaviour{
	public static Language language;

	public void Start(){
		UpdatePrefs();
	}

	public static void SavePrefs(){
		PlayerPrefs.SetInt("Language", (int)language);
	}

	public static void UpdatePrefs(){
		try{
			language = (Language)PlayerPrefs.GetInt("Language");
		}
		catch{
			//ignored}
		}
	}

	public static void SetLanguage(int i){
		language = (Language)i;
		SavePrefs();
		TextLocalization.UpdateTexts();
	}


	public static void SetFullScreen(bool b){
		if (b){
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, b);
		}
		else{
			Screen.SetResolution((int)(Screen.currentResolution.width * 0.8f), (int)(Screen.currentResolution.height * 0.8f), b);
		}
	}
}
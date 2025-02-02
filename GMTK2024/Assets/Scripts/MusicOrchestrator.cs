using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOrchestrator : MonoBehaviour{
	public AudioSource menu;
	public AudioSource music1;
	public AudioSource music1loop;
	public AudioSource music2;
	public AudioSource music2loop;
	public AudioSource music3;
	public AudioSource music3loop;

	public AudioSource musicFinal;
	// private static bool autoPlay1;
	// private static bool autoPlay2;

	public static MusicOrchestrator instance;

	private void Awake(){
		if (instance == null){
			instance = this;
			DontDestroyOnLoad(this);
			// autoPlay1 = false;
			// autoPlay2 = false;
		}
		else if (instance != this){
			Destroy(gameObject);
		}
		//Debug.Log("Finished music orch awake");
	}

	// Start is called before the first frame update
	void Start(){
		menu.Play();
	}

	// For playing loops after the intro
	void Update(){
		// Debug.Log("autoplay1: " + autoPlay1 + "autoplay2: " + autoPlay2);
		// if (music1 != null && !music1.isPlaying && !music1loop.isPlaying && autoPlay1)
		// {
		//     music1loop.Play();
		// }
		// if (music2 != null && !music2.isPlaying && !music2loop.isPlaying && autoPlay2)
		// {
		//     music2loop.Play();
		// }
	}

	public void transitionTrack0(){
		if (music1 != null && music1.isPlaying){
			StartCoroutine(FadeOut(music1, 1f, false));
		}

		if (music1loop != null && music1loop.isPlaying){
			StartCoroutine(FadeOut(music1loop, 1f, false));
		}

		if (music2 != null && music2.isPlaying){
			StartCoroutine(FadeOut(music2, 1f, false));
		}

		if (music2loop != null && music2loop.isPlaying){
			StartCoroutine(FadeOut(music2loop, 1f, false));
		}

		if (menu != null){
			StartCoroutine(pauseBeforePlay(menu, 0.65f));
		}
	}

	public void transitionTrack1(){
		// autoPlay2 = false;
		if (menu != null && menu.isPlaying){
			StartCoroutine(FadeOut(menu, 1f, false));
		}

		StartCoroutine(pauseBeforePlay(music1, 0.8f));
		if (music1loop != null){
			StartCoroutine(pauseBeforePlay(music1loop, 0.65f + music1.clip.length));
		}
	}

	public void transitionTrack2(){
		// autoPlay1 = false;
		if (menu != null && menu.isPlaying){
			StartCoroutine(FadeOut(menu, 1f, false));
		}

		if (music1 != null && music1.isPlaying){
			StartCoroutine(FadeOut(music1, 1f, false));
		}

		if (music1loop != null && music1loop.isPlaying){
			StartCoroutine(FadeOut(music1loop, 1f, false));
		}

		StartCoroutine(pauseBeforePlay(music2, 0.8f));
		if (music2loop != null){
			StartCoroutine(pauseBeforePlay(music2loop, 0.65f + music2.clip.length));
		}
	}

	public void transitionTrack3(){
		if (menu != null && menu.isPlaying){
			StartCoroutine(FadeOut(menu, 1f, false));
		}

		if (music1 != null && music1.isPlaying){
			StartCoroutine(FadeOut(music1, 1f, false));
		}

		if (music1loop != null && music1loop.isPlaying){
			StartCoroutine(FadeOut(music1loop, 1f, false));
		}

		if (music2 != null && music2.isPlaying){
			StartCoroutine(FadeOut(music2, 1f, false));
		}

		if (music2loop != null && music2loop.isPlaying){
			StartCoroutine(FadeOut(music2loop, 1f, false));
		}

		StartCoroutine(pauseBeforePlay(music3, 0.8f));
		if (music3loop != null){
			StartCoroutine(pauseBeforePlay(music3loop, 0.65f + music3.clip.length));
		}
	}

	public void transitionEnd(){
		if (music1 != null && music1.isPlaying){
			StartCoroutine(FadeOut(music1, 1f, false));
		}

		if (music1loop != null && music1loop.isPlaying){
			StartCoroutine(FadeOut(music1loop, 1f, false));
		}

		if (music2 != null && music2.isPlaying){
			StartCoroutine(FadeOut(music2, 1f, false));
		}

		if (music2loop != null && music2loop.isPlaying){
			StartCoroutine(FadeOut(music2loop, 1f, false));
		}

		StartCoroutine(pauseBeforePlay(musicFinal, 1f));
	}

	public void endMusic(){
		if (musicFinal.isActiveAndEnabled && musicFinal.isPlaying){
			StartCoroutine(FadeOut(musicFinal, 1f, true));
		}
	}

	IEnumerator FadeOut(AudioSource audioSource, float FadeTime, bool destroy){
		float startVolume = audioSource.volume;
		while (audioSource.volume > 0){
			//Debug.Log(audioSource.volume);
			audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
			yield return null;
		}

		audioSource.Stop();
		audioSource.volume = startVolume;
		if (destroy){
			Destroy(gameObject);
		}
	}

	IEnumerator pauseBeforePlay(AudioSource source, float pause){
		yield return new WaitForSeconds(pause);
		source.Play();
	}

	//IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
	//{
	//    float endVolume = audioSource.volume;
	//    audioSource.volume = 0;
	//    audioSource.Play();
	//    while (audioSource.volume < endVolume)
	//    {
	//        //Debug.Log(audioSource.volume);
	//        audioSource.volume += endVolume * Time.deltaTime / FadeTime;
	//        yield return null;
	//    }
	//}
}
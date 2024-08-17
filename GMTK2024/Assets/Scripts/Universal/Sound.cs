using UnityEngine;
using static Tools;

public class Sound : MonoBehaviour{
	public Coroutine volumeFade;

	void Start(){
		DontDestroyOnLoad(this);
	}

	public AudioSource audioSource;

	public void PlaySound(AudioClip clip, float delay = 0, bool loop = false){
		audioSource.clip = clip;
		audioSource.loop = loop;
		if (delay > 0)
			CallDelayedAsync(() => audioSource.Play(), delay);
		else audioSource.Play();
	}

	public void SetVolumeSmooth(float endVolume, float time){
		if (volumeFade != null){
			StopCoroutine(volumeFade);
		}

		float currV = audioSource.volume;

		volumeFade = StartCoroutine(Tools.Repeat((f) => { audioSource.volume = currV.Lerp(endVolume, f); }, time, new WaitForEndOfFrame(), scaled: false));
	}
}
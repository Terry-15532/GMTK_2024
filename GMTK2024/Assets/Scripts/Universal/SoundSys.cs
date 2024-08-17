using UnityEngine;
using static Tools;

public class SoundSys : MonoBehaviour {

    public static bool BGMPlayed;

    public static Sound PlaySound(string name, bool loop = false, float delay = 0, bool threeD = false, float volume = 1) {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + name);
        if (clip) {
            //if (name.Contains("BGM")) {
            //    if (BGMPlayed == true) {
            //        return null;
            //    }
            //    else BGMPlayed = true;
            //}
            var sound = (Sound)Create("Sound");
            sound.PlaySound(clip, delay, loop);
            if (threeD) {
                sound.audioSource.spatialBlend = 1;
            }
            else {
                sound.audioSource.spatialBlend = 0;
            }
            if (!loop) {
                CallDelayedAsync(() => {
                    DestroyImmediate(sound.gameObject);
                }, clip.length);
            }

            sound.audioSource.volume = volume;
            return sound;
        }
        return null;
    }
    

    public static Sound PlaySound(string name, Vector3 pos, bool loop = false, float delay = 0, bool threeD = false) {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + name);
        if (clip) {
            var sound = (Sound)Create("Sound");
            sound.transform.position = pos;
            sound.PlaySound(clip, delay, loop);
            if (threeD) {
                sound.audioSource.spatialBlend = 1;
            }
            else {
                sound.audioSource.spatialBlend = 0;
            }

            if (!loop) {
                CallDelayedAsync(() => {
                    DestroyImmediate(sound.gameObject);
                }, clip.length);
            }
            return sound;
        }
        return null;
    }
}
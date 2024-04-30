using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioExtensions
{
    public static AudioSource PlayClipAtPointWithPitch(AudioClip clip, Vector3 position, float volume, float pitch)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        AudioSource tempASource = tempGO.AddComponent<AudioSource>();
        tempASource.clip = clip;
        tempASource.volume = volume;
        tempASource.pitch = pitch;

        tempASource.Play();

        Object.Destroy(tempGO, clip.length);

        return tempASource;
    }
}

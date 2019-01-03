using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop;

    [Range(0f, 1f)]
    public float volume;

    [Range (0.1f, 3f)]
    public float pitch;

    public bool randomizePitch = false;

    public float randomPitchRange = 0.2f;

    [HideInInspector]
    public AudioSource source;

}

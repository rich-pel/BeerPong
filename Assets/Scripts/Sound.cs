using UnityEngine.Audio;
using UnityEngine;

//costum class should appear in inspector 
[System.Serializable]

public class Sound  
{
 public string name;
 
 public AudioClip clip;

 [Range(0f, 1f)]
 public float volume;

 [Range(.1f, 3f)]
 public float pitch;

 public bool loop;

// variable sound to work with in audiomanager script, do not need to see it
 [HideInInspector]
 public AudioSource source;
}
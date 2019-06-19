using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
 public Sound[] sounds;
 // static reference to the current instance of our audiomanager we have in our scene
 public static AudioManager instance;   
    void Awake() { 
    	// if we have no instance (audiomanager) in our scene we use that instance of the gameobject, if not meaning we have already audiomanager in our scene we want to remove the gameobject
    	if (instance == null)
    		instance = this;
    	else 
		{ Destroy(gameObject);
			return;
		}

    	DontDestroyOnLoad(gameObject);


    // s is the sound we are looking for
     foreach (Sound s in sounds)
     { // store audiosource in variable source 
      s.source = gameObject.AddComponent<AudioSource>();
      
      s.source.clip = s.clip;
      s.source.volume = s.volume;
      s.source.pitch = s.pitch;
      s.source.loop = s.loop;

     }
        
    }

  // play background theme all the time  
  public void Start () {
   
  	Play("Background Theme");
  }

  // loop through all our sounds and find the right one
  // then play this sound
  // if there is no sound with this name then return
  public void Play (string name) {
  	
  	Sound s = Array.Find(sounds, sound => sound.name == name);
  	if (s == null)
  		return;
    s.source.Play();
  }
}
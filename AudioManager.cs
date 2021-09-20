using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public Sound[] sounds;

	public static GameObject instance;

	void Awake(){

		if(instance == null)
			instance = this.gameObject;
		else{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		foreach(Sound s in sounds){
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}	

		Play("Theme"); 
	}

	public void Play(string name){
		if(PlayerPrefs.GetInt("Music") == 1)
			 return;

		Sound s = Array.Find(sounds, sounds => sounds.name == name);
		if(s == null){
			Debug.LogWarning("The Audio File " + name + " could not be found.");
			return;
		}
		s.source.Play();
	}

	public void Stop(string name){
		Sound s = Array.Find(sounds, sounds => sounds.name == name);
		if(s == null){
			Debug.LogWarning("The Audio File " + name + " could not be found.");
			return;
		}
		s.source.Pause();
	}
}

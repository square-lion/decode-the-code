using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour {

	public void GameOverScreenEntered(){
		GetComponent<Animator>().SetBool("MovingIn", false);
	}
	public void GameOverScreenExiting(){
		GetComponent<Animator>().SetBool("MovingOut", false);
		gameObject.SetActive(false);
	}
}

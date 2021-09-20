using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

	Image choice1, choice2, choice3, choice4, choice5, choice6;

	void Awake(){
		choice1 = transform.GetChild(2).GetComponent<Image>();
		choice2 = transform.GetChild(3).GetComponent<Image>();
		choice3 = transform.GetChild(4).GetComponent<Image>();
		choice4 = transform.GetChild(5).GetComponent<Image>();
		choice5 = transform.GetChild(6).GetComponent<Image>();
		choice6 = transform.GetChild(7).GetComponent<Image>();
	}

	void Update () {
		int packID = PlayerPrefs.GetInt("PackID");
		choice1.sprite = Resources.Load<Sprite>("Pack " + (packID) + "/Choice1");
		choice2.sprite = Resources.Load<Sprite>("Pack " + (packID) + "/Choice2");
		choice3.sprite = Resources.Load<Sprite>("Pack " + (packID) + "/Choice3");
		choice4.sprite = Resources.Load<Sprite>("Pack " + (packID) + "/Choice4");
		choice5.sprite = Resources.Load<Sprite>("Pack " + (packID) + "/Choice5");
		choice6.sprite = Resources.Load<Sprite>("Pack " + (packID) + "/Choice6");
	}
}

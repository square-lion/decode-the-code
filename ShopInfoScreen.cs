using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInfoScreen : MonoBehaviour {

	public Text nameText;
	public List<Image> choices;
	public Text priceText;

	public Sprite select;
	public Sprite selected;

	public ShopPack curPack;
	ShopManager shopManager;

	void Awake(){
		shopManager = FindObjectOfType<ShopManager>();
	}

	public void DisplayInfo(ShopPack pack){
		curPack = pack;

		nameText.text = pack.packName;

		if(PlayerPrefs.GetInt("PackID") == pack.packID){
			priceText.text = "Selected";
			priceText.fontSize = 25;
			priceText.transform.parent.GetComponent<Image>().sprite = selected;
		}
		else if(pack.bought){
			priceText.text = "Select";
			priceText.fontSize = 25;
			priceText.transform.parent.GetComponent<Image>().sprite = select;
		}else{
			//priceText.text = "$" + pack.packPrice;
			priceText.fontSize = 50;
			priceText.transform.parent.GetComponent<Image>().sprite = select;
		}

		//for(int i = 0; i < 6; i++)
		//	choices[i].sprite = Resources.Load<Sprite>(("Pack " + (pack.packID+1) + "/Choice" + (i+1)));
	}

	public void Clicked(){
		if(curPack.bought)
			Select();
		else
			Buy();
	}

	void Buy(){
		//if(PlayerPrefs.GetInt("Coins") >= curPack.packPrice){
		//	PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - curPack.packPrice);
			curPack.bought = true;
			PlayerPrefs.SetInt("Pack" + curPack.packID, 1);
			Select();
		//}
	}

	void Select(){

		//PlayerPrefs.SetInt("PackID", curPack.packID);
		//priceText.text = "Selected";
		//priceText.fontSize = 25;
		//priceText.transform.parent.GetComponent<Image>().sprite = selected;

		ShopPack[] packs = FindObjectsOfType<ShopPack>();
		foreach(ShopPack p in packs)
			p.GetComponent<Image>().sprite = select;

		curPack.GetComponent<Image>().sprite = selected;
	}
}

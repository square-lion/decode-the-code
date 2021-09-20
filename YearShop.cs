using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class YearShop : MonoBehaviour
{
    public Text timeLeft;
    public Text coins;

    public Text packName;
    public Text packDesc;
    public Image[] pegs;

    int curID;

    void Start(){
        System.DateTime end = new System.DateTime(2021,05, 1);
        System.DateTime now = System.DateTime.Now;
        int days = (int)(end-now).TotalDays+1;
        timeLeft.text = days + " Days Remaining"; 

        //PlayerPrefs.SetInt("Coins", 9999);
    }

    void Update(){
        coins.text = "$" + PlayerPrefs.GetInt("Coins");
    }

    public void Clicked(int id){
        if(PlayerPrefs.GetInt("SP" + (id-300)) > 0)
            return;

        curID = id;
        PackInfo temp = Resources.Load<PackInfo>("Pack " + (id) + "/Info");
        packName.text = temp.packName;
        pegs[0].sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice1");
        pegs[1].sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice2");
        pegs[2].sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice3");
        pegs[3].sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice4");
        pegs[4].sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice5");
        pegs[5].sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice6");
        packDesc.text = temp.packDesc;

        FindObjectOfType<MenuController>().GetComponent<Animator>().SetBool("YearInfoIn", true);
    }

    public void CloseInfo(){
        FindObjectOfType<MenuController>().GetComponent<Animator>().SetBool("YearInfoOut", true);
    }

    public void BuyPackButton(){
		if(PlayerPrefs.GetInt("Coins") >= 10){
			FindObjectOfType<CollectionController>().BuyPack(curID, Resources.Load<Sprite>("Boxes/" + curID));
			PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - 10);
			CloseInfo();
            FindObjectOfType<MenuController>().year = !FindObjectOfType<MenuController>().year;
		}else{
			NotEnoughCoinsScreen(10-PlayerPrefs.GetInt("Coins"));
		}
	}

    public void NotEnoughCoinsScreen(int diff){
		FindObjectOfType<ShopManager>().notEnoughtCoinsScreen.SetActive(true);
		FindObjectOfType<ShopManager>().notEnoughtCoinsScreen.GetComponentInChildren<Text>().text = "You need " + diff + " more coins to buy a pack.";
        CloseInfo();
		FindObjectOfType<ShopManager>().anim.SetBool("NotEnoughCoinsIn", true);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyPack : MonoBehaviour{

    public Sprite[] rarity;

    Image pack;
    GameObject openedPack;

    Text packName;
    Image new1, new2, new3, new4, new5, new6;

    Animator anim;
	void Awake(){
		anim = transform.parent.parent.GetComponent<Animator>();
        pack = transform.GetChild(2).GetChild(1).GetComponent<Image>();
        openedPack = transform.GetChild(1).gameObject;

        packName = openedPack.transform.GetChild(0).GetComponent<Text>();

        new1 = openedPack.transform.GetChild(1).GetComponent<Image>();
        new2 = openedPack.transform.GetChild(2).GetComponent<Image>();
        new3 = openedPack.transform.GetChild(3).GetComponent<Image>();
        new4 = openedPack.transform.GetChild(4).GetComponent<Image>();
        new5 = openedPack.transform.GetChild(5).GetComponent<Image>();
        new6 = openedPack.transform.GetChild(6).GetComponent<Image>();
    }

    public void OpenPack(int id, string _packName){
        pack.transform.parent.gameObject.SetActive(true);

        Debug.Log(id);

        if(id < 100)
            pack.sprite = rarity[0];
        else if(id < 200)
            pack.sprite = rarity[1];
        else if(id < 300)
            pack.sprite = rarity[2];
        else
            pack.sprite = rarity[3];

        packName.text = _packName;

        new1.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice1");
        new2.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice2");
        new3.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice3");
        new4.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice4");
        new5.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice5");
        new6.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice6");
    }

    public void OpenPack(int id, string _packName, Sprite box){
        pack.transform.parent.gameObject.SetActive(true);

        pack.sprite = box;

        packName.text = _packName;

        new1.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice1");
        new2.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice2");
        new3.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice3");
        new4.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice4");
        new5.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice5");
        new6.sprite = Resources.Load<Sprite>("Pack " + (id) + "/Choice6");
    }
    
    public void PackClicked(){
        pack.transform.parent.gameObject.SetActive(false);
        openedPack.SetActive(true);
    }
    public void ToCollection(){
        anim.SetBool("BuyPackScreenOut", true);
		FindObjectOfType<CollectionController>().pages[0].transform.parent.gameObject.SetActive(true);
        FindObjectOfType<CollectionController>().OpenPage(0);
    }
}

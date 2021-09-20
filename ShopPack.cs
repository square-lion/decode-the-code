using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopPack: MonoBehaviour{
	public string packName;
	public int packID;
	public bool bought;



	CollectionController collectionController;

	void Start(){
		int pageInt = int.Parse(transform.parent.name.Substring(name.IndexOf(" ")+1));
		if(pageInt == 1)
			packID = int.Parse(this.name.Substring(name.IndexOf(" ")+1)) + (pageInt-1) * 16;
		else if(pageInt == 2)
			packID = int.Parse(this.name.Substring(name.IndexOf(" ")+1)) + (pageInt-2) * 16 + 100;
		else if(pageInt == 3)
			packID = int.Parse(this.name.Substring(name.IndexOf(" ")+1)) + (pageInt-3) * 16 + 200;
		else if (pageInt == 4)
			packID = int.Parse(this.name.Substring(name.IndexOf(" ")+1)) + (pageInt-4) * 16 + 300;
		else
			packID = int.Parse(this.name.Substring(name.IndexOf(" ")+1)) + (pageInt-5) * 16 + 400;

		collectionController = FindObjectOfType<CollectionController>();

		if(Resources.Load<Sprite>("Pack " + (packID) + "/Choice1") == null)
			Destroy(gameObject);

		if(PlayerPrefs.GetInt("Pack" + packID) == 1 || packID == 1){
			bought = true;
		}

		if(!bought){
				GetComponent<Image>().color = Color.gray;
				var pic = GetComponentsInChildren<Image>();
				foreach(Image p in pic)
					p.color = Color.grey;
		}else{
			GetComponent<Image>().color = Color.white;
				var pic = GetComponentsInChildren<Image>();
				foreach(Image p in pic)
					p.color = Color.white;
		}

		if(PlayerPrefs.GetInt("PackID") == packID){
			collectionController.ClickPack(this);
			GetComponent<Image>().sprite = collectionController.selected;
		}

		Image temp = transform.GetChild(0).GetComponent<Image>();
		temp.sprite = Resources.Load<Sprite>("Pack " + (packID) + "/Choice6");
		temp = transform.GetChild(1).GetComponent<Image>();
		temp.sprite = Resources.Load<Sprite>("Pack " + (packID) + "/Choice3");
		temp = transform.GetChild(2).GetComponent<Image>();
		temp.sprite = Resources.Load<Sprite>("Pack " + (packID ) + "/Choice1");
			
	}
}

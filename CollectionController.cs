using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionController : MonoBehaviour {
	//Hotbar Pack
	public Image[] hotbarDots;
	public Text hotbarPackName;
	public GameObject selectButton;

	//Collection
	public int previousPage;
	public int currentPage;
	public PageController[] pages;
	public GameObject[] boxes;

	//Bottom Bar
	public GameObject leftArrow;
	public GameObject rightArrow;
	public Text RarityText;

	//Type Colors
	public Color common;
	public Color rare;
	public Color super;
	public Color special;
	public Color promo;

	//Button Sprites
	public Sprite select;
	public Sprite selected;

	//Packs
	public ShopPack[] packs;
	public ShopPack curPack;

	//Refrences
	Animator anim;

	public GameObject promoButton;

	void Awake(){
		anim = transform.parent.parent.GetComponent<Animator>();
		leftArrow.gameObject.SetActive(false);

		foreach(ShopPack p in packs){
			if(p.packID == PlayerPrefs.GetInt("PackID"))
				curPack = p;
		}
		//OpenPage(0);
	}

	void Update(){
		if(PlayerPrefs.GetInt("RZFL") == 0)
			promoButton.SetActive(false);
		else
			promoButton.SetActive(true);
	}

	public void CollectoinOpen(){
		pages[0].transform.parent.position = Vector3.zero;
	}

	//When Pack is Click
	public void ClickPack(ShopPack pack){
		hotbarPackName.text = pack.packName;
		curPack = pack;
		for(int i = 0; i < 6; i++)
			hotbarDots[i].sprite = Resources.Load<Sprite>(("Pack " + (pack.packID) + "/Choice" + (i+1)));

		if(pack.bought){
			selectButton.SetActive(true);
			if(PlayerPrefs.GetInt("PackID") == pack.packID){
				selectButton.GetComponent<Image>().sprite = selected;
				selectButton.transform.GetChild(0).GetComponent<Text>().text = "Selected";
			}else{
				selectButton.GetComponent<Image>().sprite = select;
				selectButton.transform.GetChild(0).GetComponent<Text>().text = "Select";
			}
		}else{
			selectButton.SetActive(false);
		}
	}
	
	//When left arrow is hit
	public void Left(){
		if(anim.GetInteger("Page") != -1)
			return;

		rightArrow.gameObject.SetActive(true);
		previousPage = currentPage;
		currentPage --;
		if(currentPage == 0)
			leftArrow.gameObject.SetActive(false);
		RarityText.text = pages[currentPage].rarity;

		if(pages[currentPage].rarity == "Common")
			RarityText.color = common;
		else if(pages[currentPage].rarity == "Rare")
			RarityText.color = rare;
		else if(pages[currentPage].rarity == "Super")
			RarityText.color = super;
		

		anim.SetInteger("Page", currentPage);
	}

	//When right arrow is hit
	public void Right(){
		if(anim.GetInteger("Page") != -1)
			return;
		
		leftArrow.gameObject.SetActive(true);
		previousPage = currentPage;
		currentPage ++;
		if(currentPage == pages.Length-1)
			rightArrow.gameObject.SetActive(false);
		RarityText.text = pages[currentPage].rarity;

		if(pages[currentPage].rarity == "Common")
			RarityText.color = Color.Lerp(RarityText.color, common, 1);
		else if(pages[currentPage].rarity == "Rare")
			RarityText.color = rare;
		else if(pages[currentPage].rarity == "Super")
			RarityText.color = super;
		else if(pages[currentPage].rarity == "Special")
			RarityText.color = special;
		else if(pages[currentPage].rarity == "Promo")
			RarityText.color = promo;

		anim.SetInteger("Page", currentPage);
	}

	//Select Pack
	public void Select(){
		if(curPack == null)
			return;
		PlayerPrefs.SetInt("PackID", curPack.packID);
		selectButton.GetComponent<Image>().sprite = selected;
		selectButton.transform.GetChild(0).GetComponent<Text>().text = "Selected";

		ShopPack[] packs = FindObjectsOfType<ShopPack>();
		foreach(ShopPack p in packs)
			p.GetComponent<Image>().sprite = select;

		curPack.GetComponent<Image>().sprite = selected;
	}

	//Buy Pack
	public void BuyPack(){
		if(!CheckForPacks())
			return;

		int pack = Random.Range(0,packs.Length);
		
		while(packs[pack] == null || packs[pack].bought == true || packs[pack].packID > 300)
			pack = Random.Range(0,packs.Length);

		Debug.Log(pack + "id is bought");
		packs[pack].bought = true;
		PlayerPrefs.SetInt("Pack" + packs[pack].packID, 1);
		packs[pack].GetComponent<Image>().color = Color.white;
		var pic = packs[pack].GetComponentsInChildren<Image>();
		packs[pack].GetComponent<Image>().sprite = select;
		foreach(Image p in pic)
			p.color = Color.white;
		ClickPack(packs[pack]);
		Select();
		FindObjectOfType<BuyPack>().OpenPack(packs[pack].packID, packs[pack].packName);
		anim.SetBool("BuyPackScreenIn", true);
	}

	public void BuyPack(int id){
		int pack = 0;
		for(int x = 0; x < packs.Length; x++)
			if(id == packs[x].packID)
				pack = x;
		packs[pack].bought = true;
		PlayerPrefs.SetInt("Pack" + packs[pack].packID, 1);
		packs[pack].GetComponent<Image>().color = Color.white;
		var pic = packs[pack].GetComponentsInChildren<Image>();
		foreach(Image p in pic)
			p.color = Color.white;
		ClickPack(packs[pack]);
		Select();
		FindObjectOfType<BuyPack>().OpenPack(packs[pack].packID, packs[pack].packName);
		anim.SetBool("BuyPackScreenIn", true);
	}

	public void BuyPack(int id, Sprite art){
		Debug.Log(id);
		int pack = 0;
		for(int x = 0; x < packs.Length; x++)
			if(packs[x] != null && id == packs[x].packID)
				pack = x;
		Debug.Log(packs[pack].packID);
		packs[pack].bought = true;
		PlayerPrefs.SetInt("Pack" + packs[pack].packID, 1);
		packs[pack].GetComponent<Image>().color = Color.white;
		var pic = packs[pack].GetComponentsInChildren<Image>();
		foreach(Image p in pic)
			p.color = Color.white;
		ClickPack(packs[pack]);
		Select();
		FindObjectOfType<BuyPack>().OpenPack(packs[pack].packID, packs[pack].packName, art);
		anim.SetBool("BuyPackScreenIn", true);
	}

	bool CheckForPacks(){
		for(int i = 0; i < packs.Length; i++){
			if(packs[i] != null && !packs[i].bought)
				return true;
		}
		return false;
	}

	//Open IAP Menu
	public void MoreCoins(){
		anim.SetBool("CollectionToIAP", true);
		MenuController.fromShop = false;
	}

	//When tabs are hit
	public void OpenPage(int id){
		foreach(PageController g in pages){
			g.gameObject.SetActive(false);
		}
		pages[id].gameObject.SetActive(true);
		foreach(GameObject b in boxes){
			if(id == 0)
				b.GetComponent<Image>().color = common;
			else if(id == 1)
				b.GetComponent<Image>().color = rare;
			else if(id == 2)
				b.GetComponent<Image>().color = super;
			else if(id == 3)
				b.GetComponent<Image>().color = special;
			else if(id == 4)
				b.GetComponent<Image>().color = promo;
		}
		
		foreach(ShopPack p in packs){
			if(p != null && p.GetComponent<Image>().sprite == selected && p.packID != PlayerPrefs.GetInt("PackID"))
				p.GetComponent<Image>().sprite = select;
		}
	}
	public void Exit(){
		foreach(PageController g in pages){
			g.gameObject.SetActive(true);
		}
		pages[0].transform.parent.gameObject.SetActive(true);
	}
}

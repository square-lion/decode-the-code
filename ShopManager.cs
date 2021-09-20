using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

	public int normalPackPrice;
	public int specialPackPrice;

	public int currentSpecialPackID;

	public GameObject buyPackScreen;
	public GameObject buySpecialPackScreen;
	public GameObject notEnoughtCoinsScreen;
	public GameObject promoScreen;

	private InputField inputBox;
	private Text responceText;

	[HideInInspector]
	public bool infoSreen;

	public Text[] coins;

	public Animator anim;

	void Awake(){
		anim = transform.parent.parent.GetComponent<Animator>();

		inputBox = promoScreen.transform.GetChild(4).GetComponent<InputField>();
		responceText = promoScreen.transform.GetChild(5).GetComponent<Text>();

		if(PlayerPrefs.GetInt("SP" + currentSpecialPackID) == 2)
			transform.GetChild(0).GetChild(6).gameObject.SetActive(false);

		int unbought = 0;
		foreach(ShopPack p in FindObjectOfType<CollectionController>().packs){
			if(!p.bought)
				unbought++;
		}
		if(unbought == 0)
			transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
	}

	void Update(){
		foreach(Text t in coins)
			t.text = "" + PlayerPrefs.GetInt("Coins");
	}

	public void OpenPromo(){
		promoScreen.SetActive(true);
	}

	public void ClosePromo(){
		promoScreen.SetActive(false);
	}

	//When Promo Code Is Entered
	public void PromoEnter(){
		string code = inputBox.text.ToUpper();

		if(code.Length != 4){
			responceText.text = "Code Invalid";
		}
		else if(code == "RZFL"){
			if(PlayerPrefs.GetInt("RZFL") != 1){
				PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 25);
				FindObjectOfType<CollectionController>().BuyPack(401, Resources.Load<Sprite>("Boxes/401"));
				promoScreen.SetActive(false);
				PlayerPrefs.SetInt("RZFL", 1);
			}
			else{
				responceText.text = "Code Already Used";
			}
		}
		else{
			responceText.text = "Code Invalid";
		}
	}



	//When You Click on Coins
	public void MoreCoins(){
		anim.SetBool("IAPMenuOpen", true);
	}

	//Close button of IAP menu
	public void MoreCoinsClose(){
		anim.SetBool("IAPMenuClose", true);
	}

	//When Random Pack Icon Is Touched
	public void BuyRandomPack(){
		buyPackScreen.SetActive(true);
		anim.SetBool("BuyPackIn", true);
	}	
	//When Special PackIcon Is Touched
	public void BuySpecialPack(){
		buySpecialPackScreen.SetActive(true);
		anim.SetBool("BuySpecialPackIn", true);
	}
	//When Collection Tab is Touched
	public void ShopToCollection(){
		anim.SetBool("ShopToCollection", true);
		FindObjectOfType<CollectionController>().pages[0].transform.parent.gameObject.SetActive(true);
		FindObjectOfType<CollectionController>().OpenPage(0);
	}
	//Back Button on Collection
	public void BackToMainShopMenu(){
		anim.SetBool("CollectionToShop", true);
	}
	//OpenIAPMenu
	public void GetMoreCoins(){
		anim.SetBool("IAPMenuOpen", true);
		MenuController.fromShop = true;
	}
	//CloseIAPMenu
	public void GetMoreCoinsClose(){
		if(MenuController.fromShop)
			anim.SetBool("IAPMenuClose", true);
		else
			anim.SetBool("IAPToCollection", true);
	}
	//When You Can Buy A Pack
	public void BuyPackButton(){
		if(PlayerPrefs.GetInt("Coins") >= normalPackPrice){
			FindObjectOfType<CollectionController>().BuyPack();
			PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - normalPackPrice);
			BuyRandomPackClose();
		}else{
			NotEnoughCoinsScreen(normalPackPrice-PlayerPrefs.GetInt("Coins"));
		}

		int unbought = 0;
		foreach(ShopPack p in FindObjectOfType<CollectionController>().packs){
			if(!p.bought)
				unbought++;
		}
		if(unbought == 0)
			transform.GetChild(0).GetChild(5).gameObject.SetActive(false);		
	}

	public void IAPBuySpecialPack(){
		int id = currentSpecialPackID + 300;
		FindObjectOfType<CollectionController>().BuyPack(id);
		BuySpecialPackClose();
		PlayerPrefs.SetInt("SP" + currentSpecialPackID, 2);
		transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
	}

	//When You Can Buy A Special Pack
	public void BuySpecialPackButton(){
		if(PlayerPrefs.GetInt("Coins") >= specialPackPrice){
			int id = currentSpecialPackID + 300;
			FindObjectOfType<CollectionController>().BuyPack(id);
			
			PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - specialPackPrice);
			BuySpecialPackClose();
			PlayerPrefs.SetInt("SP" + currentSpecialPackID, 2);
			transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
		}else{
			NotEnoughCoinsScreen(specialPackPrice-PlayerPrefs.GetInt("Coins"));
		}
	}

	//When the Player Does not Have Enough Coins
	public void NotEnoughCoinsScreen(int diff){
		notEnoughtCoinsScreen.SetActive(true);
		notEnoughtCoinsScreen.GetComponentInChildren<Text>().text = "You need " + diff + " more coins to buy a pack.";
		BuyRandomPackClose();
		anim.SetBool("NotEnoughCoinsIn", true);
	}
	
	//When buy more coins is clicked
	public void NotEnoughCoinsButton(){
		NotEnoughCoinsClose();
		GetMoreCoins();
	}

	public void NotEnoughCoinsClose(){
		anim.SetBool("NotEnoughCoinsOut", true);
	}

	//Close Buy Pack Menu
	public void BuyRandomPackClose(){
		anim.SetBool("BuyPackOut", true);
	}

	//Close Special Pack Menu
	public void BuySpecialPackClose(){
		anim.SetBool("BuySpecialPackOut", true);
	}
}

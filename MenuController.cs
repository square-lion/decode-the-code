using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	bool stats = false;
	public GameObject shopMenu;

	public ShopPack first;

	bool music = true;
	public Image musicButton;
	public Sprite onSprite;
	public Sprite offSprite; 

	bool shop = false;
	bool online = false;
	public bool year = false;
	bool settings = false;

	public static bool fromShop;

	//0 normal, 1 hard
	int normalOrHard;

	Animator animator;

	void Awake(){
		if(PlayerPrefs.GetInt("HasPlayed")  == 0){
			PlayerPrefs.SetInt("HasPlayed", 1);
			FindObjectOfType<CollectionController>().ClickPack(first);
			FindObjectOfType<CollectionController>().Select();
		}

		if(PlayerPrefs.GetInt("Ads") == 0){
			PlayerPrefs.SetInt("Ads", 1);
		}
		animator = GetComponent<Animator>();

		if(PlayerPrefs.GetInt("Music") == 0){
			musicButton.sprite = onSprite;
			music = true;
		}else{
			musicButton.sprite = offSprite;
			music = false;
		}

		if(PlayerPrefs.GetInt("FromGame") == 1){
			PlayerPrefs.SetInt("FromGame", 0);
			animator.SetBool("FromGame", true);
		}
	}

	public void PlayNormal(){
		PlayerPrefs.SetInt("NormalGames", PlayerPrefs.GetInt("NormalGames") + 1);
		normalOrHard = 0;
		animator.SetBool("Loading", true);
	}

	public void PlayHard(){
		PlayerPrefs.SetInt("HardGames", PlayerPrefs.GetInt("HardGames") + 1);
		normalOrHard = 1;
		animator.SetBool("Loading", true);
	}

	public void Shop(){
		shop = !shop;
		
		if(shop)
			animator.SetBool("MenuToShop", true);
		else
			animator.SetBool("ShopToMenu", true);
	}

	public void RateGame(){
		//IOS
		//Application.OpenURL("itms-apps://itunes.apple.com/app/id1421984663");
		//Andriod
		#if UNITY_IPHONE
			Application.OpenURL("itms-apps://itunes.apple.com/app/id1421984663");
 		#endif
 		#if UNITY_ANDROID
			Application.OpenURL("https://play.google.com/store/apps/details?id=com.BBApps.Mastermind");
		#endif

	}

	public void Support(){
		Application.OpenURL("https://www.squarelion.net/support");
	}

	public void PrivacyPolicy(){
		Application.OpenURL("https://www.squarelion.net/decode-the-code/privacy-policy");
	}

	public void Settings(){
		settings = !settings;

		if(settings)
			animator.SetBool("MenuToSettings", true);		
		else
			animator.SetBool("SettingsToMenu", true);
	}

	public void MenuToSettings(){
		animator.SetBool("MenuToSettings", false);
	}
	public void SettingsToMenu(){
		animator.SetBool("SettingsToMenu", false);
	}

	public void Music(){
		music = !music;

		if(music){
			PlayerPrefs.SetInt("Music", 0);
			musicButton.sprite = onSprite;
			FindObjectOfType<AudioManager>().Play("Theme");
		}else{
			PlayerPrefs.SetInt("Music", 1);
			musicButton.sprite = offSprite;
			FindObjectOfType<AudioManager>().Stop("Theme");
		}
	}

	public void Stats(){
		stats = !stats;

		if(stats)
			animator.SetBool("MenuToStats", true);
		else
			animator.SetBool("StatsToMenu", true);
	}

	public void Online(){
		online = !online;
		if(online){
			animator.SetBool("OnlineIn", true);
			FindObjectOfType<OnlineLeaderboard>().Refresh();
		}
		else
			animator.SetBool("OnlineOut", true);
	}

	public void Year(){
		year = !year;
		if(year){
			animator.SetBool("YearIn", true);
		}
		else{
			animator.SetBool("YearOut", true);
		}
	}





		public void OnlineIn(){
		animator.SetBool("OnlineIn", false);
	}
	public void OnlineOut(){
		animator.SetBool("OnlineOut", false);
	}


	public void ShopEntered(){
		animator.SetBool("MenuToShop", false);
		shopMenu.transform.position = Vector3.zero;
	}

	public void MenuEntered(){
		animator.SetBool("ShopToMenu", false);
	}

	public void InfoScreenOpened(){
		animator.SetBool("InfoOpen", false);
	}

	public void InfoScreenClosed(){
		animator.SetBool("InfoClose", false);
	}

	public void StatsEntered(){
		animator.SetBool("MenuToStats", false);
	}

	public void StatsExited(){
		animator.SetBool("StatsToMenu", false);
	}

	public void LoadingScreenIn(){
		PlayerPrefs.SetInt("Ads", (PlayerPrefs.GetInt("Ads") + 1)%4);
		if(PlayerPrefs.GetInt("Ads") == 0){
			GetComponent<WatchAd>().ShowAd();
		}
		if(normalOrHard == 0)
			SceneManager.LoadScene("Normal");
		else
			SceneManager.LoadScene("Hard");
	}

	public void BackToMenu(){
		animator.SetBool("FromGame", false);
	}

	public void CollectionMenuEntered(){
		animator.SetBool("ShopToCollection", false);
	}

	public void CollectionExited(){
		animator.SetBool("CollectionToShop", false);
		FindObjectOfType<CollectionController>().pages[0].transform.parent.gameObject.SetActive(false);
		FindObjectOfType<CollectionController>().Exit();
	}

	public void BuyPackIn(){
		animator.SetBool("BuyPackIn", false);
	}

	public void BuyPackOut(){
		animator.SetBool("BuyPackOut", false);
		FindObjectOfType<ShopManager>().buyPackScreen.SetActive(false);
	}

	public void BuySpecialPackIn(){
		animator.SetBool("BuySpecialPackIn", false);
	}

	public void BuySpecialPackOut(){
		animator.SetBool("BuySpecialPackOut", false);
		FindObjectOfType<ShopManager>().buySpecialPackScreen.SetActive(false);
	}

	public void IAPMenuOpen(){
		animator.SetBool("IAPMenuOpen", false);
	}

	public void IAPMenuClose(){
		animator.SetBool("IAPMenuClose", false);
	}

	public void PageTurn(){
		animator.SetInteger("Page", -1);
	}

	public void IAPMenuOpenCollection(){
		animator.SetBool("CollectionToIAP", false);
	}

	public void IAPMenuCloseCollection(){
		animator.SetBool("IAPToCollection", false);
	}
	public void NotEnoughCoinsOpened(){
		animator.SetBool("NotEnoughCoinsIn", false);
	}
	public void NotEnoughCoinsClosed(){
		animator.SetBool("NotEnoughCoinsOut",false);
	}
	public void BuyPackScreenIn(){
		animator.SetBool("BuyPackScreenIn", false);
	}
	public void BuyPackScreenOut(){
		animator.SetBool("BuyPackScreenOut", false);
	}
	public void YearIn(){
		animator.SetBool("YearIn", false);
	}
	public void YearOut(){
		animator.SetBool("YearOut", false);
	}
	public void YearInfoIn(){
		animator.SetBool("YearInfoIn", false);
	}
	public void YearInfoOut(){
		animator.SetBool("YearInfoOut", false);
	}
}

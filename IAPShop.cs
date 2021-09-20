using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPShop : MonoBehaviour{

    public Text coinsText;

    public GameObject removeAdsCover;

    public GameObject restoreButton;

    private string coins10 = "com.squarelion.decodethecode.coins.10";
    private string coins55 = "com.squarelion.decodethecode.coins.55";
    private string coins120 = "com.squarelion.decodethecode.coins.120";
    private string removeads = "com.squarelion.decodethecode.removeads";

    private string specialPack = "com.squarelion.decodethecode.specialpack";

    public void Coins5(){
        FindObjectOfType<WatchAd>().ShowRewardedVideo();
    }


    public void OnPurchaseComplete(Product product){
        int coins = PlayerPrefs.GetInt("Coins");
        if(product.definition.id == coins10){
            coins += 10;
        }
        else if(product.definition.id == coins55){
            coins += 55;
        }
        else if(product.definition.id == coins120){
            coins += 120;
        }
        else if(product.definition.id == removeads){
            PlayerPrefs.SetInt("PlayAds", 1);
        }
        else if(product.definition.id == specialPack){
            FindObjectOfType<ShopManager>().IAPBuySpecialPack();
        }

        PlayerPrefs.SetInt("Coins", coins);
    }

    void Update(){
        coinsText.text = "" + PlayerPrefs.GetInt("Coins");

        if (PlayerPrefs.GetInt("PlayAds") == 1){
            removeAdsCover.SetActive(true);
        }
        else{
            removeAdsCover.SetActive(false);
        }

        #if UNITY_ANDROID
			restoreButton.SetActive(false);
		#endif
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        Debug.LogError("Purchase of " + product.definition.id + "failed due to " + reason);
    }
}

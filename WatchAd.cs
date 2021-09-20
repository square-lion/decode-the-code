using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class WatchAd : MonoBehaviour {

   

    void Awake(){
        #if UNITY_IPHONE
            Advertisement.Initialize ("2799797", false);
        #endif
        #if UNITY_ANDROID
            Advertisement.Initialize ("2799798", false);
        #endif
    }

    public void ShowAd(){
        
        if (Advertisement.IsReady() && PlayerPrefs.GetInt("PlayAds") == 0) {
            Advertisement.Show();
        }      
    }

    public void ShowRewardedVideo ()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", options);
    }

    void HandleShowResult (ShowResult result)
    {
        if(result == ShowResult.Finished) {
            if(FindObjectOfType<GameController>() != null){
                FindObjectOfType<GameController>().AdRestart();
            }
            else{
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 5);
                PlayerPrefs.SetInt("LifeLongCoins", PlayerPrefs.GetInt("LifeLongCoins") + 5);
                FindObjectOfType<StatsMenu>().LifelongCoinUpdate();
            }

        }else if(result == ShowResult.Skipped) {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");

        }else if(result == ShowResult.Failed) {
            Debug.LogError("Video failed to show");
        }
    }

}
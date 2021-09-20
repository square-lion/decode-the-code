using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryScreen : MonoBehaviour
{
    public Slider timer;
    public float maxTime;
    private float curTime;

    WatchAd watchAd;

    void Awake(){
        watchAd = FindObjectOfType<WatchAd>();

        curTime = maxTime;
        timer.maxValue = maxTime;
        
    }

    void Update(){
        curTime -= Time.deltaTime;
        timer.value = curTime;

        if(curTime < 0){
            FindObjectOfType<GameController>().TimerRanOut();
        }
    }

    public void Reset(){
        curTime = maxTime;
        gameObject.SetActive(true);
    }

    public void WatchAd(){
        watchAd.ShowRewardedVideo();
    }

    void OnEnable(){
        Reset();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour {

	public Text normGameText;
	public Text normWinGameText;
	public Text normWinPerc;

	public Text hardGameText;
	public Text hardWinGameText;
	public Text hardWinPerc;

	public Text totGameText;
	public Text totWinGameText;
	public Text totWinPerc;

	public Text lifelongText;

	void OnEnable(){
		normGameText.text = "Normal Games Played: " + PlayerPrefs.GetInt("NormalGames");
		normWinGameText.text = "Normal Games Won: " + PlayerPrefs.GetInt("NormalGameWins");

		hardGameText.text = "Hard Games Played: " + PlayerPrefs.GetInt("HardGames");
		hardWinGameText.text = "Hard Games Won: " + PlayerPrefs.GetInt("HardGameWins");

		totGameText.text = "Total Games Played: " + (PlayerPrefs.GetInt("HardGames")+ PlayerPrefs.GetInt("NormalGames"));
		totWinGameText.text = "Total Games Won: " + (PlayerPrefs.GetInt("HardGameWins") + PlayerPrefs.GetInt("NormalGameWins"));

		LifelongCoinUpdate();	

		normWinPerc.text = "Normal Games Win %: " + (int)((float)PlayerPrefs.GetInt("NormalGameWins")/PlayerPrefs.GetInt("NormalGames") * 100) + "%";
		hardWinPerc.text = "Hard Games Win %: " + (int)((float)PlayerPrefs.GetInt("HardGameWins")/PlayerPrefs.GetInt("HardGames") * 100) + "%";
		totWinPerc.text = "Overall Win %: " + (int)((float)(PlayerPrefs.GetInt("NormalGameWins") + PlayerPrefs.GetInt("HardGameWins"))/((float)(PlayerPrefs.GetInt("NormalGames") + PlayerPrefs.GetInt("HardGames"))) * 100) + "%";
	}

	public void LifelongCoinUpdate(){
		lifelongText.text = "Lifetime $: " + PlayerPrefs.GetInt("LifeLongCoins");
	}

}

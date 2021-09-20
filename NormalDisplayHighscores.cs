using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NormalDisplayHighscores : MonoBehaviour {
	public Text scores;
	public NormalHighscores highscoreManager;

	public List<string> usernames;
	// Use this for initialization
	void Start () {
		scores.text = "Fetching...";			
		highscoreManager = GetComponent<NormalHighscores>();
		Refresh();
	}

	public void OnHighscoresDownloaded(Highscore[] highscoreList)
	{
        usernames.Clear();
		scores.text = "";
		for(int i = 0; i < highscoreList.Length; i++)
		{
			scores.text += (i+1) + "." + highscoreList[i].username + "-"  + highscoreList[i].wins + "\n";
			usernames.Add(highscoreList[i].username.ToString());
		}
	}

	public void OnHighscoresFailed(){
		scores.text = "No Connection";
	}

    public void Refresh(){
        StartCoroutine("RefreshHighscores");
    }

	IEnumerator RefreshHighscores()
	{
		while(true)
		{
			yield return new WaitForSeconds(10f); 
			highscoreManager.DownloadHighscores();		
		}
	}
}

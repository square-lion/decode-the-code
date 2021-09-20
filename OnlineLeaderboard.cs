using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineLeaderboard : MonoBehaviour
{
    public GameObject enterUsernameBox;
    public InputField field;
    public Text warningText;
    public Text usernameText;

    NormalDisplayHighscores normalLeaderboard;
    HardDisplayHighscores hardLeaderboard;

    void Awake(){
        normalLeaderboard = FindObjectOfType<NormalDisplayHighscores>();
        hardLeaderboard = FindObjectOfType<HardDisplayHighscores>();
    }

    void Start(){
        if(PlayerPrefs.GetString("Username") == ""){
            enterUsernameBox.SetActive(true);
        }
        else{
            enterUsernameBox.SetActive(false);
            normalLeaderboard.highscoreManager.AddNewHighscore(PlayerPrefs.GetString("Username"), PlayerPrefs.GetInt("NormalGameWins"));
            hardLeaderboard.highscoreManager.AddNewHighscore(PlayerPrefs.GetString("Username"), PlayerPrefs.GetInt("HardGameWins"));
            usernameText.text = "Username: " + PlayerPrefs.GetString("Username");
        }
    }

    //When Submit button is clicked
    public void Submit(){
        string username = field.text;
        if(Taken(username)){
            warningText.text = "Username taken";
            return;
        }      
        if(username.Length < 3)
            warningText.text = "Username too short";
        PlayerPrefs.SetString("Username", username);
        Debug.Log("Username");
        enterUsernameBox.SetActive(false);
        usernameText.text = "Username:" + PlayerPrefs.GetString("Username");

        normalLeaderboard.highscoreManager.AddNewHighscore(PlayerPrefs.GetString("Username"), PlayerPrefs.GetInt("NormalGameWins"));
        hardLeaderboard.highscoreManager.AddNewHighscore(PlayerPrefs.GetString("Username"), PlayerPrefs.GetInt("HardGameWins"));

       Refresh();
    }

    public void Refresh(){
        normalLeaderboard.Refresh();
        hardLeaderboard.Refresh();
    }

    //Check if Username is taken
    public bool Taken(string name){
        List<string> usernames = normalLeaderboard.usernames;
        foreach(string s in usernames){
            if(string.Compare(name, s) == 0)
                return true;
        }
        return false;
    }
}

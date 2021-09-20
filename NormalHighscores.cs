using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NormalHighscores : MonoBehaviour {
   
    const string privateCode = "V1KAcb4u7ECokW54_Z0s0QUxcXGgiEVki0s2ZA9qvqIw";
    const string publicCode = "5e021072fe224b04782ec4cd";
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;
    NormalDisplayHighscores display;

    void Awake(){
        display = GetComponent<NormalDisplayHighscores>();

        DownloadHighscores();
    }

    public void AddNewHighscore(string username, int wins){
        StartCoroutine(UploadNewHighscore(username, wins));
    }

    IEnumerator UploadNewHighscore(string username, int wins){
        UnityWebRequest www = new UnityWebRequest(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + wins);
        yield return www.SendWebRequest();

        if(string.IsNullOrEmpty(www.error))
            print("Upload Successful");
        else
            print("Error Uploading" + www.error);
   }

   public void DownloadHighscores(){
       StartCoroutine("DownloadHighscoresFromDatabase");
   }

   IEnumerator DownloadHighscoresFromDatabase(){
        //UnityWebRequest www = new UnityWebRequest(webURL + publicCode + "/pipe/");
        UnityWebRequest www = UnityWebRequest.Get(webURL + publicCode + "/pipe/");
        yield return www.SendWebRequest();

        if(string.IsNullOrEmpty(www.error)){
            FormatHighscores(www.downloadHandler.text);
            display.OnHighscoresDownloaded(highscoresList);
        }
        else{
            print("Error Downloading" + www.error);
            display.OnHighscoresFailed();
        }
   }

   void FormatHighscores(string textStream){
       string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
       highscoresList = new Highscore[entries.Length];

       for(int i = 0; i < entries.Length; i ++){
           string[] entryInfo = entries[i].Split(new char[] {'|'});
           string username = entryInfo[0];
           int wins = int.Parse(entryInfo[1]);
           highscoresList[i] = new Highscore(username, wins);
       }
   }
}

public struct Highscore { 
    public string username;
    public int wins;

    public Highscore(string _username, int _wins){
        username = _username;
        wins = _wins;
    }
}

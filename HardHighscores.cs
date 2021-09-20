using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HardHighscores : MonoBehaviour {
   
    const string privateCode = "Y5mwaBWx-UiCyW7CZRk_0w_hrPBvPO4kq0_3USRz3mLA";
    const string publicCode = "5e022724fe224b04782f9c41";
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;
    HardDisplayHighscores display;

    void Awake(){
        display = GetComponent<HardDisplayHighscores>();

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

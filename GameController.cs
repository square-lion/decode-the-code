using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    private const float B = 255f;
    int currentRowNum = 0;
	int currentGuessNum = 1;
	public GameObject[] Rows;
	public int guessAmount;

	public List<int> guesses;
	public List<int> answer;

	public List<int> answerCopy = new List<int>();
	public List<int> wrongGuesses = new List<int>();

	private int packID;
	private Sprite choice1, choice2, choice3, choice4, choice5, choice6;
	public Sprite empty;

	public GameObject answerRow;
	public GameObject pauseAnwerRow;
	public GameObject bottomRow;
	public Sprite filledPeg;
	public Sprite unfilledPeg;

	public GameObject gameOverScreen;

	public GameObject gameGameOverCover;
	public GameObject gameOverScreenArrow;

	public GameObject HelpScreen;

	public bool gameOver;

	public bool retry;

	AudioManager audioManager;
	WatchAd ad;

	void Awake(){
		audioManager = FindObjectOfType<AudioManager>();
		ad = FindObjectOfType<WatchAd>();
		GetComponent<Animator>().SetBool("Loading", true);
	}

	void Start(){
		PickAnswers();
		while(!validatePicks()){
			PickAnswers();
		}

		//Find Current Pack
		packID = PlayerPrefs.GetInt("PackID");
		choice1 = Resources.Load<Sprite>("Pack " + (packID) + "/Choice1");
		choice2 = Resources.Load<Sprite>("Pack " + (packID) + "/Choice2");
		choice3 = Resources.Load<Sprite>("Pack " + (packID) + "/Choice3");
		choice4 = Resources.Load<Sprite>("Pack " + (packID) + "/Choice4");
		choice5 = Resources.Load<Sprite>("Pack " + (packID) + "/Choice5");
		choice6 = Resources.Load<Sprite>("Pack " + (packID) + "/Choice6");

		//Set Bottom Row
		GameObject temp = bottomRow.transform.GetChild(0).gameObject;
		temp.GetComponent<Image>().sprite = choice1;

		temp = bottomRow.transform.GetChild(1).gameObject;
		temp.GetComponent<Image>().sprite = choice2;

		temp = bottomRow.transform.GetChild(2).gameObject;
		temp.GetComponent<Image>().sprite = choice3;

		temp = bottomRow.transform.GetChild(3).gameObject;
		temp.GetComponent<Image>().sprite = choice4;

		temp = bottomRow.transform.GetChild(4).gameObject;
		temp.GetComponent<Image>().sprite = choice5;
		
		temp = bottomRow.transform.GetChild(5).gameObject;
		temp.GetComponent<Image>().sprite = choice6;

		answerRow.SetActive(false);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Alpha1))
			ChooseColor(1);
		if(Input.GetKeyDown(KeyCode.Alpha2))
			ChooseColor(2);
		if(Input.GetKeyDown(KeyCode.Alpha3))
			ChooseColor(3);
		if(Input.GetKeyDown(KeyCode.Alpha4))
			ChooseColor(4);
		if(Input.GetKeyDown(KeyCode.Alpha5))
			ChooseColor(5);
		if(Input.GetKeyDown(KeyCode.Alpha6))
			ChooseColor(6);
		if(Input.GetKeyDown(KeyCode.Space))
			CheckAnswers();
		if(Input.GetKeyDown(KeyCode.Q))
			ClearRow();
	}

	public void PickAnswers(){
		answer.Clear();

		for(int i = 0; i < guessAmount; i++){
			int colorID = Random.Range(1,7);

			answer.Add(colorID);
		}
	}

	public bool validatePicks(){
		int temp = answer[0];
		int times = 1;
		for(int i = 1; i < answer.Count; i++){
			if(temp == answer[i])
				times++;
			else{
				temp = answer[i];
				times = 1;
			}
		}
		if(times > 2)
			return false;
		return true;
	}

	public void ChooseColor(int colorID){
		if(gameOver || guesses.Count == guessAmount)
			return;

		Rows[currentRowNum].transform.GetChild(Rows[currentRowNum].transform.childCount-1).gameObject.SetActive(true);

		Image currentGuessPad = Rows[currentRowNum].transform.GetChild(currentGuessNum-1).GetComponent<Image>();

		if(colorID == 1)
			currentGuessPad.sprite = choice1;
		else if(colorID == 2)
			currentGuessPad.sprite = choice2;
		else if(colorID == 3)
			currentGuessPad.sprite = choice3;
		else if(colorID == 4)
			currentGuessPad.sprite = choice4;
		else if(colorID == 5)
			currentGuessPad.sprite = choice5;
		else if(colorID == 6)
			currentGuessPad.sprite = choice6;

		guesses.Add(colorID);
		//audioManager.Play("PlacePeg");
		
		if(currentGuessNum >= guessAmount){
			Rows[currentRowNum].transform.GetChild(Rows[currentRowNum].transform.childCount-2).gameObject.SetActive(true);
		}else
		currentGuessNum++;
	}

	public void CheckAnswers(){
		if(guesses.Count != guessAmount)
			return;

		List<GameObject> pegs = new List<GameObject>();

		//Get a list of the current pegs
		for(int i = guessAmount; i < guessAmount*2; i++){
			pegs.Add(Rows[currentRowNum].transform.GetChild(i).gameObject);
		}

		//Deactivate Done and Clear Buttons
		Rows[currentRowNum].transform.GetChild(Rows[currentRowNum].transform.childCount-1).gameObject.SetActive(false);
		Rows[currentRowNum].transform.GetChild(Rows[currentRowNum].transform.childCount-2).gameObject.SetActive(false);

		//If You Win
		if(IsCorrect()){	
			foreach(GameObject p in pegs){
				p.GetComponent<Image>().sprite = filledPeg;
				p.GetComponent<Image>().color = Color.grey;
			}

			GameOver(true);

		//If you Lose
		}else if(currentRowNum == Rows.Length-1){
			GameOver(false);

		//If Neither
		}else{

			/*
			//Serch for any in the right spot
			for(int i = 0; i < answer.Count; i++){

				if(answer[i] == guesses[i]){
					var p = pegs[i];
					p.GetComponent<Image>().color = Color.grey;
					//pegs.Remove(p);
					wrongGuesses.Add(-1);
					wrongAnswers.Add(-1);
				}else{
					wrongGuesses.Add(guesses[i]);
					wrongAnswers.Add(answer[i]);
				}
			}
			//Search for any in the wrong spot
			for(int x = 0; x < wrongGuesses.Count; x++){
				bool foundmatch = false;
				for(int i = 0; i < wrongAnswers.Count; i++){
					if(wrongGuesses[x] == wrongAnswers[i] && !foundmatch){
						foundmatch = true;
						var p = pegs[x];
						p.GetComponent<Image>().color = Color.white;
						//pegs.Remove(p);
						wrongAnswers.Remove(wrongAnswers[i]);
					}
				}
			}
			*/

			/*
			wrongAnswers.Clear();
			wrongGuesses.Clear();

			for(int x = 0; x < guesses.Count; x++){
				if(guesses[x] == answer[x]){
					pegs[x].GetComponent<Image>().color = Color.grey;
					wrongGuesses.Add(-1);
					wrongAnswers.Add(-1);
				}else{
					wrongAnswers.Add(answer[x]);
					wrongGuesses.Add(guesses[x]);
				}
			}
			for(int x = 0; x<wrongGuesses.Count; x++){
				for(int y = 0; y < wrongAnswers.Count; y++){
						if(wrongGuesses[x] == wrongAnswers[y]){
							Debug.Log(x);
							if(x == y)
								pegs[x].GetComponent<Image>().color = Color.grey;
							else
								pegs[x].GetComponent<Image>().color = Color.white;
							wrongAnswers.Remove(wrongAnswers[y]);
							wrongGuesses.Remove(wrongGuesses[x]);
						}
					}
			}
			*/

			/* 
			answerCopy.Clear();


			for(int x = 0; x < answer.Count; x++){
				answerCopy.Add(answer[x]);
			}

			for(int x = 0; x < guesses.Count; x++){
				if(guesses[x] == answer[x]){
					pegs[x].GetComponent<Image>().color = Color.grey;
					answerCopy.Insert(x,-1);
					answerCopy.Remove(answer[x]);
					Debug.Log(x);
				}else{
					for(int y = 0; y < answerCopy.Count; y++){
						if(guesses[x] == answerCopy[y]){
							Debug.Log(x + "||" + y);
							Debug.Log(guesses[x] + "|" + answerCopy[y]);
							pegs[x].GetComponent<Image>().color = Color.white;
							answerCopy.Insert(y,-1);
							answerCopy.Remove(answerCopy[y]);
						}
					}
				}
			}
			*/

			/*
			answerCopy.Clear();

			for(int x = 0; x < answer.Count; x++){
				answerCopy.Add(answer[x]);
			}

			for(int x = 0; x < guesses.Count;x++){
				if(guesses[x] == answer[x]){
					pegs[x].GetComponent<Image>().color = Color.grey;
					guesses.RemoveAt(x);
					guesses.Insert(x,-1);
					answerCopy.RemoveAt(x);
					answerCopy.Insert(x,-2);
					Debug.Log("Grey +1");
				}
			}

			Debug.Log("Guess.Count:" + guesses.Count);
			Debug.Log("AnswerCopy.Count:" + answerCopy.Count);

			for(int x = 0; x < guesses.Count; x++){
				bool foundmatch = false;
				for(int y = 0; y < answerCopy.Count; y++){
					if(guesses[x] == answerCopy[y] && !foundmatch){
						foundmatch = true;
						pegs[x].GetComponent<Image>().color = Color.white;
						answerCopy.RemoveAt(y);
						answerCopy.Insert(y,-2);
						Debug.Log("White +1");
					}
				}
			}
			 */
			answerCopy.Clear();

			for(int x = 0; x < answer.Count; x++){
				answerCopy.Add(answer[x]);
			}

			for(int x = 0; x < guesses.Count;x++){
				if(guesses[x] == answer[x]){
					GameObject peg = pegs[Random.Range(0, pegs.Count)];
					peg.GetComponent<Image>().sprite = filledPeg;
					peg.GetComponent<Image>().color = Color.grey;
					pegs.Remove(peg);
					guesses.RemoveAt(x);
					guesses.Insert(x,-1);
					answerCopy.RemoveAt(x);
					answerCopy.Insert(x,-2);
					Debug.Log("Grey +1");
				}
			}

			Debug.Log("Guess.Count:" + guesses.Count);
			Debug.Log("AnswerCopy.Count:" + answerCopy.Count);

			for(int x = 0; x < guesses.Count; x++){
				bool foundmatch = false;
				for(int y = 0; y < answerCopy.Count; y++){
					if(guesses[x] == answerCopy[y] && !foundmatch){
						foundmatch = true;
						GameObject peg = pegs[Random.Range(0, pegs.Count)];
						peg.GetComponent<Image>().sprite = filledPeg;
						peg.GetComponent<Image>().color = Color.white;
						pegs.Remove(peg);
						answerCopy.RemoveAt(y);
						answerCopy.Insert(y,-2);
						Debug.Log("White +1");
					}
				}
			}

			//Reset for next row
			guesses.Clear();
			currentGuessNum = 1;
			currentRowNum += 1;
		}
	}

	//Check what answers are correct
	bool IsCorrect(){
		if(answer[0] == guesses[0]){
			if(answer[1] == guesses[1]){
				if(answer[2] == guesses[2]){
					if(answer[3] == guesses[3]){
						if(guessAmount == 5 && answer[4] == guesses[4])
							return true;
						else if(guessAmount == 5)
							return false;
						return true;
					}else
						return false;
				}else
					return false;
			}else	
				return false;
		}else
			return false;
	}

	public void ClearRow(){
		if(gameOver)
			return;

		for(int i = 0; i < guessAmount; i++){

			if(currentGuessNum > 0){
				Image currentGuessPad = Rows[currentRowNum].transform.GetChild(currentGuessNum-1).GetComponent<Image>();
				currentGuessPad.sprite = empty;
				currentGuessNum--;
			}
		}
		currentGuessNum = 1;

		//Deactivate Done and Clear Buttons
		Rows[currentRowNum].transform.GetChild(Rows[currentRowNum].transform.childCount-1).gameObject.SetActive(false);
		Rows[currentRowNum].transform.GetChild(Rows[currentRowNum].transform.childCount-2).gameObject.SetActive(false);

		guesses.Clear();
	}

	public void Restart(){
		PlayerPrefs.SetInt("Ads", (PlayerPrefs.GetInt("Ads") + 1)%4);
		if(PlayerPrefs.GetInt("Ads") == 0 && PlayerPrefs.GetInt("PlayAds") == 0){
			GetComponent<WatchAd>().ShowAd();
		}

		answerRow.SetActive(false);

		var b = GameObject.FindGameObjectsWithTag("Guess");
		foreach(GameObject g in b){
			g.GetComponent<Image>().sprite = empty;
		}

		b = GameObject.FindGameObjectsWithTag("Answer");
		foreach(GameObject g in b){
			g.GetComponent<Image>().color = new Color(169f/255f, 169f/255f, 169f/255f, 0);
		}

		b = GameObject.FindGameObjectsWithTag("Check");
		foreach(GameObject g in b){
			g.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255);
			g.GetComponent<Image>().sprite = unfilledPeg;
		}

		b = GameObject.FindGameObjectsWithTag("DoneClear");
		foreach(GameObject g in b){
			g.gameObject.SetActive(false);
		}

		if(guessAmount == 4)
			PlayerPrefs.SetInt("NormalGames", PlayerPrefs.GetInt("NormalGames") + 1);
		else
			PlayerPrefs.SetInt("HardGames", PlayerPrefs.GetInt("HardGames") + 1);

		currentRowNum = 0;
		currentGuessNum = 1;
		gameOver = false;
		retry = false;

		guesses.Clear();

		PickAnswers();

		gameOverScreenArrow.SetActive(false);
		gameGameOverCover.SetActive(false);

		//Revoed Till Unity Fixes Image Problem
		UnPause();
		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void AdRestart(){
		answerRow.SetActive(false);

		var b = GameObject.FindGameObjectsWithTag("Guess");
		foreach(GameObject g in b){
			g.GetComponent<Image>().sprite = empty;
		}

		b = GameObject.FindGameObjectsWithTag("Answer");
		foreach(GameObject g in b){
			g.GetComponent<Image>().color = new Color(169f/255f, 169f/255f, 169f/255f, 0);
		}

		b = GameObject.FindGameObjectsWithTag("Check");
		foreach(GameObject g in b){
			g.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255);
			g.GetComponent<Image>().sprite = unfilledPeg;
		}

		b = GameObject.FindGameObjectsWithTag("DoneClear");
		foreach(GameObject g in b){
			g.gameObject.SetActive(false);
		}	

		currentRowNum = 0;
		currentGuessNum = 1;
		gameOver = false;
		retry = true;

		guesses.Clear();

		gameOverScreenArrow.SetActive(false);
		gameGameOverCover.SetActive(false);

		GetComponent<Animator>().SetBool("RetryOut", true);

		//UnPause();
	}

	void GameOver(bool won){
		gameOverScreen.transform.GetChild(1).gameObject.SetActive(true);
		gameOverScreen.transform.GetChild(2).gameObject.SetActive(true);

		if(won){
			gameOverScreen.transform.GetChild(0).GetComponentInChildren<Text>().text = "You Won!";
			if(retry){
				gameOverScreen.transform.GetChild(2).GetComponent<Text>().text = "You earned 0 coins";
			}
			else if(guessAmount == 4){
				PlayerPrefs.SetInt("NormalGameWins", PlayerPrefs.GetInt("NormalGameWins") + 1);
				PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1);
				PlayerPrefs.SetInt("LifeLongCoins", PlayerPrefs.GetInt("LifeLongCoins") + 1);
				gameOverScreen.transform.GetChild(2).GetComponent<Text>().text = "You earned 1 coin";
			}
			else{
				PlayerPrefs.SetInt("HardGameWins", PlayerPrefs.GetInt("HardGameWins") + 1);
				PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 3);
				PlayerPrefs.SetInt("LifeLongCoins", PlayerPrefs.GetInt("LifeLongCoins") + 3);
				gameOverScreen.transform.GetChild(2).GetComponent<Text>().text = "You earned 3 coins";
			}
			gameOverScreen.transform.GetChild(1).GetComponent<Text>().text = "You have " + PlayerPrefs.GetInt("Coins") + " coins";

			gameOverScreenArrow.SetActive(true);
			GetComponent<Animator>().SetBool("MovingIn", true);
			gameOverScreen.transform.GetChild(3).gameObject.SetActive(false);
		}
		else{
			gameOverScreen.transform.GetComponentInChildren<Text>().text = "You Lost";	
			gameOverScreen.transform.GetChild(1).GetComponent<Text>().text = "You have " + PlayerPrefs.GetInt("Coins") + " coins";
			gameOverScreen.transform.GetChild(2).GetComponent<Text>().text = "You earned 0 coins";

			GetComponent<Animator>().SetBool("RetryIn", true);
		}	

		answerRow.SetActive(true);
		for(int i = 0; i < guessAmount; i++){
			Image curGuess = answerRow.transform.GetChild(i).GetComponent<Image>();
			curGuess.color = Color.white;
			curGuess.sprite = Resources.Load<Sprite>("Pack "+ (packID) + "/Choice" + answer[i]);
		}

		gameOver = true;

		Rows[currentRowNum].transform.GetChild(Rows[currentRowNum].transform.childCount-1).gameObject.SetActive(false);
		Rows[currentRowNum].transform.GetChild(Rows[currentRowNum].transform.childCount-2).gameObject.SetActive(false);

		//audioManager.Play("GameOver");

			
		//FindObjectOfType<RetryScreen>().Reset();
	}

	public void TimerRanOut(){
		gameOverScreenArrow.SetActive(true);
		GetComponent<Animator>().SetBool("MovingIn", true);
		gameOverScreen.transform.GetChild(3).gameObject.SetActive(false);
	}


	public void Exit(){
		GetComponent<Animator>().SetBool("BackToMenu", true);
	}

	public void Pause(){

		gameOverScreen.transform.GetComponentInChildren<Text>().text = "Paused";
		GetComponent<Animator>().SetBool("MovingIn", true);
		gameOverScreen.transform.GetChild(3).gameObject.SetActive(true);

		gameOverScreen.transform.GetChild(1).gameObject.SetActive(false);
		gameOverScreen.transform.GetChild(2).gameObject.SetActive(false);
	}

	public void UnPause(){

		GetComponent<Animator>().SetBool("MovingOut", true);
	}

	public void HelpOpen(){
		GetComponent<Animator>().SetBool("HelpIn", true);
	}

	public void HelpClose(){
		GetComponent<Animator>().SetBool("HelpOut", true);
	}

	public void StopHelp(){
		GetComponent<Animator>().SetBool("HelpOut", false);
		GetComponent<Animator>().SetBool("HelpIn", false);
	}

	public void RetryEnter(){
		GetComponent<Animator>().SetBool("RetryIn", false);
		GetComponent<Animator>().SetBool("RetryOut", false);
	}

	public void GameOverScreenEntered(){

		GetComponent<Animator>().SetBool("MovingIn", false);

		if(gameOver){
			gameGameOverCover.SetActive(true);
		}
	}

	public void GameOverScreenExiting(){

		GetComponent<Animator>().SetBool("MovingOut", false);
		//answerRow.transform.parent = this.transform.GetChild(0).GetChild(0);
	}

	public void LoadingOut(){
		GetComponent<Animator>().SetBool("Loading", false);

		if(PlayerPrefs.GetInt("FirstTime") == 0){
			HelpScreen.SetActive(true);
			PlayerPrefs.SetInt("FirstTime", 1);
		}

	}

	public void BackToMenu(){
		PlayerPrefs.SetInt("Ads", (PlayerPrefs.GetInt("Ads") + 1)%4);
		if(PlayerPrefs.GetInt("Ads") == 0){
			GetComponent<WatchAd>().ShowAd();
		}
		
		PlayerPrefs.SetInt("FromGame", 1);
		SceneManager.LoadScene("Menu");
	}

	public void BackToGameOver(){
		GetComponent<Animator>().SetBool("MovingIn", true);
	}
}
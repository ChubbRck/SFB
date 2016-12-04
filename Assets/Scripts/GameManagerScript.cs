﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

	public GameObject scoreboard;
	public GameObject background;
	public float gravTimer;
	public float gameTimer;
	private bool timerRunning = false;
	private bool readyForReplay;
	public int teamOneScore;
	public int teamTwoScore;
	public Text winText;
	public bool isGameOver;
	public int scorePlayedTo = 5;
	public int arenaType;
	private float timeSinceLastPowerup;
	private float powerupAppearTime;
	public GameObject speedPowerupPrefab;
	public GameObject powerupPrefab;
	public GameObject gravityIndicator;
	// Hold references to each of the players. Activate or de-activate them based on options chosen on the previous page. 
	public GameObject Player1;
	public GameObject Player2;
	public GameObject Player3;
	public GameObject Player4;

	// Hold references to each of the arenas
	public GameObject Arena1;
	public GameObject Arena2;
	public GameObject Arena3;
	public GameObject Arena4;
	public GameObject Arena5;
	public GameObject Arena6;
	public GameObject Arena7;
	public GameObject Arena8;
	public GameObject Arena9;

	public GameObject CurrentArena;

	public string startButton1 = "Start_P1";
	public string startButton2 = "Start_P2";
	public string startButton3 = "Start_P3";
	public string startButton4 = "Start_P4";



	// Static singleton property
	public static GameManagerScript Instance { get; private set; }


	// Use this for initialization

	void StartReplay(){
		EZReplayManager.get.record();
	}
	void Start () {
		

	}
	void Awake()
	{
		// Save a reference to the AudioHandler component as our singleton instance
		Instance = this;

		Player1.GetComponent<PlayerController>().playerType = DataManagerScript.playerOneType;
		Player2.GetComponent<PlayerController>().playerType = DataManagerScript.playerTwoType;
		Player3.GetComponent<PlayerController>().playerType = DataManagerScript.playerThreeType;
		Player4.GetComponent<PlayerController>().playerType = DataManagerScript.playerFourType;

		MusicManagerScript.Instance.StartRoot ();
		launchTimer ();
		timeSinceLastPowerup = 0f;
		powerupAppearTime = 10f;
		readyForReplay = false;
		winText.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
		Invoke ("StartReplay", 2f);


		// Set up players and their rigidbodies based on character selection choice
		//	Player1.SetActive (false);


		// Set up arena based on options
		arenaType = DataManagerScript.arenaType;

		switch (arenaType) {

		case 1:
			Arena1.SetActive (true);
			CurrentArena = Arena1;
			break;

		case 2:
			Arena2.SetActive (true);
			CurrentArena = Arena2;
			break;

		case 3:
			Arena3.SetActive (true);
			CurrentArena = Arena3;
			break;

		case 4:
			Arena4.SetActive (true);
			CurrentArena = Arena4;
			break;

		case 5:
			Arena5.SetActive (true);
			CurrentArena = Arena5;
			break;

		case 6:
			Arena6.SetActive (true);
			CurrentArena = Arena6;
			break;

		case 7:
			Arena7.SetActive (true);
			CurrentArena = Arena7;
			break;

		case 8:
			Arena8.SetActive (true);
			CurrentArena = Arena8;
			break;

		case 9:
			Arena9.SetActive (true);
			CurrentArena = Arena9;
			break;

		default:
			Arena1.SetActive (true);
			CurrentArena = Arena1;
			break;

		}

		if (DataManagerScript.playerOnePlaying == false) {
			Player1.SetActive (false);
		}
		if (DataManagerScript.playerTwoPlaying == false) {
			Player2.SetActive (false);
		}
		if (DataManagerScript.playerThreePlaying == false) {
			Player3.SetActive (false);
		}
		if (DataManagerScript.playerFourPlaying == false) {
			Player4.SetActive (false);
		}

	}
	void launchTimer(){
		timerRunning = true;

	}

	void LaunchTitleScreen(){
		Application.LoadLevel ("titleScene");
	}
	void LaunchStatsScreen(){
		StartCoroutine ("FadeToStats");
	}
	IEnumerator FadeToStats(){
		float fadeTime = GameObject.Find ("FadeCurtain").GetComponent<FadingScript> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel ("statsScene");
	}
	void teamWins(int whichTeam){
		Debug.Log ("Team wins running");
//		winText.text = "Team " + whichTeam.ToString () + " Wins!";
//		winText.CrossFadeAlpha(1f,.25f,false);
		switch (whichTeam) {
		case 1:
			scoreboard.GetComponent<ScoreboardManagerScript> ().TeamOneWin ();
			background.GetComponent<BackgroundColorScript> ().whoWon = 1;
			background.GetComponent<BackgroundColorScript> ().matchOver = true;
			background.GetComponent<BackgroundColorScript> ().TurnOffMatchPoint ();

			break;
		case 2: 
			scoreboard.GetComponent<ScoreboardManagerScript> ().TeamTwoWin ();
			background.GetComponent<BackgroundColorScript> ().whoWon = 2;
			background.GetComponent<BackgroundColorScript> ().matchOver = true;
			background.GetComponent<BackgroundColorScript> ().TurnOffMatchPoint ();
			break;
		}
		isGameOver = true;
		if (!readyForReplay) {
			EZReplayManager.get.stop ();
			readyForReplay = true;
		//	Invoke ("PlayReplay", 2f);
		}

		//EZReplayManager.get.play(-1,false,true,true);
		//DataManagerScript.dataManager.teamOneWins += 1;
		//Invoke ("LaunchTitleScreen", 5f);
		Invoke ("LaunchStatsScreen", 5f);
	}
	// Update is called once per frame

	void PlayReplay(){
		EZReplayManager.get.play (0, true, false, true);
	}

	void Update () {

		// if all 4 start buttons are pressed, warp back to title screen 
		if (Input.GetButton (startButton1) && Input.GetButton (startButton2) && Input.GetButton (startButton3) && Input.GetButton (startButton4)) {
			Debug.Log ("returning to title");
			Application.LoadLevel ("titleScene");
		}

		// keep track of match time
		DataManagerScript.gameTime += Time.deltaTime;


		timeSinceLastPowerup += Time.deltaTime;

		if (timerRunning) {
			gameTimer -= Time.deltaTime;
		}	
		if (teamOneScore >= scorePlayedTo && teamOneScore == teamTwoScore + 1) {
			background.GetComponent<BackgroundColorScript> ().TurnOnMatchPoint (1);
		} else if (teamTwoScore >= scorePlayedTo && teamTwoScore == teamOneScore + 1) {
			background.GetComponent<BackgroundColorScript> ().TurnOnMatchPoint (2);
		}
		if (teamOneScore >= scorePlayedTo && teamOneScore > teamTwoScore + 1) {
			//Debug.Log ("Run team one wins routine here");
			teamWins (1);
		} else if (teamTwoScore >= scorePlayedTo && teamTwoScore > teamOneScore + 1){
		//	Debug.Log ("Run team two wins routine here");
			teamWins (2);
		}

		if (!isGameOver) {
			ConsiderAPowerup ();
		}
	}

	void ConsiderAPowerup(){
		if (timeSinceLastPowerup >= powerupAppearTime) {

			// spawn a powerup
			float xVal = Random.Range(4f, 15.5f);
			float inverseXVal = -1 * xVal;
			float yVal = Random.Range (-5f, 5f);
			int whichType = Random.Range (1, 5);
			float powerupDuration = 5f + (Random.value * 5f);
			GameObject firstPowerup = (GameObject)Instantiate(powerupPrefab, new Vector3(xVal, yVal, 0), Quaternion.identity);
			firstPowerup.SendMessage("Config", whichType);
			firstPowerup.SendMessage("ResetTime", powerupDuration);
			GameObject secondPowerup = (GameObject)Instantiate(powerupPrefab, new Vector3(inverseXVal, yVal, 0), Quaternion.identity);
			secondPowerup.SendMessage("Config", whichType);
			secondPowerup.SendMessage("ResetTime", powerupDuration);
			timeSinceLastPowerup = 0f;
			powerupAppearTime = 20f + Random.value * 20f;
		}
	}
}

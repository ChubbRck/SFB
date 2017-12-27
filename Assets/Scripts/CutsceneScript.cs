﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneScript : MonoBehaviour {

	public string nextScene;
	public float sceneDuration;

	// Use this for initialization
	void Start () {
		Invoke ("NextScene", sceneDuration);
	}

	void NextScene(){
		SceneManager.LoadSceneAsync(nextScene);
	}
	// Update is called once per frame
	void Update () {

	}
}

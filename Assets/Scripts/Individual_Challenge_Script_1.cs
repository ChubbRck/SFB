﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Individual_Challenge_Script_1 : MonoBehaviour {

	private GameObject ballPrefab;
	private int deadBalls;
    public bool multiLaunch = false;
    public float gravScale = 1.0f;

    public GameObject[] pads;

    public String challengeTitle;

	private bool challengeStarted = false;
    private bool challengeOver = false;

	void Awake(){
		
	}
		
	void Start () {
		deadBalls = 0;
		// TODO: Once we're done modularizing, we won't need a unique ball prefab
		ballPrefab = ChallengeManagerScript.Instance.GetComponent<ChallengeManagerScript>().ballPrefab;
		ChallengeManagerScript.Instance.UpdateChallengeText(challengeTitle);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!challengeStarted){
			// check for challenge start
			if (ChallengeManagerScript.Instance.challengeRunning){
				challengeStarted = true;
                if (multiLaunch)
                {
                    // Using InvokeRepeating is probably a bad idea...
                    InvokeRepeating("LaunchBallRandom", 0f, 5f);
                }
                else
                {
                    LaunchBall(0f, 0f, 0f);
                }
			}
		}

		if (challengeStarted){
            //Check for victory
            //if (!pad_1.active && !pad_2.active && !pad_3.active){
            //	ChallengeManagerScript.Instance.ChallengeSucceed();
            //}
            if (!challengeOver)
            {
                for (int i = 0; i < pads.Length; i++)
                {
                    if (pads[i].active)
                    {
                        return;
                    }
                }
                ChallengeManagerScript.Instance.ChallengeSucceed();
                challengeOver = true;
            }



        }
	}

	void BallDied(int whichSide){
		Debug.Log ("the ball has died");
		deadBalls += 1;

        // Launch a replacement ball
        if (ChallengeManagerScript.Instance.challengeRunning && !multiLaunch)
        {
            LaunchBall(0f, 0f, 0f);
        }
	}

    public void LaunchBallRandom()
    {
        GameObject ball_1 = Instantiate(ballPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        ball_1.transform.parent = gameObject.transform.parent;
        IEnumerator coroutine_1 = ball_1.GetComponent<BallScript>().CustomLaunchBallWithDelay(2f, -6f, UnityEngine.Random.Range(0f,10f));
        StartCoroutine(coroutine_1);
        // set ball's gravChangeMode to true;
        Debug.Log("setting gravchange mode to true");
       ball_1.GetComponent<BallScript>().gravScale = gravScale;
        ball_1.GetComponent<BallScript>().gravChangeMode = true;
        ball_1.GetComponent<BallScript>().baseTimeBetweenGravChanges = 5f;
        ball_1.GetComponent<BallScript>().gravTimeRange = 4f;
    }

    public void LaunchBall(float x, float y, float z){
		GameObject ball_1 = Instantiate(ballPrefab, new Vector3(x, y, z), Quaternion.identity);
		ball_1.transform.parent = gameObject.transform.parent;
		IEnumerator coroutine_1 = ball_1.GetComponent<BallScript> ().CustomLaunchBallWithDelay (2f, -6f, 10f);
		StartCoroutine(coroutine_1);
        // set ball's gravChangeMode to true;
        Debug.Log("setting gravchange mode to true");
        ball_1.GetComponent<BallScript>().gravScale = gravScale;
        ball_1.GetComponent<BallScript>().gravChangeMode = true;
        ball_1.GetComponent<BallScript>().baseTimeBetweenGravChanges = 5f;
        ball_1.GetComponent<BallScript>().gravTimeRange = 4f;
    }



    public IEnumerator LaunchBalls(float interval, int invokeCount)
	{
		for (int i = 0; i < invokeCount; i++)
		{
			LaunchBall(0f,0f,0f);

			yield return new WaitForSeconds(interval);
		}
	}
}


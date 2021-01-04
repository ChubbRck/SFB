﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Challenge_Script_Multiball : MonoBehaviour
{

    private GameObject ballPrefab;
    private int deadBalls;
    public bool multiLaunch = false;
    private bool gameUnderway = false;
    public GameObject[] pads;
    public float gravScale = 1.0f;

    public String challengeTitle;

    private bool challengeStarted = false;
    private bool challengeOver = false;

    public float startingPolarity = 1f;

    public bool canDie = true;

    void Awake()
    {

    }

    void Start()
    {
        deadBalls = 0;
        // TODO: Once we're done modularizing, we won't need a unique ball prefab
        ballPrefab = ChallengeManagerScript.Instance.GetComponent<ChallengeManagerScript>().ballPrefab;
        ChallengeManagerScript.Instance.UpdateChallengeText(challengeTitle);
        Invoke("GameIsUnderway", 5f);
    }

    // Update is called once per frame
    void Update()
    {

        if (!challengeStarted)
        {
            // check for challenge start
            if (ChallengeManagerScript.Instance.challengeRunning)
            {
                challengeStarted = true;
                if (multiLaunch)
                {
                    // Using InvokeRepeating is probably a bad idea...
                    InvokeRepeating("LaunchBallRandom", 0f, 6.5f);
                }
                else
                {
                    LaunchBall(0f, 0f, 0f);
                }
            }
        }

        if (challengeStarted)
        {
           
            if (!challengeOver)
            {   
                if (GameObject.FindGameObjectsWithTag("Ball").Length == 0 && gameUnderway && canDie)
                {
                   ChallengeManagerScript.Instance.ChallengeFail();
                }

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
    void GameIsUnderway()
    {
        gameUnderway = true;
    }

    void BallDied(int whichSide)
    {
        Debug.Log("the ball has died");
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
        IEnumerator coroutine_1 = ball_1.GetComponent<BallScript>().CustomLaunchBallWithDelay(2f, -6f, UnityEngine.Random.Range(0f, 10f));
        StartCoroutine(coroutine_1);
        // set ball's gravChangeMode to true;
        Debug.Log("setting gravchange mode to true");
        // TODO: There has to be a more scalable way to set these settings
        ball_1.GetComponent<BallScript>().gravScale = gravScale;
        ball_1.GetComponent<BallScript>().startWithRandomGrav = true;
        ball_1.GetComponent<BallScript>().gravChangeMode = true;
        ball_1.GetComponent<BallScript>().baseTimeBetweenGravChanges = 7f;
        ball_1.GetComponent<BallScript>().gravTimeRange = 4f;
        ball_1.GetComponent<BallScript>().playSoundOnGravChange = false;

        public void LaunchBall(float x, float y, float z)
    {
        GameObject ball_1 = Instantiate(ballPrefab, new Vector3(x, y, z), Quaternion.identity);
        ball_1.transform.parent = gameObject.transform.parent;
        IEnumerator coroutine_1 = ball_1.GetComponent<BallScript>().CustomLaunchBallWithDelay(2f, -6f, 10f);
        StartCoroutine(coroutine_1);
        // set ball's gravChangeMode to true;
        Debug.Log("setting gravchange mode to true");

        ball_1.GetComponent<BallScript>().gravScale = gravScale;
        ball_1.GetComponent<BallScript>().startWithRandomGrav = true;
        ball_1.GetComponent<BallScript>().gravChangeMode = true;
        ball_1.GetComponent<BallScript>().baseTimeBetweenGravChanges = 7f;
        ball_1.GetComponent<BallScript>().gravTimeRange = 4f;
        ball_1.GetComponent<BallScript>().playSoundOnGravChange = false;
    }



    public IEnumerator LaunchBalls(float interval, int invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
            LaunchBall(0f, 0f, 0f);

            yield return new WaitForSeconds(interval);
        }
    }
}


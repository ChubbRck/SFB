﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewFadeScript : MonoBehaviour {

	private Image Image;

	// Use this for initialization
	void Start () {
		Image = gameObject.GetComponentInChildren<Image>();
	}

	public float Fade(float destinationAlpha){
		float fadeTime = .5f;

		if (!Image) {
            Image = gameObject.GetComponentInChildren<Image>();
        }

		if (Image) {
			Image.CrossFadeAlpha(destinationAlpha, fadeTime, false);
			return fadeTime;
		}

		return 0f;
	}
}

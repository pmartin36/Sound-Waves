using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySpeedButton : MonoBehaviour {

	public bool FastForwarded { get; private set; }
	Image image;

	public Sprite playSprite;
	public Sprite ffSprite;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseDown() {
		FastForwarded = !FastForwarded;
		if (FastForwarded) {
			Time.timeScale = 2f;
			image.sprite = playSprite;
		}	
		else {
			Time.timeScale = 1f;
			image.sprite = ffSprite;
		}
	}
}

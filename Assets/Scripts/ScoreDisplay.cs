using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ScoreDisplay : MonoBehaviour {

	public TMP_Text scoreLabel;
	public TMP_Text score;

	// Use this for initialization
	void Start () {
		var texts = GetComponentsInChildren<TMP_Text>();

		score = texts.Single( g => g.tag == "ScoreNumber");
		scoreLabel = texts.Single( g => g.tag != "ScoreNumber");
	}
	
	// Update is called once per frame
	void Update () {
		score.text = GameManager.Instance.scoreManager.Score.ToString();
	}
}

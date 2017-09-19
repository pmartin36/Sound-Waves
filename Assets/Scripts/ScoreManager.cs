using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	public float Score { get; set; }

	// Use this for initialization
	void Start () {	
		GameManager.Instance.scoreManager = this;
		StartCoroutine(IncrementScore());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator IncrementScore() {
		while (!GameManager.Instance.GameOver) {
			yield return new WaitForSeconds(1f);
			Score += 10f;			
		}
	}
}

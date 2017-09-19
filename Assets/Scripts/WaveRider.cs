using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveRider : MonoBehaviour {

	[SerializeField]
	private Wave wave;

	public bool AttachedToWave { get; set; }
	public int RestingPointIndex { get; set; }

	public bool Selected { get; set; }

	// Use this for initialization
	void Start () {
		AttachedToWave = false;
		RestingPointIndex = 400;

		GameManager.Instance.WaveRiders.Add(this);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//if we haven't found the resting point yet

		if(AttachedToWave) {
			transform.position = wave.WavePositions[RestingPointIndex];
		}
		else {
			if (wave.WavePositions.Count > 0 && wave.WavePositions.Count - 1 <= RestingPointIndex) {
				transform.position = wave.WavePositions[wave.WavePositions.Count - 1];
			}
			else if (wave.WavePositions.Count > 0 && wave.WavePositions.Count - 1 > RestingPointIndex) {
				AttachedToWave = true;
			}
		}
		
	}

	internal void Move(int diff) {
		RestingPointIndex += diff;
		transform.position = wave.WavePositions[RestingPointIndex];
	}

	public void OnMouseDown() {
		Selected = true;
	}

	public void OnCollisionEnter2D(Collision2D collision) {
		if(collision.collider.tag == "Spikes") {
			GameManager.Instance.GameOver = true;

			//temporory
			Destroy(this.gameObject);
		}
	}
}

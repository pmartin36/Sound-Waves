using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveformPower : MonoBehaviour {

	WaveformGenerator parent;
	public Color onColor;
	Image image;

	// Use this for initialization
	void Start () {
		parent = this.transform.parent.GetComponent<WaveformGenerator>();
		image = GetComponent<Image>();
	}

	private void Update() {
		SetLightColor();
	}

	public void SetLightColor() {
		if (parent._internalActive) {
			image.color = Color.green;
		}
		else {
			image.color = Color.red;
		}
	}

	private void OnMouseDown() {
		parent.ToggleActive();	
	}
}

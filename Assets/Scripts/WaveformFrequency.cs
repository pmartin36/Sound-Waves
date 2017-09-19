using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveformFrequency : WaveformOption {

	private float _frequency;
	private string _displayFrequency;
	public float Frequency {
		get { return _frequency; }
		private set
		{
			_frequency = value / 20f;
			_displayFrequency = value + " Hz";
		}
	}

	private static int[] AvailableFrequencies = { 30, 40, 55, 75, 100, 130 };
	private int fIndex;

	public override void Awake() {
		base.Awake();	
	}

	public override void Start() {
		fIndex = 0;
		Frequency = AvailableFrequencies[fIndex];
	}

	public override void Update () {
		displayText.text = _displayFrequency;
	}

	public override void ChangeValue(int diff) {
		base.ChangeValue(diff);

		fIndex += diff;
		if(fIndex < 0) {
			fIndex = AvailableFrequencies.Length - 1;
		}
		else if(fIndex >= AvailableFrequencies.Length) {
			fIndex = 0;
		}
		Frequency = AvailableFrequencies[fIndex];
	}

	protected override void OnMouseDown() {
		base.OnMouseDown();
	}
}

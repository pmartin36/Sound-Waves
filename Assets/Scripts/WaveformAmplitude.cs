using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveformAmplitude : WaveformOption {

	private float _amplitude;
	private string _displayAmplitude;
	public float Amplitude {
		get { return _amplitude; }
		private set
		{
			_amplitude = value * (5f / 11f); //maximum amplitude we want is 5, knob goes up to 11
			_displayAmplitude = value.ToString();
		}
	}

	public override void Awake() {
		base.Awake();		
	}

	public override void Start() {
		Amplitude = Random.Range(1,11);
	}

	public override void Update() {
		displayText.text = _displayAmplitude;
	}

	public override void ChangeValue(int diff) {
		base.ChangeValue(diff);

		float pd = int.Parse(_displayAmplitude) + diff;
		Amplitude = Mathf.Clamp(pd, 1f, 11f);
	}

	protected override void OnMouseDown() {
		base.OnMouseDown();
	}
}

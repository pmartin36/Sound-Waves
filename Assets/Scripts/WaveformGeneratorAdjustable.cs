using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class WaveformGeneratorAdjustable : MonoBehaviour {

	public float CurrentValue { get; set; }

	private WaveformFrequency frequencyOption;
	private WaveformAmplitude amplitudeOption;

	private float AmplitudeModifier;

	public bool _internalActive;
	public bool Active { get; set; }
	private float TurnedOnTime;

	private float usedFrequency;
	private float usedAmplitude;
	private float lastValue;

	private Coroutine powerCoroutine;

	void Start() {
		TurnedOnTime = Time.fixedTime;

		_internalActive = false;
		Active = false;
		AmplitudeModifier = 1f;

		frequencyOption = GetComponentInChildren<WaveformFrequency>();
		amplitudeOption = GetComponentInChildren<WaveformAmplitude>();
	}

	public void Update() {

	}

	// Update is called once per frame
	void FixedUpdate() {
		CurrentValue = AmplitudeModifier * usedAmplitude * Mathf.Sin( usedFrequency * (Time.fixedTime - TurnedOnTime));

		if( Mathf.Abs(CurrentValue) < 0.1f ) {
			if (frequencyOption.Changed) {
				usedFrequency = frequencyOption.Frequency;
				frequencyOption.Changed = false;

				TurnedOnTime = Time.fixedTime;
				if( Mathf.Sign( CurrentValue - lastValue ) < 0 ) {
					//decreasing so we need to offset by 180
					TurnedOnTime = Time.fixedTime - (Mathf.PI / (usedFrequency));
				}
			}
			if (amplitudeOption.Changed) {
				usedAmplitude = amplitudeOption.Amplitude;
				amplitudeOption.Changed = false;
			}			
		}

		lastValue = CurrentValue;
	}

	public bool ToggleActive() {
		_internalActive = !_internalActive;

		if (powerCoroutine != null) {
			StopCoroutine(powerCoroutine);
		}

		if (_internalActive) {
			Active = _internalActive;
			TurnedOnTime = Time.fixedTime;
			usedFrequency = frequencyOption.Frequency;
			usedAmplitude = amplitudeOption.Amplitude;
			powerCoroutine = StartCoroutine(PowerOnOff(true, amplitudeOption.Amplitude));
		}
		else {
			powerCoroutine = StartCoroutine(PowerOnOff(false, amplitudeOption.Amplitude));
		}
		return _internalActive;
	}

	IEnumerator PowerOnOff(bool on, float amp) {
		float startTime = Time.time;
		float shutdownTime = (Mathf.PI) / usedFrequency;
		while( Time.time - startTime < shutdownTime) {
			AmplitudeModifier = ((Time.time - startTime) / shutdownTime);
			AmplitudeModifier = on ? AmplitudeModifier : 1 - AmplitudeModifier;
			yield return new WaitForEndOfFrame();
		}
		Active = on;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class WaveformGenerator : MonoBehaviour {

	public float CurrentValue { get; set; }

	public float Frequency;
	private float usedFrequency;

	private float MaximumAmplitude;
	private float usedAmplitude;
	private bool amplitudeChanged;
	public float Amplitude;

	private bool powering;

	private float AmplitudeModifier;

	public bool _internalActive;
	public bool Active { get; set; }
	private float TurnedOnTime;

	private Coroutine powerCoroutine;
	private TMP_Text frequencyDisplay;
	private Image PowerLight;

	public Color onColor;

	void Start() {
		TurnedOnTime = Time.fixedTime;

		_internalActive = false;
		Active = false;
		AmplitudeModifier = 1f;

		MaximumAmplitude = 2f;
		Amplitude = 0f;
		amplitudeChanged = false;
		usedAmplitude = Amplitude;

		powering = false;

		frequencyDisplay = GetComponentInChildren<TMP_Text>();
		frequencyDisplay.text = Frequency.ToString() + " Hz";

		usedFrequency = Frequency / 25f;

		PowerLight = GetComponentsInChildren<Image>().Single(g => g.tag == "GeneratorPowerDisplay");

		Color c = onColor;
		c.a = 0.1f;
		PowerLight.color = c;
	}

	public void Update() {

	}

	// Update is called once per frame
	void FixedUpdate() {
		CurrentValue = AmplitudeModifier * usedAmplitude * Mathf.Sin(usedFrequency * (Time.fixedTime - TurnedOnTime));

		if (Mathf.Abs(CurrentValue) < 0.02f) {
			/*
			if (frequencyOption.Changed) {
				usedFrequency = frequencyOption.Frequency;
				frequencyOption.Changed = false;

				TurnedOnTime = Time.fixedTime;
				if (Mathf.Sign(CurrentValue - lastValue) < 0) {
					//decreasing so we need to offset by 180
					TurnedOnTime = Time.fixedTime - (Mathf.PI / (usedFrequency));
				}
			}
			*/
			if (amplitudeChanged) {
				if(Amplitude < 0.1f) {
					StartCoroutine(PowerOnOff(false));
				}
				else {				
					//we should start a coroutine that takes Frequency/PI time to complete
					//over the time, we should slowly transition from one amplitude to the other
					usedAmplitude = Amplitude;					
				}
				amplitudeChanged = false;
			}
		}

		//lastValue = CurrentValue;
	}

	public void ToggleActive() {
		if (Amplitude < MaximumAmplitude-0.1f) {			
			Amplitude += (MaximumAmplitude / 2f);

			Color c = onColor;
			c.a = Amplitude / MaximumAmplitude;
			PowerLight.color = c;
			
			if(Amplitude > (MaximumAmplitude/2f)+0.1f) {
				amplitudeChanged = true;
			}
			else if(!_internalActive) {
				usedAmplitude = Amplitude;
				StartCoroutine(PowerOnOff(true));
			}
		}
		else {
			Amplitude = 0f;
			amplitudeChanged = true;

			Color c = onColor;
			c.a = 0.1f;
			PowerLight.color = c;
		}
	}

	public void OnMouseDown() {
		ToggleActive();
	}

	IEnumerator PowerOnOff(bool on) {
		float startTime = Time.time;

		while(powering) {
			yield return new WaitForEndOfFrame();
		}
		powering = true;
		_internalActive = on;		
		if (on) {
			TurnedOnTime = Time.fixedTime;
			Active = true;
		}

		float shutdownTime = (Mathf.PI) / usedFrequency;
		while (Time.time - startTime < shutdownTime) {
			AmplitudeModifier = ((Time.time - startTime) / shutdownTime);
			AmplitudeModifier = on ? AmplitudeModifier : 1 - AmplitudeModifier;
			yield return new WaitForEndOfFrame();
		}

		powering = false;
		Active = on;
	}
}

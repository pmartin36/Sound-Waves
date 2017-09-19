using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveformGeneratorManager : MonoBehaviour {

	List<WaveformOption> options;
	public int SelectedIndex { get; set; }

	// Use this for initialization
	void Start () {
		options = new List<WaveformOption>();

		options = GetComponentsInChildren<WaveformOption>().OrderByDescending(g => g.transform.position.y).ToList();

		SelectedIndex = 0;
		options[SelectedIndex].Select();

		//GameManager.Instance.waveformManager = this;
	}
	
	public void HandleInputs(InputPackage p) {
		if(p.Up) {
			SelectedIndex--;
			if(SelectedIndex < 0) {
				SelectedIndex = options.Count - 1;
			}
			SetSelection();
		}
		else if(p.Down) {
			SelectedIndex++;
			if(SelectedIndex >= options.Count) {
				SelectedIndex = 0;
			}
			SetSelection();
		}
		else if(p.Left) {
			options[SelectedIndex].ChangeValue(-1);
		}
		else if(p.Right) {
			options[SelectedIndex].ChangeValue(1);
		}

		if(p.Select) {
			options[SelectedIndex].transform.parent.GetComponent<WaveformGenerator>().ToggleActive();
		}
	}

	public void SetSelection() {
		foreach( WaveformOption o in options.Where( g => g.Selected )) {
			o.Deselect();
		}
		options[SelectedIndex].Select();
	}

	internal void SetSelectedIndex(WaveformOption waveformOption) {
		SelectedIndex = options.IndexOf(waveformOption);
		SetSelection();
	}

	// Update is called once per frame
	void Update () {
		
	}
}

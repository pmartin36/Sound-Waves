using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainWave : Wave {

	public bool AllowRiderMovement { get; set; }

	// Use this for initialization
	protected override void Start () {
		base.Start();

		WaveWidth = new Vector3(0,0.2f,0);
		lr.endWidth = lr.startWidth = WaveWidth.y;

		Generators = new List<WaveformGenerator>();
		Generators.AddRange(GameObject.FindGameObjectsWithTag("Generator").Select(g => g.GetComponent<WaveformGenerator>()));

		AllowRiderMovement = false;

		GameManager.Instance.wave = this;
	}

	protected override void PropagateWave() {
		base.PropagateWave();

		if (WavePositions.Count > WavePoints) {
			WavePositions.RemoveRange(WavePoints, WavePositions.Count - WavePoints);
			AllowRiderMovement = true;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}

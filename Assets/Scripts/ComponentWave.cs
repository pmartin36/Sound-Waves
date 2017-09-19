using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentWave : Wave {

	protected override void Start() {
		base.Start();

		WaveWidth = new Vector3(0, 0.1f, 0);
		lr.endWidth = lr.startWidth = WaveWidth.y;

		if( Generators.Count > 0 ) {
			Color c = Generators[0].onColor;
			c.a = 0.05f;
			lr.startColor = lr.endColor = c;
		}

		amplitudeMultiplier = 0.35f;
	}

	protected override void PropagateWave() {
		base.PropagateWave();

		if (WavePositions.Count > WavePoints) {
			WavePositions.RemoveRange(WavePoints, WavePositions.Count - WavePoints);
		}
	}

	void Update() {

	}
}

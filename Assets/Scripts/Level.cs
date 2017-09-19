using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Level : MonoBehaviour {

	public List<Tube> tubes;

	// Use this for initialization
	void Awake () {
		tubes = new List<Tube>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveAmount = new Vector3(1f * Time.deltaTime, 0f, 0);
		transform.position = transform.position - moveAmount;

		foreach(Tube t in tubes) {	
			Vector3[] positions = t.linePositions.Select(l => l - (moveAmount + t.totalMoved)).ToArray();
			t.lineRenderer.SetPositions( positions );
			t.totalMoved += moveAmount;
		}
	}
}

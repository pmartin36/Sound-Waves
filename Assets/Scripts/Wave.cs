using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wave : MonoBehaviour {

	private MeshFilter mf;
	public List<Vector3> WavePositions;

	public List<WaveformGenerator> Generators;

	protected Vector3 WaveWidth;
	protected float amplitudeMultiplier;

	public int WavePoints;
	public float WaveLength;
	public Vector3 MoveAmountPerUpdate;

	private Vector3[] Vertices;

	protected LineRenderer lr;
	
	private int lastFrameActiveGenerators;
	private float baselineStartTime;

	// Use this for initialization
	protected virtual void Start () {
		mf = GetComponent<MeshFilter>();
		WavePositions = new List<Vector3>();	

		WaveWidth = new Vector3(0,0.2f,0);

		WaveLength = 20f;
		WavePoints = 500;
		MoveAmountPerUpdate = new Vector3(WaveLength / (float)WavePoints, 0f, 0f);

		Vertices = new Vector3[WavePoints * 2];

		lr = GetComponentInChildren<LineRenderer>();

		amplitudeMultiplier = 1f;

		StartCoroutine(GenerateWave());
	}

	protected virtual void PropagateWave() {
		for (int i = 0; i < WavePositions.Count; i++) {
			WavePositions[i] -= MoveAmountPerUpdate;
		}

		float yval = 0.1f * Mathf.Sin(5f * baselineStartTime);
		var activeGenerators = Generators.Where( g => g != null && g.Active );
		if(activeGenerators.Count() > 0) {
			yval += activeGenerators.Sum( g => g.CurrentValue );
		}
		else {
			baselineStartTime += Time.deltaTime;
		} 
		yval *= amplitudeMultiplier;
		lastFrameActiveGenerators = activeGenerators.Count();

		Vector3 newWavePosition = new Vector3(10f, yval, 0f) + transform.position;
		WavePositions.Insert(0,newWavePosition);
	}

	protected virtual void DrawWaveMesh() {
		lr.positionCount = WavePositions.Count;
		lr.SetPositions(WavePositions.ToArray());
	}

	// Update is called once per frame
	void Update () {
		
	}

	protected virtual IEnumerator GenerateWave() {
		yield return new WaitForSeconds(Mathf.PI/5f);
		while(true) {
			PropagateWave();

			if (WavePositions.Count > 2) {
				DrawWaveMesh();
			}

			yield return new WaitForFixedUpdate();
		}
	}
}

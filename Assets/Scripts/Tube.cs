using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Tube : MonoBehaviour {

	public enum TubeType {
		Straight,
		Macaroni,
		DroppingSquiggly,
		RisingSquiggly
	}

	public LineRenderer lineRenderer;
	public Vector3 startPosition;
	public GameObject RingPrefab;

	public int NumRings;
	public float Rotation;

	public List<Vector3> linePositions;
	public Vector3 totalMoved;

	// Use this for initialization
	void Start () {
		lineRenderer = gameObject.GetComponent<LineRenderer>();

		TubeType tubeType = TubeType.Straight;//(TubeType)Random.Range(0,4);
		switch (tubeType) {
			default:
			case TubeType.Straight:
				CreateStraightPipe();
				break;
			case TubeType.Macaroni:
				break;
			case TubeType.DroppingSquiggly:
				break;
			case TubeType.RisingSquiggly:
				break;					
		}

		this.transform.parent.GetComponent<Level>().tubes.Add(this);
	}

	public void CreateStraightPipe() {
		int numRings = NumRings;//Random.Range(4,11);
		//float rotation = Random.Range(-75,75);
		float ringSpacing = 2f;
		float numSegmentsBetweenRings = 15f;

		Vector2 pivotPoint = (2*startPosition + new Vector3(0.5f * numRings, startPosition.y)) / 2f;

		List<Vector3> ringPositions = new List<Vector3>();
		linePositions = new List<Vector3>();

		for (int i = 0; i < numRings; i++) {
			bool success = AddRing(ringPositions, startPosition + new Vector3(i * ringSpacing, 0, 0), Rotation, pivotPoint, 0);

			if(i > 0 && success) {
				for(int j = 0; j < numSegmentsBetweenRings; j++) {
					linePositions.Add ( Vector3.Lerp( ringPositions[i-1], ringPositions[i], j / numSegmentsBetweenRings ));
				}
			}
		}

		lineRenderer.positionCount = linePositions.Count;
		lineRenderer.SetPositions(linePositions.ToArray());
	}

	public bool AddRing(List<Vector3> ringPositions, Vector3 position, float rotation, Vector3 pivotPoint, float localDirection) {
		Vector3 dir = position - pivotPoint;
		dir = Quaternion.Euler(0,0, rotation) * dir;
		var result = dir + pivotPoint;

		//only spawn rings that the player can see
		Vector2 onCamera = Camera.main.WorldToViewportPoint(result);
		if ( onCamera.y >= 0 && onCamera.y < 1) {
			Instantiate(RingPrefab, result, Quaternion.Euler(0,0, (rotation+localDirection)), this.transform);
			ringPositions.Add(result);
			return true;
		}
		return false;
	}

	public float xyToAngle(float x, float y) {
		return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
	}

	public float VectorToAngle(Vector2 vector) {
		return xyToAngle(vector.x, vector.y);
	}

	// Update is called once per frame
	void Update () {
		
	}
}

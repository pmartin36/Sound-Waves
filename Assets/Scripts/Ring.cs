using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Player" && !collision.isTrigger) {
			GameManager.Instance.chargeManager.ChangeCharge(50);
			GameManager.Instance.scoreManager.Score += 50;
			Destroy(this.gameObject);
		}
	}
}

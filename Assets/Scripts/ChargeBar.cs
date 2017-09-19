using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour {

	Material material;

	// Use this for initialization
	void Start () {
		material = GetComponent<Image>().material;
	}
	
	// Update is called once per frame
	void Update () {
		material.SetFloat("_Cutoff", 1-GameManager.Instance.chargeManager.ChargePct);
	}
}

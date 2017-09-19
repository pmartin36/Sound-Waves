using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeManager : MonoBehaviour {

	private float _charge;
	public float ChargePct { get; private set; }
	public float MaxCharge { get; private set; }
	public float Charge {
		get
		{
			return _charge;
		}
		private set
		{
			_charge = value;
			ChargePct = _charge / MaxCharge;
		}
	}

	// Use this for initialization
	void Start () {
		MaxCharge = 500;
		Charge = 250;

		GameManager.Instance.chargeManager = this;
	}

	public void ChangeCharge(float diff) {
		Charge += diff;
	}

	private void Update() {
			
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WaveformOption : MonoBehaviour {

	protected TMP_Text displayText;
	protected BoxCollider2D box;

	public bool Changed { get; set; }

	public bool Selected { get; set; }

	public virtual void Awake() {
		displayText = GetComponent<TMP_Text>();
		box = GetComponent<BoxCollider2D>();		
	}

	public virtual void Start() {

	}

	public virtual void Update() {

	}

	public virtual void Select() {
		Selected = true;
		displayText.color = new Color(0.4f, 0.7f, 0.9f);
	}

	public virtual void Deselect() {
		Selected = false;
		displayText.color = Color.white;
	}

	public virtual void ChangeValue(int diff) {
		Changed = true;
	}

	protected virtual void OnMouseDown() {
		if(Selected) return;
		this.transform.parent.parent.GetComponent<WaveformGeneratorManager>().SetSelectedIndex(this);
	}
}

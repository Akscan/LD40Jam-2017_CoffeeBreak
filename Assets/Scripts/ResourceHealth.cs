using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResourceHealth : MonoBehaviour {
	const int MaxHealth = 3;
	public int health = MaxHealth;
	private Slider healthSlider;
	// Use this for initialization
	void Awake () {
		healthSlider = GetComponentInChildren<Slider> ();
		//healthSlider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		healthSlider.value = health;
		if (health == 0) {
			Destroy (gameObject);
		}
	}

	public void NotMaxHealth()
	{
		healthSlider.enabled = true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerInteract : MonoBehaviour {
	int coffeeCount = 0;
	float InterractTextTimer = 0;
	const int sleepLong = 240;
	const int walkLong = 360;
	const int coffeeK = 60;
	const int FinalTime = 3000;
	const int FinalNeedRock = 200;
	const int FinalNeedWood = 200;
	float	 mainTimer = 0;
	float sleepTimer = 0;
	const int inventoryMax = 10;
	int woodCount = 0;
	int rockCount = 0;
	int needRock = 0;
	int needWood = 0;
	bool keyDown = false;
	private Camera fpsCam;
	public Slider mainTimerSlider;
	private Slider healthSlider;
	public Slider inventorySlider;
	public Slider sleepSlider;
	public Text woodText;
	public Text stoneText;
	public Text NeedWood;
	public Text NeedRock;
	public DayNightController LightControll;
	public Text interractText;
	public RawImage badFeelingsImage;
	Color colorFeelsBad;
	// Use this for initialization
	void Awake () {
		fpsCam = GetComponentInChildren<Camera> ();
		sleepSlider.maxValue = walkLong;
		inventorySlider.maxValue = inventoryMax;
		mainTimerSlider.maxValue = FinalTime;
		NeedRock.text = FinalNeedRock.ToString ();
		NeedWood.text = FinalNeedWood.ToString ();
		badFeelingsImage.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () {
		mainTimer += Time.deltaTime;
		mainTimerSlider.value = mainTimer;
		sleepTimer += Time.deltaTime;
		sleepSlider.value = sleepTimer;
		InterractTextTimer += Time.deltaTime;
		if (InterractTextTimer > 0.1f) {
			interractText.text = "";
		}
	}

	void FixedUpdate()
	{
		Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
		RaycastHit hit;
		if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, 5f)) {
			if (hit.collider.gameObject.name == "Bed") {
				interractText.text = "Sleep";
				InterractTextTimer = 0;
				if (Input.GetKey (KeyCode.E) && keyDown == false) {
					mainTimer += sleepTimer;
					mainTimer += mainTimer / 600;
					sleepTimer = 0;
					sleepSlider.value = 0;
					sleepSlider.maxValue = walkLong;
					keyDown = true;
					coffeeCount = 0;
					LightControll.currentTimeOfDay += mainTimer / 600;
				} else {
					keyDown = false;
				}
			}
			if (hit.collider.tag == "Rock") {
				GameObject tempObject = hit.collider.gameObject;
				interractText.text = "Smash";
				InterractTextTimer = 0;
				if (Input.GetKey (KeyCode.E) && keyDown == false) {
					if (rockCount + woodCount < inventoryMax) {
						rockCount += 1;
						ResourceHealth tempResourceHealth = tempObject.GetComponentInChildren<ResourceHealth> ();
						tempResourceHealth.health -= 1;
						inventorySlider.value += 1;
						keyDown = true;
						InterractTextTimer = 0;
					} else {
						keyDown = false;
					}
				}
			}
			if (hit.collider.tag == "Wood") {
				GameObject tempObject = hit.collider.gameObject;
				interractText.text = "Chop";
				InterractTextTimer = 0;
				if (Input.GetKey (KeyCode.E) && keyDown == false) {
					if (woodCount + rockCount < inventoryMax) {
						woodCount += 1;
						ResourceHealth tempResourceHealth = tempObject.GetComponentInChildren<ResourceHealth> ();
						tempResourceHealth.health -= 1;
						inventorySlider.value += 1;
						keyDown = true;
						InterractTextTimer = 0;
					} else {
						keyDown = false;
					}
				}
			}
			if (hit.collider.name == "Coffee" && keyDown == false) {
				interractText.text = "Drink";
				InterractTextTimer = 0;
				if (Input.GetKey (KeyCode.E) && keyDown == false) {
					sleepSlider.maxValue += coffeeK;
					keyDown = true;
					coffeeCount += 1;
					InterractTextTimer = 0;
					badFeelingsImage.color = new Color (1f, 0f, 0f, 0.2f*coffeeCount);
				} else {
					keyDown = false; 
				}
			}
			if (hit.collider.name == "Stock") {
				interractText.text = "Store item";
				InterractTextTimer = 0;
				if (Input.GetKey (KeyCode.E) && keyDown == false) {
					needRock += rockCount;
					rockCount = 0;
					needWood += woodCount;
					woodCount = 0;
					inventorySlider.value = 0;
					keyDown = true;
					woodText.text = needWood.ToString ();
					stoneText.text = needRock.ToString ();
					InterractTextTimer = 0;
				}
			}
			if (coffeeCount == 0) {
				badFeelingsImage.color = Color.clear;
			}
			if (coffeeCount == 5) {
				SceneManager.LoadSceneAsync ("DieByCoffee");
				SceneManager.UnloadSceneAsync ("MainScene");
			}
			if (needRock == FinalNeedRock && needWood == FinalNeedWood) {
				SceneManager.LoadScene ("WinScreen");
				SceneManager.UnloadSceneAsync ("MainScene");
			}
			if (!Input.GetKey (KeyCode.E)) {
				keyDown = false;
			}
		} else {
			keyDown = false;
		}
	}

}

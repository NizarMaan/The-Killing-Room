using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIndicatorUI : MonoBehaviour {

	public GameObject _PLAYER;
	private  PlayerController _PLAYER_SCRIPT;

	private int currentWeapon;

	private List<GameObject> weaponIndicators;

	// Use this for initialization
	void Start () {
		_PLAYER_SCRIPT = _PLAYER.GetComponent<PlayerController>();
		weaponIndicators = new List<GameObject>();
		weaponIndicators.Add(gameObject.transform.GetChild(0).gameObject);
		weaponIndicators.Add(gameObject.transform.GetChild(1).gameObject);
		currentWeapon = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(_PLAYER_SCRIPT.currentWeapon == (PlayerController.Weapon) 1 && currentWeapon != 1){
			SetSelectedAlpha(weaponIndicators[1]);
			SetSelectedAlpha(weaponIndicators[1].transform.GetChild(0).gameObject);

			SetDeselectedAlpha(weaponIndicators[currentWeapon]);
			SetDeselectedAlpha(weaponIndicators[currentWeapon].transform.GetChild(0).gameObject);

			currentWeapon = 1;
		}

		if(Input.GetKeyDown(KeyCode.Tab)){
			if(_PLAYER_SCRIPT.obtainedGun && currentWeapon + 1 == 1){
				SwapWeapons();
			}
			else{
				SwapWeapons();
			}
		}
	}

	private void SetSelectedAlpha(GameObject indicatorElement){
		SpriteRenderer sr = indicatorElement.GetComponent<SpriteRenderer>();
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
	}

	private void SetDeselectedAlpha(GameObject indicatorElement){
		SpriteRenderer sr = indicatorElement.GetComponent<SpriteRenderer>();
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.14f);
	}

	private void SwapWeapons(){
		SetDeselectedAlpha(weaponIndicators[currentWeapon]);
		SetDeselectedAlpha(weaponIndicators[currentWeapon].transform.GetChild(0).gameObject);

		if(currentWeapon >= weaponIndicators.Count - 1){
			currentWeapon = 0;
		}
		else{
			currentWeapon++;
		}

		SetSelectedAlpha(weaponIndicators[currentWeapon]);
		SetSelectedAlpha(weaponIndicators[currentWeapon].transform.GetChild(0).gameObject);

		_PLAYER_SCRIPT.currentWeapon = (PlayerController.Weapon) currentWeapon;
	}
}

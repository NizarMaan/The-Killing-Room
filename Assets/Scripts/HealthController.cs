using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

	public static int ReduceHealth(int damage, int initialHealth){
		return initialHealth - damage;
	}

	public static void UpdatePlayerHealthBar(float currentHealth, float maxHealth, GameObject healthbar, float healthbarMaxScale){
		Transform hbTransform = healthbar.GetComponent<Transform>();

		if(maxHealth > 0 && currentHealth >= 0){
			float ratio = currentHealth/maxHealth;
			
			hbTransform.localScale = new Vector2(healthbarMaxScale * ratio, hbTransform.localScale.y);
		}
	}
}

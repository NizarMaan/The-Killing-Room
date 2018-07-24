using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntController : MonoBehaviour {

	private int damage, health;
	public int Health{
		get{ return health; }
		set{ health = value; }
	}
	// Use this for initialization
	void Start () {
		damage = 5;
		health = 10;
	}
	
	// Update is called once per frame
	void Update () {
		//this condition prevents grunt from tipping over
		if(transform.rotation.z != 0){
			transform.rotation = new Quaternion();
		}

		if(health <= 0){
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// OnCollisionStay is called once per frame for every collider/rigidbody
	/// that is touching rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionStay2D(Collision2D other)
	{
		if(other.transform.tag.Equals("Player")){
			PlayerController playerScript = other.gameObject.GetComponent<PlayerController>();
			playerScript.Health = HealthController.ReduceHealth(damage, playerScript.Health);
			HealthController.UpdatePlayerHealthBar(playerScript.Health, playerScript.MaxHealth, playerScript.healthBar, playerScript.healthBarMaxScale);
		}
	}
}

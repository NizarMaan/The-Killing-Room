using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour {
	private GameObject _CROSSHAIR;
	private Transform transform;

	private Renderer renderer;

	private float speed, velocity;

	private Vector2 direction;

	private int damage;

	public int Damage{
		get{ return damage;}
		set{ damage = value;}
	}
	// Use this for initialization
	void Start () {
		damage = 50;
		transform = gameObject.GetComponent<Transform>();	
		renderer = gameObject.GetComponent<Renderer>();
		speed = 20.0f;
		_CROSSHAIR = GameObject.Find("crosshair");
		direction = _CROSSHAIR.transform.position - transform.position;
		direction.Normalize();

		transform.rotation = Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 90);	
	}
	
	// Update is called once per frame
	void Update () {
		velocity = speed * Time.deltaTime;
		//transform.Translate(direction.x * velocity, direction.y * velocity, 0.0f);
		transform.Translate(velocity, 0.0f, 0.0f);

		if(!renderer.isVisible){
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{

		if(other.transform.tag.Equals("BulletStopper")){
			Destroy(gameObject);
		}

		if(other.transform.tag.Equals("Grunt")){
			GruntController script = other.gameObject.GetComponent<GruntController>();
			script.Health = HealthController.ReduceHealth(damage, script.Health);
			Destroy(gameObject);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private Animator animator;
	private Transform transform;
	private Rigidbody2D rigidBody;
	private int direction;
	private float movementSpeed;
	private float jumpSpeed;
	private bool grounded;
	private int health, maxHealth;

	private int damageMelee;

	private int layerMask;

	private GameObject _CROSSHAIR;

	public Weapon currentWeapon;

	public bool obtainedGun;

	public GameObject platform, playerTop, playerBottom, bullet, pistol, healthBar;

	public float healthBarMaxScale;
	public enum Weapon
	{
		Fist = 0,  //represented by 0
		Pistol = 1, //represented by 1

		Katana = 2 //represented by 2
	}

	public int Health{
		get{ return health; }
		set{ health = value; }
	}

	public int MaxHealth{
		get{ return maxHealth; }
		set{ maxHealth = value; }
	}

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator>();
		transform = gameObject.GetComponent<Transform>();
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
 		direction = 1;
		movementSpeed = 0.035f;
		jumpSpeed = 9.0f;
		grounded = true;
		currentWeapon = Weapon.Fist;
		obtainedGun = false;
		health = maxHealth = 50;
		damageMelee = 10;
		healthBarMaxScale = healthBar.transform.localScale.x;

		layerMask = 0;
		int myLayer = gameObject.layer;
		for(int i = 0; i < 32; i++) {
			if(!Physics.GetIgnoreLayerCollision(myLayer, i))  {
				layerMask = layerMask | 1 << i;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		//if dead, trigger death animation state and destroy object
		if(health <= 0){
			//trigger death animation here...
			Destroy(gameObject); 
		}

		if(currentWeapon != Weapon.Pistol && _CROSSHAIR != null){
			HideObject(_CROSSHAIR);
			HideObject(pistol);
		}

		//this condition prevents player from tipping over
		if(transform.rotation.z != 0){
			transform.rotation = new Quaternion();
		}

		//if player is below the platform, ignore platform collision
		if(playerTop.transform.position.y <= platform.transform.position.y){
			//platform.GetComponent<BoxCollider2D>().isTrigger = true;
			Physics2D.IgnoreLayerCollision(9, 12);
		}

		//if whole body below platform, make platform "solid"
		if(playerBottom.transform.position.y >= platform.transform.position.y){
			//platform.GetComponent<BoxCollider2D>().isTrigger = false;
			Physics2D.SetLayerCollisionMask(9, layerMask);
		}

		//if player is walking, move him in respective direction
		if(animator.GetBool("Walking")){
			transform.position = new Vector2(transform.position.x + movementSpeed*direction, transform.position.y);
		}

		//if 'left click' key is pressed and current weapon is Fist, perform punching animation
		if(Input.GetMouseButtonDown(0) && currentWeapon == Weapon.Fist){
			animator.SetBool("Punching", true);
		}

		//if 'space' key is pressed, apply force in y-direction (jump)
		if(Input.GetKeyDown("space") && grounded){
			grounded = false;
			rigidBody.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
		}

		if(Input.GetKeyDown("a")){
			//if not already facing "left", face left
			if(direction == 1){
				direction *= -1;
				transform.localScale = new Vector2(transform.localScale.x * direction, transform.localScale.y);
			}

			if(!animator.GetBool("Walking")){
				animator.SetBool("Walking", true);
			}
		}

		if(Input.GetKeyDown("d")){
			//if not already facing "right", face right
			if(direction == -1){
				transform.localScale = new Vector2(transform.localScale.x * direction, transform.localScale.y);
				direction *= -1;
			}

			if(!animator.GetBool("Walking")){
				animator.SetBool("Walking", true);
			}
		}

		//if walking left and 'a' key goes up, stop walking
		if(Input.GetKeyUp("a") && direction == -1){
			animator.SetBool("Walking", false);
		}

		//if walking right and 'd' key goes up, stop walking
		if(Input.GetKeyUp("d") && direction == 1){
			animator.SetBool("Walking", false);
		}

		//instantiate gun and make crosshair visible when gun is obtained
		if(currentWeapon == Weapon.Pistol){
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = false;			
			_CROSSHAIR = GameObject.Find("crosshair");
			ShowObject(_CROSSHAIR);
			ShowObject(pistol);

			//move crosshair
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

			_CROSSHAIR.transform.position = mousePos;	

			if(Input.GetMouseButtonDown(0)){
				//only shoot if crosshair is on side that player is facing
				if(mousePos.x < transform.position.x && direction == -1){
					Instantiate(bullet, pistol.transform.position, Quaternion.identity);
				}
				else if(mousePos.x > transform.position.x && direction == 1){
					Instantiate(bullet, pistol.transform.position, Quaternion.identity);
				}	
			}
		}
		//if player does not have a gun, lock mouse to center of viewport so when gun is retrieved, crosshair is immediately visible
		else{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
	//End update()

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.transform.tag.Equals("Terrain") || other.transform.tag.Equals("Platform")){
			if(!grounded){
				grounded = true;
			}
		}

		if(other.transform.tag.Equals("Pistol")){
			Destroy(other.gameObject);
			pistol = Instantiate(pistol, transform.position, Quaternion.identity);
			pistol.transform.parent = transform;
			pistol.GetComponent<BoxCollider2D>().isTrigger = true;
			pistol.transform.Translate(new Vector2(0.1f, 0));

			//set direction pistol is facing
			pistol.transform.localScale = new Vector2(pistol.transform.localScale.x * direction, pistol.transform.localScale.y);
			
			currentWeapon = Weapon.Pistol;
			obtainedGun = true;
		}
	}

	/// <summary>
	/// Sent each frame where a collider on another object is touching
	/// this object's collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionStay2D(Collision2D other)
	{
		if(other.transform.tag.Equals("Grunt")){
			if(animator.GetBool("Punching")){
				animator.SetBool("Punching", false); //set punching state to false otherwise while collision stays health is reduced continually on a single punch
				GruntController script = other.gameObject.GetComponent<GruntController>();
				script.Health = HealthController.ReduceHealth(damageMelee, script.Health);
			}
		}
	}

	//triggered on end of player_punching animation
	void OnPunchEnd(){
		animator.SetBool("Punching", false);
	}

	private void ShowObject(GameObject obj){
		SpriteRenderer chRenderer = obj.GetComponent<SpriteRenderer>();
		Color chColor = new Color(chRenderer.color.r, chRenderer.color.g, chRenderer.color.b, 1);
		chRenderer.color = chColor;
	}

	private void HideObject(GameObject obj){
		SpriteRenderer chRenderer = obj.GetComponent<SpriteRenderer>();
		Color chColor = new Color(chRenderer.color.r, chRenderer.color.g, chRenderer.color.b, 0);
		chRenderer.color = chColor;
	}
}

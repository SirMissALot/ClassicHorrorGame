using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
	private Rigidbody m_rgbd;
	private Animator m_anim;
	private Vector3 movement;
	private float moveHorizontal;
	private float moveVertical;
	public float speed;
	private float stamina;
	private bool m_canmove;
	private bool isRunning;
	private bool pressingUp;
	private bool pressingDown;
	private bool pressingLeft;
	private bool pressingRight;
	private int actions;
	public Slider staminaBar;
	void Start () {
		m_canmove = true;
		isRunning = false;
		speed = 1.5f;
		stamina = 5f;
		pressingUp = false;
		pressingDown = false;
		pressingLeft = false;
		pressingRight = false;
		m_rgbd = GetComponent<Rigidbody>();
		m_anim = GetComponent<Animator>();
	}
	
	void Update () {
		staminaBar.value = stamina;

		// STAMINA CALCULATION
		if(isRunning) {
			stamina -= Time.deltaTime * 1.5f;
				if(stamina < 0) {
					speed = 1.5f;
					stamina = 0;
					isRunning = false;
				}
		}

		if(!isRunning) {
			stamina += Time.deltaTime;
				if(stamina >= 5){
					stamina = 5;
				}
		}

		// SOMETHING
		if(m_canmove == true){
			// CONTROLLER INPUT HERE
			// HOLDING THE BUTTON DOWN
			if(Input.GetKeyDown(KeyCode.UpArrow)) {
				actions = 1;
				pressingUp = true;
			}

			if(Input.GetKeyDown(KeyCode.DownArrow)) {
				actions	= 2;
				pressingDown = true;
			}

		    if(Input.GetKeyDown(KeyCode.LeftArrow)) {
				actions	= 4;
				pressingLeft = true;
			}

			if(Input.GetKeyDown(KeyCode.RightArrow)) {
				actions	= 3;
				pressingRight = true;
			}

			if(stamina >= 0) {
				if(Input.GetKeyDown(KeyCode.LeftShift)) {
					speed = 3;
					isRunning = true;	
				}
			}


			// RELEASING THE BUTTON
			if(Input.GetKeyUp(KeyCode.UpArrow)) {
				pressingUp = false;
			}

			if(Input.GetKeyUp(KeyCode.DownArrow)) {
				pressingDown = false;
			}

			if(Input.GetKeyUp(KeyCode.LeftArrow)) {
				pressingLeft = false;
			}

			if(Input.GetKeyUp(KeyCode.RightArrow)) {
				pressingRight = false;
			}

			if(Input.GetKeyUp(KeyCode.LeftShift)) {
				isRunning = false;
			}
			
			// STOPPER
			else if((!pressingUp) && (!pressingDown) && (!pressingLeft) && (!pressingRight)) {
				actions = 5;
			}

			// DIRECTIONS 
			switch(actions) {
				case 1:
				moveVertical = 1;
				moveHorizontal = 0;
				break;
				
				case 2:
				moveVertical = -1;
				moveHorizontal = 0;
				break;

				case 3:
				moveHorizontal = 1;
				moveVertical = 0;
				break;

				case 4:
				moveHorizontal = -1;
				moveVertical = 0;
				break;
				
				case 5:
				moveHorizontal = 0;
				moveVertical = 0;
				break;
			}

			// ACTUAL MOVEMENT
		    movement = new Vector3(moveHorizontal, 0.0f, moveVertical); 
		
			// ROTATION BASED ON THE DIRECTION OF TRAVEL					
			if(moveHorizontal != 0 || moveVertical != 0) {
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), .2F);
			}
				
			m_rgbd.MovePosition(transform.position + movement * speed * Time.deltaTime);	
			
			// ANIMATIONS HERE
			if(movement != Vector3.zero && !isRunning) {
				m_anim.SetBool("Moving", true);
				m_anim.SetBool("Running", false);
			} 
			else if(movement != Vector3.zero && isRunning) {
				m_anim.SetBool("Running", true);
			} else {
				m_anim.SetBool("Moving", false);
				m_anim.SetBool("Running", false);
			}		
		}
	}

	void FixedUpdate () {
	
	}
}

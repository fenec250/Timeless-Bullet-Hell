using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (Rigidbody))]
public class PlayerController : MonoBehaviour, HittableObject {
	protected Rigidbody physic;


	/// <summary>
	/// The time flow of the player. 
	/// Can be negative to Rewind.
	/// </summary>
	public static float deltaTime;
	public static float lifetime;
	//public static Vector3 deltaTimeScaleVec3;
	public static PlayerController instance;

	public recordVector currentAction;
	public Stack<recordData> previousActions;// public only for debuging from the inspector, should be private.
	private Camera cam;
	// Use this for initialization
	void Start () {
		if (PlayerController.instance != null && PlayerController.instance != this)
			GameObject.Destroy (gameObject);

		cam = Camera.main;
		physic = GetComponent<Rigidbody> ();
		previousActions = new Stack<recordData> ();
		previousActions.Push (new recordBlockingCheckPoint (transform.position));
		currentAction = null;

	}

	public float differencedebug = 0;//debug shown in Inspector
	// Update is called once per frame
	void Update () {

		PlayerController.deltaTime = 0;
		if (previousActions.Count % 1 == 0)//debug
		{
			differencedebug = previousActions.Count;
		}// debug

		if (moveKeyboard())
			PlayerController.deltaTime = Time.deltaTime;
		else if (rewind ())
			PlayerController.deltaTime = - Time.deltaTime;

		PlayerController.lifetime += PlayerController.deltaTime;
		//Vector3 mousePos = Input.mousePosition;
		//mousePos = cam.ScreenToWorldPoint (new Vector3(mousePos.x, mousePos.y, cam.transform.position.y-transform.position.y));

		//deltaTimeScaleVec3 = new Vector3 (PlayerController.deltaTime, PlayerController.deltaTime, PlayerController.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		//Destroy(other.gameObject);
		Bullet bullet = other.GetComponentInParent<Bullet> ();
		if (bullet != null)
			bullet.hit (this);
		else
			Debug.Log ("Ghost Collision");
	}

	/// <summary>
	/// Moves the player according to the keyboard controls.
	/// </summary>
	/// <returns><c>true</c>, if the player moved, <c>false</c> otherwise.</returns>
	bool moveKeyboard(){
		Vector3 movement = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		//Vector3 position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		movement.Normalize ();
		if (movement != Vector3.zero) {
			Vector3 scale = new Vector3(Time.deltaTime,0f,Time.deltaTime);

			movement.Scale(scale);

			if (currentAction != null){
				if(currentAction.direction == movement.normalized)
					currentAction.duration += movement.magnitude;
				// looks like Moveposition does not move the position in the method and waits for its own update.
				// this complicates things if the movement predicted and recorded is interrupted by a collision.
				// it would probably be best to record the movement in the next update (record the movement from the previous update here)
				else{
					previousActions.Push(currentAction);
					if (previousActions.Count%5==0)
						previousActions.Push(new recordCheckPoint(movement + transform.position));
					currentAction = new recordVector(/*(transform.position - position)*/movement.magnitude, movement);
				}
			}
			else{
				previousActions.Push(new recordCheckPoint(transform.position));
				currentAction = new recordVector(/*(transform.position - position)*/movement.magnitude, movement);
			}
			
			physic.MovePosition (movement + transform.position);
			return true;
		} 
		return false;
	}
	/// <summary>
	/// Checks for input and rewinds if needed. 
	/// May change currentAction and previousActions.
	/// </summary>
	bool rewind(){		
		if (Input.GetAxisRaw ("Rewind") != 0) {
			float debug = Input.GetAxisRaw ("Rewind");
			Vector3 position = transform.position;
			// TODO: rewind the player position
			recordData current;
			if (currentAction != null)
				current = currentAction;
			else
				current = previousActions.Pop();

			for( float t = Time.deltaTime; t >0; ){
				t = current.rewind( t, ref position);
				if (t > 0)
					current = previousActions.Pop();//put a blocking checkpoint at the bottom of the stack to prevent out of bound.

				/*
				Vector3 movement;
				if (t >= currentAction.duration){
					t -= currentAction.duration;
					movement = new Vector3(-currentAction.duration * currentAction.direction.x,
					                        -currentAction.duration * currentAction.direction.y,
					                        -currentAction.duration * currentAction.direction.z);
					// would delete currentDuration here, but C#.
					if (previousActions.Count > 0)
						currentAction = previousActions.Pop();
					else{
						t = 0f;
					}
				} else {
					movement = new Vector3(-t * currentAction.direction.x,
					                        -t * currentAction.direction.y,
					                        -t * currentAction.direction.z);
					currentAction.duration -= movement.magnitude;
					t = 0f;
				}
				position += movement;*/
			}
			if (current.GetType() == typeof(recordVector))
				currentAction = (recordVector) current;
			else{
				currentAction = null;
				previousActions.Push(current);// replaces the action (probably blockingCheckPoint) on the stack.
			}
			physic.MovePosition(position);
			return true;
		}
		//else
		return false;
	}
}

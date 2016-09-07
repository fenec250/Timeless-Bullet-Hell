using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody))]
public class StraightBulletScript : ObjectTCScript, Bullet {

	public Vector3 posInitiale, speed;
	protected Rigidbody physic;

	// Use this for initialization
	protected override void Start () {
		physic = GetComponent<Rigidbody> ();
		transform.rotation = Quaternion.LookRotation (speed);
		transform.position = new Vector3(posInitiale.x, 0, posInitiale.z);
	}

	/// <summary>
	/// Updates the position. Called in Base Update
	/// </summary>
	protected override void Update(){
		if (timeControl ()) {
			Vector3 deplacement = new Vector3 (speed.x, speed.y, speed.z);
			deplacement.Scale (new Vector3 (lifetime % 5f, lifetime, lifetime));
			physic.MovePosition (deplacement + posInitiale);
		}
		//transform.Rotate (transform.up, PlayerController.deltaTime);
	}
	/// <summary>
	/// Called by a target hit by this, filters it and takes action.
	/// </summary>
	/// <param name="player">target hit</param>
	public void hit (HittableObject target){
		//Destroy (gameObject);
		Debug.Log ("hit");
	}
}

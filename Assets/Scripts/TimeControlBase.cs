using UnityEngine;
using System.Collections;

public abstract class TimeControlBase : MonoBehaviour {

	// <summary>
	/// The lifetime of this object, controlled by the PlayerController. 
	/// Rewinding before 0 will destroy this object; when spawning objects, 
	/// be sure to spawn according to lifetime to ensure it can respawn.
	/// </summary>/
	public float lifetime;
	public float minLifetime, maxLifetime;
	public bool targetIsActive;
	public bool isEnabled;

	// Use this for initialization
	protected abstract void Start ();
	
	// Update is called once per frame
	protected abstract void Update ();

	/// <summary>
	/// Controls the object's lifetime. Use this in Update.
	/// </summary>
	/// <returns><c>true</c>, if the object is in its active lifetime, <c>false</c> otherwise.</returns>
	protected bool timeControl(){
		
		if (isEnabled && PlayerController.deltaTime != 0) {
			
			lifetime += PlayerController.deltaTime;
			
			if (targetIsActive) {
				if (lifetime < minLifetime) {
					disableTarget();
					return false;
				} 
				if (lifetime > maxLifetime) {
					disableTarget();
					return false;
				}
				return true;
			} else if (lifetime <= maxLifetime && lifetime >= minLifetime) {
				enableTarget();
				return true;
			}
		}
		return false;
	}

	protected virtual void enableTarget(){
	}

	protected virtual void disableTarget(){
	}

}

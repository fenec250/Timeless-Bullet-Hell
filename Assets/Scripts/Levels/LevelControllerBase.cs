using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelControllerBase : MonoBehaviour
{
	public List<PartControllerBase> parts;
	List<PartControllerBase>.Enumerator enablerHead, enablerTail;
	public int maxConcurrentParts;
	public int concurrentParts;

		
	// Use this for initialization
	protected virtual void Start ()
	{
		parts = new List<PartControllerBase> ();
		GetComponentsInChildren<PartControllerBase>(parts);
		foreach (PartControllerBase pc in parts)
			pc.gameObject.SetActive (false);
		enablerHead = parts.GetEnumerator ();
		enablerTail = parts.GetEnumerator ();
		concurrentParts = 0;
		Debug.Log ("LevelStart");
		next ();
	}
	
	// Update is called once per frame
	protected virtual void Update (){
	}

	public void next()
	{
		if (enablerHead.MoveNext ()) {
			enablerHead.Current.gameObject.SetActive(true);	
			Debug.Log ("activate " + enablerHead.Current.gameObject.name);
			++concurrentParts;
			while (concurrentParts > maxConcurrentParts) {
				enablerTail.MoveNext ();
				enablerTail.Current.gameObject.SetActive(false);
				Debug.Log ("deactivate " + enablerTail.Current.gameObject.name);
				--concurrentParts;
			} 
		} else {
			endLevel();
		}
	}

	/// <summary>
	/// Ends the current level.
	/// Each level must implement this. 
	/// It is called automatically by LevelControllerBase when the last Part ends.
	/// </summary>
	protected void endLevel(){
		Debug.Log ("Level ended");
		gameObject.SetActive (false);
	}

}


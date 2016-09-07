using UnityEngine;
using System.Collections;

public class ObjectTCScript : TimeControlBase
{
	public GameObject TCTarget;

	// Use this for initialization
	protected override void Start ()
	{
		//
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
	
	}
	
	protected override void enableTarget(){
		TCTarget.SetActive (true);		
		targetIsActive = true;
	}
	
	protected override void disableTarget(){
		TCTarget.SetActive(false);
		targetIsActive = false;
	}
}


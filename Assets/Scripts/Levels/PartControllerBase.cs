using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartControllerBase : MonoBehaviour
{
	public LevelControllerBase level;
	public bool isEnabled;
	public List<TimeControlBase> partMembers;

	// Use this for initialization
	protected void Start ()
	{
		level = transform.GetComponentInParent<LevelControllerBase> ();
		Debug.Log (gameObject.name + " started");
		partMembers = new List<TimeControlBase> ();
		
		foreach (TimeControlBase t in GetComponentsInChildren<TimeControlBase>()) {
			partMembers.Add(t);
			t.gameObject.SetActive(false);
		}
	}
	protected void OnEnable(){
		Debug.Log (gameObject.name + " enabled");
		foreach (TimeControlBase t in partMembers) {
			t.gameObject.SetActive(true);
		}
	}

	public float lifeTime = 0f;//debug
	// Update is called once per frame
	protected void Update ()
	{
		lifeTime += PlayerController.deltaTime;
		if (lifeTime > 3f) {
			endPart();
			lifeTime = -91f; 
		}
	}

	/*protected void enableTarget ()
	{
		foreach (TimeControlBase t in partMembers) {
			t.gameObject.SetActive(isEnabled);
			t.lifetime = 0f;
		}
		lifeTime = 0f;//debug
	}*/


	protected virtual void endPart(){
		level.next ();
	}
}


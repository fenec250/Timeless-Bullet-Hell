using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public interface recordData{
	float rewind (float rewindTime, ref Vector3 transformedPosition);
}

public class recordVector : recordData{
	public float duration;
	public Vector3 direction{ get; protected set;}
	// float speed;
	// bool shooting;
	// bool phasing;
	
	public recordVector(float pDuration, Vector3 pDirection){
		duration = pDuration;
		direction = pDirection.normalized;
	}
	/// <summary>
	/// Rewind the specified rewindTime and modify transformedPosition.
	/// </summary>
	/// <param name="rewindTime">Rewind time.</param>
	/// <param name="transformedPosition">Transformed position.</param>
	/// <returns>the remaining time to rewind</returns>
	public float rewind(float rewindTime, ref Vector3 transformedPosition){
		Vector3 movement = Vector3.zero;

		if (rewindTime >= duration){
			movement += new Vector3(-duration * direction.x,
			                        -duration * direction.y,
			                        -duration * direction.z);
			return rewindTime - movement.magnitude;
		} //else {
		movement += new Vector3(-rewindTime * direction.x,
		                        -rewindTime * direction.y,
		                        -rewindTime * direction.z);
		duration -= movement.magnitude;

		transformedPosition += movement;
		return 0f;
		//}
	}

}

/// <summary>
/// Records a position.
/// </summary>
public class recordCheckPoint : recordData{
	Vector3 position;

	public recordCheckPoint(Vector3 currentPosition){
		position = currentPosition;
	}

	/// <summary>
	/// Sets transformedPosition to the saved position, no matter where it is.
	/// </summary>
	/// <param name="rewindTime">For interface compatibility. Is returned without change</param>
	/// <param name="transformedPosition">transformed position to rewind the player to.</param>
	public virtual float rewind(float rewindTime, ref Vector3 transformedPosition){
		transformedPosition = position;
		return rewindTime;
	}
}

/// <summary>
/// records a position and prevents the player from rewinding before this point.
/// </summary>
public class recordBlockingCheckPoint : recordCheckPoint{
	public recordBlockingCheckPoint(Vector3 currentPosition):base(currentPosition){}

	public override float rewind(float rewindTime, ref Vector3 transformedPosition){
		base.rewind (rewindTime, ref transformedPosition);
		return 0f;
	}
}






using UnityEngine;

public class Vehicle : Codeplay.SceneElement<Vehicle>
{
	protected override void OnInit(object data)
	{
		Debug.Log("vehicle's parent = " + Parent);
	}
}
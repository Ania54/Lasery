using UnityEngine;

public class Laser : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		Debug.Log("arsas");
		// on even numbered frames:
		if (Time.frameCount % 2 == 0)
		{
			Debug.Log("even");
			transform.position = Vector3.zero;
		}
		else
		{
			Debug.Log("odd");
			// teleport 100 units up
			transform.position += Vector3.up * 100;
		}
	}
}

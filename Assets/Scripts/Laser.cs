using System.Linq;
using UnityEngine;

public class Laser : MonoBehaviour
{

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public LineRenderer lineRenderer;

	// Update is called once per frame
	void Update()
	{
		Vector3 direction = transform.forward;
		Vector3 origin = transform.position;

		lineRenderer.positionCount = 1;
		lineRenderer.SetPosition(0, origin);

		bool isInsideMaterial = false;
		float currentRefractiveIndex = 1.0f;

		for (int i = 0; i < 100; i++)
		{
			Ray ray = new Ray(origin, direction);

			// 1) find which collider you’re in
			Collider[] overlaps = Physics.OverlapSphere(ray.origin, 0.1f);

			// Find the first collider that overlaps with the ray and has the "Glass" tag
			// This is the collider that the ray is currently inside
			Collider inside = overlaps.FirstOrDefault(c => c.CompareTag("Glass"));

			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				//Debug.Log(hit.ToString());
				lineRenderer.positionCount++;
				lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

				if (hit.collider.CompareTag("Mirror"))
				{
					//Debug.Log("aa");
					direction = Vector3.Reflect(direction, hit.normal);
					origin = hit.point + direction * 0.01f;
				}
				else if (hit.collider.tag != inside.tag)
				{
					Debug.Log("refract");
				}
				else
				{
					break;
				}

			}
			else
			{
				lineRenderer.positionCount++;
				lineRenderer.SetPosition(lineRenderer.positionCount - 1, origin + direction * 100f);
			}
		}
	}
}

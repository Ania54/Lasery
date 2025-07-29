using System.Linq;
using UnityEngine;

public class Laser : MonoBehaviour
{

	bool Refract(Vector3 incident, Vector3 normal, float n1, float n2, out Vector3 refracted)
	{
		float ratio = n1 / n2;
		float cosI = -Vector3.Dot(normal, incident);
		float sinT2 = ratio * ratio * (1 - cosI * cosI);

		if (sinT2 > 1f)
		{
			refracted = Vector3.zero;
			return false; // Total internal reflection
		}

		float cosT = Mathf.Sqrt(1 - sinT2);
		refracted = ratio * incident + (ratio * cosI - cosT) * normal;
		refracted.Normalize();
		return true;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public LineRenderer lineRenderer;
	void Start()
	{

	}

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
			Collider inside = overlaps.FirstOrDefault(c => c.CompareTag("Glass"));

			RaycastHit hit;

			if (inside != null)
			{
				if (inside.Raycast(ray, out hit, 100f))
				{
					Debug.Log("Leaving collider at " + hit.point);
				}
			}
			else
			{

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
					else if (hit.collider.CompareTag("Glass"))
					{
						float n1 = currentRefractiveIndex;
						float n2 = isInsideMaterial ? 1.0f : 1.5f; // Air to glass or glass to air

						Vector3 refractedDir;
						if (Refract(direction, hit.normal, n1, n2, out refractedDir))
						{
							direction = refractedDir;
							origin = hit.point + direction * 0.01f;
							isInsideMaterial = !isInsideMaterial;
							currentRefractiveIndex = n2;
						}
						else
						{
							// Total internal reflection
							direction = Vector3.Reflect(direction, hit.normal);
							origin = hit.point + direction * 0.01f;
						}
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
}



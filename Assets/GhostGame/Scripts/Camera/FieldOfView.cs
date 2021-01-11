using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public float viewRange = 3;
	public float shadowDrawDistance = 6;

	void Update()
    {
        DrawFOV();
    }

    public void DrawFOV()
	{
		Collider2D[] proximityObjects = Physics2D.OverlapCircleAll(transform.position, viewRange);

		foreach (Collider2D collider in proximityObjects)
		{
			if (collider is PolygonCollider2D)
			{
				PolygonCollider2D pCollider = (PolygonCollider2D)collider;
				for (int i = 0; i < pCollider.pathCount; i++)
				{
					Vector2[] path = pCollider.GetPath(i);
					for (int j = 0; j < path.Length; j++)
					{
						path[j] = pCollider.gameObject.transform.TransformPoint(path[j]);
					}
					DrawShadow(path);
				}
			}
			if (collider is CompositeCollider2D)
			{
				CompositeCollider2D cCollider = (CompositeCollider2D)collider;
				for (int i = 0; i < cCollider.pathCount; i++)
				{
					Vector2[] path = new Vector2[cCollider.GetPathPointCount(i)];
					cCollider.GetPath(i, path);
					for (int j = 0; j < path.Length; j++)
					{
						path[j] = cCollider.gameObject.transform.TransformPoint(path[j]);
					}
					DrawShadow(path);
				}
			}
		}
	}

	private void DrawShadow(Vector2[] path)
	{
		List<Vector2> drawPoints = new List<Vector2>();

		bool lastPointWasBorder = false;
		bool edgeHasBeenFlipped = false;

		for (int j = 0; j < path.Length; j++)
		{
			Vector2 point = path[j];

			//Make the outRay
			Vector2 direction = (point - (Vector2)transform.position).normalized;
			Vector2 rayEnd = point + direction * shadowDrawDistance;

			//Check going out
			if (!CollidesWithPath(point, rayEnd, path))
			{
				//Check coming in

				//TODO optimize this with cacheing, maybe
				Vector2 nextNode = path[(j + 1) % path.Length];
				Vector2 lastNode = j == 0 ? path[path.Length - 1] : path[j - 1];

				if (!CollidesWithPath(point, transform.position, path) || PointIsOnLine(point, transform.position, nextNode) || PointIsOnLine(point, transform.position, lastNode))
				{
					lastPointWasBorder = true;
					drawPoints.Add(point);
					drawPoints.Add(rayEnd);
				}
				else
				{
					if (lastPointWasBorder)
					{
						//Last added points are out of order
						Vector2 cache = drawPoints[drawPoints.Count - 1];
						drawPoints[drawPoints.Count - 1] = drawPoints[drawPoints.Count - 2];
						drawPoints[drawPoints.Count - 2] = cache;
						edgeHasBeenFlipped = true;
					}
					lastPointWasBorder = false;
					drawPoints.Add(point);
				}
			} else
			{
				lastPointWasBorder = false;
			}
		}
		if (!edgeHasBeenFlipped)
		{
			Vector2 cache = drawPoints[drawPoints.Count - 1];
			drawPoints[drawPoints.Count - 1] = drawPoints[drawPoints.Count - 2];
			drawPoints[drawPoints.Count - 2] = cache;
		}
		CreateShadowMesh(drawPoints.ToArray());
	}

	bool CollidesWithPath(Vector2 rayStart, Vector2 rayEnd, Vector2[] path)
	{
		for (int i = 0; i < path.Length; i++)
		{
			Vector2 lineStart = path[i];
			Vector2 lineEnd = path[(i + 1) % path.Length];
			if (rayStart != lineStart && rayStart != lineEnd)
			{
				if (IsIntersecting(rayStart, rayEnd, lineStart, lineEnd))
				{
					return true;
				}
			}

		}
		return false;
	}

	private void CreateShadowMesh(Vector2[] vertices2D)
	{
		// Create Vector2 vertices

		var vertices3D = System.Array.ConvertAll<Vector2, Vector3>(vertices2D, v => v);

		// Use the triangulator to get indices for creating triangles
		var triangulator = new Triangulator(vertices2D);
		var indices = triangulator.Triangulate();

		// Generate a color for each vertex
		// Create the mesh
		var mesh = new Mesh
		{
			vertices = vertices3D,
			triangles = indices,
		};

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		// Set up game object with mesh;

		AddMeshToCombinedMesh(mesh);
	}

	private void AddMeshToCombinedMesh(Mesh mesh)
	{
		FieldOfViewRenderer.I.AddShadowMesh(mesh);
	}


	bool IsIntersecting(Vector2 l1_start, Vector2 l1_end, Vector2 l2_start, Vector2 l2_end)
	{
		bool isIntersecting = false;

		float denominator = (l2_end.y - l2_start.y) * (l1_end.x - l1_start.x) - (l2_end.x - l2_start.x) * (l1_end.y - l1_start.y);

		//Make sure the denominator is > 0, if so the lines are parallel
		if (denominator != 0)
		{
			float u_a = ((l2_end.x - l2_start.x) * (l1_start.y - l2_start.y) - (l2_end.y - l2_start.y) * (l1_start.x - l2_start.x)) / denominator;
			float u_b = ((l1_end.x - l1_start.x) * (l1_start.y - l2_start.y) - (l1_end.y - l1_start.y) * (l1_start.x - l2_start.x)) / denominator;

			//Is intersecting if u_a and u_b are between 0 and 1
			if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
			{
				isIntersecting = true;
			}
		}

		return isIntersecting;
	}

	bool PointIsOnLine(Vector2 start, Vector2 end, Vector2 point)
	{
		float AB = Mathf.Sqrt((end.x - start.x) * (end.x - start.x) + (end.y - start.y) * (end.y - start.y));
		float AP = Mathf.Sqrt((point.x - start.x) * (point.x - start.x) + (point.y - start.y) * (point.y - start.y));
		float PB = Mathf.Sqrt((end.x - point.x) * (end.x - point.x) + (end.y - point.y) * (end.y - point.y));
		if (AB == AP + PB)
			return true;
		return false;
	}
}

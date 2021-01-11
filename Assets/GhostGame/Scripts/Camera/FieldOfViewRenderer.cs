using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewRenderer : MonoBehaviour
{

    static FieldOfViewRenderer i;

    public static FieldOfViewRenderer I
	{
        get
		{
            return i;
		}
	}

	MeshFilter filter;
	private List<Mesh> shadowMeshes;


	private void Awake()
	{
		i = this;
		shadowMeshes = new List<Mesh>();
	}

	private void Start()
	{
		filter = GetComponent<MeshFilter>();
	}

    public void AddShadowMesh(Mesh mesh)
	{
		shadowMeshes.Add(mesh);
	}

    void LateUpdate()
    {

		CombineInstance[] combine = new CombineInstance[shadowMeshes.Count];

		int i = 0;
		while (i < shadowMeshes.Count)
		{
			combine[i].mesh = shadowMeshes[i];
			combine[i].transform = gameObject.transform.localToWorldMatrix;
			i++;
		}
		filter.mesh = new Mesh();
		filter.mesh.CombineMeshes(combine);

		shadowMeshes.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class SetTilemapShadows : MonoBehaviour
{
    private CompositeCollider2D tilemapCollider;

    private bool needsUpdating = true;

    public GameObject shadowCasterContainer;

    float northOffset = 4f / 32f;
    float southOffset = 24f / 32f;
    float eastOffset = 4f / 32f;
    float westOffset = 4f / 32f;


	public void Start()
	{
        tilemapCollider = GetComponent<CompositeCollider2D>();
    }

    public void AddShadows()
    {
        int sortingLayerId = GetComponent<TilemapRenderer>().sortingLayerID;
        for (int i = 0; i < tilemapCollider.pathCount; i++)
        {
            Vector2[] pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];
            tilemapCollider.GetPath(i, pathVertices);
            GameObject shadowCaster = new GameObject("shadow_caster_" + i);
            shadowCaster.layer = gameObject.layer;
            PolygonCollider2D shadowPolygon = (PolygonCollider2D)shadowCaster.AddComponent(typeof(PolygonCollider2D));
            shadowCaster.transform.parent = shadowCasterContainer.transform;
            //OffsetVertices(pathVertices);
            shadowPolygon.points = OffsetVertices(pathVertices);
            shadowPolygon.enabled = false;
            ShadowCaster2D shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
            shadowCasterComponent.selfShadows = false;
            shadowCasterComponent.m_ApplyToSortingLayers = new int[] { sortingLayerId };
        }
    }

    public void Update()
	{
		if (needsUpdating)
		{
            if (tilemapCollider.pathCount > 0 && tilemapCollider != null)
            {
                AddShadows();
                needsUpdating = false;
            }
		}
	}

    private Vector2[] OffsetVertices(Vector2[] pathVertices)
	{
        Vector2[] returnArray = (Vector2[])pathVertices.Clone();
        float epsilon = .001f;
		for (int i = 0; i < pathVertices.Length; i++)
		{
            int index0 = i;
            int index1 = (i + 1) % pathVertices.Length;
            if (pathVertices[index0].x - pathVertices[index1].x > epsilon)
            {
                returnArray[index0].y -= northOffset; returnArray[index1].y -= northOffset;
            }
            if (pathVertices[index0].x - pathVertices[index1].x < -epsilon)
            {
                returnArray[index0].y += southOffset; returnArray[index1].y += southOffset;
            }
            if (pathVertices[index0].y - pathVertices[index1].y < -epsilon)
            {
                returnArray[index0].x -= eastOffset; returnArray[index1].x -= eastOffset;
            }
            if (pathVertices[index0].y - pathVertices[index1].y > epsilon) {
                returnArray[index0].x += westOffset; returnArray[index1].x += westOffset;
            }
        }
        return returnArray;
	}
}
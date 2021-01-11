﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SetTilemapShadows : MonoBehaviour
{
    private CompositeCollider2D tilemapCollider;

    private bool needsUpdating = true;

	public void Start()
	{
        tilemapCollider = GetComponent<CompositeCollider2D>();
    }

    public void AddShadows()
    {
        GameObject shadowCasterContainer = GameObject.Find("shadow_casters");
        for (int i = 0; i < tilemapCollider.pathCount; i++)
        {
            Vector2[] pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];
            tilemapCollider.GetPath(i, pathVertices);
            GameObject shadowCaster = new GameObject("shadow_caster_" + i);
            PolygonCollider2D shadowPolygon = (PolygonCollider2D)shadowCaster.AddComponent(typeof(PolygonCollider2D));
            shadowCaster.transform.parent = shadowCasterContainer.transform;
            shadowPolygon.points = pathVertices;
            shadowPolygon.enabled = false;
            ShadowCaster2D shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
            shadowCasterComponent.selfShadows = true;
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
}
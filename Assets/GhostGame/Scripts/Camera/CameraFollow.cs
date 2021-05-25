using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	private Entity target;
	public Camera cameraComponent;

	private int baseCullingMask;

	private void Start()
	{
		cameraComponent = GetComponent<Camera>();
		baseCullingMask = cameraComponent.cullingMask;
		EventHub.PlayerChangeStory += PlayerChangeFloor;
	}

	void Update()
	{
		if (target)
		{
			transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
		}
	}

	public void SetTarget(Entity target)
	{
		this.target = target;
		UpdateCullingMask();
	}

	private void UpdateCullingMask()
	{
		cameraComponent.cullingMask = baseCullingMask + (1 << (int)Layering.StoryToPhysicsLayer(target.storyLocation));
	}

	private void PlayerChangeFloor(PlayerCharacter playerCharacter)
	{
		if (playerCharacter == target)
		{
			UpdateCullingMask();
		}
	}

}
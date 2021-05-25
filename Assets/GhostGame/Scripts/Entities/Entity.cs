using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public abstract class Entity : NetworkBehaviour
{
	[SyncVar]
	public int storyLocation;
	protected SpriteRenderer spriteRenderer;

	private Rigidbody2D rb2D;

	public virtual void Init(int storyLocation)
	{
		this.storyLocation = storyLocation;
	}

	protected virtual void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		rb2D = GetComponent<Rigidbody2D>();
		SortIntoParent();
		UpdateLayering();
	}

	public virtual bool IsInteractable()
	{
		return false;
	}
	public virtual void Interact(PlayerCharacter playerCharacter)
	{
		//To be overriden
	}

	protected virtual void UpdateLayering()
	{
		gameObject.layer = (int)Layering.StoryToPhysicsLayer(storyLocation);
		if (spriteRenderer) {
			spriteRenderer.sortingLayerID = Layering.StoryToSortingLayerID(storyLocation);
			spriteRenderer.sortingOrder = Constants.ENTITY_SORTING_ORDER;
		}
	}

	public virtual void ChangeStory(Verticality verticality)
	{
		print(verticality);
		if (verticality == Verticality.Up)
			storyLocation++;
		else
			storyLocation--;
		UpdateLayering();
		SortIntoParent();
	}

	public void JumpToPosition(Vector3 position)
	{
		if (rb2D != null)
			rb2D.MovePosition(position);
		else
			transform.position = position;
	}
	protected abstract void SortIntoParent();
}

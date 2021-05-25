using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardinalSprite : MonoBehaviour
{
    protected Animator animator;
    public Direction direction;

    private int northID;
    private int southID;
    private int eastID;
    private int westID;

    public virtual void UpdateDirection(Direction direction)
	{
        if (!animator)
		{
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                throw new System.Exception("CardinalSprite has not been given an Animator");
            }

            northID = Animator.StringToHash(Constants.NORTH_ANIMATION_STRING);
            southID = Animator.StringToHash(Constants.SOUTH_ANIMATION_STRING);
            eastID = Animator.StringToHash(Constants.EAST_ANIMATION_STRING);
            westID = Animator.StringToHash(Constants.WEST_ANIMATION_STRING);
        }

        this.direction = direction;
        switch (direction)
        {
            case Direction.North:
                animator.SetBool(northID, true);
                break;
            case Direction.South:
                animator.SetBool(southID, true);
                break;
            case Direction.East:
                animator.SetBool(eastID, true);
                break;
            case Direction.West:
                animator.SetBool(westID, true);
                break;
        }
    }
}

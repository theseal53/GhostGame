using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCardinalSprite : CardinalSprite
{

    private int northLeftID;
    private int northRightID;
    private int southLeftID;
    private int southRightID;
    private int eastTopID;
    private int eastBottomID;
    private int westTopID;
    private int westBottomID;

    public override void UpdateDirection(Direction direction)
	{
        if (!animator)
		{
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                throw new System.Exception("CardinalSprite has not been given an Animator");
            }

            northLeftID = Animator.StringToHash(Constants.NORTH_LEFT_ANIMATION_STRING);
            northRightID = Animator.StringToHash(Constants.NORTH_RIGHT_ANIMATION_STRING);
            southLeftID = Animator.StringToHash(Constants.SOUTH_LEFT_ANIMATION_STRING);
            southRightID = Animator.StringToHash(Constants.SOUTH_RIGHT_ANIMATION_STRING);
            eastTopID = Animator.StringToHash(Constants.EAST_TOP_ANIMATION_STRING);
            eastBottomID = Animator.StringToHash(Constants.EAST_BOTTOM_ANIMATION_STRING);
            westTopID = Animator.StringToHash(Constants.WEST_TOP_ANIMATION_STRING);
            westBottomID = Animator.StringToHash(Constants.WEST_BOTTOM_ANIMATION_STRING);
        }

        this.direction = direction;
        switch (direction)
        {
            case Direction.North:
                if (Random.value > .5)
                    animator.SetBool(northLeftID, true);
                else
                    animator.SetBool(northRightID, true);
                break;
            case Direction.South:
                if (Random.value > .5)
                    animator.SetBool(southLeftID, true);
                else
                    animator.SetBool(southRightID, true);
                break;
            case Direction.East:
                if (Random.value > .5)
                    animator.SetBool(eastTopID, true);
                else
                    animator.SetBool(eastBottomID, true);
                break;
            case Direction.West:
                if (Random.value > .5)
                    animator.SetBool(westTopID, true);
                else
                    animator.SetBool(westBottomID, true);
                break;
        }
    }
}

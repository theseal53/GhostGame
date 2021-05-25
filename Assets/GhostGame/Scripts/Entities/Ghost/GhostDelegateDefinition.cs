using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate Vector2 DetermineDesiredPositionDelegate();
public delegate Vector2 MoveTowardDesiredPosition(Vector2 desiredPosition);
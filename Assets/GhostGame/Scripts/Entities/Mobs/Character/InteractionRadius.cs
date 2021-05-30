using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionRadius : MonoBehaviour
{
    public delegate void RadiusEnterDelegate(Collider2D other);
    public delegate void RadiusExitDelegate(Collider2D other);

    public event RadiusEnterDelegate OnRadiusEnter;
    public event RadiusExitDelegate OnRadiusExit;

    public List<Entity> colliderList = new List<Entity>();
 
    //called when something enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        //if the object is not already in the list
        if (entity && !colliderList.Contains(entity))
        {
            //add the object to the list
            colliderList.Add(entity);
            OnRadiusEnter?.Invoke(other);
        }
    }

    //called when something exits the trigger
    void OnTriggerExit2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        //if the object is in the list
        if (entity && colliderList.Contains(entity))
        {
            //remove it from the list
            colliderList.Remove(entity);
            OnRadiusExit?.Invoke(other);
        }
    }
}

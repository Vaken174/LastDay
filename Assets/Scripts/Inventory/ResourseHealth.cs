using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourseHealth : MonoBehaviour
{
    public int health;
    public ItemScriptableObject resourseType;

    public void TreeFall() 
    {
        gameObject.AddComponent<Rigidbody>();
        Rigidbody rig = GetComponent<Rigidbody>();
        rig.isKinematic = false;
        rig.useGravity = true;
        rig.mass = 200;

        Destroy(gameObject, 5f); 
    }
    public void StoneGather() 
    {
        Destroy(gameObject);
    }
}

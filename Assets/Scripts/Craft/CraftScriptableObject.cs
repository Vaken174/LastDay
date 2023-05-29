using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CraftScriptableObject : ScriptableObject
{
    public enum CraftType {Common,Tools,Medical, Weapons}
    public CraftType craftType;
    public ItemScriptableObject finalObject;
    public int craftAmount;
    public int craftTime;
    
    public List<CraftResourse> craftResourses;
}

[System.Serializable]
public class CraftResourse
{
    public ItemScriptableObject craftObject;
    public int craftObjectAmount;

}

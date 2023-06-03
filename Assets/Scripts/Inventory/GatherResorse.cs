using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GatherResorse : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private int resourseAmount;
    [SerializeField]
    private ItemScriptableObject resource;
    [SerializeField]
    private GameObject hitFX;
    [SerializeField]
    private float reachDistants = 2f;


    private Quickslotinventory quickslotinventory;

    private void Start()
    {
        quickslotinventory = FindObjectOfType<Quickslotinventory>();
    }

    public void GatherResoirse() 
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));

        if (Physics.Raycast(ray,out hit, reachDistants, layerMask))
        {
            if (resource.name == hit.collider.GetComponent<ResourseHealth>().resourseType.name)
            if (hit.collider.GetComponent<ResourseHealth>().health >= 1)
            {
                Instantiate(hitFX, hit.point,Quaternion.LookRotation(hit.normal));
                inventoryManager.AddItem(resource, resourseAmount);
                hit.collider.GetComponent<ResourseHealth>().health--;


                    if (hit.collider.GetComponent<ResourseHealth>().health <= 0 && hit.collider.gameObject.layer==6)
                {
                    hit.collider.GetComponent<ResourseHealth>().TreeFall();
                    hit.collider.GetComponent<Rigidbody>().AddForce(mainCamera.transform.forward * 10, ForceMode.Impulse);

                    }
                if (hit.collider.GetComponent<ResourseHealth>().health <= 0 && hit.collider.gameObject.layer == 7)
                {
                    hit.collider.GetComponent<ResourseHealth>().StoneGather();
                    }

            }
        }
    }
}

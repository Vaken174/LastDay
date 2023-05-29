using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    private float WalkSpeed;
    [SerializeField]
    private float RunSpeed;
    [SerializeField]
    private float JumpPower;
    [SerializeField]
    private float gravityScale;

    private Vector3 velocity;
    private Vector3 moveVector;

    [SerializeField]
    public Animator anim;
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform aimTarget;
    [SerializeField]
    private Transform mainCamera;

    private float reachDistante = 0.3f;
    private bool isGround;
    private float currentSpeed;
    private float animationInterpolation = 1f;
    private float PlayerHeight = 1.84f;
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private Quickslotinventory Quickslotinventory;
    [SerializeField]
    private Indicators Indicators;
    private CraftManager craftManager;
    
    //private Quickslotinventory quickslotinventory;

    private void Start()
    {
        //quickslotinventory = FindObjectOfType<Quickslotinventory>();
        craftManager = FindObjectOfType<CraftManager>();
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Walk();
            }
            else
            {
                Run();
            }
        }
        else
        {
            Walk();
        }

        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            anim.SetBool("IsJumping",true);
        }
        else
        {
            anim.SetBool("IsJumping", false);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            SneakWalk();
        }
        else
        {
            controller.height = PlayerHeight;
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Quickslotinventory.activeSlot != null)
            {
                if (Quickslotinventory.activeSlot.item != null)
                {
                    if (Quickslotinventory.activeSlot.item.itemType == ItemType.Instrument || Quickslotinventory.activeSlot.item.itemType == ItemType.Weapons)
                    {
                        if (inventoryManager.isOpen == false)
                        {
                            if (!craftManager.isOpen)
                                anim.SetBool("Hit", true);
                        }
                    }
                }
            }
        }
        else
        {
            anim.SetBool("Hit",false);
        }
        Ray disiredTargetRay = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Vector3 desiredTargetPosition = disiredTargetRay.origin + disiredTargetRay.direction * 0.7f;
        aimTarget.position = desiredTargetPosition;

    }
    public void ChangeLayerWeight(float newLayerWeight)
    {
        anim.SetLayerWeight(1, newLayerWeight);
    }
    private void FixedUpdate()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.y = Input.GetAxis("Vertical");

        isGround = Physics.CheckSphere(groundCheck.position, reachDistante, layerMask);

        Vector3 move = transform.right * moveVector.x + transform.forward * moveVector.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        Gravity();
    }
    private void Gravity()
    {
        velocity.y += gravityScale * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (velocity.y < gravityScale)
        {
            velocity.y = gravityScale;
        }
    }
    private void Run()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, RunSpeed, Time.deltaTime * 3);
        animationInterpolation = Mathf.Lerp(animationInterpolation, 1.5f, Time.deltaTime * 3);

        anim.SetFloat("Horizontal", moveVector.x * animationInterpolation);
        anim.SetFloat("Vertical", moveVector.y * animationInterpolation);
        anim.SetFloat("Speed", moveVector.y * animationInterpolation);
    }
    private void Jump()
    {
            velocity.y = Mathf.Sqrt(JumpPower * -2f * gravityScale);
    }
    private void Walk()
    {

        animationInterpolation = Mathf.Lerp(animationInterpolation, 1f, Time.deltaTime * 3);
        currentSpeed = Mathf.Lerp(currentSpeed, WalkSpeed, Time.deltaTime * 3);

        anim.SetFloat("Horizontal", moveVector.x * animationInterpolation);
        anim.SetFloat("Vertical", moveVector.y * animationInterpolation);
        anim.SetFloat("Speed", moveVector.y * animationInterpolation);
    }
    private void SneakWalk()
    {
        controller.height = PlayerHeight/2;
    }

    public void Hit() 
    {
        foreach (Transform item in Quickslotinventory.allWeapons)
        {
            if (item.gameObject.activeSelf)
            {
                item.GetComponent<GatherResorse>().GatherResoirse();
                craftManager.currentCraftItem.FillItemDetails();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer ==4)
        {
            Indicators.isInWater = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer ==4)
        {
            Indicators.isInWater = false;
        }
    }
}
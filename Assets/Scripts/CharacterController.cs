using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed;
    [HideInInspector] public float storedSpeed;
    [HideInInspector] public bool hurt, dead;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider col;
    public Animator animator;


    virtual public void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();
    }

    virtual public void Update()
    {
        if (dead && !isPlaying("Dead"))
        {
            //animator.SetTrigger("isDead");
            rb.useGravity = false;
            rb.isKinematic = true;
            col.enabled = false;
        }
        else if (hurt && !isPlaying("Hurt"))
        {
            print($"{gameObject.name} is hurt");
            //animator.SetTrigger("isHurt");
            hurt = false;
        }
    }

    public bool isPlaying(string stateName)
    {
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        //    return true;
        //else
            return false;
    }

    public float GetClipLength(string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            return animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        else
        {
            print($"Clip for {stateName} not found");
            return 0;
        }
    }
}

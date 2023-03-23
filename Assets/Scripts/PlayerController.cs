
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterController
{
    public static PlayerController instance;

    [SerializeField] float rotSpeed;
    private float horizontal, vertical;
    private Vector3 lastDir, lastDir1, lastDir2;


    //[NaughtyAttributes.HorizontalLine]
    //[Header("Player State Variables")]
    public enum States { wakeUp, idle, interacting, moving, attacking, listening, hurt, dead }
    public States state;// { get; private set; }

    [NaughtyAttributes.HorizontalLine]
    [Header("Interact Variables")]
    [SerializeField] private LayerMask layer;
    [SerializeField] private float checkDist;
    [HideInInspector] public InteractObject interactObj { get; private set; }

    [NaughtyAttributes.HorizontalLine]
    [Header("Player Avatar Variables")]
    [SerializeField] private GameObject playerAvatar;
    public InputAction inputMaster { get; private set; }
    private InputControlScheme controlScheme;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        inputMaster = new InputAction();
        inputMaster.Enable();
    }

    override public void Start()
    {
        storedSpeed = speed;

        base.Start();
    }

    override public void Update()
    {
        Vector3 rayDir = lastDir.normalized;
        Ray ray = new Ray(transform.position, rayDir);
        Ray ray1 = new Ray(transform.position, lastDir1);
        Ray ray2 = new Ray(transform.position, lastDir2);
        RaycastHit hit, hit1, hit2;

        if (state == States.attacking || state == States.listening)
        {
            interactObj = null;
        }
        else if (Physics.Raycast(ray, out hit, checkDist, layer))
        {
            interactObj = hit.transform.gameObject.GetComponent<InteractObject>();
        }
        else if (Physics.Raycast(ray1, out hit1, checkDist, layer))
        {
            interactObj = hit1.transform.gameObject.GetComponent<InteractObject>();
        }
        else if (Physics.Raycast(ray2, out hit2, checkDist, layer))
        {
            interactObj = hit2.transform.gameObject.GetComponent<InteractObject>();
        }
        else
        {
            interactObj = null;
        }

        //if (interactObj != null
        //    && interactObj.active
        //    && !interactObj.hasActivated
        //    && !interactObj.GetComponent<Outline>())
        //{
        //    interactObj.gameObject.AddComponent<Outline>();
        //}



        //Player hurt/death triggers
        if (hurt)
            SetState(States.hurt);
        if (dead)
            SetState(States.dead);




        //Store player move values
        //Used in FixedUpdate for correct timing with animation flags
        Vector2 move = inputMaster.ReadValue<Vector2>();//inputMaster.Player.Move.ReadValue<Vector2>();
        switch (state)
        {
            case States.wakeUp:
                if (!isPlaying("Wake Up"))
                {
                    SetState(States.idle);
                }
                break;
            case States.idle:
                if (move.x != 0f || move.y != 0f)
                {
                    SetState(States.moving);
                }
                break;
            case States.moving:
                speed = storedSpeed;

                horizontal = Mathf.Round(move.x * 10f) * 0.1f;
                vertical = Mathf.Round(move.y * 10f) * 0.1f;

                //Save last input vector for interact raycast
                if (horizontal != 0f || vertical != 0f)
                {
                    lastDir.x = horizontal;
                    lastDir.z = vertical;
                }

                Vector3 tempMove = new Vector3(horizontal, 0f, vertical);
                if (tempMove.magnitude > 1)
                    tempMove = tempMove.normalized;
                rb.velocity = new Vector3(tempMove.x * speed, rb.velocity.y, tempMove.z * speed);

                if (move.x == 0f && move.y == 0f)
                {
                    SetState(States.idle);
                }
                break;
            case States.attacking:
                if (!isPlaying("Melee"))
                {
                    SetState(States.idle);
                }
                break;
            case States.hurt:
                if (!hurt && !isPlaying("Hurt"))
                {
                    SetState(States.idle);
                }
                break;
            default:
                break;
        }


        //Handle player interaction inputs
        if (interactObj != null
            && interactObj.active
            && inputMaster.triggered)
        {
            interactObj.Interact();
            if (interactObj.active && !interactObj.hasActivated && interactObj.interacting)
                SetState(States.interacting);
            else
                SetState(States.idle);
        }

        base.Update();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state != States.moving || isPlaying("Hurt"))
        {
            speed = 0;
        }

        // Determine which direction to rotate towards
        Vector3 targetDirection = lastDir;

        // The step size is equal to speed times frame time.
        float singleStep = rotSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        lastDir1 = (2 * transform.forward - transform.right).normalized; //Left ray
        lastDir2 = (2 * transform.forward + transform.right).normalized; //Right ray

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);



        //if (isPlaying("Wake Up"))
        //{
        //    SetState(States.wakeUp);
        //}
        //if (isPlaying("Melee"))
        //{
        //    SetState(States.attacking);
        //}
        //if (isPlaying("Hurt"))
        //{
        //    SetState(States.hurt);
        //}


        //Moving
        //animator.SetBool("isMoving", state == States.moving);
        //Falling
        //animator.SetBool("isFalling", rb.velocity.y < -1f ? true : false);
        //Standing Up
        //if (isPlaying("Wake Up")) { state = States.wakeUp; }
        //Interacting
        //animator.SetBool("isInteracting", state == States.interacting);
        //Listening
        //animator.SetBool("isListening", state == States.listening); //toggle listening animation based on bool value
    }


    //Used for the door controller to set exit direction
    public void SetLastDir(Vector3 newDir)
    {
        lastDir = newDir;
    }


    public void SetState(States stateToSet)
    {
        state = stateToSet;
    }
}

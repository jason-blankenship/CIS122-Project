using JetBrains.Annotations;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //fields for moving
    [Header("Movement")]
    private float moveSpeed;
    public float sprintSpeed;
    public float walkSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public int maxJumps;
    bool readyTojump;

    //fields for coruching
    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    //fields for keybinds
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    //fields for checking ground
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround; //can set the world peieces to be of this layer
    bool grounded;
    public bool groundPound;



    public Transform orientation; //a transform for orentation game obj

    float horizontalInput; //for mouse input
    float verticalInput;

    int airJumps; // for a double jump

    Vector3 moveDirection; //a vector to handle move direction on 3 axis

    Rigidbody rb; //puling the ridig body from first person controller

    public MovementState state; //changes based on movent

    public enum MovementState //state class for moventstates changes bassed on movement
    {
        walking,
        sprinting,
        crouching,
        air
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() //sets all of these on start
    {
        rb = GetComponent<Rigidbody>(); //pulls rigidbody
        rb.freezeRotation = true; //freezes rotation
        readyTojump = true; //so you can jump at the start
        airJumps = maxJumps; //for max jumps
        startYScale = transform.localScale.y; // starting position
        groundPound = grounded; //for checking gorund otuside of script
    }

    // Update is called once per frame
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); //chekcing if im off the ground

        MyInput();
        SpeedControl();
        StateHandeler();

        if (grounded) 
        {
            rb.drag = groundDrag; //setting drag of rigid body if on ground
            airJumps = maxJumps; //setting max jumps
        }
        else
            rb.drag = 0; //drag is 0 if not
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); //for keyboard x input
        verticalInput = Input.GetAxisRaw("Vertical"); //for kwyboard y input

        //when to jump
        if (Input.GetKey(jumpKey) && readyTojump && airJumps > 0) //jump handeling
        {
            airJumps--; //so you cant double jump forever

            readyTojump = false; //for jump cooldown

            Jump(); //calls jump

            Invoke(nameof(ResetJump), jumpCooldown); //cant call again until cooldown. gives time to land and you cant spam
        }
        if (Input.GetKeyDown(crouchKey)) // crouch handling while key is pressed down...
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z); //creating a new vector to set player height while crouching
            rb.AddForce(Vector3.down, ForceMode.Impulse); //pushes player to gorund on crouch
        }

        if (Input.GetKeyUp(crouchKey)) //if key is released left ctrl
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z); //retun to starting yscale height
            rb.AddForce(Vector3.down, ForceMode.Impulse); //keeps player on gorund so they dont fly up upon uncrouching
        }
    }

    private void StateHandeler() //for setting the states of movement
    {
        if (grounded && Input.GetKey(crouchKey)) //if crouching and on gorund
        {
            state = MovementState.crouching; //then the game know im crouching
            moveSpeed = crouchSpeed; //sets my movespeed to crouchSpeed
        }

        else if (grounded && Input.GetKey(sprintKey)) //if sprinting and on gorund
        {
            state = MovementState.sprinting; //then the game know im sprinting
            moveSpeed = sprintSpeed; //sets my movespeed whole sprintSpeed
        }

        else if (grounded)//if walking and on gorund
        {
            state = MovementState.walking; //then the game know im walking
            moveSpeed = walkSpeed; //sets my movespeed to walkSpeed
        }

        else //otherwise im in the air
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer() //handeles moving the player
    {   //find movement direciton
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //if on ground apply grounded movement direction forces
        if (grounded)
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);

        //if in air apply air movement direction forces
        else if (!grounded)
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);

    }

    private void SpeedControl() //sets speed of the player
    {

        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z); //a new vector that has the flat velocity of the rigid body

        if (flatVel.magnitude > moveSpeed) //if the val of flatVel > that allowed max Movespeed
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed; //calculate what my max velocity would be
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); //then apply it
        }

    }

    private void Jump() //jump force handeling

    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //oncall sets y velocity

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //apply the force using ".up"
    }

    private void ResetJump()
    {
        readyTojump = true; //resets ready to jump to jump again

    }
}

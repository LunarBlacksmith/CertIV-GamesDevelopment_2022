using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Erika
{
    public class PlayerMovement : MonoBehaviour
    {
        //Access modifier, Type of data, reference name, optional assigned value
        [Header("Player Speeds")]
        [Tooltip("The speed applied to the character and what we pass to the animator controller")]
        public float speed = 0;// the speed applped to the character and what  what we pass to the controller
                               // these values below are so we have set values to change between
        public float crouchSpeed = 2.5f; // speed for crouching
        public float walkSpeed = 5.0f; // walk speed
        public float runSpeed = 10.0f; // run speed
        public float jumpSpeed = 8.0f; // jump speed
        public float isJumping = 0.0f; // are we jumping
        public Vector2 input;

        [Header("Directions")]
        public float leftRight = 0.0f; // control left to right direction and what we pass to the controller
        public float forwardBack = 0.0f; // forward to back direction and  what we pass to the controller
        public Vector3 moveDirection; // we will do this to move in 3d space and 
                                      // we will do this by apply the leftRight value to our x axis of the world
                                      // and by applying the forwardBack value to our z axis of the world
        public float isCrouching = 0.0f; // this is whatwill control our 2 idle states, crouching v standing
        public float gravity = 20.0f; // character controller does not have inbuilt gravity so we make our own


        [Header("Components")]
        //when we create variables that conect to components on objects in our game scene
        //we must then tell component which object to get the componnent from
        public Animator animator; // this is a reference to the players animator/ animator controller
        public CharacterController characterController;
        // this is a reference to th eplayers character controller, this allows us to move the player





        // containers upper case Start
        //




        // Start is called before the first frame update
        void Start()
        {
            // the variable is equal to the Animator component that this script is attached to
            animator = GetComponent<Animator>();
            //attching the Animator on our game object to our reference
            characterController = GetComponent<CharacterController>();
            //attaching the CharacterController on our game object to our reference

#if UNITY_EDITOR
            HandleTextFile.ReadSaveFile();
#endif


        }



        // Update is called once per frame
        void Update()
        {
            Debug.Log(characterController.isGrounded);

            #region Input GETAXIS
            ////isGrounded is built into the CharacterController Component, it checks if we are standing on a surface that has a collider attched to it
            //if (characterController.isGrounded)
            //{
            //    //using unity's inbuilt input system
            //    leftRight = Input.GetAxis("Horizontal"); // get the input value for left and right
            //    forwardBack = Input.GetAxis("Vertical"); // get the input value for forward and back
            //                                             //apply the inputs to our move direction value
            //    moveDirection = transform.TransformDirection(new Vector3(leftRight, 0, forwardBack));
            //    //adjust speed
            //    //if this condition is met
            //    //if our value for leftRight isnt equal to 0 we are moving side to side
            //    //if our value for forwardBack isnt equal to 0 we are moving forward and back
            //    //if our leftRight value is not equal to 0 or out forwardBack value is not equal to 0 then we are moving
            //    if (leftRight != 0 || forwardBack != 0)
            //    {
            //        //if we are sprinting
            //        if (Input.GetKey(KeyCode.LeftShift))
            //        {
            //            // set our speed to runSpeed
            //            speed = runSpeed;
            //        }
            //        //else if we are crouching
            //        else if (Input.GetKey(KeyCode.LeftControl))
            //        {
            //            //set our speed to crouchSpeed
            //            speed = crouchSpeed;
            //        }
            //        //we must be walking
            //        else
            //        {
            //            //set our speed to walkSpeed
            //            speed = walkSpeed;
            //        }

            //    }
            //    else// we are not moving
            //    {
            //        //set our speed to 0
            //        speed = 0;
            //        //we are not moving
            //        //if we arepressing the crouch button
            //        if (Input.GetKey(KeyCode.LeftControl))
            //        {
            //            //we are crouching
            //            isCrouching = 0;

            //        }
            //        else// we are  not pressing crouch
            //        {
            //            //we are standing
            //            isCrouching = 1;
            //        }

            //    }
            //    //apply the speed that we set to our direction
            //    moveDirection *= speed;

            //    if (Input.GetButton("Jump"))
            //    {
            //        isJumping = 1;
            //        moveDirection.y = jumpSpeed;
            //    }
            //    else
            //    {
            //        isJumping = 0;
            //    }
            //}
            #endregion

            #region KEYBINDS
            if (characterController.isGrounded)
            {

                /*
                 Using ? which is now as a temporary conditional operator ? allows is to evaluate 
                a boolean expression and return results based off which expression is met 
                you can also return catch all default value if neither are met.
                */
                /* if (Input.GetKey(KeyBinds.keys["Forward"]))
                 {
                     input.y = 1;
                 }
                 else if (Input.GetKey(KeyBinds.keys["Backward"]))
                 {
                     input.y = -1;
                 }
                 else
                 {
                     input.y = 0;
                 }*/


                //The above text means the same as the below text

                //the input.y equals 1 if Forward is pressed, equals -1 if Backwards is pressed and 0 if neither are pressed

                input.y = Input.GetKey(KeyBinds.keys["Forward"]) ? 1 : Input.GetKey(KeyBinds.keys["Backwards"]) ? -1 : 0;
                input.x = Input.GetKey(KeyBinds.keys["Right"]) ? 1 : Input.GetKey(KeyBinds.keys["Left"]) ? -1 : 0;

                /*Speed*/
                speed = Input.GetKey(KeyBinds.keys["Sprint"]) ? runSpeed : Input.GetKey(KeyBinds.keys["Crouch"]) ? crouchSpeed : walkSpeed;

                /*Moving according to our inputs and forward direction*/
                moveDirection = transform.TransformDirection(new Vector3(input.x, 0f, input.y));

                /*Movement is affected by our speed*/
                moveDirection *= speed;

                /*Jump*/
                //if (Input.GetKey(KeyBinds.keys["Jump"]))
                //{
                //    moveDirection.y = jumpSpeed;
                //}
                moveDirection.y = Input.GetKey(KeyBinds.keys["Jump"]) ? jumpSpeed : 0f;

            }
            #endregion

            //apply downward force to the character that simulates gravity
            moveDirection.y -= gravity * Time.deltaTime;
            //using the CharacterController, we are utilizing the inbuilt move
            //function to apply out movement
            characterController.Move(moveDirection * Time.deltaTime);
            //connect our values to our animations
            //apply speed value to the animator
            animator.SetFloat("Speed", speed);
            //apply leftRight value to the animator
            animator.SetFloat("LeftRight", leftRight);
            //apply forwardBack value to the animator
            animator.SetFloat("ForwardBack", forwardBack);
            //apply IsCrouching value to the animator
            animator.SetFloat("IsCrouching", isCrouching);
            //apply isJumping value to the animator
            animator.SetFloat("IsJumping", isJumping);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("RPG Game/Character/Movement")]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Character")]
    [Tooltip("Use this to apply the direction we are moving")]
    public Vector3 moveDir;
    public CharacterController charC;
    [Header("Speed")]
    public float moveSpeed = 5f;
    public float walkSpeed = 5f, crouchSpeed = 2.5f, runSpeed = 10f;
    public float jumpSpeed = 8f, gravity = 20f;
    public Vector2 input;
    #endregion

    void Start()
    {
        charC = GetComponent<CharacterController>();
        #if UNITY_EDITOR
        HandleTextFile.ReadSaveFile();
        #endif
    }

    void Update()
    {
        if (GameManager.gamePlayStates == GamePlayStates.Game)
        {
            #region Input GETAXIS
            /* if (charC.isGrounded)
             {
                 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                 moveDir = transform.TransformDirection(moveDir);
                 moveDir *= moveSpeed;
                 if (Input.GetButton("Jump"))
                 {
                     moveDir.y = jumpSpeed;
                 }
             }*/
            #endregion
            #region KEYBINDS
            if (charC.isGrounded)
            {
                /*
                 Using ? which is know as a ternary conditional operator
                ? allows us to evaluate a boolean expression and return results based off which expression is met.
                you can also return a catch all default value if neither are met
                                
                if (Input.GetKey(KeyBinds.keys["Forward"]))
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
                }
                
                The Above text means the same as the below text
                */
                // the input.y equals 1 if Forward is pressed, equals -1 if Backward is pressed and 0 if neither are pressed
                // input.y = Input.GetKey(KeyBinds.keys["Forward"]) ? 1 : Input.GetKey(KeyBinds.keys["Backward"]) ? -1 : 0;
                
                /*Will be in moveDir z axis*/  
                input.y = Input.GetKey(KeyBinds.keys["Forward"]) ? 1 : Input.GetKey(KeyBinds.keys["Backward"]) ? -1 : 0;
                /*Will be in moveDir x axis*/
                input.x = Input.GetKey(KeyBinds.keys["Right"]) ? 1 : 
                Input.GetKey(KeyBinds.keys["Left"]) ? -1 : 0;
                /*Speed*/
                moveSpeed = Input.GetKey(KeyBinds.keys["Sprint"]) ? runSpeed: 
                Input.GetKey(KeyBinds.keys["Crouch"]) ? crouchSpeed :walkSpeed;
                /*Moving according to our inputs and forward direction*/
                moveDir = transform.TransformDirection(new Vector3(input.x, 0, input.y));
                /*movement is affected by our speed*/
                moveDir *= moveSpeed;
                /*Jump*/
                if (Input.GetKey(KeyBinds.keys["Jump"]))
                {
                    moveDir.y = jumpSpeed;
                }
               // moveDir.y = Input.GetKey(KeyBinds.keys["Jump"]) ? jumpSpeed : moveDir.y;
            }
            #endregion
            moveDir.y -= gravity;
            charC.Move(moveDir * Time.deltaTime);
        }
    }
}
/*
 If Statement - can check multiple conditions with && and ||
if
else if
else if
else


Switch Statement checks against 1 type/condition(Value change)
case 0
case 1
.
.
.
.
default

? - Ternary conditional operator - can check multiple conditions with && and ||
? valueA : valueB
you must have : - else
 
 */
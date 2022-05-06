using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("RPG Game/Character/MouseLook")]
public class MouseLook : MonoBehaviour
{
    public enum RotationalAxis
    {
        MouseX,
        MouseY
    }
    [Header("Rotation")]
    public RotationalAxis axis;
    [Header("Sensitivity")]
    [Range(0, 500)]
    public float sensitivity = 10f;
    [Header("Rotation Clamp")]
    public float minY = -60f;
    public float maxY = 60f;
    private float _rotY;
    public bool invert;

    void Start()
    {
        //if our GameObj has a rigidbody attached to it
        if (GetComponent<Rigidbody>())
        {
            //set the rigidbody freezeRotation to true
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        //if the object is the main camera
        if (GetComponent<Camera>())
        {
            //our axis changes to allow us to look up and down
            axis = RotationalAxis.MouseY;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gamePlayStates == GamePlayStates.Game)
        {
            //CAMERA MOVEMENT
            #region Mouse X
            //if we are rotating on the X direction of our mouse
            if (axis == RotationalAxis.MouseX)
            {
                //transform the rotation on our gameobj Y axis
                //by our Mouse Input - Mouse X times sensitivity
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
            }
            #endregion

            #region Mouse Y
            //else we are only rotating on the Y
            else
            {
                //our rotation Y is plus equals our mouse input for Mouse Y
                _rotY += Input.GetAxis("Mouse Y") * sensitivity;
                //the rotation Y is CLamped using Mathf and we are clamping the Y rotation
                // to the Y min and Y max
                _rotY = Mathf.Clamp(_rotY, minY, maxY);
                //transform our local position to the next Vector3 rotation - Y rotation
                // on the x axis
                if (invert)
                {
                    transform.localEulerAngles = new Vector3(_rotY, 0, 0); //inverted
                }
                else
                {
                    transform.localEulerAngles = new Vector3(-_rotY, 0, 0); //not inverted
                }
            }
            #endregion
        }
    }
}

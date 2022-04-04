using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour
{
    //private Vector2 buttonStartPos = new Vector2();
    /// <summary>
    /// Inhabitants per click variable
    /// </summary>
    public static int ipclick = 1;

    //TODO: Figure out how to make text move downwards when the button is pressed, and back to original position when it is not pressed.

    #region Help/Test Fields
    //image reference in the unity editor
    //public Sprite nextImage;
    //private bool isHatched = false;
    #endregion

    public void Start()
    {
        /*
        if (gameObject.GetComponentInChildren<Text>())
        {
            buttonStartPos = gameObject.GetComponentInChildren<Text>().transform.position;
        }*/
    }

    public void Update()
    {
        #region Pressed/Unpressed Not Working
        //if the current gameobject has the tag Pressable Button
        /*if (gameObject.tag == "PressableButton")
        {
            switch (isPressed)
            {
                case true:
                    {
                        //moving the button text down relative to the world space (.down is just (0,-1) and we're multiplying that by 3)
                        gameObject.GetComponentInChildren<Text>().transform.Translate(Vector2.down * 3);
                        break;
                    }
                    case false:
                    {
                        Debug.Log("We made it?");
                        //moving the button text to its starting position
                        gameObject.GetComponentInChildren<Text>().transform.position = buttonStartPos;
                        break;
                    }
            }
            isPressed = false;

        }*/
        #endregion
    }

    /// <summary>
    /// Increases inhabitants in GameManager using ipclick.
    /// </summary>
    public void Click()
    {
        //if the current gameobject has the tag Pressable Button
        //if (gameObject.tag == "PressableButton")
        //{
        //    isPressed = true;
        //}
        GameManager.inhabitants += ipclick;
        //Writes current inhabitants value in GameManager script to console.
        Debug.Log(GameManager.inhabitants);
    }
    /// <summary>
    /// Increases inhabitants in GameManager using ipclick, and prints message arg to console with inhabitants value.
    /// </summary>
    /// <param name="message_p"></param>
    public void Click(string message_p)
    {
        GameManager.inhabitants += ipclick;
        Debug.Log($"{message_p} {GameManager.inhabitants}");
    }
    /*public void Click(bool testNextImage_p)
    {
        GameManager.inhabitants += ipclick;
        //GameManager.pokes += ppc;

        //Writes current inhabitants value in GameManager script to console.
        Debug.Log(GameManager.inhabitants);

        //do the block of code only while the condition is true
        do
        {
            //if the population is greater than or equal to 10
            if (GameManager.inhabitants >= 10)
            {
                //gameObject = this object (the button)
                //GetComponent<Image> = (I want the image part of my button)
                //.sprite = specifically the sprite part which is how Unity changes images
                //nextImage = our Unity reference to the next image we want to switch to
                gameObject.GetComponentInChildren<Image>().sprite = nextImage;
                isHatched = true;
            }
            else //if it's not >= 10
            {
                //exit this do/while loop to avoid infinite looping
                break;
            }
        } while (!isHatched);

    }*/

    #region Help/Test Methods
    //This method is just for the use of assisting classmates with the code to test image on the button and not as a child.
    /*public void ClickDemo(bool testNextImage_p)
    {
        GameManager.inhabitants += ipclick;
        //GameManager.pokes += ppc;

        //Writes current inhabitants value in GameManager script to console.
        Debug.Log(GameManager.inhabitants);

        do
        {
            //if happiness is greater than or equal to 10
            if (GameManager.inhabitants >= 10)
            {
                //gameObject = this object (the button)
                //GetComponent<Image> = (I want the image part of my button)
                //.sprite = specifically the sprite part which is how Unity changes images
                //nextImage = our Unity reference to the next image we want to switch to
                gameObject.GetComponent<Image>().sprite = nextImage;
                isHatched = true;
            }
            else
            {
                break;
            }
        } while (!isHatched);

    }*/
    #endregion
}




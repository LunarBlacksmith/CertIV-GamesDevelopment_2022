using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour
{
    /// <summary>
    /// Inhabitants per click variable
    /// </summary>
    public static int ipclick = 1;


    #region Help/Test Fields
    //image reference in the unity editor
    //public Sprite nextImage;
    //private bool isHatched = false;
    #endregion

    /// <summary>
    /// Increases inhabitants in GameManager using ipclick.
    /// </summary>
    public void Click()
    {
        GameManager.inhabitants += ipclick;
        //Writes current inhabitants value in GameManager script to console.
        //Debug.Log(GameManager.inhabitants);
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




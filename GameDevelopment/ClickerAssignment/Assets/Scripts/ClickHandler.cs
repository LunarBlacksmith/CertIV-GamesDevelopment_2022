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
    public GameManager gameManager;

    /// <summary>
    /// Increases inhabitants in GameManager using ipclick variable.
    /// </summary>
    public void Click()
    { GameManager.inhabitants += ipclick; } //adds the value of ipclick to the total population
}




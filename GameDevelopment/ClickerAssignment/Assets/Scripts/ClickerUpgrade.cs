using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FingerUpgrade : MonoBehaviour
{
    public Text costText;
    int cost = 20;

    public void Purchase()
    {
        if (GameManager.eyesPoked >= cost)
        {
            GameManager.eyesPoked -= cost;
            Debug.Log("You purchased an upgrade!");
            CookieClickHandler.ppc += 1;
            cost += 10;
            costText.text = $"Finger Upgrade - {cost} pokes";
        }
        else
        {
            Debug.Log("You cannot afford this upgrade.");
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUpgrade : MonoBehaviour
{
    public Text costText;
    int _cost = 0;
    public int Cost
    {
        get { return _cost; }
        set { _cost = (value >= 0) ? value : _cost; } //if the value trying to set cost to is less than 0, nothing happens (NO REFUNDS)
    }

    /// <summary>
    /// Decrease current inhabitants amount by the cost value. If cost is greater than population, prints a console message.
    /// </summary>
    public void Purchase()
    {
        if (GameManager.inhabitants >= _cost)
        {
            //subtract cost value from current population value
            GameManager.inhabitants -= _cost;
            Debug.Log("You purchased an upgrade!");
            //increase the amount of inhabitants per click by 1
            ClickHandler.ipclick += 1;
            //increase the cost of next purchase by 100
            Cost += 100;
            //if the costText reference isn't null
            if (!costText)
            {
                //Show UPGRADE [cost amount formatted to separate by comma in thousands] Population
                costText.text = $"UPGRADE\n{Cost.ToString("N0")} Population";
            }
        }
        else
        {
            Debug.Log("You need more inhabitants before you can afford this upgrade.");
        }
    }

    #region Dev Cheats
    /// <summary>
    /// Decrease current inhabitants amount by the cost value and manually set the next cost amount by the value passed in.
    /// </summary>
    /// <param name="setCost_p"></param>
    public void Purchase(int setCost_p)
    {
        if (GameManager.inhabitants >= _cost)
        {
            GameManager.inhabitants -= _cost;
            Debug.Log("You purchased an upgrade!");
            ClickHandler.ipclick += 1;
            Cost += setCost_p;
        }
        else
        {
            Debug.Log("You need more inhabitants before you can afford this upgrade.");
        }
    }
    #endregion
}

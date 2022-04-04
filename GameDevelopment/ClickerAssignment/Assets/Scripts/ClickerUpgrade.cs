using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerUpgrade : MonoBehaviour
{
    [Tooltip("This is the Text relating to this specific Game Object.")]
    public Text costText;
    int _cost = 100;
    int _clickerGrowthVar = 1;
    int _costGrowthVar = 1;
    public int Cost 
    {
        get { return _cost; }
        set { _cost = (value >= 0) ? value : _cost; } //if the value trying to set cost to is less than 0, nothing happens (NO REFUNDS)
    }
    public int ClickerGrowthVar { get; private set; }
    public int CostGrowthVar { get; private set; }

    /// <summary>
    /// Decrease current inhabitants amount by the cost value. If cost is greater than population, prints a console message.
    /// </summary>
    public void Purchase()
    {
        if (GameManager.inhabitants >= Cost)
        {
            //subtract cost value from current population value
            GameManager.inhabitants -= Cost;
            Debug.Log("You purchased an upgrade!");
            //increase the amount of inhabitants per click by 1 times our clicker growth variable
            ClickHandler.ipclick += 2 * ClickerGrowthVar;
            //increase the cost of next purchase by 100
            Cost += 110 * CostGrowthVar;
            //increment both growth variables by 1
            ++ClickerGrowthVar;
            ++CostGrowthVar;
            //if the costText reference isn't null
            if (costText)
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
        if (GameManager.inhabitants >= Cost)
        {
            GameManager.inhabitants -= Cost;
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


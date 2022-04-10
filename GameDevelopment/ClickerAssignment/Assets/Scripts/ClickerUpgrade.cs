using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickerUpgrade : MonoBehaviour
{
    //[Tooltip("This is the Text relating to this specific Game Object.")]
    public Text costText;
    public GameManager gameManager;
    int _cost = 100; //initial cost of a cost upgrade
    int _clickerGrowthVar = 1, _costGrowthVar = 1; //despite what VS thinks, these are manipulated through properties in local methods
    public int Cost 
    {
        get { return _cost; }
        set { _cost = (value >= 0) ? value : _cost; } //if the value trying to set cost to is less than 0, nothing happens (NO REFUNDS)
    }
    public int ClickerGrowthVar { get; private set; }
    public int CostGrowthVar { get; private set; }

    public void Start()
    {
        Debug.Log($"Cost Text for Clicker: {costText.text}");
        gameManager.UpdateTextField(2); //Updates the text displaying how many inhabitants per click the player has when game starts
        costText.text = $"UPGRADE POP/CLICK\n{Cost.ToString("N0")} Population"; //update button text for upgrade
    } 

    /// <summary>
    /// Decrease current inhabitants amount by the cost value. If cost is greater than population, prints a console message.
    /// </summary>
    public void Purchase()
    {
        if (GameManager.inhabitants >= Cost)
        {
            //subtract cost value from current population value
            GameManager.inhabitants -= Cost;
            //increase the amount of inhabitants per click by 1 times our clicker growth variable
            ClickHandler.ipclick += 2 * ClickerGrowthVar;
            gameManager.SendMessageToUser($"You now have {ClickHandler.ipclick} inhabitants per click!", 2f);
            //increase the cost of next purchase using a curved algorithm
            Cost += 110 * CostGrowthVar;
            //increment both growth variables by 1
            ++ClickerGrowthVar;
            ++CostGrowthVar;
            costText.text = $"UPGRADE POP/CLICK\n{Cost.ToString("N0")} Population";
        }
        else
        { gameManager.SendMessageToUser($"You need more inhabitants before you can afford this upgrade.", 2f); }
        //don't need to call this every frame since it's only affected when a new clicker upgrade is purchased
        gameManager.UpdateTextField(2); 
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


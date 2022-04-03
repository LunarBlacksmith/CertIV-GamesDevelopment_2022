using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static int _houseArraySize;
    /// <summary>
    /// Property field to get or set the available housing array size. Value cannot be 0 or lower.
    /// </summary>
    public static int HouseArraySize
    {
        get { return _houseArraySize; }
        set { _houseArraySize = (value > 0) ? value : _houseArraySize;
            /*
            above line same as the following if statement:
            if (value > 0)
            { _HouseArraySize = value; }
            */
        }
    }
    public Sprite[] housingSprites = new Sprite[HouseArraySize];
    public ClickerUpgrade cUpgrade;
    public LifeUpgrade lifeUpgrade;
    public HouseUpgrade houseUpgrade;
    public Text populationText, deathTollText, userMessageDisplay;
    public static int inhabitants = 0, deathToll = 0;

    public void Update()
    {
        #region Dev Cheats
    #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.C))
        {
            inhabitants += 100000;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            cUpgrade.Purchase(100000);
        }
        //Update our Upgrade button text to display the next cost amount formatted by thousands.
        cUpgrade.costText.text = $"UPGRADE\n{FormatMyInt(cUpgrade.Cost)} Population";
#endif
        #endregion

        //Update the population text to reflect our current number of inhabitants formatted by thousands.
        UpdateTextField(0);
    }

    /// <summary>
    /// Separates the number passed in by thousands with a comma, no decimal places, and returns a string.
    /// </summary>
    /// <param name="number_p"></param>
    /// <returns></returns>
    public static string FormatMyInt(int number_p)
    {
        return number_p.ToString("N0");
    }
    /// <summary>
    /// Updates a text field. The field is dependant on the int passed in as an index: 0 = population, 1 = death toll.
    /// </summary>
    /// <param name="indexOfTextField"></param>
    public void UpdateTextField(int indexOfTextField)
    {
        switch (indexOfTextField)
        {
            case 0:
                { 
                    populationText.text = $"{FormatMyInt(inhabitants)} \nInhabitants";
                    break;
                }
            case 1:
                {
                    deathTollText.text = $"{FormatMyInt(deathToll)} \nDeaths";
                    break;
                }
            default:
                {
        #if UNITY_EDITOR
                    Debug.Log("Not a valid index for a text field, we only have 0 for Inhabitants and 1 for Deaths.");
        #endif
                    break;
                }
                
        }
    }
}

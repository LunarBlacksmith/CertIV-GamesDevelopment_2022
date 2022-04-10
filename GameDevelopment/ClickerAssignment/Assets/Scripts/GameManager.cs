using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ClickerUpgrade cUpgrade;
    public LifeUpgrade lifeUpgrade;
    public HouseUpgrade houseUpgrade;
    public Text populationText, deathTollText, userMessageDisplay;
    public static long inhabitants = 0, deathToll = 0, _markedForDeath = 2;
    public static float lifeTimer = 2f;
    private int _numberOfUpgrades = 0, _reaperMultiplier = 1;
    private float _time = 0f;
    private bool _timerIsActive = false;

    public void Update()
    {
        _time += Time.deltaTime;
        if (_time >= lifeTimer)
        {
            KillHumans(_markedForDeath);
            _time -= lifeTimer;
        }

        ThanosGlasses();
        //culls an amount of the population at a repeating amount of time
        //time based on our life timer
        //amount of population culled based on markedForDeath
        //KillHumans(lifeTimer, _markedForDeath);
        //Update the population text to reflect our current number of inhabitants, formatted by thousands.
        UpdateTextField(0);
        //Update death toll text to reflect current number of deaths, formatted by thousands.
        UpdateTextField(1);

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
        cUpgrade.costText.text = $"UPGRADE\n{FormatMyLong(cUpgrade.Cost)} Population";
#endif
        #endregion

    }

    /// <summary>
    /// Removes an amount of inhabitants at a repeating amount of time based on the arguments.
    /// NOTE: cannot be 0 seconds.
    /// </summary>
    /// <param name="secondsToWait_p"></param>
    /// <param name="amountOfHumans_p"></param>
    public void KillHumans(long amountOfHumans_p)
    {
        //TODO: UPDATE THE DEATHTOLL VARIABLE
        Debug.Log($"Killing {_markedForDeath} humans");
        //StartCoroutine(Timer(secondsToWait_p));
        //check if removing the amount of the population would decrease it beyond the permanent population
        if ((inhabitants - _markedForDeath) < houseUpgrade.PermanentPop)
        { inhabitants = houseUpgrade.PermanentPop; } //set current population to only that of the permanent population
        else 
        { inhabitants -= _markedForDeath; } //entirely remove those marked for death from the current population
    }

    /// <summary>
    /// Perfectly balanced, every frame, as all things should be.
    /// </summary>
    public void ThanosGlasses()
    {
        Debug.Log($"Number of Upgrades: {_numberOfUpgrades}, Clicker Growth Var: {cUpgrade.ClickerGrowthVar}");
        //if the number of upgrades the player has does not equal the number of times ClickerGrowthVar is called (every upgrade)
        if (_numberOfUpgrades != cUpgrade.ClickerGrowthVar)
        {
            //that means the player has purchased another upgrade since the last update
            //so make more people die. The reaper multiplier adds a smooth growth curve for the number
            _markedForDeath += 5 * _reaperMultiplier;
            //increase our multiplier for that sweet curve
            _reaperMultiplier++;
            //set the number of upgrades tracked here to equal the number the player has
            _numberOfUpgrades = cUpgrade.ClickerGrowthVar;
        }
    }

    /// <summary>
    /// Separates the number passed in by thousands with a comma, no decimal places, and returns a string.
    /// </summary>
    /// <param name="number_p"></param>
    /// <returns></returns>
    public static string FormatMyLong(long number_p)
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
                    populationText.text = $"{FormatMyLong(inhabitants)} \nInhabitants";
                    break;
                }
            case 1:
                {
                    deathTollText.text = $"{FormatMyLong(deathToll)} \nDeaths";
                    break;
                }
            default:
                {
        #if UNITY_EDITOR
                    Debug.Log("Not a valid index for a text field, we only have 0 for Inhabitants and 1 for Deaths.");
                    SendMessageToUser("Not a valid index for a text field, we only have 0 for Inhabitants and 1 for Deaths.", 4f);
        #endif
                    break;
                }
                
        }
    }

    /// <summary>
    /// Shows the string arg to the user for the amount of seconds given in the float arg.
    /// </summary>
    /// <param name="message_p"></param>
    /// <param name="secondsDisplayed_p"></param>
    public void SendMessageToUser(string message_p, float secondsDisplayed_p)
    {
        //as long as the string isn't null and the seconds to wait is greater than 0
        if (message_p != null && secondsDisplayed_p > 0) 
        {
            //call our coroutine to make the text display the message
            StartCoroutine(ShowMessage(message_p, secondsDisplayed_p));
        }
    }
    private IEnumerator ShowMessage(string message_p, float delay_p)
    {
        userMessageDisplay.text = message_p;
        userMessageDisplay.enabled = true;
        yield return new WaitForSeconds(delay_p);
        userMessageDisplay.enabled = false;
    }
    private IEnumerator Timer(float seconds_p)
    {

        yield return new WaitForSeconds(seconds_p);
    }
}

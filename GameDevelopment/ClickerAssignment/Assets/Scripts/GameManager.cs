using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ClickerUpgrade cUpgrade;
    public LifeUpgrade lifeUpgrade;
    public HouseUpgrade houseUpgrade;
    //so we can update these when needed for feedback to player
    public Text userMessageDisplay, populationText, deathTollText, iPClickText, lifeTimerText, permanentPopText; 
    public static long inhabitants = 0, deathToll = 0, _markedForDeath = 2;
    public static float lifeTimer = 1f; //how often x amount of inhabitants are removed
    private int _numberOfUpgrades = 0, _reaperMultiplier = 1; //reaperMult allows smoother difficulty curve for continuity
    private bool _killingIsActive = false;  //manipulation prevents timer to run every frame

    public void Update()
    {
        //culls an amount of the population at a repeating amount of time
        //time based on our life timer
        //amount of population culled based on markedForDeath
        StartCoroutine(IKillHumans(lifeTimer, _markedForDeath));
        ThanosGlasses();
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
    public IEnumerator IKillHumans(float secondsToWait_p, long amountOfHumans_p)
    {
        //mostly for the coroutine to not run every frame, we control it with a boolean
        if (!_killingIsActive)
        {
            //once we've started, set it to true
            _killingIsActive = true;
            //check if removing the amount of the population would decrease it beyond the permanent population
            if ((inhabitants - _markedForDeath) < houseUpgrade.PermanentPop)
            {
                //set current population to only that of the permanent population
                inhabitants = houseUpgrade.PermanentPop; 
                //we've already checked the remaining amount after removing markedForDeath, so the
                //amount added to the death toll would be the permanent population subtracted from the inhabitants count
                //since that remaining amount is what is being completely removed
                deathToll += inhabitants - houseUpgrade.PermanentPop;
            } 
            else
            { 
                inhabitants -= _markedForDeath; //entirely remove those marked for death from the current population
                deathToll += _markedForDeath; //add those that were removed to the death toll
            } 
            yield return new WaitForSeconds(secondsToWait_p);
            //only set the boolean to false after we've finished waiting the amount of seconds
            _killingIsActive = false;
        }
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
    { return number_p.ToString("N0"); }

    /// <summary>
    /// Updates a text field. The field is dependant on the int passed in as an index: 0 = population, 1 = death toll, 2 = iPClick, 3 = life timer.
    /// </summary>
    /// <param name="indexOfTextField"></param>
    public void UpdateTextField(int indexOfTextField)
    {
        switch (indexOfTextField)
        {
            case 0:
                { populationText.text = FormatMyLong(inhabitants); break; }
            case 1:
                { deathTollText.text = FormatMyLong(deathToll); break; }
            case 2:
                { iPClickText.text = ClickHandler.ipclick.ToString("N0"); break; }
            case 3:
                { lifeTimerText.text = $"{lifeTimer.ToString("0.00")} \nseconds"; break; }
            case 4:
                { permanentPopText.text = houseUpgrade.PermanentPop.ToString("N0"); break; }
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
        { StartCoroutine(ShowMessage(message_p, secondsDisplayed_p)); } //call our coroutine to make the text display the message
    }
    private IEnumerator ShowMessage(string message_p, float delay_p)
    {
        userMessageDisplay.text = message_p;
        userMessageDisplay.enabled = true;
        yield return new WaitForSeconds(delay_p);
        userMessageDisplay.enabled = false;
    }
}

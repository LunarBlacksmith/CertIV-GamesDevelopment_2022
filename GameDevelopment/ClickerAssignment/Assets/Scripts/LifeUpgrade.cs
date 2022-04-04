using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUpgrade : MonoBehaviour
{
    public Text costText;
    public int Cost
    {
        get { return _cost; }
        set { _cost = (value >= 0) ? value : _cost; } //if the value trying to set cost to is less than 0, nothing happens (NO REFUNDS)
    }
    public int LifePurchases
    {
        get { return _lifePurchases; }
        set
        {
            //if the value trying to set cost to is less than 0, nothing happens (Can't have negative life upgrades)
            _lifePurchases = (value >= 0) ? value : _lifePurchases;
            //if the result of a mathmatical operation causes life purchases to be less than 0, set it to 0
            if (_lifePurchases < 0)
            {
                _lifePurchases = 0;
            }
        }
    }
    private float[] _lifeUpgradeValues = new float[3] { 1, 2, 3 };
    private int _cost = 0, _lifePurchases = 0;
    private bool _isButtonDisabled = false, _hasFinalUpgrade = false;


    public void Update()
    {
        //check if we have the final life upgrade already
        if (!_hasFinalUpgrade)
        {
            //set our has final upgrade to true if purchases is the same as the array length, otherwise remain false
            _hasFinalUpgrade = (LifePurchases == _lifeUpgradeValues.Length) ? true : false;
        }
        //check if we have already disabled the button
        else if (!_isButtonDisabled)
        { //and if we haven't then check if our life purchases is also as big as our life upgrades array (the number of available life upgrades)
            switch (LifePurchases == _lifeUpgradeValues.Length)
            {
                case true:
                    { //set our button to disabled (we don't want the button to disappear!)
                        gameObject.GetComponent<Button>().enabled = false;
                        //set the image of the button to a grey overtone
                        gameObject.GetComponent<Image>().color = Color.grey;
                        //set the button's text to a grey overtone
                        gameObject.GetComponentInChildren<Text>().color = Color.grey;
                        //set our bool of disabling our button to true so the code doesn't constantly run this code block.
                        _isButtonDisabled = true;
                        break;
                    }
                default: //if we can still purchase life upgrades we still want to leave the button enabled.
                    { break; }
            }
        }
    }


    /// <summary>
    /// Increase the time before the population is automatically decreased.
    /// </summary>
    public void Purchase()
    {
        if (GameManager.inhabitants >= Cost)
        {
            //increase life timer on purchase, set index by the value of how many purchases we have
            GameManager.lifeTimer += _lifeUpgradeValues[LifePurchases];
            //subtract cost value from current population value
            GameManager.inhabitants -= Cost;
            //increment the amount of life purchases
            ++LifePurchases;
            Debug.Log("You purchased a Life Upgrade!");
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
}

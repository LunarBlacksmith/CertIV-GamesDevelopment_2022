using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUpgrade : MonoBehaviour
{
    public GameManager gameManager;
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
    private float[] _lifeUpgradeValues = new float[3] { 0.25f, 0.5f, 0.75f };
    private int _cost = 800, _lifePurchases = 0;
    private bool _isButtonDisabled = false, _hasFinalUpgrade = false;

    public void Start()
    { gameManager.UpdateTextField(3); costText.text = $"UPGRADE LIFESPAN\n{Cost.ToString("N0")} Population"; }

    public void Update()
    {
        //check if we have the final life upgrade already
        // and set our has final upgrade to true if purchases is the same as the array length, otherwise remain false
        if (!_hasFinalUpgrade)
        { _hasFinalUpgrade = (LifePurchases == _lifeUpgradeValues.Length) ? true : false; } 
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
            if (LifePurchases < _lifeUpgradeValues.Length)
            {
                //increase life timer on purchase, set index by the value of how many purchases we have
                GameManager.lifeTimer += _lifeUpgradeValues[LifePurchases];
                GameManager.inhabitants -= Cost;
                //increment the amount of life purchases
                ++LifePurchases;
                switch (LifePurchases)
                {
                    case 1: { Cost = 30000; break; }
                    case 2: { Cost = 110000; break; }
                    default:
                        {
                            Debug.Log($"Something's gone wrong with setting your HousePurchases if you're seeing this message." +
                                $"\nHousePurchases: {LifePurchases}");
                            break;
                        }
                }
                gameManager.UpdateTextField(3);
                gameManager.SendMessageToUser("You purchased a Life Upgrade!", 2f);
                costText.text = $"UPGRADE LIFESPAN\n{Cost.ToString("N0")} Population";
            }
        }
        else
        { gameManager.SendMessageToUser("You need more inhabitants before you can afford this upgrade.", 2f); }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseUpgrade : MonoBehaviour
{
#if UNITY_EDITOR
    //only created this for dev purposes in the editor
    [Tooltip("This changes the number of house upgrades available.")]
    public int houseUpgrades = 5;
#endif
    
    [Tooltip("DO NOT CHANGE THIS VALUE. Use 'Accessible House Array Size' to change the size of the array.")]
    public Sprite[] housingSprites;
    public Text costText;
    public GameManager gameManager;
    [Tooltip("The image object that the current house upgrade image will display through.")]
    public Image houseImage;
    private int _houseArraySize;
    private int _cost = 1000;
    private int _housePurchases = 0, _permanentPop = 0;
    private bool _isButtonDisabled = false;
    private bool _hasFinalHouse = false;
    public int Cost
    {
        get { return _cost; }
        set { _cost = (value >= 0) ? value : _cost; } //if the value trying to set cost to is less than 0, nothing happens (NO REFUNDS)
    }
    public int HousePurchases
    {
        get { return _housePurchases; }
        set 
        {
            //if the value trying to set cost to is less than 0, nothing happens (Can't have negative houses)
            _housePurchases = (value >= 0) ? value : _housePurchases;
            //if the result of a mathmatical operation causes house purchases to be less than 0, set it to 0
            if (_housePurchases < 0)
            {
                _housePurchases = 0;
            }
        } 
    }
    /// <summary>
    /// Property field to get or set the available housing array size. Value cannot be 0 or lower.
    /// </summary>
    public int HouseArraySize
    {
        get { return _houseArraySize; }
        set
        {
            _houseArraySize = (value > 0) ? value : _houseArraySize;
            Debug.Log($"{_houseArraySize}");
            /*
            above line same as the following if statement:
            if (value > 0)
            { _HouseArraySize = value; }
            */
        }
    }
    public int PermanentPop { get; private set; }

    public void Start()
    {
    #if UNITY_EDITOR
        HouseArraySize = houseUpgrades;
    #endif
        //set our house image sprite to the first house sprite in our array
        houseImage.sprite = housingSprites[0];
    }

    public void Update()
    {
        //if we haven't bought the final house in the available upgrades
        if (!_hasFinalHouse)
        {
            //check the number of houses we've purchased
            switch (HousePurchases)
            {
                case 0:
                    {
                        //if we don't have housing upgrades, disable the image
                        houseImage.enabled = false;
                        Debug.Log($"Update1 {HousePurchases} + {HouseArraySize}");
                        break;
                    }
                case 1:
                    {
                        //set our house sprite that is displayed in UI to the number of our purchases value - 1 because arrays are zero based.
                        houseImage.sprite = housingSprites[HousePurchases - 1];
                        //obviously if we've purchased a house we want it to show up, so we set the image to active
                        houseImage.enabled = true;
                        break;
                    }
                case 2:
                    {
                        houseImage.sprite = housingSprites[HousePurchases - 1];
                        houseImage.enabled = true;
                        break;
                    }
                case 3:
                    {
                        houseImage.sprite = housingSprites[HousePurchases - 1];
                        houseImage.enabled = true;
                        break;
                    }
                case 4:
                    {
                        houseImage.sprite = housingSprites[HousePurchases - 1];
                        houseImage.enabled = true;
                        break;
                    }
                case 5:
                    {
                        houseImage.sprite = housingSprites[HousePurchases - 1];
                        houseImage.enabled = true;
                        break;
                    }
                default:
                    {
                        houseImage.enabled = false;
                        Debug.Log($"You probably don't have an image set to the array index. Check your housing sprites array." +
                            $"\nHousePurchases: {HousePurchases}" +
                            $"\nHousingSprites array: {housingSprites[HousePurchases - 1]}");
                        break;
                    }
            }
            if (HousePurchases == HouseArraySize) { _hasFinalHouse = true; }
            
        }
        

        //check if we have already disabled the button
        if (!_isButtonDisabled)
        { //and if true then check if our house purchases is also as big as our house array (the number of available house upgrades)
            switch (HousePurchases == HouseArraySize)
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
                        Debug.Log($"Update2: {HousePurchases} + {HouseArraySize}, we're disabling our button.");
                        break;
                    }
                default: //if we can still purchase house upgrades we still want to leave the button enabled.
                    { break; }
            }
        }
        
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
            gameManager.SendMessageToUser("You purchased a House upgrade!", 2f);
            //If HousePurchases plus 1 is greater than our house array size, equals itself. Otherwise add 1.
            HousePurchases = ((HousePurchases+1) > HouseArraySize) ? HousePurchases : HousePurchases+1;
            
            //INCREASE COST of next purchase:
            switch (HousePurchases) //check what our current number of house upgrades is
            {
                //don't need to set cost here since default is 1000
                case 0: { break; }
                case 1: { Cost = 3000; break; }//set the cost value based on the number of housing upgrades purchased
                case 2: { Cost = 10000; break; }
                case 3: { Cost = 25000; break; }
                case 4: { Cost = 110000; break; }
                case 5: { break; } //if we already have the last upgrade, we can't buy any more anyway, so don't set a new cost.
                default:
                    {
                        Debug.Log($"Something's gone wrong with setting your HousePurchases if you're seeing this message." +
                            $"\nHousePurchases: {HousePurchases}");
                        break;
                    }
            }
            //if the costText reference isn't null
            if (costText)
            {
                Debug.Log("Updating the button text.");
                //Show UPGRADE [cost amount formatted to separate by comma in thousands] Population
                costText.text = $"UPGRADE\n{Cost.ToString("N0")} Population";
            }
        }
        else
        {
            Debug.Log("You need more inhabitants before you can afford this upgrade.");
            gameManager.SendMessageToUser("You need more inhabitants before you can afford this upgrade.", 4f);
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

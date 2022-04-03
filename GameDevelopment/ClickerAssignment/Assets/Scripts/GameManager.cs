using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ClickerUpgrade cU;
    public Text populationText;
    public static int inhabitants = 0;

    public void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.C))
        {
            inhabitants += 10000;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            cU.Purchase(10000);
        }
        cU.costText.text = $"UPGRADE\n{cU.Cost.ToString("N0")} Population";
#endif
        populationText.text = $"{inhabitants} \nInhabitants";
    }
}

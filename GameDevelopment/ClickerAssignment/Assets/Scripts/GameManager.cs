using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text pokesText;
    public static int eyesPoked = 0;

    public void Update()
    {
        pokesText.text = $"{eyesPoked} \nEyes Poked";
    }
}

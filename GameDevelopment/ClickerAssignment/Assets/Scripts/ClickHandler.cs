using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieClickHandler : MonoBehaviour
{
    public static int ppc = 1;

    public void Click()
    {
        GameManager.eyesPoked += ppc;
        Debug.Log(GameManager.eyesPoked);
    }
    public void Click(string message_p)
    {
        GameManager.eyesPoked += ppc;
        Debug.Log($"{message_p} {GameManager.eyesPoked}");
    }
}

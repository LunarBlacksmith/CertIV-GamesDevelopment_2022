using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : Stats
{
    // singleton
    public static PlayerHandler playerHandlerInstance;

    private void Awake()
    {
        // THERE CAN BE ONLY ONE
        if (playerHandlerInstance != null && playerHandlerInstance != this)
        { Destroy(this); }
        else
        { playerHandlerInstance = this; }
    }
}

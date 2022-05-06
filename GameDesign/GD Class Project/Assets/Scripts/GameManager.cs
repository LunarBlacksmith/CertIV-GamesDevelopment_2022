using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GamePlayStates gamePlayStates;
    public static Vector2 scr;

    // Start is called before the first frame update
    void Start()
    {
        scr.x = Screen.width / 16;
        scr.y = Screen.height / 9;
        gamePlayStates = GamePlayStates.Game;

    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.width / 16 != scr.x)
        {
            scr.x = Screen.width / 16;
            scr.y = Screen.height / 9;

        }

        switch (gamePlayStates)
        {
            //A case is the same as an if or else if
            //This is what allows us to check our condition

            case GamePlayStates.PreGame:
                if (!Cursor.visible)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                break;
            case GamePlayStates.Game:
                if (Cursor.visible)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                break;
            case GamePlayStates.MenuPause:
                if (!Cursor.visible)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                break;
            case GamePlayStates.PostGame:
                if (Cursor.visible)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }


                break;
            //default is your else
            //it gets anything that you didnt state above
            default:
                if (!Cursor.visible)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                break;
        }

         //void CursorToggle()
         //{
         //    if (Cursor.visible)
         //    {
         //        Cursor.visible = false;
         //        Cursor.lockState = CursorLockMode.None;
         //    }
         //    if (Cursor.visible)
         //    {
         //        Cursor.visible = true;
         //        Cursor.lockState = CursorLockMode.Locked;
         //    }
         //}
        

    }

}

    public enum GamePlayStates
{
    PreGame,
    Game,
    MenuPause,
    PostGame
}

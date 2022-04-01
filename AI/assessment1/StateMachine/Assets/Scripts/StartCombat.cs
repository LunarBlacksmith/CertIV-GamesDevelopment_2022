using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StartCombat : MonoBehaviour
{
    [SerializeField] GameObject _combatCanvas;
    GameObject enemy;
    protected AIManager _aiManager;
    protected PlayerManager _playerManager;
    private bool onCooldown = false;

    //_buttonGroup.interactable = true;

    private void OnCollisionEnter2D(Collision2D collision_p)
    {
       //getting script component on our game object that is called "AIMovement"
        AIMovement aiMove = collision_p.collider.gameObject.GetComponent<AIMovement>();
        _playerManager = GameObject.Find("Combat Manager").GetComponent<PlayerManager>(); //setting our PlayerManager reference to the instance of the Player's PlayerManager
        string _objectName = collision_p.collider.gameObject.name;


        //if our script component is not null (this also means if what we collided with IS an AI/enemy basically)
        if (aiMove != null)
        {
            try
            {
                _aiManager = GameObject.Find("Combat Manager").GetComponent<AIManager>(); //setting our AIManager reference to the instance of the AI's AIManager
            }
            catch (MissingComponentException e) //prevents code from breaking if the error that happens is a missing component exception, which is likely from the above code.
            {
                Debug.Log($"{e} \nEy yo, we can't find your \"AIManager\" component bruv."); //display that error to the console
                return;
            }
            if (_aiManager == null)
            {
                string e = new NullReferenceException().ToString();
                Debug.Log($"Check if the name of the GameObject you're looking for is correct! \n{e}");
            }

            enemy = collision_p.collider.gameObject; //setting our GameObject "enemy" to the instance of the AI's GameObject
            Debug.Log("You have collided with an AI and are now entering combat.");
            _combatCanvas.SetActive(true); //the screen that appears for combat to happen is set to active
            Time.timeScale = 0; //pause
        }
        else if (_objectName != null && _objectName == "Health Station") //otherwise, if what we collided with is NOT an AI, the collided thing is not null, and its name is "Health Station"
        {
            switch (onCooldown)
            {
                case false:
                    {
                        //heal the player to full health
                        _playerManager.Refresh();
                        Debug.Log("You have fully healed!");
                        onCooldown = true;
                        //GeneralWait(10f);
                        break;
                    }
                    case true:
                    {
                        return;
                    }
            }
            
        }
        else //If its not an AI and its not a Health Station
        {
            //exit this OnCollisionEnter2D method
            return;
        }
    }

    public void EndCombat(bool isVictorious_p) //When all combatants have been eradicated
    {
        if (isVictorious_p == true)
        {
            Debug.Log("All combatants but one have been defeated! The game will now resume.");
            onCooldown = false; //we can heal again
            _combatCanvas.SetActive(false); //the screen that appears for combat to happen is set to inactive
            try
            {
                _aiManager.Refresh(); //calling to the AI GameObject's BaseManager script (since it is inherited by AIManager)
                Debug.Log("We have successfully Refreshed our AI health status for the next combat!");
                Destroy(enemy); //destroy the instance of the GameObject that we initially collided with
            }
            catch (System.Exception exception) //if Destroying the GameObject fails, throw an error rather than crashing the game
            {
                Debug.Log($"Well, I guess that ({exception}) didn't work.");  //Write the exception code to console
                return;
            }
            Time.timeScale = 1; //unpause
        }
        else
        {
            onCooldown = false; //we can heal again
            _combatCanvas.SetActive(false); //the screen that appears for combat to happen is set to inactive
            try
            {
                _aiManager.Refresh(); //calling to the AI GameObject's BaseManager script (since it is inherited by AIManager)
                Debug.Log("We have successfully Refreshed our AI's health status for the next combat. \n It's GAME OVER bruh.");

                //If we're in the Unity Editor
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode(); //exit out of the editor playmode because it's game over and we don't have a menu screen
                #endif
            }
            catch (System.Exception exception) //if Destroying the GameObject fails, throw an error rather than crashing the game
            {
                Debug.Log($"Well, I guess that ({exception}) didn't work.");  //Write the exception code to console
                return;
            }
            Time.timeScale = 1; //unpause
        }
        
    }

    //IEnumerator GeneralWait(float seconds_p)
    //{
    //    if (onCooldown == true) //if we're currently on a cool down
    //    {
    //        yield return new WaitForSecondsRealtime(seconds_p); //waiting in real time for the amount of seconds is specified when this method is called
    //        onCooldown = false; //once we finish the timer, reset our cooldown
    //        Debug.Log("Cooldown has reset.");
    //    }
    //    else //if we're not on a cooldown
    //    {
    //        yield return new WaitForSecondsRealtime(seconds_p); //just wait in real time the specified seconds
    //    }
    //}
}

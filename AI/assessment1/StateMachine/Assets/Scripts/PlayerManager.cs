using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerManager : BaseManager
{
    [SerializeField] protected CanvasGroup _buttonGroup;
    [SerializeField] protected Animator _anim;
    protected AIManager _aiManager;
    protected StartCombat _startCombat;

    protected override void Start()
    {
        base.Start();
        try //when you try so hard but you don't succeeeeeeeeed
        {
            _startCombat = GameObject.Find("Player").GetComponent<StartCombat>(); //tries to get the StartCombat component from any GameObject called "Player"
            _aiManager = GetComponent<AIManager>(); //tries to get the AIManager component from the AI
        }
        catch (MissingComponentException e) //prevents code from breaking if the error that happens is a missing component exception, which is likely from the above code.
        {
            //you don't get what you want just MissingComponentException eeeeeeeeee
            Debug.Log($"{e} \nEy yo, either we can't find your \"AIManager\" component or your \"StartCombat\" component bruv.");
            return;
        }
        if (_startCombat == null) //hello darkness my old frieeeennddd
        {
            string e = new NullReferenceException().ToString();
            Debug.Log($"Check if the name of the GameObject you're looking for is correct (no spaces bruh). \n{e}"); //you don't even wanna know 
            return;
        }
        else if (_aiManager == null)
        {
            string e = new NullReferenceException().ToString();
            Debug.Log($"Check if the AIManager exists? \n{e}");
            return;
        }
    }

    #region Turns
    public override void TakeTurn()
    {
        if (_health <= 0) //if our health is less than or equal to 0
        {
            Debug.Log("Okay, you're dead in Player Manager");
            Timer(1.5f);
            _startCombat.EndCombat(false); //accessing this game object's (player's) StartCombat script component and calling its EndCombat method and telling it we weren't victorious
            Debug.Log("We successfully disabled the combat canvas and quit the game.");
            _buttonGroup.interactable = true; //if we don't re-enable when combat ends, the next combat won't start interactable, and it's much more of a headache and resource-heavy to enable it in another script
        }
        switch (_aiManager.currentState)
        {
            case AIManager.State.Dead:
                {
                    Debug.Log("Okay, AI is dead in Player Manager");
                    Timer(1.5f);
                    _startCombat.EndCombat(true); //accessing this game object's (player's) StartCombat script component and calling its EndCombat method and telling it we were victorious
                    Debug.Log("We successfully Destroyed the enemy and resumed time!");
                    _buttonGroup.interactable = true; //if we don't re-enable when combat ends, the next combat won't start interactable, and it's much more of a headache and resource-heavy to enable it in another script
                    break;
                }
            default:
                _buttonGroup.interactable = true; //if we're not ending combat it is safe to continue taking our turn
                break;
        }
    }
    /// <summary>
    /// Checks if the player is dead and calls <see cref="StartCombat.EndCombat"/>, re-enables player controls for the next combat, and resumes time.
    /// </summary>
    IEnumerator EndTurn()
    {
        if (_health <= 0) //if our health is less than or equal to 0
        {
            Debug.Log("Okay, you're dead in Player Manager");
            Timer(1.5f);
            _startCombat.EndCombat(false); //accessing this game object's (player's) StartCombat script component and calling its EndCombat method and telling it we weren't victorious
            Debug.Log("We successfully disabled the combat canvas and quit the game.");
            _buttonGroup.interactable = true; //if we don't re-enable when combat ends, the next combat won't start interactable, and it's much more of a headache and resource-heavy to enable it in another script
        }
        Debug.Log("You have ended your turn.");
        _buttonGroup.interactable = false;
        yield return new WaitForSecondsRealtime(1.5f);
        _aiManager.TakeTurn();
    }
    #endregion

    #region Timers
    /// <summary>
    /// A simple timer that waits in real time for the seconds provided as an argument.
    /// </summary>
    private IEnumerator Timer(float seconds_p)  //This is basically our timer coroutine
    {
        yield return new WaitForSecondsRealtime(seconds_p);
    }
    /// <summary>
    /// An overload timer method that waits in relation to framed seconds.
    /// Second argument heals the Player for the amount provided as an argument if the last argument is false.
    /// Third argument true deals damage to the AI for the amount in the second argument.
    /// </summary>
    private IEnumerator Timer(float seconds_p, float amount_p, bool attackingAI_p)
    {
        switch (attackingAI_p)
        {
            case false: //if we're not using it to do DOT to the AI
                {
                    for (int i = 0; i < seconds_p; i++) //this is just so we can track the seconds passed in Console
                    {
                        Heal(amount_p);
                        Debug.Log($"Seconds passed since timer for heal was called: {i}");
                        yield return new WaitForSeconds(seconds_p);
                    }
                    break;
                }
            case true: //if we are using it for DOT to the AI
                {
                    for (int i = 0; i < seconds_p; i++)
                    {
                        _aiManager.DealDamage(amount_p);
                        Debug.Log($"Seconds passed since timer for AI DOT was called: {i}");
                        yield return new WaitForSeconds(seconds_p);
                    }
                    break;
                }
        }
    }
    #endregion

    #region Moves
    public void EatBerries()
    {
        Debug.Log("You used EatBerry.");
        StartCoroutine(Timer(3f, 5f, false));
        StartCoroutine(EndTurn());
    }
    public void DestructoBerry()
    {
        _anim.SetTrigger("BerryBomb");
        Debug.Log("You used DestructoBerry.");
        _aiManager.DealDamage(80f);
        DealDamage(_maxHealth);

        StartCoroutine(EndTurn());
    }
    public void BerryBomb()
    {
        _anim.SetTrigger("BerryBomb");
        Debug.Log("You used BerryBomb.");
        _aiManager.DealDamage(30f);
        StartCoroutine(EndTurn());
    }
    public void PoisonBerry()
    {
        _anim.SetTrigger("BerryBomb");
        Debug.Log("You used PoisonBerry.");
        StartCoroutine(Timer(5f, 8f, true));
        StartCoroutine(EndTurn());
    }
    #endregion
}

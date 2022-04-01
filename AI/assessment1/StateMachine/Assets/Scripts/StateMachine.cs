using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{
    public enum State
    {
        attack,
        defend,
        flee,
        patrol
    }
    public State currentState;
    public AIMovement aiMovement;
    public float chaseDist, safeDist;
    private GameObject[] _enemiesArr;
    [SerializeField] private GameObject _displayTextPrefab;
    private GameObject tempPrefab;
    public bool _qeuedForDestruction = false;


    private void Start()
    {
        aiMovement = GetComponent<AIMovement>();
        NextState();
    }

    public void Update()
    {
        if (_qeuedForDestruction)
        {
            Destroy(tempPrefab, 1f);
        }
    }

    private void NextState()
    {
        switch (currentState)
        {
            case State.attack:
                StartCoroutine(AttackState());
                break;
            case State.defend:
                StartCoroutine(DefendState());
                break;
            case State.flee:
                StartCoroutine(FleeState());
                break;
            case State.patrol:
                StartCoroutine(PatrolState());
                break;
        }
    }

    #region State IEnumerators
    private IEnumerator AttackState()
    {
        DisplayState("Attacking!"); //when an agent enters the attack state it will display "Attacking!" above the agent
        //Debug.Log("Attack State: Enter");

        while (currentState == State.attack)
        {
            aiMovement.AIMove(aiMovement.player);
            //checking if the player is within the chase distance
            bool _playerWithinReach = Vector2.Distance(transform.position, aiMovement.player.position) <= chaseDist;

            //if the number of enemies becomes less than or equal 1 (ourself)
            if (_enemiesArr.Length <= 1)
            {
                //go to patroling
                currentState = State.patrol;

            }
            if (!_playerWithinReach)
            {
                currentState = State.patrol;
            }
            yield return null;
        }
        aiMovement.ChaseEnd();
        Debug.Log("Attack State: Exit");
        NextState();
    }

    private IEnumerator DefendState()
    {
        DisplayState("Defending"); //when an agent enters the attack state it will display "Denfending" above the agent
        //Debug.Log("Defend State: Enter");
        while (currentState == State.defend)
        {
            Debug.Log("Defending");
            yield return null;
        }
        Debug.Log("Defend State: Exit");
        NextState();
    }

    private IEnumerator FleeState()
    {
        DisplayState("AAAAhhhh! Fleeing!"); //when an agent enters the attack state it will display.. well, you know, above the agent.
        //Debug.Log("Flee State: Enter");
        while (currentState == State.flee)
        {
            //TODO:make AI move away from Player somehow

            bool _withinSafeDist = Vector2.Distance(transform.position, aiMovement.player.position) >= safeDist;
            bool _playerWithinReach = Vector2.Distance(transform.position, aiMovement.player.position) <= chaseDist;
            //if we're fleeing and the number of enemies becomes greater than 1 (ourself)
            if (_enemiesArr.Length > 1)
            {
                switch (_playerWithinReach)
                {
                    //if not
                    case false:
                        {
                            //go to patroling
                            currentState = State.patrol;
                            break;
                        }
                    //if yes
                    case true:
                        {
                            //go to attacking the player
                            currentState = State.attack;
                            break;
                        }
                }
            }
            else if (_withinSafeDist) //if AI is safe distance away from player
            {
                //go to patroling
                currentState = State.patrol;
            }
            yield return null;
        }
        Debug.Log("Flee State: Exit");
        NextState();
    }

    private IEnumerator PatrolState()
    {
        DisplayState("Searching for berries.."); //theoretically, when an agent enters the attack state it will display "Denfending" above the agent
        //Debug.Log("Berry State: Enter");

        while (currentState == State.patrol)
        {
            aiMovement.AIMove(aiMovement.waypoints[aiMovement.waypointIndex]);
            aiMovement.WaypointUpdate();
            _enemiesArr = GameObject.FindGameObjectsWithTag("enemy"); //get all the GameObjects within the scene that are tagged with "enemy"
            bool _playerWithinReach = Vector2.Distance(transform.position, aiMovement.player.position) <= chaseDist;
            bool _withinSafeDist = Vector2.Distance(transform.position, aiMovement.player.position) >= safeDist;
            if (_enemiesArr.Length <= 1 && !_withinSafeDist)
            {
                currentState = State.flee;
            }
            else if (_playerWithinReach)
            {
                currentState = State.attack;
            }            
            yield return null;
        }
        Debug.Log("Berry State: Exit");
        NextState();
    }
    #endregion

    /// <summary>
    /// Instantiates a prefab and accesses its text component to display different text originating from the parent GameObject of the script.
    /// </summary>
    /// <param name="text_p"></param>
    private void DisplayState(string text_p)
    {
        if (_displayTextPrefab && !tempPrefab) //if our display text prefab variable isn't null, and the temp prefab variable IS null
        {
            tempPrefab = Instantiate(_displayTextPrefab, transform.position, Quaternion.identity); //instantiating the prefab at 0,0,0 on the parent's transform and storing the prefab
            tempPrefab.transform.parent = gameObject.transform; //making the instantiated prefab a child to the object that created them (the AI)
            tempPrefab.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = text_p; //getting the instantiated GameObject children's text component so we can assign its text to display what we want
        }
        else if (_displayTextPrefab && tempPrefab) //if our display text prefab and the temp prefab variables aren't null (meaning we have already instantiated the display text prefab)
        {
            tempPrefab.SetActive(false);
            tempPrefab.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = text_p; //getting the instantiated GameObject children's text component so we can assign its text to display what we want
            tempPrefab.transform.position = gameObject.transform.position;
            tempPrefab.SetActive(true);
        }
    }
}
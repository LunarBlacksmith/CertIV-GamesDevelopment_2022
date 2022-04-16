using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /* TODO: 
        * Clear generated word before displaying one from WordFactory
        * Turn off alphabet panel in editor when running the game
        * Change End Game Menu title depending on win or lose state
        * Change graphics depending on guesses left
        * Have a BodyCondition enum state for Audio later on

    */

    #region Private Variables
    // list of alphabet characters
    private string sInput = "";
    private string _wordGuessing = "";
    private char[] _alphabetArray
        = new char[26]
        {
            'a','b','c','d',
            'e','f','g','h',
            'i','j','k','l',
            'm','n','o','p',
            'q','r','s','t',
            'u','v','w','x','y','z'
        };
    private char currLetter = ' ';
    #endregion

    // This section is for objects on which the other class scripts are attached to allow for use of their fields and behaviours.
    [Header("Script Objects")]
    public WordFactory wordFactory;     // handles the word generation and manipulation in the game
    public SoundManager soundManager;   // handles the audio in the game

    // This section is for objects that are involved in game play, such as the player input field, hangman graphics, and alphabet.
    [Header("Gameplay Objects")]
    public InputField playerInputField; // text box that gets player keystrokes (set to alphanumeric only)
    public GameObject alphabetPanel;    // alphabet letters holder that displays guessed letters
    public Text underscoredWord;        // text object that is updated through gameplay and holds the missing word for the player
    public Text guessesLeftNumberText;  // text that holds the number of guesses left for the player
    public Text endGameTitle;           // text that displays the win/lose message to the player on the End Game Menu
    public Image gallowsImage;          // holds the image that displays the gallows and will change through gameplay
    public Image hangmanImage;          // holds the image that displays the man and will change through gameplay
    

    void Start()
    {
        // this needs to be within the same method event that the player presses enter
        currLetter = sInput[0];
    }

    
    void Update()
    {
        // needs to be on Enter button in InputField:
            // check if character has been used and clear InputField if it has

            // clear our existing list local to Game Manager

            // set our list to equal the list this returns (indices the character is at)
        // ourLocalList = wordFactory.GetIndicesInWord(playerInputField.text, wordFactory.GeneratedWord);
            // loop through our generated word string and change the letters at each index in our list to the letter
            // this enables the letter object at the same index of the character in the alphbet 
        EnableLetter(alphabetPanel, GetAlphabetIndex(currLetter));
    }

    // enables a Text object at an index in a Text array 
    public void EnableLetter(GameObject alphabetObject_p, int index_p)
    {
        alphabetObject_p.GetComponentsInChildren<Text>(true)[index_p].enabled = true;
    }

    // returns the index of the letter in the alphabet
    public int GetAlphabetIndex(char letter_p)
    {
        int index = 0;
        for (int i = 0; i < _alphabetArray.Length; i++)
        {
            if (_alphabetArray[i] == currLetter)
            { index = i; }
        }
        return index;
    }

}

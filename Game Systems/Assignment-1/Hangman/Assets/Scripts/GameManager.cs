using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /* TODO:
        * Need to make comparisons change characters to lower case otherwise logic will fail
        * InputField not working for input - Change to regular text field for now
        * Change graphics depending on guesses left
        * Have a BodyCondition enum state for Audio later on
    */

    #region Private Variables
    
    private string sInput = "";         // holds the player's input text
    public string _wordGuessing = "";  // holds the word generated by WordFactory
    private char[] _alphabetArray       // array of alphabet characters
        = new char[26]
        {
            'a','b','c','d',
            'e','f','g','h',
            'i','j','k','l',
            'm','n','o','p',
            'q','r','s','t',
            'u','v','w','x','y','z'
        };
    private char _currLetter = ' ';     // used to get the character in the player's input
    private int _characterLimit = 1;    // used to assign the character limit in the player input field
    private int _wrongGuessesLeft = 6;  // controls how many guesses the player can get wrong before losing
    private bool _haveReset = true;     // to control times the reset code block functions
    private enum GameState              // enum to define the different states of the game
    { 
        Playing,
        PreGame,
        Paused,
        Won, 
        Lost 
    }

    // state to control method execution that control the game flow and variable setting
    // set to PreGame state at the start of the game launch
    private GameState _gameState = GameState.PreGame;

    // used to store the list of indices the letter is at in the word
    private List<int> _letterPositionIndices = new List<int>();
    #endregion

    // This section is for objects on which the other class scripts are attached to allow for use of their fields and behaviours.
    [Header("Script Objects")]
    public WordFactory wordFactory;     // handles the word generation and manipulation in the game
    public SoundManager soundManager;   // handles the audio in the game

    // This section is for objects that are involved in game play, such as the player input field, hangman graphics, and alphabet.
    [Header("Gameplay Objects")]
    public InputField playerInputField; // text box that gets player keystrokes (set to alphanumeric only)
    public GameObject gamePanel;        // the main panel in which the game takes place
    public GameObject alphabetPanel;    // alphabet letters holder that displays guessed letters
    public GameObject pauseMenuPanel;   // panel for the pause menu
    public GameObject mainMenuPanel;    // panel for the main menu
    public GameObject endGameMenuPanel; // panel for the end game menu
    public GameObject creditsPanel;     // panel for the credits screen
    public Text underscoredWord;        // text object that is updated through gameplay and holds the missing word for the player
    public Text guessesLeftNumberText;  // text that holds the number of guesses left for the player
    public Text endGameTitle;           // text that displays the win/lose message to the player on the End Game Menu
    public Image gallowsImage;          // holds the image that displays the gallows and will change through gameplay
    public Image hangmanImage;          // holds the image that displays the man and will change through gameplay
    public Sprite[] gallowsSprites;     // holds sprites the gallowsImage will be set to at different stages of the game
    public Sprite[] hangmanSprites;     // holds sprites the hangmanImage will be set to at different stages of the game
    public bool easyMode = false, mediumMode = false, hardMode = false; // used to control the difficulty of the word generated
    

    void Start()
    {
        // setting our gallows graphic to the 'Empty' version
        gallowsImage.sprite = gallowsSprites[0];

        // setting our hangman graphic to the 'full body of hangman' version
        hangmanImage.sprite = hangmanSprites[0];
    }

    
    void Update()
    {
        // checking the current state of the game to run code specific to that state
        switch (_gameState)
        {
            case GameState.Playing:
                {
                    // if the player's input field doesn't have focus
                    if (!playerInputField.isFocused)
                    {
                        playerInputField.Select();              // set it to have focus
                        playerInputField.ActivateInputField();  // activate it
                    }

                    // setting to false for next time game state = PreGame
                    _haveReset = false;

                    // label to jump to if the player enters a word they've guessed before
                    //Skip:

                    // if the player has no more incorrect guesses left
                    if (_wrongGuessesLeft == 0)
                    { _gameState = GameState.Lost; }    // tell the game they Lost

                    // if player presses Escape key
                    if (Input.GetKeyDown(KeyCode.Escape))
                    { _gameState = GameState.Paused; }  // tell the game we're in the paused state

                    guessesLeftNumberText.text = _wrongGuessesLeft.ToString();

                    break;
                }
            case GameState.PreGame:
                {
                    if (!_haveReset)
                    {
                        mainMenuPanel.SetActive(true);  // activate the main menu panel
                        gamePanel.SetActive(false);     // deactivate the game
                        ResetGraphicsAndVariables();    // reset main graphics and variables
                    }
                    break; 
                }
            case GameState.Paused:
                {
                    _haveReset = false;
                    if (!pauseMenuPanel.active)         // if Pause Menu isn't active already
                    { pauseMenuPanel.SetActive(true); } // enable Pause Menu 
                    break;
                }
            case GameState.Won:
                {
                    _haveReset = false;
                    endGameTitle.text = "You Won!";         // set title on end screen to show game result to player
                    if (!endGameMenuPanel.active)           // if End Game Menu isn't active already
                    { endGameMenuPanel.SetActive(true); }   // enable End Game Menu

                    if (gamePanel.active)                   // if the main game panel is active
                    { 
                        gamePanel.SetActive(false); // disable the game panel
                        _wrongGuessesLeft = 6;      // reset number of incorrect guesses the player has left

                        // disable all alphabet letters
                        foreach (Text letterObject in alphabetPanel.GetComponents<Text>())
                        { letterObject.enabled = false; }

                        // setting our gallows graphic to the 'Empty' version
                        gallowsImage.sprite = gallowsSprites[0];

                        // setting our hangman graphic to the 'full body of hangman' version
                        hangmanImage.sprite = hangmanSprites[0];
                        sInput = string.Empty;                      // reset the stored input
                        playerInputField.text = string.Empty;       // reset the player InputField's text
                        _wordGuessing = string.Empty;               // reset the word being guessed
                        underscoredWord.text = string.Empty;        // reset the underscored word the player can see
                    }
                    break; 
                }
            case GameState.Lost:
                {
                    _haveReset = false;
                    endGameTitle.text = "You Lost";         // set title on end screen to show game result to player
                    if (!endGameMenuPanel.active)           // if End Game Menu isn't active already
                    { endGameMenuPanel.SetActive(true); }   // enable End Game Menu

                    if (gamePanel.active)                   // if the main game panel is active
                    {
                        gamePanel.SetActive(false); // disable the game panel
                        _wrongGuessesLeft = 6;      // reset number of incorrect guesses the player has left

                        // disable all alphabet letters
                        foreach (Text letterObject in alphabetPanel.GetComponents<Text>())
                        { letterObject.enabled = false; }

                        // setting our gallows graphic to the 'Empty' version
                        gallowsImage.sprite = gallowsSprites[0];

                        // setting our hangman graphic to the 'full body of hangman' version
                        hangmanImage.sprite = hangmanSprites[0];
                        sInput = string.Empty;                      // reset the stored input
                        playerInputField.text = string.Empty;       // reset the player InputField's text
                        underscoredWord.text = string.Empty;        // reset the underscored word the player can see
                    }
                    break; 
                }
            default:
                {
                    // something is wrong if this code executes, so reset everything by sending them
                    // back to the Main Menu:

                    if (!_haveReset)
                    {
                        _haveReset = true;
                        _gameState = GameState.PreGame; // setting the state back to pregame
                    }
                    break; 
                }
        }
        
    }

    public void OnInputEnter()
    {
        // if we're still playing the game and not in a menu
        if (_gameState == GameState.Playing)
        {
            // assigning our input to the text the player enters without the whitespaces
            sInput = wordFactory.RemoveWhiteSpaces(playerInputField.text);

            // if the player entered 1 character
            if (sInput.Length == 1)
            {
                // get the first character from their input without whitespaces
                _currLetter = wordFactory.RemoveWhiteSpaces(sInput)[0];

                // new Text array same size as number of enabled alphabet objects
                Text[] enabledLetters = new Text[alphabetPanel.GetComponentsInChildren<Text>(false).Length];

                // store all the enabled alphabet letters (letters used before)
                enabledLetters = alphabetPanel.GetComponentsInChildren<Text>(false);

                // loop through our enabled letters
                for (int i = 0; i < enabledLetters.Length; i++)
                {
                    // if we find the current letter guessed in the already-enabled array
                    if (enabledLetters[i].text.ToString() == _currLetter.ToString())
                    { return; }  // exit the method immediately
                }

                // if the letter is not in the word the player is guessing (they guessed wrong)
                if (!wordFactory.EvaluateCharacter(_currLetter, _wordGuessing))
                {
                    _wrongGuessesLeft--;    // decrement wrong guesses left

                    // this enables the letter object at the same index of the character in the alphbet
                    // to show they have guessed the current letter
                    EnableLetter(alphabetPanel, GetAlphabetIndex(_currLetter));
                }
                else // if they didn't guess the letter wrong
                {
                    // since we know the letter exists in the word already, we can clear our previous indices
                    _letterPositionIndices.Clear();

                    //  and get the new index(s) position(s) of it in the word
                    _letterPositionIndices = wordFactory.GetIndicesInWord(_currLetter, _wordGuessing);

                    // assign new string to word with underscores at index positions replaced with letter
                    string tempUnderscoreWord = ReplaceUnderscoreWithLetter(_wordGuessing, _currLetter);

                    // assign created string to itself with spaces between each letter
                    tempUnderscoreWord = AddSpacesBetweenLetters(tempUnderscoreWord);

                    // set our original text the player will see to the formatted word that includes spaces
                    // and previous underscores, aside from where our guessed letter was
                    underscoredWord.text = tempUnderscoreWord;

                    // enable the letter object in the alphabet panel at the same index of the character in the alphbet
                    EnableLetter(alphabetPanel, GetAlphabetIndex(_currLetter));
                }
            }
            else // otherwise if they entered more than 1 character
            {
                // if the word they entered is the correct word
                if (wordFactory.EvaluateWord(sInput, _wordGuessing))
                { _gameState = GameState.Won; } // tell the game the player Won
                else
                { _wrongGuessesLeft--; }   // if incorrect guess, decrement the number of wrong guesses left
            }

            // check if underscored word (without spaces) equals the generated word (without spaces)
            // if it does, set game state to 'Won'
            if (underscoredWord.text == _wordGuessing)
            { _gameState = GameState.Won; } // tell the game the player Won
        }
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);    // deactivate the pause menu
        _gameState = GameState.Playing;     // tell the game we're playing the game
    }

    public void NewGame()
    {
#if UNITY_EDITOR
        if (easyMode && !mediumMode && !hardMode)
        { _wordGuessing = wordFactory.GenerateWord("easy"); }
        if (mediumMode && !easyMode && !hardMode)
        { _wordGuessing = wordFactory.GenerateWord("moderate"); }
        if (hardMode && !easyMode && !mediumMode)
        { _wordGuessing = wordFactory.GenerateWord("hard"); }
        else
        { _wordGuessing = wordFactory.GenerateWord(); } // set our local word variable to a newly generated word

        // if it's empty here, we ran out of words to generate or something went wrong in the factory
        if (_wordGuessing == string.Empty)
        {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #endif
            Application.Quit();
        }

        // disable all alphabet letters
        foreach (Text letterObject in alphabetPanel.GetComponentsInChildren<Text>())
        { letterObject.enabled = false; }

        string unityGeneratedUnderscores = "";
        Debug.Log(wordFactory.LetterCount);

        // for every letter in the word, replace it with an underscore and assign our string to the value
        for (int i = 0; i < wordFactory.LetterCount; i++)
        { unityGeneratedUnderscores += "_"; }

        // put whitespace between every letter for presentation
        // and assign our visable text field to the returned string value
        underscoredWord.text = AddSpacesBetweenLetters(unityGeneratedUnderscores);

        Debug.Log(unityGeneratedUnderscores);

        _characterLimit = wordFactory.LetterCount;          // setting the max number of characters player can input (to assist)

        gamePanel.SetActive(true);          // activate our game
        endGameMenuPanel.SetActive(false);  // deactivate the end game panel
        mainMenuPanel.SetActive(false);     // deactivate the main menu panel
        _gameState = GameState.Playing;     // set our game state to play
        return;
#endif
        // set our local word variable to a newly generated word
        _wordGuessing = wordFactory.GenerateWord();

        // if it's empty here, we ran out of words to generate or something went wrong in the factory
        if (_wordGuessing == string.Empty)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }

        // disable all alphabet letters
        foreach (Text letterObject in alphabetPanel.GetComponents<Text>())
        { letterObject.enabled = false; }

        string generatedUnderscores = "";

        // for every letter in the word, replace it with an underscore and assign our string to the value
        for (int i = 0; i < wordFactory.LetterCount; i++)
        { generatedUnderscores += "_"; }

        // put whitespace between every letter for presentation
        // and assign our visable text field to the returned string value
        underscoredWord.text = AddSpacesBetweenLetters(generatedUnderscores);    

        gamePanel.SetActive(true);          // activate our game
        endGameMenuPanel.SetActive(false);  // deactivate the end game panel
        mainMenuPanel.SetActive(false);     // deactivate the main menu panel
        _gameState = GameState.Playing;     // set our game state to play
    }

    public void Retry()
    {
        _wordGuessing = wordFactory.GeneratedWord;
        string generatedUnderscores = "";

        // for every letter in the word, replace it with an underscore and assign our string to the value
        for (int i = 0; i < wordFactory.LetterCount; i++)
        { generatedUnderscores += "_"; }

        // put whitespace between every letter for presentation
        // and assign our visable text field to the returned string value
        underscoredWord.text = AddSpacesBetweenLetters(generatedUnderscores);

        gamePanel.SetActive(true);          // activate our game
        endGameMenuPanel.SetActive(false);  // deactivate the end game panel
        _gameState = GameState.Playing;     // set our game state to play
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
    #endif
        Application.Quit();
    }

    public void MainMenu()
    {
        mainMenuPanel.SetActive(true);      // activate the main menu panel
        gamePanel.SetActive(false);         // deactivate our game
        endGameMenuPanel.SetActive(false);  // deactivate the end game panel
        creditsPanel.SetActive(false);      // deactivate credits panel
        _gameState = GameState.PreGame;     // set our game state to pregame
    }  

    public void CreditsScreen()
    { creditsPanel.SetActive(true); }   // activate the credits panel

    // enables a Text object at an index in a Text array 
    public void EnableLetter(GameObject alphabetObject_p, int index_p)
    { alphabetObject_p.GetComponentsInChildren<Text>(true)[index_p].enabled = true; }

    // returns the index of the letter in the alphabet
    public int GetAlphabetIndex(char letter_p)
    {
        int index = 0;
        for (int i = 0; i < _alphabetArray.Length; i++)
        {
            if (_alphabetArray[i] == _currLetter)
            { index = i; }
        }
        return index;
    }

    /// <summary>
    /// Replaces any index the character is at in the word with the letter itself and removes the underscore.
    /// </summary>
    /// <param name="word_p"></param>
    /// <param name="letter_p"></param>
    /// <returns></returns>
    public string ReplaceUnderscoreWithLetter(string word_p, char letter_p)
    {
        word_p = wordFactory.RemoveWhiteSpaces(underscoredWord.text);
        string halfWord1 = "", halfWord2 = "";
        Debug.Log("Number of times letter is in word: " + _letterPositionIndices.Count);
        foreach (int indexOfLetter in _letterPositionIndices)
        {
            if (indexOfLetter == 0)        // if the index is the first letter
            {
                halfWord2 = word_p.Substring(1);// get the word from after the letter to replace
                word_p = "";                    // set the word to return to nothing
                word_p += letter_p;             // add the letter to replace to the word to return
                word_p += halfWord2;            // append the rest of the word to the replaced letter, to the word returning
            }
            else if (indexOfLetter == _letterPositionIndices.Count-1)   // if the index is the last letter
            {
                halfWord1 = word_p.Substring(0, indexOfLetter);         // get word before letter position

                // word to be returned is the original word minus the character to be replaced
                // and append the letter we're placing on the end as the last letter in the word
                word_p = halfWord1 + letter_p;
            }
            else
            {
                // split the word just before the index of the letter position
                halfWord1 = word_p.Substring(0, indexOfLetter); // half of word before letter position
                halfWord2 = word_p.Substring(indexOfLetter + 1);// half of word after letter position
                halfWord1 += letter_p;          // add letter to end of first half of the word
                word_p = halfWord1 + halfWord2; // join word back together, now including letter placed
            }
        }
        return word_p;  // return the word with its replaced letter(s)
    }

    /// <summary>
    /// Returns the string passed with whitespace between each letter.
    /// </summary>
    /// <param name="word_p"></param>
    /// <returns></returns>
    public string AddSpacesBetweenLetters(string word_p)
    {
        // create a new char array
        char[] wordLetters = new char[word_p.Length];

        // get all the characters of our underscored word into a char array
        wordLetters = word_p.ToCharArray();
        word_p = "";    // empty the temporary underscored word

        // loop as many times as we have letters in the word
        for (int i = 0; i < wordLetters.Length; i++)
        {
            if (i == wordLetters.Length)    // if we're at the last letter index
            { word_p += wordLetters[i]; }   // don't add a space
            word_p += wordLetters[i] + " "; // else, add the character and a space to the word
        }
        return word_p;
    }

    /// <summary>
    /// Resets main variables and graphics in the game. Does not reset local word to guess variable.
    /// </summary>
    public void ResetGraphicsAndVariables()
    {
        // setting our gallows graphic to the 'Empty' version
        gallowsImage.sprite = gallowsSprites[0];

        // setting our hangman graphic to the 'full body of hangman' version
        hangmanImage.sprite = hangmanSprites[0];
        _letterPositionIndices.Clear(); // reset our indices list
        _currLetter = ' ';              // reset the current letter
        sInput = string.Empty;          // reset the recorded player input
        _characterLimit = 1;            // reset character limit
        _wrongGuessesLeft = 6;          // reset number of incorrect guesses the player has left
        _haveReset = true;
    }
}

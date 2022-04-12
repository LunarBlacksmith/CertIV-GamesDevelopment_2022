using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordFactory : MonoBehaviour
{
    #region Private Variables
    private string _generatedWord = ""; //used to store the word generated for the current level
    //used to convert generated word to a character array for individual character manipulation
    private char[] _genWordCharArray = new char[0]; 
    //permanent difficulty keys for accessing different words in our dictionary of stored lists.
    private const string _diffKeyEasy = "easy", _diffKeyModerate = "moderate", _diffKeyHard = "hard";
    private int _letterCount = 0; //used to count the amount of letters in the generated word
    //instantiating System object "Random" for random number generation
    //have to distinguish between System and UnityEngine Random()
    private System.Random _randomObj = new System.Random();
    private bool _convertedToWord = false; //used to control when any generated word is converted to the _genWordCharArray
    
        #region Lists & Dictionaries
    //used to store the permanent list of words in the game.
    //each list stores different difficulty words (shorter or longer, or less or more complex)
    [SerializeField] private List<string> _easyWords;
    [SerializeField] private List<string> _moderateWords;
    [SerializeField] private List<string> _hardWords;
    //used to store the words we've already used.
    private Dictionary<int, string> _wordsUsed = new Dictionary<int,string>();
    //used to store separate lists of words that are categorised by difficulty setting, which is determined by the Dictionary's key (i.e: "easy", "moderate", "hard")
    //having this Dictionary helps to decouple the code.
    private Dictionary<string, List<string>> _wordListDictionary = new Dictionary<string, List<string>>();
        #endregion

    #endregion

    #region Properties
    public string GeneratedWord { get; private set; } //public property for accessing its variable outside the class
    public char[] GenWordCharArray { get; private set; } //public property for accessing the array outside the class
    public int LetterCount //property for lettercount to allow verification on setting of letter count
    {
        get { return _letterCount; }
        private set { _letterCount = (value >= 0) ? value : _letterCount; } //set lettercount to value if its greater than or equal to 0, otherwise set to self
    }
    #endregion

    void Start()
    {
        // loop through each string list in the dictionary
        // and sort the lists of words by length, from shortest to longest using the System's CompareTo() method
        foreach (List<string> list in _wordListDictionary.Values)
        { list.Sort((a, b) => a.Length.CompareTo(b.Length)); }

        //adding the lists of differing difficulty words to our dictionary of lists,
        //each list is assigned to the keys declared as variables, for modularity. (i.e: diffKeyEasy = "easy")
        _wordListDictionary.Add(_diffKeyEasy, _easyWords);
        _wordListDictionary.Add(_diffKeyModerate, _moderateWords);
        _wordListDictionary.Add(_diffKeyHard, _hardWords);

    }
    void Update()
    {
        #region Char Array Manipulation
        //if GeneratedWord isn't empty AND we haven't converted our word to a char array yet, OR the word isn null 
        if ((GeneratedWord != string.Empty && !_convertedToWord) || GeneratedWord != null)
        {   //check if we have previously assigned value to our char array
            if (GenWordCharArray.Length > 0) 
            { Array.Clear(GenWordCharArray, 0, GeneratedWord.Length); } //if so, clear its values
            GenWordCharArray = GeneratedWord.ToCharArray(); //convert our word to a char array
            _convertedToWord = (GenWordCharArray.Length > 0) ? true : false; //we have converted our word if our char array isn't empty
        }
        #endregion
    }



    //All Evaluation methods will finish as soon as the first match is found, returning true
    #region Evaluations

    /// <summary>
    /// Compares one string to another, both with and without whitespaces, and returns true if either condition is met.
    /// </summary>
    /// <param name="word1_p"></param>
    /// <param name="word2_p"></param>
    /// <returns>True or False</returns>
    public bool EvaluateWord(string word1_p, string word2_p)
    { return (word1_p == word2_p) || (RemoveWhiteSpaces(word1_p) == RemoveWhiteSpaces(word2_p)); }

    /// <summary>
    /// Compares a character, passed as a string in the first arg, to all characters within the second string. Returns true on first match found.
    /// </summary>
    /// <param name="word1_p"></param>
    /// <param name="word2_p"></param>
    /// <returns>True or False</returns>
    public bool EvaluateCharacter(string word1_p, string word2_p)
    {
        // creating a temporary char so we only call this method once, not every loop
        // removing all whitespace from the first word and then assigning its first character in the string to our tempChar
        char tempChar = RemoveWhiteSpaces(word1_p)[0];
        // loop through the second string as individual characters
        foreach (char character in word2_p.ToCharArray())
        // return true if the first character in the first word (without any whitespace)
        // is the same as the current character in the second string
        { if (tempChar == character) { return true; } } 

        //else if we have never returned true, aka: character doesn't exist in the second string
        return false;
    }

    /// <summary>
    /// Compares a character in the first arg, to all characters within the second string. Returns true on first match found.
    /// </summary>
    /// <param name="character_p"></param>
    /// <param name="word_p"></param>
    /// <returns>True or False</returns>
    public bool EvaluateCharacter(char character_p, string word_p)
    {
        foreach (char character in word_p.ToCharArray())
        { if (character_p == character) { return true; } } //return true if the character param is in the string param

        //else if we have never returned true, aka: character doesn't exist in the string
        return false;
    }

    /// <summary>
    /// Compares a character, passed as a string in the first arg, to all characters within the character array. Returns true on first match found.
    /// </summary>
    /// <param name="word_p"></param>
    /// <param name="charArray_p"></param>
    /// <returns>True or False</returns>
    public bool EvaluateCharacter(string word_p, char[] charArray_p)
    {
        // creating a temporary char so we only call this method once, not every loop
        // removing all whitespace from the first word and then assigning its first character in the string to our tempChar
        char tempChar = RemoveWhiteSpaces(word_p)[0];
        foreach (char character in charArray_p)
        // return true if the first character in the word (without any whitespace)
        // is the same as the current character in the character array
        { if (tempChar == character) { return true; } } 

        //else if we have never returned true, aka: character doesn't exist in the string
        return false;
    }
    #endregion

    // All Compare methods indlude whitespace as a valid character
    #region Length Comparisons

    /// <summary>
    /// Compares the length of the first character array to the second and returns true if they are equal.
    /// </summary>
    /// <param name="charArray1_p"></param>
    /// <param name="charArray2_p"></param>
    /// <returns>True or False</returns>
    public bool CompareLength(char[] charArray1_p, char[] charArray2_p)
    { return (charArray1_p.Length == charArray2_p.Length); }

    /// <summary>
    /// Compares the length of the first character array to the second and returns true if they are equal.
    /// </summary>
    /// <param name="word_p"></param>
    /// <param name="charArray_p"></param>
    /// <returns>True or False</returns>
    public bool CompareLength(string word_p, char[] charArray_p)
    { return (word_p.Length == charArray_p.Length); }

    /// <summary>
    /// Compares the length of the first character array to the second and returns true if they are equal.
    /// </summary>
    /// <param name="word1_p"></param>
    /// <param name="word2_p"></param>
    /// <returns>True or False</returns>
    public bool CompareLength(string word1_p, string word2_p)
    { return (word1_p.Length == word2_p.Length); }

    #endregion

    #region Word Generations
    /// <summary>
    /// Gets a random word from the entire collection of words available, regardless of difficulty.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException">
    /// </exception><exception cref="Exception"></exception> 
    public string GenerateWord()
    {
        try
        {
            //instantiating a temporary list of strings
            List<string> tempList = new List<string>();
            //looping through our dictionary Values, which are lists of type string
            foreach (List<string> list in _wordListDictionary.Values)
            {
                //for each list in our dictionary, loop through that list's values (in this case, words)
                foreach (string word in list)
                { tempList.Add(word); }//add each word to our temporary list
            }
            int tempIndex = _randomObj.Next(tempList.Count); //get a random index with a limit of our new temporary list's count total
            //assign our generated word variable to the word at the random index of our temp list
            GeneratedWord = tempList[tempIndex];
            _convertedToWord = false; //since we just re-assigned a new word, we haven't converted it yet
            return GeneratedWord; //return the word to the caller
        }
        catch (IndexOutOfRangeException indexE) //a specific exception catch if there is an index out of range
        { throw indexE; }
        catch (Exception e) //a general exception catch
        {
            //preserves original exception error by using the new keyword
            throw new Exception($"An error ocurred getting the word in the list, within the dictionary.\n{e}");
        }
    }
    /// <summary>
    /// Gets a random word with a length specified by the argument passed in, within the entire collection of words in the game.
    /// </summary>
    /// <param name="wordLength_p"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="IndexOutOfRangeException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public string GenerateWord(int wordLength_p)
    {
        if (wordLength_p > 1)
        {
            try
            {
                List<string> tempList = new List<string>();
                foreach (List<string> list in _wordListDictionary.Values)
                {
                    foreach (string word in list)
                    {
                        if (word.Length == wordLength_p)
                        { tempList.Add(word); }
                    }
                }
                if (tempList.Count > 0) //if our temp list of words isn't empty
                {
                    //get a random index with a limit of our new temporary list's count total
                    int tempIndex = _randomObj.Next(tempList.Count);
                    //assign our generated word variable to the word at the random index of our temp list
                    GeneratedWord = tempList[tempIndex];
                    _convertedToWord = false;
                    return GeneratedWord; //return the word to the caller
                }
                else //if our temp list is empty
                {
                    Debug.Log($"There were no words with the provided length of: {wordLength_p}.");
                    throw new EntryPointNotFoundException($"There were no words with the provided length of: {wordLength_p}.");
                }
            }
            //if the try block code doesn't work
            catch (IndexOutOfRangeException indexE) //a specific exception catch if there is an index out of range
            { throw indexE; }
            catch (Exception e) //a general exception catch
            {
                //preserves original exception error by using the new keyword
                throw new Exception($"An error ocurred getting the word in the list, within the dictionary.\n{e}");
            }
        }
        else //if the word length argument is less than or equal to 1
        {
            throw new ArgumentException($"The word length of, {wordLength_p}, has to be greater than 1.");
        }
        
    }
    /// <summary>
    /// Gets a random word within a list, categorised by its difficulty value (which relates to the dictionary key) passed in.
    /// </summary>
    /// <param name="difficultyKey_p"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    /// <exception cref="System.IndexOutOfRangeException"></exception>
    /// <exception cref="System.ArgumentException"></exception>
    public string GenerateWord(string difficultyKey_p)
    {
        //assigning a local variable to the method argument parsed into lower casing
        string difficultyKey = difficultyKey_p.ToLower();
        //validating if the argument is a valid Dictionary key
        //Note that both key variable values are lower case for comparison
        if (difficultyKey == _diffKeyEasy ||
            difficultyKey == _diffKeyModerate ||
            difficultyKey == _diffKeyHard )
        {
            try
            {
                /*creating a local scope int to store the random index (tempIndex)
                *using the Random() class instantiation to access rng capability
                *accessing our list through our dictionary of lists (to avoid coupling), passing in the string arg as a key
                *then using the .Count property of a list to return the maximum value the 
                *random number can return so that we don't access an item outside the bounds of the list*/
                int tempIndex = _randomObj.Next(_wordListDictionary[difficultyKey_p].Count);
                /*assigning the _generatedWord string to the string value of the list item at our temprary index
                *the list accessed is also determined by the key passed into the method*/
                GeneratedWord = _wordListDictionary[difficultyKey_p][tempIndex];
                _convertedToWord = false;
                return GeneratedWord; //returning the word we've generated
            }
            catch (IndexOutOfRangeException indexE) //a specific exception catch if there is an index out of range
            { throw indexE; }
            catch (Exception e) //a general exception catch
            {
                //preserves original exception error by using the new keyword
                throw new Exception($"An error ocurred getting the word in the list, with the dictionary key of {difficultyKey_p}: {e}");
            }
        }
        else //if the method argument is not a valid dictionary key
        { throw new ArgumentException("The difficulty key passed to the method is not valid."); }
    }
    #endregion

    #region Index Retrievals In Words
    public int[] GetIndicesInWord(string word1_p, string word2_p)
    {
        return null;
    }
    public int[] GetIndicesInWord(string word_p, char[] charArray_p)
    {
        return null;
    }
    public int[] GetIndicesInWord(char character_p, string word_p)
    {
        return null;
    }
    public int[] GetIndicesInWord(char character_p, char[] charArray_p)
    {
        return null;
    }
    #endregion

    /// <summary>
    /// Removes all whitespace values from the string. (Includes space, tab, return, & newline)
    /// </summary>
    /// <param name="word_p"></param>
    /// <returns>String argument without whitespaces</returns>
    public string RemoveWhiteSpaces(string word_p)
    { // passing the string arg through multiple methods that eventually return it after the operations are carried out
        // and then we return that value back to the method caller
        return word_p
            .Replace(" ", "")   // first,   replacing spaces with nothing
            .Replace("\t", "")  // second,  replacing tab spaces with nothing 
            .Replace("\r", "")  // third,   replacing return spaces with nothing
            .Replace("\n", ""); // fourth,  replacing newline spaces with nothing
    }
}

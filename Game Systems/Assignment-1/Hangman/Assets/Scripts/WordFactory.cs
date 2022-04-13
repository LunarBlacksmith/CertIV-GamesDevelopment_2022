using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordFactory : MonoBehaviour
{
    //TODO: 

    #region Private Variables

    private string _generatedWord = "";         // used to store the word generated for the current level
    private int _letterCount = 0;               // used to count the amount of letters in the generated word (includes whitespace)
    private int _totalWordCount = 0;            // used to keep track of the total amount of words in the word dictionary
    private bool _convertedToWord = false;      // used to control when any generated word is converted to the _genWordCharArray
    private bool _assignedLetterCount = false;  // used to control when any generated word's length has been retrieved
    private List<int> _indicesOfChar = new List<int>();  // used to store which indices a character is in a string or array

    // permanent difficulty keys for accessing different words in our dictionary of stored lists.
    private const string _diffKeyEasy = "easy", _diffKeyModerate = "moderate", _diffKeyHard = "hard";

    // instantiating System object "Random" for random number generation
    // have to distinguish between System and UnityEngine Random()
    private System.Random _randomObj = new System.Random();

        #region Lists & Dictionaries

    // used to store the permanent list of words in the game.
    // each list stores different difficulty words (shorter or longer, or less or more complex)
    [SerializeField] private List<string> _easyWords;
    [SerializeField] private List<string> _moderateWords;
    [SerializeField] private List<string> _hardWords;
    private List<string> _totalWordList = new List<string>(); // list of all words available, only for private use in class

    // used to store separate lists of words that are categorised by difficulty setting, which is determined by the Dictionary's key (i.e: "easy", "moderate", "hard")
    // having this Dictionary helps to decouple the code.
    private Dictionary<string, List<string>> _wordListDictionary = new Dictionary<string, List<string>>();

    // this is already instantiated by the property WordsUsed, no need to declare it separately except for rare cases
    // private Dictionary<int, string> _wordsUsed;
        #endregion
    #endregion

    #region Properties

    /// <summary>
    /// Stores the generated word just after the generation methods are called, and is accessible outside its class.
    /// </summary>
    public string GeneratedWord { get; private set; }   // public property    

    /// <summary>
    /// Stores the number of letters in the current word and verifies values passed in aren't less than 0. NOTE: Includes whitespaces.
    /// </summary>
    public int LetterCount
    {
        get { return _letterCount; }
        // set lettercount to value if its greater than or equal to 0, otherwise set to self
        private set { _letterCount = (value >= 0) ? value : _letterCount; }
    }

    /// <summary>
    /// Stores the total number of words stored in the word list dictionary. The count can never be less than 0.
    /// </summary>
    public int TotalWordCount   // Maths.Abs() returns the passed value in its absolute value (non-negative)
    { get { return _totalWordCount; } private set { _totalWordCount = Math.Abs(value); } }

    /// <summary>
    /// Stores the indices that the last character searched for is at within a list, accessible outside of its class.
    /// </summary>
    public List<int> IndicesOfChar 
    { 
        get { return _indicesOfChar; }
        // set our indices list to the value passed as long as its size is greater than 0, or the value isn't null
        // though the private variable initialises an empty list, the list will be utilised elsewhere and the check is a failsafe
        private set 
        {
            // check if the caller's size is not equal to our indices list
            // aka: our indices of char list needs to be resized so there are no inaccurate or missing elements in it
            if (_indicesOfChar.Count != value.Count && (value.Count >= 0) || (value != null))
            // resize the list to the size of the passed array, and make our list elements equal the passed list elements
            { _indicesOfChar = value; }
        } 
    }

    /// <summary>
    /// Contains a list of all the words used/generated from the word dictionary.
    /// </summary>
    public List<string> WordsUsed { get; private set; } = new List<string>(); 
    #endregion

    void Start()
    {
        // adding the lists of differing difficulty words to our dictionary of lists,
        // each list is assigned to the keys declared as variables, for modularity. (i.e: diffKeyEasy = "easy")
        _wordListDictionary.Add(_diffKeyEasy, _easyWords);
        _wordListDictionary.Add(_diffKeyModerate, _moderateWords);
        _wordListDictionary.Add(_diffKeyHard, _hardWords);

        // loop through each string list in the dictionary
        foreach (List<string> listOfWords in _wordListDictionary.Values)
        {   // sort each list of words by length of the word, from shortest to longest using the System's CompareTo() method
            listOfWords.Sort((a, b) => a.Length.CompareTo(b.Length)); 
            TotalWordCount += listOfWords.Count;    // add the number of words in each list to our total counter
        }
    }
    void Update()
    {
        // if we haven't got our current word's letter count, AND the generated word's character array isn't null or empty
        if (!_assignedLetterCount)
        // assign number of letters in word to the number of characters in the array of the generated word
        { LetterCount = GeneratedWord.Length; _assignedLetterCount = true; }

        // check if our total word list has expanded
        if (CheckIfWordListExpanded
            (GetStringListFromDictionary(_wordListDictionary),  // getting a total list of words in the game
            TotalWordCount))    // passing in our previously calculated total count of words)
        // set our total word count to our new exapanded list count value
        { TotalWordCount = GetStringListFromDictionary(_wordListDictionary).Count; }
        // I wrote this at 3am 0_o idk if this is sound logic anymore
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

        // else if we have never returned true, aka: character doesn't exist in the second string
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
        { if (character_p == character) { return true; } }  // return true if the character param is in the string param

        // else if we have never returned true, aka: character doesn't exist in the string
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

        // else if we have never returned true, aka: character doesn't exist in the string
        return false;
    }
    #endregion

    // All Compare methods indlude whitespace as a valid character
    #region Length Comparisons

    /// <summary>
    /// Compares the length of the first array arg to the second array arg and returns true if they are equal. Any array type can be passed as parameters.
    /// </summary>
    /// <param name="array1_p"></param>
    /// <param name="array2_p"></param>
    /// <returns>True or False</returns>
    public bool CompareLength<T>(T[] array1_p, T[] array2_p)
    { return (array1_p.Length == array2_p.Length); }
    // The above method is an improved and inclusive version of the following three methods:

        #region Old Length Comparison Methods
    ///// <summary>
    ///// Compares the length of the first char array to the second and returns true if they are equal.
    ///// </summary>
    ///// <param name="charArray1_p"></param>
    ///// <param name="charArray2_p"></param>
    ///// <returns>True or False</returns>
    //public bool CompareLength(char[] charArray1_p, char[] charArray2_p)
    //{ return (charArray1_p.Length == charArray2_p.Length); }

    ///// <summary>
    ///// Compares the length of the first char array to the second and returns true if they are equal.
    ///// </summary>
    ///// <param name="word_p"></param>
    ///// <param name="charArray_p"></param>
    ///// <returns>True or False</returns>
    //public bool CompareLength(string word_p, char[] charArray_p)
    //{ return (word_p.Length == charArray_p.Length); }

    ///// <summary>
    ///// Compares the length of the first char array to the second and returns true if they are equal.
    ///// </summary>
    ///// <param name="word1_p"></param>
    ///// <param name="word2_p"></param>
    ///// <returns>True or False</returns>
    //public bool CompareLength(string word1_p, string word2_p)
    //{ return (word1_p.Length == word2_p.Length); }
        #endregion

    #endregion

    #region Word Generations
    /// <summary>
    /// Gets a random word from the entire collection of words available, regardless of difficulty. Returns an empty string if no unused words are found from the dictionary.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException">
    /// </exception><exception cref="Exception"></exception> 
    public string GenerateWord()
    {
        try
        {
            // instantiating a temporary list of strings
            List<string> tempList = new List<string>();

            // looping through our dictionary Values, which are lists of type string
            foreach (List<string> list in _wordListDictionary.Values)
            {
                // for each list in our dictionary, loop through that list's values (in this case, words)
                foreach (string word in list)
                { tempList.Add(word); } // add each word to our temporary list
            }
            int tempIndex = _randomObj.Next(tempList.Count);    // get a random index with a limit of our new temporary list's count total

            int loopCounter = 0;
            // if we've already used the random word, generate another one to get until we have an unused one
            while (WordsUsed.Contains(tempList[tempIndex]))
            {
                // if we've cycled through all the words available and there are no new words
                if (loopCounter == TotalWordCount)
                { tempIndex = -1; break; } // set our index out of bounds and break out of the while loop

                ++loopCounter;                              // increment our loop count
                tempList.RemoveAt(tempIndex);               // remove the used word from our search
                tempIndex = _randomObj.Next(tempList.Count);// generate another word index
            }

            // if our temp index is less than 0 (we didn't find an unused word)
            if (tempIndex < 0)
            { return string.Empty; }    // return an empty word

            // assign our generated word variable to the word at the random index of our temp list
            GeneratedWord = tempList[tempIndex];
            _convertedToWord = false;       // since we just re-assigned a new word, we haven't converted it yet
            _assignedLetterCount = false;   // need to tell our update function to re-assign our LetterCount variable with the new word

            WordsUsed.Add(GeneratedWord);   // add the generated word to our list of words used
            return GeneratedWord;           // return the generated word to the caller
        }
        catch (IndexOutOfRangeException indexE) //a specific exception catch if there is an index out of range
        { throw indexE; }
        catch (Exception e) // a general exception catch
        {
            // preserves original exception error by using the new keyword
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
                foreach (string word in _totalWordList)
                {
                    if (word.Length == wordLength_p)
                    { tempList.Add(word); }
                }
                
                if (tempList.Count > 0) //if our temp list of words isn't empty
                {
                    // get a random index with a limit of our new temporary list's count total
                    int tempIndex = _randomObj.Next(tempList.Count);

                    // assign our generated word variable to the word at the random index of our temp list
                    GeneratedWord = tempList[tempIndex];
                    _convertedToWord = false;
                    _assignedLetterCount = false;
                    return GeneratedWord;
                }
                else   // if our temp list is empty
                {
                    Debug.Log($"There were no words with the provided length of: {wordLength_p}.");
                    throw new EntryPointNotFoundException($"There were no words with the provided length of: {wordLength_p}.");
                }
            }
            // if the try block code doesn't work
            catch (IndexOutOfRangeException indexE) // a specific exception catch if there is an index out of range
            { throw indexE; }
            catch (Exception e) // a general exception catch
            {
                // preserves original exception error by using the new keyword
                throw new Exception($"An error ocurred getting the word in the list, within the dictionary.\n{e}");
            }
        }
        else   // if the word length argument is less than or equal to 1
        { throw new ArgumentException($"The word length of, {wordLength_p}, has to be greater than 1."); }
        
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
        // assigning a local variable to the method argument parsed into lower casing
        string difficultyKey = difficultyKey_p.ToLower();
        
        // verifying the argument is a valid Dictionary key
        // note that both key variable values are lower case for comparison
        if (difficultyKey == _diffKeyEasy ||
            difficultyKey == _diffKeyModerate ||
            difficultyKey == _diffKeyHard )
        {
            try
            {
                /* creating a local scope int to store the random index (tempIndex)
                *using the Random() class instantiation to access rng capability
                *accessing our list through our dictionary of lists (to avoid coupling), passing in the string arg as a key
                *then using the .Count property of a list to return the maximum value the 
                *random number can return so that we don't access an item outside the bounds of the list */
                int tempIndex = _randomObj.Next(_wordListDictionary[difficultyKey_p].Count);

                /* assigning the _generatedWord string to the string value of the list item at our temprary index
                *the list accessed is also determined by the key passed into the method */
                GeneratedWord = _wordListDictionary[difficultyKey_p][tempIndex];
                _convertedToWord = false;
                _assignedLetterCount = false;
                return GeneratedWord;
            }
            catch (IndexOutOfRangeException indexE) // a specific exception catch if there is an index out of range
            { throw indexE; }
            catch (Exception e) // a general exception catch
            {
                // preserves original exception error by using the new keyword
                throw new Exception($"An error ocurred getting the word in the list, with the dictionary key of {difficultyKey_p}: {e}");
            }
        }
        else   // if the method argument is not a valid dictionary key
        { throw new ArgumentException("The difficulty key passed to the method is not valid."); }
    }
    #endregion

    #region Index Retrievals In Words

    /// <summary>
    /// Returns a list of integers containing the index value(s) of the letter in question, in the first arg, within the word in the second arg. Returns an empty list if the letter is not found in the word.
    /// </summary>
    /// <param name="word1_p"></param>
    /// <param name="word2_p"></param>
    /// <returns></returns>
    public List<int> GetIndicesInWord(string word1_p, string word2_p)
    {
        IndicesOfChar.Clear();      // clearing our list property of indices stored
        int startPositionIndex = 0; // setting a start position for our IndexOf() search

        // setting a temp char variable to the first character of the string passed in after removing all whitespace
        char letterInQuestion = RemoveWhiteSpaces(word1_p)[0];

        // the code block runs if we find the letter in the word - IndexOf() returns -1 if nothing is found
        // we start searching from the beginning of the word at first
        while (word2_p.IndexOf(letterInQuestion, startPositionIndex) > 0)
        {
            // add the index value of the letter in the word to our list of indices
            IndicesOfChar.Add(word2_p.IndexOf(letterInQuestion, startPositionIndex));

            // assigning our next starting index in the word to the last found index value + 1 to avoid infinite loop
            startPositionIndex = IndicesOfChar[IndicesOfChar.Count-1] + 1;
        }
        return IndicesOfChar;

        #region Old Way
        //// currWordIndex for keeping track of the number of the loop in the foreach
        //// intArrSize for keeping track of which index in our character indices array to store the next index of the found character in the word
        //int currWordIndex = 0, intArrSize = 0, currIndexStorerIndex = 0;

        //// cannot pass a property as an out or ref value for resizing our array, to making a temp variable of our property
        //int[] indicesArray = IndicesOfChar;

        //// removing the whitespaces in the first string so we can accurately get the first valid character in the word
        //char tempChar = RemoveWhiteSpaces(word1_p)[0];

        //// here we loop through the word and increment our intArrSize, so we can set our Property array to the correct size
        //foreach (char character in word2_p.ToCharArray())
        //{ if (tempChar == character) { ++intArrSize; } }

        //// if we didn't find the character in the word
        //if (intArrSize == 0)
        //{ return null; } // return null (no value)
        //// else, continue the method code

        //// resizing our index array so we don't assign a value at an out of bounds index
        //Array.Resize(ref indicesArray, intArrSize);

        //// re-assigning our Property array to the temporary array for correct size
        //IndicesOfChar = indicesArray;

        //// looping through the word again
        //foreach (char character in word2_p.ToCharArray())
        //{
        //    if (tempChar == character) //if we find the character in the word
        //    {
        //        // this time adding the character's index value within the word to our array of stored index values
        //        IndicesOfChar[currIndexStorerIndex] = word2_p.ToCharArray()[currWordIndex];
        //        ++currIndexStorerIndex; // incrementing this to get the next place in our Property array to store the next char index found
        //    }
        //    ++currWordIndex; // incrementing this to keep track of the current index of the character in the word
        //}
        //return IndicesOfChar; // return our Property array that holds the index values
        #endregion
    }

    // -- OVER ENGINEERING (KISS!), FOR ADDITION AT LATER TIME --
    //public int[] GetIndicesInWord(string word_p, char[] charArray_p)
    //{
    //    return null;
    //}
    //public int[] GetIndicesInWord(char character_p, string word_p)
    //{
    //    return null;
    //}
    //public int[] GetIndicesInWord(char character_p, char[] charArray_p)
    //{
    //    return null;
    //}
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

    /// <summary>
    /// This takes any array as a parameter, that matches the type defined in < > when called, and returns true if it is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array_p"></param>
    /// <returns>True or False</returns>
    public static bool IsNullOrEmpty<T>(T[] array_p)
    { return (array_p == null) || (array_p.Length == 0); }

    /// <summary>
    /// Returns true if the count of total words in the list passed in is a different total to what it was previously. (If true, the list has expanded.)
    /// </summary>
    /// <param name="listOfWords_p"></param>
    /// <param name="lastCalculatedWordTotal_p"></param>
    /// <returns></returns>
    public bool CheckIfWordListExpanded(List<string> listOfWords_p, int lastCalculatedWordTotal_p) 
    { return listOfWords_p.Count != lastCalculatedWordTotal_p;}

    /// <summary>
    /// Returns a list of all the words within every list in the dictionary passed in.
    /// </summary>
    /// <param name="dictionary_p"></param>
    /// <returns></returns>
    public List<string> GetStringListFromDictionary(Dictionary<string,List<string>> dictionary_p) 
    {
        _totalWordList.Clear();
        foreach (List<string> listOfWords in dictionary_p.Values)   // loop through dictionary
        { _totalWordList.AddRange(listOfWords); } // add elements of each list in the dictionary to total word list
        return _totalWordList;    // return our list containing all words in game
    }
}

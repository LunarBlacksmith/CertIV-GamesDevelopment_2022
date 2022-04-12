using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordFactory : MonoBehaviour
{
    private string _generatedWord = ""; //used to store the word generated for the current level
    private char[] _genWordCharArray; //used to convert generated word to a character array for individual character manipulation
    //permanent difficulty keys for accessing different words in our dictionary of stored lists.
    private string _diffKeyEasy = "easy", _diffKeyModerate = "moderate", _diffKeyHard = "hard";
    private int _letterCount = 0; //used to count the amount of letters in the generated word
    //instantiating System object "Random" for random number generation
    //have to distinguish between System and UnityEngine Random()
    private System.Random _randomObj = new System.Random(); 

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

    public string GeneratedWord { get; private set; } //public property for accessing its variable outside the class
    public char[] GenWordCharArray { get; private set; } //public property for accessing the array outside the class
    //property for lettercount to allow verification on setting of letter count
    public int LetterCount 
    {
        get { return _letterCount; }
        private set 
        {
            if (value >= 0)
            { _letterCount = value; }
        }
    }


    void Start()
    {
        //sorting the lists of words by length, from shortest to longest using the System's CompareTo() method
        _easyWords.Sort((a, b) => a.Length.CompareTo(b.Length));
        _moderateWords.Sort((a, b) => a.Length.CompareTo(b.Length));
        _hardWords.Sort((a, b) => a.Length.CompareTo(b.Length));
        //adding the lists of differing difficulty words to our dictionary of lists,
        //each list is assigned to the keys declared as variables, for modularity. (i.e: diffKeyEasy = "easy")
        _wordListDictionary.Add(_diffKeyEasy, _easyWords);
        _wordListDictionary.Add(_diffKeyModerate, _moderateWords);
        _wordListDictionary.Add(_diffKeyHard, _hardWords);
    }
    void Update()
    {
        
    }

    public bool EvaluateWord(string word1_p, string word2_p)
    {
        bool _isTheSame = false;
        return _isTheSame;
    }

    public bool EvaluateCharacter(string word1_p, string word2_p)
    {
        bool _isTheSame = false;
        return _isTheSame;
    }
    public bool EvaluateCharacter(char character_p, string word_p)
    {
        bool _isTheSame = false;
        return _isTheSame;
    }
    public bool EvaluateCharacter(string word1_p, char[] charArray_p)
    {
        bool _isTheSame = false;
        return _isTheSame;
    }

    public bool CompareLength(char[] charArray1_p, char[] charArray2_p)
    {
        bool _isTheSame = false;
        return _isTheSame;
    }
    public bool CompareLength(string word_p, char[] charArray_p)
    {
        bool _isTheSame = false;
        return _isTheSame;
    }
    public bool CompareLength(string word1_p, string word2_p)
    {
        bool _isTheSame = false;
        return _isTheSame;
    }

    /// <summary>
    /// Gets a random word from the entire collection of words available, regardless of difficulty.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.IndexOutOfRangeException">
    /// </exception><exception cref="System.Exception"></exception> 
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
                {
                    //add each word to our temporary list
                    tempList.Add(word);
                }
            }
            int tempIndex = _randomObj.Next(tempList.Count); //get a random index with a limit of our new temporary list's count total
            //assign our generated word variable to the word at the random index of our temp list
            GeneratedWord = tempList[tempIndex];
            return GeneratedWord; //return the word to the caller
        }
        catch (System.IndexOutOfRangeException indexE) //a specific exception catch if there is an index out of range
        { throw indexE; }
        catch (System.Exception e) //a general exception catch
        {
            //preserves original exception error by using the new keyword
            throw new System.Exception($"An error ocurred getting the word in the list, within the dictionary.\n{e}");
        }
    }
    /// <summary>
    /// Gets a random word with a length specified by the argument passed in, within the entire collection of words in the game.
    /// </summary>
    /// <param name="wordLength_p"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    /// <exception cref="System.IndexOutOfRangeException"></exception>
    /// <exception cref="System.NullReferenceException"></exception>
    /// <exception cref="System.ArgumentException"></exception>
    public string GenerateWord(int wordLength_p)
    {
        if (wordLength_p > 1)
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
                    {
                        //if the word at the current index has a length equal to the word length argument
                        if (word.Length == wordLength_p)
                        { tempList.Add(word); } //add the word to our temporary list of words
                    }
                }
                if (tempList != null) //if our temp list of words isn't empty
                {
                    //get a random index with a limit of our new temporary list's count total
                    int tempIndex = _randomObj.Next(tempList.Count);
                    //assign our generated word variable to the word at the random index of our temp list
                    GeneratedWord = tempList[tempIndex];
                    return GeneratedWord; //return the word to the caller
                }
                else //if our temp list is empty
                {
                    Debug.Log($"There were no words with the provided length of: {wordLength_p}.");
                    throw new System.NullReferenceException();
                }
            }
            //if the try block code doesn't work
            catch (System.IndexOutOfRangeException indexE) //a specific exception catch if there is an index out of range
            { throw indexE; }
            catch (System.Exception e) //a general exception catch
            {
                //preserves original exception error by using the new keyword
                throw new System.Exception($"An error ocurred getting the word in the list, within the dictionary.\n{e}");
            }
        }
        else //if the word length argument is less than or equal to 1
        {
            throw new System.ArgumentException($"The word length of, {wordLength_p}, has to be greater than 1.");
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
                return GeneratedWord; //returning the word we've generated
            }
            catch (System.IndexOutOfRangeException indexE) //a specific exception catch if there is an index out of range
            { throw indexE; }
            catch (System.Exception e) //a general exception catch
            {
                //preserves original exception error by using the new keyword
                throw new System.Exception($"An error ocurred getting the word in the list, with the dictionary key of {difficultyKey_p}: {e}");
            }
        }
        else //if the method argument is not a valid dictionary key
        {
            throw new System.ArgumentException("The difficulty key passed to the method is not valid.");
        }
        
    }

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
}

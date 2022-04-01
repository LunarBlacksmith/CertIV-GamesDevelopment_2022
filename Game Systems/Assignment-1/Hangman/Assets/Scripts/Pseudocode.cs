//Load Main Menu object
//MAIN MENU
//Every button has OnClick event referencing the GameManager script
        /*New Game button
            *Creates a new save file (non-goal)
            *Deactivate the Main Menu
            *Activates the Game UI object (the object the main game takes place through) */
        /*Load Game button (non-goal)
            *Checks if there is more than 1 save file that exists
            *If save files > 1
                *Activate prompt UI to choose a save file
                *Text box for user to enter a number relating to the save file
                *Check if valid number
                    *If true, load save file and activate Game UI object
                    *Else, activate error prompt UI and ask for different save number
                    *Back button
                        *Deactivate prompt UI for save files
            *Else if save files !null
                *Back button
                    *Deactivate prompt UI for save files
                *Load first existing save file in directory
            *Else (if there are no save files)
                *Back button
                    *Deactivate prompt UI for save files
                *Activate prompt UI to tell player there are no saves */
        /*Exit Game button
            *Update current save file if there is one
            *Check if the game is run in Unity Editor
                *If true: exit play mode
                *Else: quits the application to the Desktop */


//PAUSE MENU
//Stop time
    /*Resume button
        *Deactivate Pause Menu
        *Resume time */
    /*Main Menu button
        *Deactivate Game UI object (the game)
        *Activate Main Menu object
        *Resume time */
    /*Exit Game button
        *Check if the game is run in Unity Editor
            *If true, exit play mode
            *Else, quits the application to the Desktop */


//END MENU
    /*Retry / Replay button
        *Update current save file
        *Set total incorrect guesses to 0
        *Activates the Game UI object */
    /*Main Menu button
        *Deactivate Game UI object (the game)
        *Activate Main Menu object */
    /*Exit Game button
        *Update current save file 
        *Check if the game is run in Unity Editor
            *If true: exit play mode
            *Else: quits the application to the Desktop */


//GAME
    //Declares and assigns List<string> of words
    //Sorts word list by length of word
    /*Check difficulty variable
        *Choose a word in the list at random with a certain length, dependant on variable, and store it in a variable
        *Convert chosen word to char array */
    //Get word length, generate, and display letter placements to the player equal to the length of the word
    //Display total incorrect guesses on Game UI
    //While user has incorrect guesses left:
        //Display text box for user input
            //Text box has Enter arrow symbol faintly on right side of bar for visual direction of action 
        //User inputs character(s)
            //Check if Enter key
                /*If enter key is true:
                    *Check if input string is null
                    *Is null is true: 
                        *Activate prompt UI to display error that the input cannot be empty
                        *Wait seconds
                        *Deactivate prompt */
                    //Check if input is longer than 1 character
                    /*Input longer than 1 char true:
                        *Compare input from input text box to final word
                        *If not the same:
                            * increment total incorrect guesses count
                        *If the same:
                            *Deactivate Game
                            *Activate End Menu object
                            *Activate prompt UI to tell player they won */
                    /*Input is 1 char
                        *Convert input from input text box to char
                        *Loop through the char array variable storing the previously converted word
                            *Compare input to each item in the char array
                                *Every time input = current item in array, store array index in another array
                        *Check if other index array is null (meaning no identical letters found in word)
                            *If null:
                                *Increment total wrong guesses variable and UI
                                *Compare total wrong guesses to max guesses
                                    *If equal: 
                                        *Update Hangman sprite to reflect game loss
                                        *Wait seconds
                                        *Deactivate Game
                                        *Activate End Menu object
                                        *Activate prompt UI to tell player they lost
                                    *Else: continue game
                            *If other index array !null:
                                *Update every letter placement UI using the indices stored in the array with the player's input
                                *Loop through and check if every letter placement UI text is !null 
                                    *If true: 
                                        *Deactivate Game
                                        *Activate End Menu
                                        *Activate prompt UI to tell player they won
                                    *If false:
                                        *Continue game */
            //Check if Esc key
                /*If Esc key is true:
                    *Stop time
                    *Activate Pause Menu*/
            //Otherwise, if not Enter or Esc key, convert input string to character array
            /*Loop through and check if any character is invalid character (not alphabetical or Space)
                *If true: 
                    *Activate prompt UI to display error that characters must be of [valid character] type
                    *Wait seconds
                    *Deactivate prompt
                *If false:
                    *Add character to player input text box */
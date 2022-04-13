using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public WordFactory wordFactory;     // handles the word generation and manipulation in the game
    public SoundManager soundManager;   // handles the audio in the game
    
    void Start()
    {
        
    }

    
    void Update()
    {
        // when activating game objects using the indices of the letters
        IterateThroughList(wordFactory.IndicesOfChar);
    }

    private void IterateThroughList<T>(List<T> list)
    {
        if (list.Count > 0)
        {
            foreach (var index in list)
            {
                // Activate game object at the index value
            }
        }
    }
}

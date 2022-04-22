using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//you will need to change Scenes
public class CustomisationSet : MonoBehaviour {

    #region Variables
    [Header("Texture List")]
    //Texture2D List for skin,hair, mouth, eyes
    public List<Texture2D> skin = new List<Texture2D>();
    public List<Texture2D> hair = new List<Texture2D>();
    public List<Texture2D> mouth = new List<Texture2D>();
    public List<Texture2D> eyes = new List<Texture2D>();
    public List<Texture2D> clothes = new List<Texture2D>();
    public List<Texture2D> armour = new List<Texture2D>();
    [Header("Index")]
    //index numbers for our current skin, hair, mouth, eyes textures
    public int skinIndex;
    public int hairIndex, mouthIndex, eyesIndex, clothesIndex, armourIndex;
    [Header("Renderer")]
    //renderer for our character mesh so we can reference a material list
    public Renderer charRend;
    [Header("Max Index")]
    //max amount of skin, hair, mouth, eyes textures that our lists are filling with
    public int skinMax;
    public int hairMax, mouthMax, eyesMax, clothesMax, armourMax;
    [Header("Character Name")]
    //name of our character that the user is making
    public string characterName = "Adventurer";
    #endregion

    #region Start
    //in start we need to set up the following
    private void Start()
    {
        if (GameManager.scr.x != Screen.width/16)
        {
            GameManager.scr.x = Screen.width / 16;
            GameManager.scr.y = Screen.width / 9;
        }

        #region for loop to pull textures from file
        //for loop looping from 0 to less than the max amount of skin textures we need
        for (int i = 0; i < skinMax; i++)
        {
            //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Skin_#
            Texture2D temp = Resources.Load("Character/Skin_" + i) as Texture2D;
            //add our temp texture that we just found to the skin List
            skin.Add(temp);
        }
        for (int i = 0; i < hairMax; i++)
        {
            //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Skin_#
            Texture2D temp = Resources.Load("Character/Hair_" + i) as Texture2D;
            //add our temp texture that we just found to the skin List
            hair.Add(temp);
        }
        for (int i = 0; i < mouthMax; i++)
        {
            //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Skin_#
            Texture2D temp = Resources.Load("Character/Mouth_" + i) as Texture2D;
            //add our temp texture that we just found to the skin List
            mouth.Add(temp);
        }
        for (int i = 0; i < eyesMax; i++)
        {
            //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Skin_#
            Texture2D temp = Resources.Load("Character/Eyes_" + i) as Texture2D;
            //add our temp texture that we just found to the skin List
            eyes.Add(temp);
        }
        for (int i = 0; i < clothesMax; i++)
        {
            //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Skin_#
            Texture2D temp = Resources.Load("Character/Clothes_" + i) as Texture2D;
            //add our temp texture that we just found to the skin List
            clothes.Add(temp);
        }
        for (int i = 0; i < armourMax; i++)
        {
            //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Skin_#
            Texture2D temp = Resources.Load("Character/Armour_" + i) as Texture2D;
            //add our temp texture that we just found to the skin List
            armour.Add(temp);
        }
        //for loop looping from 0 to less than the max amount of hair textures we need
        //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Hair_#
        //add our temp texture that we just found to the hair List
        //for loop looping from 0 to less than the max amount of mouth textures we need    
        //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Mouth_#
        //add our temp texture that we just found to the mouth List

        //for loop looping from 0 to less than the max amount of eyes textures we need
        //creating a temp Texture2D that it grabs using Resources.Load from the Character File looking for Eyes_#
        //add our temp texture that we just found to the eyes List            
        #endregion
        //connect and find the SkinnedMeshRenderer thats in the scene to the variable we made for Renderer
        charRend = GameObject.Find("Mesh").GetComponent<Renderer>();
        #region do this after making the function SetTexture
        //SetTexture skin, hair, mouth, eyes to the first texture 0
        SetTexture("Skin", 0);
        SetTexture("Hair", 0);
        SetTexture("Eyes", 0);
        SetTexture("Mouth", 0);
        SetTexture("Clothes", 0);
        SetTexture("Armour", 0);
        #endregion
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SetTexture("Skin", -1);
            SetTexture("Hair", -1);
            SetTexture("Eyes", -1);
            SetTexture("Mouth", -1);
            SetTexture("Clothes", -1);
            SetTexture("Armour", -1);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetTexture("Skin", +1);
            SetTexture("Hair", +1);
            SetTexture("Eyes", +1);
            SetTexture("Mouth", +1);
            SetTexture("Clothes", +1);
            SetTexture("Armour", +1);
        }
    }

    #region SetTexture
    //Create a function that is called SetTexture it should contain a string and int
    //the string is the name of the material we are editing, the int is the direction we are changing
    void SetTexture(string type, int dir)
    {
        //we need variables that exist only within this function
        //these are ints index numbers, max numbers, material index and Texture2D array of textures
        int index = 0, max = 0, matIndex = 0;
        Texture2D[] textures = new Texture2D[0];
        //inside a switch statement that is swapped by the string name of our material
        switch (type)
        {
            #region Switch Material
            //case type
            case "Skin":
                {
                    //index is the same as our skin index
                    index = skinIndex;
                    //max is the same as our skin max
                    max = skinMax;
                    //textures is our skin list .ToArray()
                    textures = skin.ToArray();
                    //material index element number is 1
                    matIndex = 1;
                    //break
                    break;
                }
            case "Hair":
                {
                    index = hairIndex;
                    max = hairMax;
                    textures = hair.ToArray();
                    matIndex = 4;
                    break;
                }
            case "Mouth":
                {
                    index = mouthIndex;
                    max = mouthMax;
                    textures = mouth.ToArray();
                    matIndex = 3;
                    break;
                }
            case "Eyes":
                {
                    index = eyesIndex;
                    max = eyesMax;
                    textures = eyes.ToArray();
                    matIndex = 2;
                    break;
                }
            case "Clothes":
                {
                    index = clothesIndex;
                    max = clothesMax;
                    textures = clothes.ToArray();
                    matIndex = 5;
                    break;
                }
            case "Armour":
                {
                    index = armourIndex;
                    max = armourMax;
                    textures = armour.ToArray();
                    matIndex = 6;
                    break;
                }
            #endregion
            default:
                {
                    Debug.Log("You fucked up");
                    break;
                }
                
        }

        #region OutSide Switch
        //outside our switch statement
        //index plus equals our direction
        index += dir;
        //cap our index to loop back around if is is below 0 or above max take one
        if (index < 0)
        {
            index = max - 1;
        }
        if (index > max-1)
        {
            index = 0;
        }
        //Material array is equal to our characters material list
        Material[] materials = charRend.materials;
        //our material arrays current material index's main texture is equal to our texture arrays current index
        materials[matIndex].mainTexture = textures[index];
        //our characters materials are equal to the material array
        charRend.materials = materials;
        //create another switch that is goverened by the same string name of our material
        #endregion
        #region Set Material Switch
        //case type
        
        switch (type)
        {//type index equals our index
            case "Skin": 
                { skinIndex = index; break; }
            case "Hair":
                { hairIndex = index; break; }
            case "Mouth":
                { mouthIndex = index; break; }
            case "Eyes":
                { eyesIndex = index; break; }
            case "Clothes":
                { clothesIndex = index; break; }
            case "Armour":
                { armourIndex = index; break; }
            default:
                { Debug.Log("Something went wrong in SetTexture second switch."); break; }
        }
        #endregion
    }

    #endregion

    #region Save
    //Function called Save this will allow us to save our indexes to PlayerPrefs
    //SetInt for SkinIndex, HairIndex, MouthIndex, EyesIndex
    //SetString CharacterName
    #endregion

    #region OnGUI
    private void OnGUI()
    {
        #region Skin
        //GUI button on the left of the screen with the contence <
        //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  -1
        //GUI Box or Lable on the left of the screen with the contence Skin
        //GUI button on the left of the screen with the contence >
        //when pressed the button will run SetTexture and grab the Skin Material and move the texture index in the direction  1
        //move down the screen with the int using ++ each grouping of GUI elements are moved using this
        int i = 0;

        if (GUI.Button(new Rect(0.25f*GameManager.scr.x, GameManager.scr.y+i*(0.5f*GameManager.scr.y), 0.5f*GameManager.scr.x, 0.5f*GameManager.scr.y),"<"))
        {
            SetTexture("Skin", -1);
        }

        GUI.Box(new Rect(0.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), GameManager.scr.x, 0.5f * GameManager.scr.y), "Skin");

        if (GUI.Button(new Rect(1.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Skin", 1);
        }
        i++;
        #endregion
        #region Hair
        if (GUI.Button(new Rect(0.25f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Hair", -1);
        }

        GUI.Box(new Rect(0.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), GameManager.scr.x, 0.5f * GameManager.scr.y), "Hair");

        if (GUI.Button(new Rect(1.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Hair", 1);
        }
        i++;
        //GUI button on the left of the screen with the contence <
        //when pressed the button will run SetTexture and grab the Material and move the texture index in the direction  -1
        //GUI Box or Lable on the left of the screen with the contence material Name
        //GUI button on the left of the screen with the contence >
        //when pressed the button will run SetTexture and grab the  Material and move the texture index in the direction  1
        //move down the screen with the int using ++ each grouping of GUI elements are moved using this
        #endregion
        #region Mouth
        if (GUI.Button(new Rect(0.25f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Mouth", -1);
        }

        GUI.Box(new Rect(0.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), GameManager.scr.x, 0.5f * GameManager.scr.y), "Mouth");

        if (GUI.Button(new Rect(1.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Mouth", 1);
        }
        i++;
        //GUI button on the left of the screen with the contence <
        //when pressed the button will run SetTexture and grab the Material and move the texture index in the direction  -1
        //GUI Box or Lable on the left of the screen with the contence material Name
        //GUI button on the left of the screen with the contence >
        //when pressed the button will run SetTexture and grab the  Material and move the texture index in the direction  1
        //move down the screen with the int using ++ each grouping of GUI elements are moved using this
        #endregion
        #region Eyes
        if (GUI.Button(new Rect(0.25f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Eyes", -1);
        }

        GUI.Box(new Rect(0.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), GameManager.scr.x, 0.5f * GameManager.scr.y), "Eyes");

        if (GUI.Button(new Rect(1.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Eyes", 1);
        }
        i++;
        //GUI button on the left of the screen with the contence <
        //when pressed the button will run SetTexture and grab the Material and move the texture index in the direction  -1
        //GUI Box or Lable on the left of the screen with the contence material Name
        //GUI button on the left of the screen with the contence >
        //when pressed the button will run SetTexture and grab the  Material and move the texture index in the direction  1
        //move down the screen with the int using ++ each grouping of GUI elements are moved using this
        #endregion
        #region Clothes
        if (GUI.Button(new Rect(0.25f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Clothes", -1);
        }

        GUI.Box(new Rect(0.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), GameManager.scr.x, 0.5f * GameManager.scr.y), "Clothes");

        if (GUI.Button(new Rect(1.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Clothes", 1);
        }
        i++;
        #endregion
        #region Armour
        if (GUI.Button(new Rect(0.25f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Armour", -1);
        }

        GUI.Box(new Rect(0.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), GameManager.scr.x, 0.5f * GameManager.scr.y), "Armour");

        if (GUI.Button(new Rect(1.75f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Armour", 1);
        }
        i++;
        #endregion

        #region Random Reset
        //create 2 buttons one Random and one Reset
        //Random will feed a random amount to the direction 
        //reset will set all to 0 both use SetTexture
        //move down the screen with the int using ++ each grouping of GUI elements are moved using this
        if (GUI.Button(new Rect(0.25f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            skinIndex = Random.Range(0, skinMax);
            hairIndex = Random.Range(0, hairMax);
            eyesIndex = Random.Range(0, eyesMax);
            mouthIndex = Random.Range(0, mouthMax);
            clothesIndex = Random.Range(0, clothesMax);
            armourIndex = Random.Range(0, armourMax);
            SetTexture("Skin", 0);
            SetTexture("Hair", 0);
            SetTexture("Eyes", 0);
            SetTexture("Mouth", 0);
            SetTexture("Clothes", 0);
            SetTexture("Armour", 0);
        }
        if (GUI.Button(new Rect(0.25f * GameManager.scr.x, GameManager.scr.y + i * (0.5f * GameManager.scr.y), 0.5f * GameManager.scr.x, 0.5f * GameManager.scr.y), "<"))
        {
            SetTexture("Skin", skinMax);
            SetTexture("Hair", hairMax = 0);
            SetTexture("Eyes", eyesMax);
            SetTexture("Mouth", mouthMax);
            SetTexture("Clothes", clothesMax);
            SetTexture("Armour", armourMax);
        }
        i++;
        #endregion
        #region Character Name and Save & Play
        //name of our character equals a GUI TextField that holds our character name and limit of characters
        //move down the screen with the int using ++ each grouping of GUI elements are moved using this
        characterName = GUI.TextField(new Rect(
            0.25f * GameManager.scr.x,                          // x
            GameManager.scr.y + i * (0.5f * GameManager.scr.y), // y
            2 * GameManager.scr.x,                              // width
            0.5f * GameManager.scr.y),                          // height
            characterName,                                      // text in TextField            
            12);                                                // maxLength of TextField
        i++;

        //GUI Button called Save and Play
        //this button will run the save function and also load into the game level
        #endregion
    }
    //Function for our GUI elements
    //create the floats scrW and scrH that govern our 16:9 ratio
    //create an int that will help with shuffling your GUI elements under eachother

    //set up same things for Hair, Mouth and Eyes





    #endregion
}

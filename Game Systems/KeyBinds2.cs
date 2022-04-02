using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinds : MonoBehaviour
{
    [SerializeField]
    public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    [System.Serializable]
    public struct KeyUISetup
    {
        public string keyName;
        public Text keyDisplayText;
        public string defaultKey;
    }
    public KeyUISetup[] baseSetup;
    public GameObject currentKey;
    public Color32 changedKey = new Color32(39, 171, 249, 255);
    public Color32 selectedKey = new Color32(239, 116, 36, 255);
    void Start()
    {
        //forloop to add the keys to the dictionary with the save or default data depending on load
        for (int i = 0; i < baseSetup.Length; i++)
        {
            //add key according to the saved string or default value
            keys.Add(baseSetup[i].keyName, (KeyCode)System.Enum.Parse(typeof(KeyCode), 
                PlayerPrefs.GetString(baseSetup[i].keyName, baseSetup[i].defaultKey)));
            //for all the UI text elements change the display to what bind is in our dictionary
            baseSetup[i].keyDisplayText.text = keys[baseSetup[i].keyName].ToString();
        }
    }

    public void SaveKeys()
    {
        foreach(var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    public void ChangeKey(GameObject clickedKey)
    {
        currentKey = clickedKey;
        //if we have a key selected
        if (clickedKey != null)
        {
            //change the colour of the key to the selected colour
            currentKey.GetComponent<Image>().color = selectedKey;
        }
    }
    private void OnGUI()
    {
        //temp reference to the string value of our keycode
        string newKey = "";
        //temp reference to current event
        Event e = Event.current;
        //if we have a key selected
        if(currentKey != null)
        {
            //if the key event is pressed
            if (e.isKey)
            {
                //our temp key reference is the event key press value
                newKey = e.keyCode.ToString();
            }
            //there is an issue getting left and right shift to register
            //the following part is q quick fix
            if (Input.GetKey(KeyCode.LeftShift))
            {
                newKey = "LeftShift";
            }
            if (Input.GetKey(KeyCode.RightShift))
            {
                newKey = "RightShift";
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                newKey = "Mouse0";
            }
            if(newKey != "")//if we have set a key
            {
                //changes the key value in the dictionary
                keys[currentKey.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey);
                //changes the display text to match the change
                currentKey.GetComponentInChildren<Text>().text = newKey;
                //change key colour to changed
                currentKey.GetComponent<Image>().color = changedKey;
                //forget the object we were editing
                currentKey = null;
            }
        }
    }
}



// SPRINT, CROUCH, ATTACK SET UP
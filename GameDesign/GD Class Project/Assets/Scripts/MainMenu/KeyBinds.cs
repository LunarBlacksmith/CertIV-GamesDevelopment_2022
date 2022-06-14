using System.Collections.Generic; //allows us to access and use Dictionaries
using UnityEngine; //Connects to Unity
using UnityEngine.UI; //Allows us to access and use Canvas UI elements

public class KeyBinds : MonoBehaviour
{
    [SerializeField] public static Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
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
        if (!PlayerPrefs.HasKey("FirstLoad"))
        {
            for (int i = 0; i < baseSetup.Length; i++)
            {
                //add key according to the saved string or default value
                keys.Add(baseSetup[i].keyName, (KeyCode)System.Enum.Parse(typeof(KeyCode),baseSetup[i].defaultKey));
            }
            HandleTextFile.WriteSaveFile();
            PlayerPrefs.SetString("FirstLoad", "");
        }
        else
        {
            HandleTextFile.ReadSaveFile();
        }

        for (int i = 0; i < baseSetup.Length; i++)
        {
            //for all the UI text elements change the display to what the bind is in our dictionary
            baseSetup[i].keyDisplayText.text = keys[baseSetup[i].keyName].ToString();
        }
        
    }

    public void SaveKeys()
    {
        //press return button
        HandleTextFile.WriteSaveFile();
    }
    public void ChangeKey(GameObject clickedKey_p)
    {
        currentKey = clickedKey_p;
        //if we have a key selected 
        if (clickedKey_p != null)
        {
            //change the colour of the key to the selected key colour
            clickedKey_p.GetComponent<Image>().color = selectedKey;
        }
    }

    private void OnGUI() //allow us to run events... such as key press
    {
        //temp reference to the string value of our keycode
        string newKey = "";
        //temp reference to the current event
        Event e = Event.current;
        //if we have a key selected 
        if (currentKey != null)
        {
            //if the event is a key press
            if (e.isKey)
            {
                //our temp key reference is the event key that was pressed
                newKey = e.keyCode.ToString();
            }
            //There is an issue with Unity in getting the left and right shift keys
            //The following part fixes this issue
            if (Input.GetKey(KeyCode.LeftShift))
            {
                newKey = "LeftShift";
            }
            if (Input.GetKey(KeyCode.RightShift))
            {
                newKey = "RightShift";
            }

            //if we have set a key
            if (newKey != "")
            {
                //change the key value in the Dictionary
                keys[currentKey.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey);
                //change the display text to match the changed key
                currentKey.GetComponentInChildren<Text>().text = newKey;
                //change the colour of our button to the changed colour
                currentKey.GetComponent<Image>().color = changedKey;
                //forget the object we were editing
                currentKey = null;
            }
        }
    }
}

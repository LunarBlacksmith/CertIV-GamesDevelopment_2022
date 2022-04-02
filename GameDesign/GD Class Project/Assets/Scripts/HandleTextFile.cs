using UnityEngine; //needed for Resources.Load()
using UnityEditor; //to make our own tool, update our editor
using System; //convert string to enum
using System.IO; //have access to characters from a byte stream


/*
bit == is a singular 0 or 1 
byte = 8 bits = 01100001 = 1 char
so a byte is a group of bits
a stream is known as a sequence
sequence is a succession
a byte stream is a succession of a group of bits
 */
public class HandleTextFile
{
    //at this file location
    static string path = "Assets/Resources/Save/Keybinds.txt";

    //Unity Editor allows me to create a tool in my Menus
    [MenuItem("Tool/Save/Write File")]
    //This is a public static behaviour that we can call in our scripts
    public static void WriteSaveFile()
    {
        //true - add to file
        //false - overwrite file
        StreamWriter sWriter = new StreamWriter(path, true);
        //write each of our keys in the file
        foreach (var keyEntry in KeyBinds.keys)
        {
            //each Key name and Key value will be written in with a : to separate them
            sWriter.Write(keyEntry.Key + ":" + keyEntry.Value.ToString());
        }
        //writing is done
        sWriter.Close();

        //re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load("Save/Keybinds.txt") as TextAsset;
    }

    [MenuItem("Tool/Save/Read File")]
    public static void ReadSaveFile()
    {
        //Read text from file
        StreamReader sReader = new StreamReader(path, true);

        //ref to the line we are reading
        string line;
        while ((line = sReader.ReadLine()) != null)
        {
            string[] parts = line.Split(':');
            //if we have keys and are just updating
            if (KeyBinds.keys.Count > 0)
            {
                KeyBinds.keys[parts[0]] = (KeyCode)System.Enum.Parse(typeof(KeyCode), parts[1]);
            }
            //else we need to also make the keys when we load
            else
            {
                KeyBinds.keys.Add(parts[0], (KeyCode)System.Enum.Parse(typeof(KeyCode), parts[1]));
            }
        } 
        sReader.Close();
    }

}

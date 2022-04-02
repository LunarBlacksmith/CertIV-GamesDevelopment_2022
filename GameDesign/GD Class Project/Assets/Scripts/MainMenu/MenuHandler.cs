using UnityEngine;
using UnityEngine.SceneManagement; //allows us to change and manipulate scenes

public class MenuHandler : MonoBehaviour
{
    //Change scene based on the scene index value
    public void ChangeScene(int sceneIndex_p)
    {
        //using Scene Manager load the scene that corresponds to the scene index int
        SceneManager.LoadScene(sceneIndex_p);
    }

    //Quit game
    public void QuitGame()
    {
        //this only runs if we are running the game in the Unity Editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //stops playmode
        #endif
        //Quits the application (only applies to game builds)
        Application.Quit();
    }
}

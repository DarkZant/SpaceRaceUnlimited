using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoutonsMenu : MonoBehaviour
{
    public void GoToRecords() => SceneManager.LoadScene("Records");  
    public void GoToSettings() => SceneManager.LoadScene("Settings");
    public void GoToLobby() => SceneManager.LoadScene("Lobby");
    public void Quitter()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

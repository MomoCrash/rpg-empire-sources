using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private string key = "unity_key_idle";

    public void Play()
    {

        SceneManager.LoadScene(1);

    }

    public void Reset()
    {
        var playerDataPath = Application.persistentDataPath + "/data.json";
        var ressourcesDataPath = Application.persistentDataPath + "/ressources_data.json";
        var buildingPath = Application.persistentDataPath + "/build_data.json";

        File.Delete(playerDataPath);
        File.Delete(ressourcesDataPath);
        File.Delete(buildingPath);

    }

    public void LeaveGame()
    {

        Application.Quit();

    }

}

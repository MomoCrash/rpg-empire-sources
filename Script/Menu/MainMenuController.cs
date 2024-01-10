using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    private string key = "unity_key_idle";
    public LoadingBar LoadingBar;

    public void Play()
    {

        LoadingBar.ActivateBar();
        StartCoroutine(LoadMainLevel());

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

    IEnumerator LoadMainLevel()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone)
        {
            LoadingBar.UpdateBar(asyncLoad.progress);
            Debug.Log(asyncLoad.progress);
            yield return null;
        }

    }

}

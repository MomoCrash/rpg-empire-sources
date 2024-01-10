using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    public static bool _IsPause = false;

    [SerializeField] SaveSystem Save;

    [SerializeField] GameObject PauseMenuObject;
    [SerializeField] GameObject SubExitMenuObject;

    void Update()
    {
        
        if (!_IsPause && Input.GetKeyDown(KeyCode.Escape))
        {

            PauseMenuObject.SetActive(true);

            Time.timeScale = 0f;
            _IsPause = true;

        } else if (Input.GetKeyDown(KeyCode.Escape))
        {

            UnPause();

        }

    }

    public void UnPause()
    {

        Time.timeScale = 1f;
        _IsPause = false;
        PauseMenuObject.SetActive(false);

    }

    public void TryExitGame()
    {

        PauseMenuObject.SetActive(false);
        SubExitMenuObject.SetActive(true);

    }

    public void Exit(bool save)
    {

        if (save)
        {
            Save.SaveGameData();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);

    }

}

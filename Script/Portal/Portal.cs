using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class Portal : MonoBehaviour
{

    private bool inTeleport = false;
    public int NextLevel;
    public LoadingBar Bar;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision != null && collision.CompareTag("Player"))
        {
            var gameObject = collision.gameObject;
            inTeleport = true;
            StartCoroutine(WaitForPlayerStay());
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision != null && collision.CompareTag("Player"))
        {
            inTeleport = false;
        }

    }

    public IEnumerator WaitForPlayerStay()
    {

        yield return new WaitForSeconds(3);

        if (inTeleport)
        {
            Bar.ActivateBar();
            StartCoroutine(LoadLevel(NextLevel));
        }

    }

    public IEnumerator LoadLevel(int level)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);

        while (!asyncLoad.isDone)
        {
            Bar.UpdateBar(asyncLoad.progress);
            yield return null;
        }

    }

}

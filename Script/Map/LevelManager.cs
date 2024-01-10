using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using TMPro;

public class LevelManager : MonoBehaviour
{

    [SerializeField] string MenuName;

    [SerializeField] string DisplayName;
    [SerializeField] string SubInfo;

    Animator Animator;
    bool Wait = false;

    void Start()
    {

        if (SceneManager.GetActiveScene().name == MenuName) return;
        
        gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DisplayName;
        gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = SubInfo;

        Animator = gameObject.GetComponent<Animator>();
        Animator.SetBool("out", false);
        Animator.SetBool("in", true);
        Wait=true;

        StartCoroutine(DelayAnimator());

    }

    IEnumerator DelayAnimator() {

        Wait=false;
        yield return new WaitForSeconds(4);
        Animator.SetBool("in", false);
        Animator.SetBool("out", true);
        yield return null;


    }

}

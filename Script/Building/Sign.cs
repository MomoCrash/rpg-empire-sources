using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class Sign : MonoBehaviour
{

    [SerializeField] string title;
    [SerializeField] GameObject titleObject;
    [SerializeField] [TextArea] string content;
    [SerializeField] GameObject contentObject;

    [SerializeField] Animator signAnimation;
    [SerializeField] Light2D signLight;

    private Text titleText;
    private Text contentText;

    // Start is called before the first frame update
    void Start()
    {
        titleText = titleObject.GetComponent<Text>();
        contentText = contentObject.GetComponent<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            signAnimation.SetBool("has_exit", false);

            titleText.text = title;
            contentText.text = content;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            signAnimation.SetBool("has_exit", true);
        }

    }

    public void ToggleLight()
    {
        if (signLight.enabled)
        {
            signLight.enabled = false;
        } else
        {
            signLight.enabled = true;
        }
    }

}

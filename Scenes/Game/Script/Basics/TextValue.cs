using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextValue : MonoBehaviour
{

    private Text text;
    public string textBase;
    public float value;

    public KeyCode code;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (code != KeyCode.None)
        {
            if (Input.GetKey(code))
            {
                text.enabled = true;
                if (value == -1)
                {
                    text.text = textBase;
                    return;
                }
                text.text = value.ToString("f2") + " " + textBase;
            } else
            {
                text.enabled = false;
            }
        } else
        {
            if (value == -1)
            {
                text.text = textBase;
                return;
            }
            text.text = value.ToString("f2") + " " + textBase;

        }
        
        
    }
}

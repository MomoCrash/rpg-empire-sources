using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    private AudioListener _audio;

    void Start()
    {

        _audio = gameObject.GetComponent<AudioListener>();
        
    }

    public void toggleSound() {

        if (_audio.enabled) {
            _audio.enabled = false;
        } else {
            _audio.enabled = true;
        }

    }
}

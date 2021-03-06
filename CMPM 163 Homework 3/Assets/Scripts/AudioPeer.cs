﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// create this required component....
[RequireComponent (typeof (AudioSource))]

public class AudioPeer : MonoBehaviour {

    AudioSource _audioSource;

    // NOTE: make this a 'static' float so we can access it from any other script.
    public static float[] spectrumData = new float[512];



	// Use this for initialization
	void Start () {
		
		_audioSource = gameObject.GetComponent<AudioSource> ();

    }


	// Update is called once per frame
	void Update () {

        GetSpectrumAudioSource();
    }


    void GetSpectrumAudioSource()
	{
		// this method computes the fft of the audio data, and then populates spectrumData with the spectrum data.
		_audioSource.GetSpectrumData (spectrumData, 0, FFTWindow.Hanning);
	}

    public void toggleMusic()
    {
        if(_audioSource.isPlaying)
        {
            _audioSource.Pause();
        }
        else
        {
            _audioSource.Play();
        }
    }
}



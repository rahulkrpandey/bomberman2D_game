using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource AS;
    public AudioClip expClip, collectClip, hurt;
    public static SoundManager sm;

    private void Awake()
    {
        sm = this;
    }

    public void PlayExplosion() {
        if (AS != null && expClip != null) {
            AS.PlayOneShot(expClip);
	    } else {
            //Debug.Log("Sound is not found");
        }
    }

    public void PlayCollected() {
        if (AS != null && collectClip != null) {
            AS.PlayOneShot(collectClip);
	    } else {
            //Debug.Log("Sound is not found");
        }
    }

    public void PlayHurt() {
		if (AS != null && hurt != null) {
		    AS.PlayOneShot(hurt);
	    } else {
            //Debug.Log("Sound is not found");
		}
    }
} 

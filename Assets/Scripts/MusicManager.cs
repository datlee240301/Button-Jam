using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public AudioSource bgSound;
    int musicId;


    private void Awake() {
        bgSound = GetComponent<AudioSource>();
    }

    private void Start() {
        musicId = PlayerPrefs.GetInt(StringManager.bgSound,1);
        PlayerPrefs.SetInt(StringManager.bgSound, musicId);
        SetUpVolume();
    }

    void SetUpVolume() {
        if (PlayerPrefs.GetInt(StringManager.bgSound) == 1)
            bgSound.volume = 1;
        else
            bgSound.volume = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public AudioSource soundSource;
    [SerializeField] AudioClip scissorSound;
    [SerializeField] AudioClip uiClickSound;
    [SerializeField] AudioClip levelCompleteSound;
    [SerializeField] AudioClip restoreSound;
    int soundId;

    private void Start() {
        soundSource = GetComponent<AudioSource>();
        soundId = PlayerPrefs.GetInt(StringManager.effectSound, 1);
        PlayerPrefs.SetInt(StringManager.effectSound, soundId);
        SetUpVolume();
    }

    void SetUpVolume() {
        if(PlayerPrefs.GetInt(StringManager.effectSound) == 1)
            soundSource.volume = 1;
        else
            soundSource.volume = 0;
    }

    public void PlayScissorSound() {
        soundSource.PlayOneShot(scissorSound);
    }

    public void PlayUIClickSound() {
        soundSource.PlayOneShot(uiClickSound);
    }

    public void PlayLevelCompleteSound() {
        soundSource.PlayOneShot(levelCompleteSound);
    }

    public void PlayRestoreSound() {
        soundSource.PlayOneShot(restoreSound);
    }
}

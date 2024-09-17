using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneUiManager : MonoBehaviour {
    [SerializeField] TextMeshProUGUI ticketNumberText;
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] GameObject noticePanel;
    [SerializeField] GameObject[] soundButtons;
    [SerializeField] GameObject[] musicButtons;
    [SerializeField] GameObject[] vibrateButtons;
    int ticketNumber;

    private void Awake() {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    private void Start() {
        SetUpTicket();
        SetUpSoundStatus();
    }

    void SetUpTicket() {
        ticketNumber = PlayerPrefs.GetInt(StringManager.ticketNumber, 5);
        PlayerPrefs.SetInt(StringManager.ticketNumber, ticketNumber);
        ticketNumberText.text = ticketNumber.ToString();
        currentLevelText.text = "LEVEL " + (PlayerPrefs.GetInt(StringManager.levelId)+1).ToString();
    }

    void SetUpSoundStatus() {
        if (PlayerPrefs.GetInt(StringManager.bgSound) == 1) {
            musicButtons[0].SetActive(false);
            musicButtons[1].SetActive(true);
        } else {
            musicButtons[0].SetActive(true);
            musicButtons[1].SetActive(false);
        }
        if (PlayerPrefs.GetInt(StringManager.effectSound) == 1) {
            soundButtons[0].SetActive(false);
            soundButtons[1].SetActive(true);
        } else {
            soundButtons[0].SetActive(true);
            soundButtons[1].SetActive(false);
        }

        if (PlayerPrefs.GetInt(StringManager.vibrate) == 1) {
            vibrateButtons[0].SetActive(false);
            vibrateButtons[1].SetActive(true);
        } else {
            vibrateButtons[0].SetActive(true);
            vibrateButtons[1].SetActive(false);
        }
    }

    /// button Zone

    public void BuyTicket(int number) {
        ticketNumber += number;
        ticketNumberText.text = ticketNumber.ToString();
        PlayerPrefs.SetInt(StringManager.ticketNumber, ticketNumber);
    }

    public void CallPanel() {
        FindObjectOfType<UiPanelDotween>().PanelFadeIn();
    }

    public void ShowPanel() {
        noticePanel.SetActive(true);
        StartCoroutine(HidePanel(noticePanel));
    }

    IEnumerator HidePanel(GameObject panel) {
        yield return new WaitForSeconds(1f);
        panel.SetActive(false);
    }

    public void MusicButton() {
        if (musicButtons[0].activeSelf) {
            musicButtons[0].SetActive(false);
            musicButtons[1].SetActive(true);
            FindObjectOfType<MusicManager>().bgSound.volume = 1;
            PlayerPrefs.SetInt(StringManager.bgSound, 1);
        } else {
            musicButtons[0].SetActive(true);
            musicButtons[1].SetActive(false);
            FindObjectOfType<MusicManager>().bgSound.volume = 0;
            PlayerPrefs.SetInt(StringManager.bgSound, 0);
        }
    }

    public void SoundButton() {
        if (soundButtons[0].activeSelf) {
            soundButtons[0].SetActive(false);
            soundButtons[1].SetActive(true);
            FindObjectOfType<SoundManager>().soundSource.volume = 1;
            PlayerPrefs.SetInt(StringManager.effectSound, 1);
        } else {
            soundButtons[0].SetActive(true);
            soundButtons[1].SetActive(false);
            FindObjectOfType<SoundManager>().soundSource.volume = 0;
            PlayerPrefs.SetInt(StringManager.effectSound, 0);
        }
    }

    public void VibrateButton() {
        if (vibrateButtons[0].activeSelf) {
            vibrateButtons[0].SetActive(false);
            vibrateButtons[1].SetActive(true);
            PlayerPrefs.SetInt(StringManager.vibrate, 1);
        } else {
            vibrateButtons[0].SetActive(true);
            vibrateButtons[1].SetActive(false);
            PlayerPrefs.SetInt(StringManager.vibrate, 0);
        }
    }

    public void LoadSceneButton(string sceneName) {
        if (sceneName == "PlayScene") {
            PlayerPrefs.SetInt(StringManager.playAgainId, 0);
            PlayerPrefs.SetInt(StringManager.currentLevelText, PlayerPrefs.GetInt(StringManager.levelId));
            SceneManager.LoadScene(sceneName);
        }
    }
}

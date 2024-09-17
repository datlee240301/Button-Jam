using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneUiManager : MonoBehaviour {
    public TextMeshProUGUI ticketNumberText;
    public TextMeshProUGUI ticketNumberText2;
    public TextMeshProUGUI timeText;
    public int ticketNumber;
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] TextMeshProUGUI nextLevelText;
    [SerializeField] UiPanelDotween uiPanelDotween;
    bool hasFadeIn = false;

    private void Awake() {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    private void Start() {
        ticketNumber = PlayerPrefs.GetInt(StringManager.ticketNumber);
        ticketNumberText.text = ticketNumber.ToString();
        ticketNumberText2.text = ticketNumber.ToString();
        currentLevelText.text = "Level " + (PlayerPrefs.GetInt(StringManager.currentLevelText) + 1);
        Debug.Log(PlayerPrefs.GetInt(StringManager.currentLevelText));
    }

    private void Update() {
        if (!hasFadeIn) {
            if (PlayerPrefs.GetInt(StringManager.levelProgressId) == 1) {
                uiPanelDotween.PanelFadeIn();
                FindObjectOfType<SoundManager>().PlayLevelCompleteSound();
                if(PlayerPrefs.GetInt(StringManager.playAgainId) == 1) 
                nextLevelText.text = "BACK TO MAIN MENU";
                hasFadeIn = true;
            }
        }
    }

    public void BuyTicket(int number) {
        ticketNumber += number;
        ticketNumberText.text = ticketNumber.ToString();
        ticketNumberText2.text = ticketNumber.ToString();
        PlayerPrefs.SetInt(StringManager.ticketNumber, ticketNumber);
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void NextLevelButton() {
        if (PlayerPrefs.GetInt(StringManager.levelProgressId) == 1) {
            if (PlayerPrefs.GetInt(StringManager.playAgainId) == 0) {
                Debug.Log(PlayerPrefs.GetInt(StringManager.playAgainId));
                //int nextLevelId = PlayerPrefs.GetInt(StringManager.levelId) + 1;
                //PlayerPrefs.SetInt(StringManager.levelId, nextLevelId);
                //PlayerPrefs.SetInt(StringManager.currentLevelText, nextLevelId);
                SceneManager.LoadScene("PlayScene");
            }
            else SceneManager.LoadScene("HomeScene");
        }
    }
}

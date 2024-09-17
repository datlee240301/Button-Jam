using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    [SerializeField] Image levelButton;
    [SerializeField] Sprite lockIcon;
    [SerializeField] Sprite onGoingIcon;
    [SerializeField] Sprite finishedIcon;
    [SerializeField] TextMeshProUGUI levelText;
    public int levelId;

    private void Start() {
        SetCurrentSprite();
        PlayerPrefs.SetInt(StringManager.playAgainId, 0);
        //Debug.Log(PlayerPrefs.GetInt(StringManager.levelId));
    }

    public void SetCurrentSprite() {
        levelText.text = (levelId + 1).ToString();
        if (levelId == PlayerPrefs.GetInt(StringManager.levelId)) {
            levelButton.sprite = onGoingIcon;
            levelButton.rectTransform.localPosition = new Vector3(levelButton.rectTransform.localPosition.x, levelButton.rectTransform.localPosition.y-13, levelButton.rectTransform.localPosition.z);
        } else if (levelId < PlayerPrefs.GetInt(StringManager.levelId))
            levelButton.sprite = finishedIcon;
        else if (levelId > PlayerPrefs.GetInt(StringManager.levelId))
            levelButton.sprite = lockIcon;
        levelButton.SetNativeSize();
    }

    public void LoadLevel() {
        if (levelButton.sprite == onGoingIcon) {
            PlayerPrefs.SetInt(StringManager.levelId, levelId);
            PlayerPrefs.SetInt(StringManager.currentLevelText, levelId);
            SceneManager.LoadScene("PlayScene");
        }
        else if (levelButton.sprite == finishedIcon) {
            PlayerPrefs.SetInt(StringManager.playAgainId, 1);
            PlayerPrefs.SetInt(StringManager.levelAgainId, levelId);
            PlayerPrefs.SetInt(StringManager.currentLevelText, levelId);
            Debug.Log(PlayerPrefs.GetInt(StringManager.playAgainId));
            SceneManager.LoadScene("PlayScene");
        }
        else if (levelButton.sprite == lockIcon)
            FindObjectOfType<HomeSceneUiManager>().ShowPanel();
    }
}

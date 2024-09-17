using System.Collections;
using UnityEngine;

public class GridManager : MonoBehaviour {
    public int gridWidth = 5;
    public int gridHeight = 5;
    public GameObject buttonPrefab;
    public ButtonConfig[] buttonConfig;
    int levelId;
    int levelAgainId;

    private ButtonManager[,] grid;
    [SerializeField] GameObject hand; 
    [SerializeField] GameObject brokenLine;
    private float handSpeed = 1.5f; 

    private void Awake() {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    void Start() {
        grid = new ButtonManager[gridWidth, gridHeight];
        GenerateButtonsFromConfig();

        if (hand != null && PlayerPrefs.GetInt(StringManager.handId) == 0) {
            StartCoroutine(MoveHandAndHide());
        }
    }

    void GenerateButtonsFromConfig() {
        if(PlayerPrefs.GetInt(StringManager.playAgainId) == 0) {
            levelId = PlayerPrefs.GetInt(StringManager.levelId);
            //levelId = 20;
            foreach (ButtonData buttonData in buttonConfig[levelId].buttonsData) {
                int x = buttonData.gridPosition.x;
                int y = buttonData.gridPosition.y;

                if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight) {
                    GameObject buttonObj = Instantiate(buttonPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    ButtonManager button = buttonObj.GetComponent<ButtonManager>();
                    button.SetButtonSprite(buttonData.buttonSprite);
                    button.gridX = x;
                    button.gridY = y;

                    grid[x, y] = button;
                }
            }
        } else {
            levelAgainId = PlayerPrefs.GetInt(StringManager.levelAgainId);
            //levelId = 20;
            foreach (ButtonData buttonData in buttonConfig[levelAgainId].buttonsData) {
                int x = buttonData.gridPosition.x;
                int y = buttonData.gridPosition.y;

                if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight) {
                    GameObject buttonObj = Instantiate(buttonPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    ButtonManager button = buttonObj.GetComponent<ButtonManager>();
                    button.SetButtonSprite(buttonData.buttonSprite);
                    button.gridX = x;
                    button.gridY = y;

                    grid[x, y] = button;
                }
            }
        }
    }

    IEnumerator MoveHandAndHide() {
        hand.SetActive(true);
        brokenLine.SetActive(true);
        hand.transform.position = new Vector3(1, 3, 0); 
        brokenLine.transform.position = new Vector3(1, 3, 0);
        Vector3 targetPosition = new Vector3(1, 0, 0);  
        yield return new WaitForSeconds(0.5f);
        while (Vector3.Distance(hand.transform.position, targetPosition) > 0.01f) {
            hand.transform.position = Vector3.MoveTowards(hand.transform.position, targetPosition, handSpeed * Time.deltaTime);
            yield return null;
        }
        hand.transform.position = targetPosition; // Ensure the hand reaches the target position
        yield return new WaitForSeconds(0.5f);
        hand.SetActive(false); 
        brokenLine.SetActive(false);
        PlayerPrefs.SetInt(StringManager.handId, 1);
    }
}

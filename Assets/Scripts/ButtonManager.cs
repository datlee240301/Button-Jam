using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {
    [SerializeField] GameObject border;
    public Sprite buttonSprite;
    public int gridX;
    public int gridY;

    private SpriteRenderer spriteRenderer;
    private Vector3 originalPosition;
    private bool wasHidden = false;
    private Rigidbody2D rb;
    private Collider2D buttonCollider;
    private bool originalIsTriggerState;
    public int countButton;

    // Static list to track all Button instances
    public static List<ButtonManager> allButtons = new List<ButtonManager>();

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetButtonSprite(buttonSprite);
        originalPosition = transform.position;

        buttonCollider = GetComponent<Collider2D>();
        if (buttonCollider != null) {
            originalIsTriggerState = buttonCollider.isTrigger;
        }

        // Add this button to the list when it starts
        allButtons.Add(this);
        PlayerPrefs.SetInt(StringManager.levelProgressId, 0);
    }

    private void OnDestroy() {
        // Remove this button from the list when it is destroyed
        allButtons.Remove(this);
    }

    public void SetButtonSprite(Sprite sprite) {
        buttonSprite = sprite;
        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = buttonSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Barrier")) {
            gameObject.SetActive(false);
            wasHidden = true;
            FindObjectOfType<InputManager>().RegisterHiddenButton(this);
            CountActiveButtons();
            countButton = CountActiveButtons();
            Debug.Log(CountActiveButtons());
            if (CountActiveButtons() == 0) {
                PlayerPrefs.SetInt(StringManager.levelProgressId, 1);
                Debug.Log(PlayerPrefs.GetInt(StringManager.levelId));
                int nextLevelId = PlayerPrefs.GetInt(StringManager.levelId) + 1;
                PlayerPrefs.SetInt(StringManager.levelId, nextLevelId);
                PlayerPrefs.SetInt(StringManager.currentLevelText, nextLevelId);
                FindObjectOfType<TimeManager>().StopTimer();
                FindObjectOfType<PlaySceneUiManager>().timeText.text = FindObjectOfType<TimeManager>().timerText.text;
            }
        } else if (collision.gameObject.CompareTag("Scissor")) {
            FindObjectOfType<Scissor>().scissorAnim.SetTrigger("isCut");
            FindObjectOfType<SoundManager>().PlayScissorSound();
            StartCoroutine(DropButton());
        }
    }

    IEnumerator DropButton() {
        if (rb != null) {
            yield return new WaitForSeconds(0.1f);
            rb.gravityScale = 1; // Enable gravity
        }
    }

    public void ShowBorder() {
        border.SetActive(true);
    }

    public void HideBorder() {
        border.SetActive(false);
    }

    public void RestorePositionAndShow() {
        if (wasHidden) {
            gameObject.SetActive(true);

            // Set collider to non-trigger while moving back to the original position
            if (buttonCollider != null) {
                buttonCollider.isTrigger = false;
            }

            // Start coroutine to move slowly to the original position
            StartCoroutine(MoveToOriginalPosition());

            wasHidden = false;
        }
    }

    private IEnumerator MoveToOriginalPosition() {
        float duration = 0.5f;  // Time to move
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;
        while (elapsedTime < duration) {
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the button stops exactly at the original position
        transform.position = originalPosition;

        // Restore Rigidbody2D state
        if (rb != null) {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;  // Reset gravityScale to prevent falling
            rb.isKinematic = false;  // Ensure Rigidbody2D is not kinematic
        }

        // Set collider back to trigger after moving
        if (buttonCollider != null) {
            buttonCollider.isTrigger = originalIsTriggerState;
        }
    }

    // Static method to count active buttons
    public static int CountActiveButtons() {
        int count = 0;
        foreach (ButtonManager button in allButtons) {
            if (button.gameObject.activeSelf) {
                count++;
            }
        }
        return count;
    }
}

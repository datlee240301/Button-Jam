using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public GridManager gridManager;
    private List<ButtonManager> selectedButtons = new List<ButtonManager>();
    private List<ButtonManager> hiddenButtons = new List<ButtonManager>(); // Danh sách các nút bị ẩn
    private bool isSelecting = false;
    bool isTouch = true;
    bool isRestore = true;
    public bool isRestored = true;
    [SerializeField] GameObject scissor;
    [SerializeField] float scissorSpeed = 5f;

    void Update() {
        if (isTouch) {
            if (Input.GetMouseButtonDown(0)) {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ButtonManager button = GetButtonAtPosition(mousePos);

                if (button != null) {
                    selectedButtons.Clear();
                    selectedButtons.Add(button);
                    button.ShowBorder();
                    isSelecting = true;
                }
            }

            if (Input.GetMouseButton(0) && isSelecting) {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ButtonManager button = GetButtonAtPosition(mousePos);

                if (button != null && !selectedButtons.Contains(button)) {
                    if (button.buttonSprite == selectedButtons[selectedButtons.Count - 1].buttonSprite) {
                        selectedButtons.Add(button);
                        button.ShowBorder();
                    } else {
                        isSelecting = false;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                if (selectedButtons.Count > 1 && isSelecting) {
                    if (IsValidSelection(selectedButtons)) {
                        StartCoroutine(MoveScissorAndDropButtons()); // Bắt đầu di chuyển scissor và xử lý nút
                    }
                }

                HideAllBorders();
                selectedButtons.Clear();
                isSelecting = false;
            }
        }
        if(scissor.activeSelf)
            isRestore = false;
        else
            isRestore = true;
    }

    private IEnumerator MoveScissorAndDropButtons() {
        if (scissor != null && selectedButtons.Count > 0) {
            isTouch = false;
            // Đặt scissor tại vị trí của nút đầu tiên
            scissor.transform.position = selectedButtons[0].transform.position;
            scissor.SetActive(true);

            // Di chuyển scissor từ nút đầu tiên đến nút cuối cùng
            Vector3 startPosition = selectedButtons[0].transform.position;
            Vector3 endPosition = selectedButtons[selectedButtons.Count - 1].transform.position;

            float distance = Vector3.Distance(startPosition, endPosition);
            float travelTime = distance / scissorSpeed;
            float elapsedTime = 0f;

            // Tính toán góc quay của scissor
            float angle = Mathf.Atan2(endPosition.y - startPosition.y, endPosition.x - startPosition.x) * Mathf.Rad2Deg;
            scissor.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            while (elapsedTime < travelTime) {
                scissor.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / travelTime);
                elapsedTime += Time.deltaTime;

                // Cập nhật góc quay của scissor trong khi di chuyển
                float currentAngle = Mathf.Atan2(endPosition.y - startPosition.y, endPosition.x - startPosition.x) * Mathf.Rad2Deg;
                scissor.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));

                // Kiểm tra va chạm với các button
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(scissor.transform.position, 0.1f);
                foreach (var hitCollider in hitColliders) {
                    if (hitCollider.CompareTag("Button")) {
                        // Dừng lại 1 giây khi chạm vào button
                        yield return new WaitForSeconds(.25f);
                    }
                }

                yield return null;
            }

            // Đảm bảo scissor ở vị trí cuối cùng và góc quay đúng
            scissor.transform.position = endPosition;
            scissor.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Ẩn scissor sau khi di chuyển xong
            yield return new WaitForSeconds(0.25f);
            scissor.SetActive(false);
            isTouch = true;

            // Thực hiện việc làm rơi các nút đã chọn
            foreach (ButtonManager button in selectedButtons) {
                DropButton(button);
            }
        }
    }


    public void RegisterHiddenButton(ButtonManager button) {
        if (!hiddenButtons.Contains(button)) {
            // Nếu danh sách đã có 3 button, loại bỏ button cũ nhất
            if (hiddenButtons.Count >= 3) {
                hiddenButtons.RemoveAt(0);
            }
            hiddenButtons.Add(button);
        }
    }

    public void RestoreHiddenButtons() {
        if (isRestore) {
            if (PlayerPrefs.GetInt(StringManager.ticketNumber) > 0) {
                int countToRestore = Mathf.Min(3, hiddenButtons.Count);
                Dictionary<Sprite, List<ButtonManager>> spriteGroups = new Dictionary<Sprite, List<ButtonManager>>();

                for (int i = hiddenButtons.Count - countToRestore; i < hiddenButtons.Count; i++) {
                    ButtonManager button = hiddenButtons[i];
                    Sprite sprite = button.buttonSprite;

                    if (!spriteGroups.ContainsKey(sprite)) {
                        spriteGroups[sprite] = new List<ButtonManager>();
                    }
                    spriteGroups[sprite].Add(button);
                }

                Sprite mostCommonSprite = null;
                int maxCount = 0;
                foreach (var group in spriteGroups) {
                    if (group.Value.Count > maxCount) {
                        mostCommonSprite = group.Key;
                        maxCount = group.Value.Count;
                    }
                }

                if (mostCommonSprite != null && maxCount >= 2) {
                    FindObjectOfType<SoundManager>().PlayRestoreSound();
                    FindObjectOfType<PlaySceneUiManager>().ticketNumber -= 1;
                    FindObjectOfType<PlaySceneUiManager>().ticketNumberText.text = FindObjectOfType<PlaySceneUiManager>().ticketNumber.ToString();
                    FindObjectOfType<PlaySceneUiManager>().ticketNumberText2.text = FindObjectOfType<PlaySceneUiManager>().ticketNumber.ToString();
                    PlayerPrefs.SetInt(StringManager.ticketNumber, FindObjectOfType<PlaySceneUiManager>().ticketNumber);
                    foreach (ButtonManager button in spriteGroups[mostCommonSprite]) {
                        button.RestorePositionAndShow();
                    }

                    hiddenButtons.RemoveAll(button => button.buttonSprite == mostCommonSprite);
                }
            }
        }
    }

    ButtonManager GetButtonAtPosition(Vector2 pos) {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider != null) {
            return hit.collider.GetComponent<ButtonManager>();
        }
        return null;
    }

    bool IsValidSelection(List<ButtonManager> buttons) {
        if (buttons.Count < 2) return false;

        bool isRow = true;
        bool isColumn = true;
        bool isDiagonal = true;

        int firstX = buttons[0].gridX;
        int firstY = buttons[0].gridY;
        Sprite firstSprite = buttons[0].buttonSprite;

        foreach (ButtonManager button in buttons) {
            if (button.gridY != firstY)
                isRow = false;
            if (button.gridX != firstX)
                isColumn = false;
            if (Mathf.Abs(button.gridX - firstX) != Mathf.Abs(button.gridY - firstY))
                isDiagonal = false;
            if (button.buttonSprite != firstSprite)
                return false;
        }

        return isRow || isColumn || isDiagonal;
    }

    void DropButton(ButtonManager button) {
        Rigidbody2D rb = button.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.gravityScale = 1;
            rb.isKinematic = false;
        }
    }

    void HideAllBorders() {
        foreach (ButtonManager button in selectedButtons) {
            button.HideBorder();
        }
    }
}
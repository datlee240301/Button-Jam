using System.Collections;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    float elapsedTime = 0f;
    int secondsCount = 0;
    private bool isRunning = false;
    public TextMeshProUGUI timerText;

    void Start() {
        StartTimer();
    }

    public void StartTimer() {
        isRunning = true;
        StartCoroutine(UpdateTimer());
    }

    private void Update() {
        UpdateTimerText();
    }

    public void StopTimer() {
        isRunning = false;
        StopCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer() {
        while (isRunning) {
            yield return new WaitForSeconds(1f);
            //elapsedTime += 1f; 
            secondsCount++;
        }
    }

    public float GetElapsedTime() {
        return elapsedTime;
    }

    public int GetSecondsCount() {
        return secondsCount;
    }

    private void UpdateTimerText() {
        int minutes = Mathf.FloorToInt(secondsCount / 60);
        int seconds = Mathf.FloorToInt(secondsCount % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
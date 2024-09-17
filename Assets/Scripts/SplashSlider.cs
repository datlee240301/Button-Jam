using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashSlider : MonoBehaviour {
    [SerializeField] Image image;
    [SerializeField] float fillSpeed = 0.2f; // Tốc độ tăng fillAmount

    // Start is called before the first frame update
    void Start() {
        image.fillAmount = 0;
        StartCoroutine(IncreaseFillAmountValue());
    }

    IEnumerator IncreaseFillAmountValue() {
        while (image.fillAmount < 1) {
            image.fillAmount += fillSpeed * Time.deltaTime; // Tăng dần fillAmount
            yield return null; // Đợi frame tiếp theo để tiếp tục
        }
        SceneManager.LoadScene("HomeScene"); // Khi fillAmount đạt 1, chuyển scene
    }
}

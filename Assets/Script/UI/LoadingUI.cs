using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField]
    private Text loadText;
    [SerializeField]
    private Image loadImage;
    private void Start()
    {
        ActiveLoadingImage(false);
    }
    public void StartLoadingUI()
    {
        LoadingProgress(0f);
    }
    public void LoadingProgress(float progress)
    {
        loadImage.fillAmount = progress;
        loadText.text = $"Loading...({(progress * 100).ToString("F2")}%)";
    }
    public void ActiveLoadingImage(bool active)
    {
        this.transform.gameObject.SetActive(active);
    }
}

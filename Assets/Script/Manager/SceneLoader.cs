using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    static public SceneLoader Instance { get; private set; }
    [SerializeField] private LoadingUI loadingUI;
    public string[] scene;
    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        scene[0] = "StartScene";
        scene[1] = "GameScene";
        scene[2] = "RankScene";
    }

    public void SceneChange(string index)
    {
        StartCoroutine(LoadScene(index));
    }
    private IEnumerator LoadScene(string scene)
    {
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        loadingUI.gameObject.SetActive(true);
        loadingUI.StartLoadingUI();
        float currentTIme = 0f;
        float maxTime = 10f;
        while (!operation.isDone)
        {
            float percent = currentTIme / maxTime;
            float result = Mathf.Min(percent, operation.progress * maxTime);

            loadingUI.LoadingProgress(result);


            if (currentTIme >= maxTime)
            {
                operation.allowSceneActivation = true;
                loadingUI.ActiveLoadingImage(false);
                break;
            }

            currentTIme += Time.deltaTime;
            yield return null;
        }

    }
}

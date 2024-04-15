using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text nicknameText;
    public Text scoreText;
    public GameObject LogoutPannel;
    public Button LogoutButton;
    bool ison = false;


    private void Start()
    {
        nicknameText.text = FirebaseManager.instance.user.DisplayName;
        LogoutButton.onClick.AddListener(LogoutButtonClick);
        LogoutPannel.SetActive(false);
    }


    public void LogoutButtonClick()
    {
        FirebaseManager.instance.Logout();
        SceneLoader.Instance.SceneChange(SceneLoader.Instance.scene[0]);
    }
    private void Update()
    {
        scoreText.text = GameManager.Instance.score.ToString();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (ison == true)
            {
                ison = false;
            }
            else
            {
                ison = true;
            }

            LogoutPannel.SetActive(ison);
        }
    }

}

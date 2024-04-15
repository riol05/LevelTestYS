using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class LoginPannel : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    
    
    [SerializeField]
    private Button loginButton;
    [SerializeField]
    private Button joinButton;
    [SerializeField]
    private InputField emailInput;
    [SerializeField]
    private InputField pwInput;
    [SerializeField]
    private GameObject changeNicknamePannel;

    public Text infoText;
    private void Start()
    {
        startButton.onClick.AddListener(StartButtonClick);
        loginButton.onClick.AddListener(LoginButtonClick);
        joinButton.onClick.AddListener(JoinButtonClick);

    }
    private void OnEnable()
    {
        UIInteractableUpdate();
    }

    public void LoginPanelOpen(bool ison)
    {
        loginButton.gameObject.SetActive(ison);
        joinButton.gameObject.SetActive(ison);
        emailInput.gameObject.SetActive(ison);
        pwInput.gameObject.SetActive(ison);
        changeNicknamePannel.gameObject.SetActive(!ison);
        startButton.gameObject.SetActive(!ison);
    }
    private void StartButtonClick()
    {
        changeNicknamePannel.gameObject.SetActive(false);
        SceneLoader.Instance.SceneChange(SceneLoader.Instance.scene[1]);
    }
    private void LoginButtonClick()
    {
        FirebaseManager.instance.Login(emailInput.text, pwInput.text, OnLogin);
        LoginPanelOpen(false);

    }
    private void JoinButtonClick()
    {
        FirebaseManager.instance.CreateUser(emailInput.text, pwInput.text,
    user =>
    {
        LoginPanelOpen(true);
    });
        // 회원가입
    }

    public void UIInteractableUpdate()
    {
        if (FirebaseManager.instance != null)
        {
            emailInput.interactable = FirebaseManager.instance.isInitialize;
            pwInput.interactable = FirebaseManager.instance.isInitialize;
            joinButton.interactable = FirebaseManager.instance.isInitialize;
            loginButton.interactable = FirebaseManager.instance.isInitialize;
        }
    }

    //public void OnLoginButtonClick()
    //{
    //    // 먼저 firebase 로그인 진행후 photon 로그인
    //    // auth value 를 활용하여 유저의 고유 ID를 부여
    //    FirebaseManager.instance.Login(emailInput.text, pwInput.text,
    //    	user =>
    //    	{
    //        });
    //     string userID = "";
         
    //     // firebase user ID를 할당.
    //     loginButton.interactable = false;
    //}

    public void OnLogin(FirebaseUser user)
    {
        string userId = user.UserId;
        FirebaseManager.instance.curUserData.userNickname = user.DisplayName;
        emailInput.interactable = false;
        loginButton.interactable = false;
        pwInput.interactable = false;
        joinButton.interactable = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNickname : MonoBehaviour
{
    public InputField nicknameInput;
    public Button changeNicknamebutton;
    public Text sayHello;


    public GameObject nicknameUpdate;
    public Button wannaChange;

    public void Start()
    {
        changeNicknamebutton.onClick.AddListener(ChangeNicknameButtonClick);
        wannaChange.onClick.AddListener(WannaChangeButtonClick);
    }

    public void WannaChangeButtonClick()
    {
        nicknameUpdate.SetActive(true);
        sayHello.text = $"������ �����ұ��? {FirebaseManager.instance.user.DisplayName}";
    }

    public void ChangeNicknameButtonClick()
    {
        FirebaseManager.instance.NicknameUpdate(nicknameInput.text,
            user =>
            {
                nicknameInput.interactable = true;
                changeNicknamebutton.interactable = true;

                sayHello.text = $"������ �����ұ��? {user.DisplayName}";
            }) ;
        nicknameInput.interactable = false;
        changeNicknamebutton.interactable = false;
    }

}

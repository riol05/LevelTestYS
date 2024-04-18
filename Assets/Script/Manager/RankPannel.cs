using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class RankPannel : MonoBehaviour
{
    public Button LoginPannelButton;

    public RectTransform rankPan;
    public GameObject rankPrefab;


    public List<string> rankList;
    private void Start()
    {
        LoginPannelButton.onClick.AddListener(LoginPannelButtonClick);
    }

    private void OnEnable()
    {
        RefreshRanking();
    }
    public void RefreshRanking()
    {
        FirebaseManager.instance.GetRanking(UpdateRankingUI);
    }

    Text nullText;

    private void UpdateRankingUI(List<string> rankData)
    {
        print($"{rankData.Count}");
        //print($"{rankData[0]}");

        // 기존 데이터를 지우고 새로운 데이터로 채움
        //ClearRankingUI();
        
            foreach (string playerText in rankData)
            {
                print(playerText);
                var man = Instantiate(rankPrefab, rankPan);
                man.GetComponent<RankUser>().rankText.text = playerText;
                nullText.text = playerText;
            }
        
       
    }

    private void ClearRankingUI()
    {
        foreach (Transform child in rankPan)
        {
            Destroy(child.gameObject);
        }
    }

    private void LoginPannelButtonClick()
    {
        SceneLoader.Instance.SceneChange(SceneLoader.Instance.scene[0]);
        FirebaseManager.instance.Logout();
    }
}

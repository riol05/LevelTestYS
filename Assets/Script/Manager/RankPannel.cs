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
    
    private void Start() => LoginPannelButton.onClick.AddListener(LoginPannelButtonClick);

    private void OnEnable() => RefreshRanking();
    public void RefreshRanking() => FirebaseManager.instance.GetRanking(UpdateRankingUI);
    private void UpdateRankingUI(List<string> rankData)
    {
        print($"{rankData.Count}");
        // 기존 데이터를 지우고 새로운 데이터로 채움
            foreach (string playerText in rankData)
            {
                print(playerText);
                var man = Instantiate(rankPrefab, rankPan);
                man.GetComponent<RankUser>().rankText.text = playerText;
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

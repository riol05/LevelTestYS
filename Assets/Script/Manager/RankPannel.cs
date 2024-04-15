using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPannel : MonoBehaviour
{
    public Button LoginPannelButton;

    public RectTransform rankPan;
    public RankUser rankPrefab;

    private void Start()
    {
        //LoginPannelButton.onClick.AddListener(LoginPannelButtonClick);
        RefreshRanking();
    }

    private void RefreshRanking()
    {
        FirebaseManager.instance.GetRanking((rankData) =>
        {
            // UI 업데이트를 수행하지 않고 가져온 데이터를 반환
            UpdateRankingUI(rankData);
        });
    }

    private void UpdateRankingUI(List<string> rankData)
    {
        // 기존 데이터를 지우고 새로운 데이터로 채움
        ClearRankingUI();
        foreach (string playerText in rankData)
        {
            RankUser man = Instantiate(rankPrefab, rankPan);
            man.rankText.text = playerText;
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

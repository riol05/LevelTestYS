using System.Collections;
using System.Collections.Generic;
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
        FirebaseManager.instance.GetRanking((rankData) =>
        {
            // UI 업데이트를 수행하지 않고 가져온 데이터를 반환
            UpdateRankingUI(rankData);
        });
    }

    private void UpdateRankingUI(List<string> rankData)
    {
        print($"{rankData.Count}");
        print($"{rankData[0]}");
        print($"{rankData[1]}");
        print($"{rankData[2]}");

        // 기존 데이터를 지우고 새로운 데이터로 채움
        //ClearRankingUI();
        foreach (string playerText in rankData)
        {
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

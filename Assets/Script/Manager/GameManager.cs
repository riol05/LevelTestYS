using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public Player player;

    bool isDone = false;
    public List<string> Rank;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
            isDone = false;
    }

    public void showRank()
    {
        rankPannel.gameObject.SetActive(true);
    }
    public void GetScore()
    {
        score += 1;
    }
    public RankPannel rankPannel;

    public void GameOver()
    {
        if (!isDone)
        {
            FirebaseManager.instance.UpdateRank(FirebaseManager.instance.user.DisplayName, score);
            isDone = true;
            showRank();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public RankPannel rankpan;
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

    }
    public void GetScore()
    {
        score += 1;
    }
    public void GameOver()
    {
        if (!isDone)
        {
            FirebaseManager.instance.UpdateRank(FirebaseManager.instance.user.DisplayName, score);

            SceneLoader.Instance.SceneChange(SceneLoader.Instance.scene[2]);
            isDone = true;
        }
    }
}

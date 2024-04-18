using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    FirebaseApp app;
    FirebaseDatabase db;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(
        task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                print("초기화 실패 : " + task.Status);
            }
            else if (task.IsCompleted)
            {
                print("파이어 베이스 초기화 성공" + task.Result);
                app = FirebaseApp.DefaultInstance;
                db = FirebaseDatabase.DefaultInstance;
            }
        }
        );
        yield return new WaitUntil(()=>db!=null);

        GetRanking((list) => {
        
            foreach (var item in list)
            {
                print(item);
            }

        });

    }

    public void GetRanking(Action<List<string>> callback)
    {
        Query rankingQuery = db.GetReference("users").Child("Ranking").OrderByValue().LimitToLast(10);

        rankingQuery.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting ranking: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            List<string> rankData = new List<string>();
            int index = 0;

            foreach (DataSnapshot playerSnapshot in snapshot.Children)
            {
                string playerName = playerSnapshot.Key;
                string playerScore = playerSnapshot.Value.ToString();

                string playerText;
                playerText = $"{index + 1}위.   {playerName}: {playerScore}";
                rankData.Add(playerText);

                index++;
            }

            Debug.Log(rankData.Count);

            callback?.Invoke(rankData);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance { get; private set; }
    public FirebaseApp app { get; private set; }
    public FirebaseAuth auth { get; private set; }
    public FirebaseDatabase db { get; private set; }
    public FirebaseUser user { get; private set; }

    public UserData curUserData { get; set; }
    public bool isInitialize = false;



    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        // 파이어 베이스 초기화
        FirebaseInitialize();
        yield return new WaitUntil(() => app != null && auth != null && auth != null && db != null);
        isInitialize = true;
        // 로그인 활성화
        yield return new WaitUntil(() => user != null);
        //로그인 후 메인 메뉴로 넘어감
        //PanelManager.Instance.PanelOpen("Menu");
    }

    private void FirebaseInitialize()
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
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseDatabase.DefaultInstance;
            }
        }
        );
    }
    public void CreateUser(string email, string password, Action<FirebaseUser> callback = null)
    {
        // 회원가입
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(
            task => {
                if (task.IsFaulted)
                {
                    if (task.Exception != null)
                    {
                        print(task.Exception.Message);
                        return;
                    }
                }
                else if (task.IsCompleted)
                {
                    print("회원가입 성공");
                    user = task.Result.User;
                    UserData userData = new UserData();
                    userData.Score = 0;
                    userData.userNickname = "";
                    curUserData = userData;

                    callback?.Invoke(task.Result.User);
                    var userRef = db.GetReference("users").Child(user.UserId);
                    userRef.Child("Score").SetValueAsync(userData.Score);
                    userRef.Child("userNickname").SetValueAsync(userData.userNickname);
                }
            }
        );
    }


    public void Login(string email, string password, Action<FirebaseUser> callback = null)
    {
        // 로그인
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsFaulted)
                {
                    if (task.Exception != null)
                    {
                        print(task.Exception.Message);
                        return;
                    }
                }
                else if (task.IsCompleted)
                {
                    print("로그인 성공");
                    this.user = task.Result.User;
                    var userRef = db.GetReference("users").Child(user.UserId);
                    curUserData = new UserData();
                    userRef.Child("Score").GetValueAsync().ContinueWithOnMainThread(
                        snap =>
                        {
                            curUserData.Score = int.Parse(snap.Result.Value.ToString());
                            userRef.Child("userNickname").GetValueAsync().ContinueWithOnMainThread(
                                snap =>
                                {
                                    curUserData.userNickname = snap.Result.Value.ToString();
                                    callback?.Invoke(task.Result.User);
                                });
                        });
                }
            }
        );
    }
    public void NicknameUpdate(string nickname, Action<FirebaseUser> callback = null)
    {
        var task = auth.CurrentUser.UpdateUserProfileAsync(new UserProfile() { DisplayName = nickname });
        task.ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                //print("닉네임 갱신 성공");
                callback?.Invoke(auth.CurrentUser);
            }
            else if (task.IsFaulted)
            {
                // 닉네임 갱신 실패
                Debug.LogError(task.Exception.Message);
                Debug.LogError(task.Status);
            }
        });
    }

    public List<string> RankData;

    public void UpdateRank(string userId, int score)
    {
        var userRef = db.GetReference("users").Child("Ranking").Child(userId);
        userRef.SetValueAsync(score);
    }
    //public async void GetRanking(Action<List<string>> callback) // 주석대로 바꾸는게 조금 더 세련된 방법이었다..
    public async void GetRanking(Action<List<string>> callback)
    {
        Query rankingQuery = db.GetReference("users").Child("Ranking").OrderByValue().LimitToLast(7);

        //var datasnap = await rankingQuery.GetValueAsync();

        await rankingQuery.GetValueAsync().ContinueWithOnMainThread(task =>
            {
            if (task.IsFaulted)
            {
                Debug.LogError("Error getting ranking: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            List<string> rankData = new List<string>();
            Debug.Log("ChidrenCount"+snapshot.ChildrenCount);
            
            foreach (DataSnapshot playerSnapshot in snapshot.Children)
            {
                string playerName = playerSnapshot.Key;
                string playerScore = playerSnapshot.Value.ToString();

                string playerText;
                playerText = $"   {playerName}: {playerScore}";
                rankData.Add(playerText);
            }
            rankData.Reverse();
            List<string> rank2 = new List<string>();
            int index = 0;
                foreach (string rank in rankData)
                {
                    ++index;
                    string rankText;
                     rankText = $"{index}위."+rank;
                    rank2.Add(rankText);
                }
            Debug.Log(rank2.Count);
           
            callback?.Invoke(rank2);
        });
    }
    public void Logout()
    {
        auth.SignOut();
    }
}

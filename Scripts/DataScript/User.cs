using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : Singleton<User>
{
    private string userId;
    private string username;
    private int papel;
    private string userInfo;
    private bool noAds;

    public string Username { get => username; set => username = value; }
    public int Papel { get => papel; set => papel = value; }
    public string UserId { get => userId; set => userId = value; }
    public string UserInfo { get => userInfo; set => userInfo = value; }
    public bool NoAds { get => noAds; set => noAds = value; }
}

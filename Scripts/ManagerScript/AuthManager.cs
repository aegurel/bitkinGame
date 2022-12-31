using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthManager : Singleton<AuthManager>
{
    public FirebaseAuth auth;
    public User user;
    private RegisterScript rs;
    private void Awake()
    {
        rs = GameObject.FindObjectOfType<RegisterScript>();
        user = User.Instance;
        auth = FirebaseAuth.DefaultInstance;
    }

    public void SignUp(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {            
            if (!task.IsCompleted)
            {
                user.UserInfo = "Hata tekrar deneyiniz";
                return;
            }
            if (task.IsCanceled)
            {
                user.UserInfo = "Hata tekrar deneyiniz";
                return;
            }
            if (task.IsFaulted)
            {
                user.UserInfo = "Hatalı email veya şifre girdiniz";
                return;
            }

            FirebaseUser newUser = task.Result;
            user.UserId = newUser.UserId;
        });
    }
    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                user.UserInfo = "Hata tekrar deneyiniz";
                return;
            }
            if (task.IsFaulted)
            {
                user.UserInfo = "Hatalı email veya şifre girdiniz";
                return;
            }
            if (task.IsCanceled)
            {
                user.UserInfo = "Hata tekrar deneyiniz";
                return;
            }

            FirebaseUser newUser = task.Result;
            user.UserId = newUser.UserId;
        });
    }
    public void AutoLogin(string userId)
    {
        FirebaseUser firebaseUser = auth.CurrentUser;
        user.UserId = userId;
    }
    public void ResetPassword(string email)
    {
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                user.UserInfo = "Hata tekrar deneyiniz";
                return;
            }
            if (task.IsFaulted)
            {
                user.UserInfo = "Hatalı email girdiniz";
                return;
            }
        });
    }
}

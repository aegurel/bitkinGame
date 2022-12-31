using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Messaging;
using System;

public class cloudmes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += TokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += MessageReceived;
    }
    void subscribe()
    {
        //Firebase.Messaging.FirebaseMessaging.SubscribeAsync("/topics/projectname"); //this is cloud messaging function if you want to use this function, firstly you have to set firebase cloud messaging part.
    }

    private void MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("bildirim " + e.Message);
    }

    private void TokenReceived(object sender, TokenReceivedEventArgs e)
    {
        Debug.Log("Token " + e.Token);
    }
}

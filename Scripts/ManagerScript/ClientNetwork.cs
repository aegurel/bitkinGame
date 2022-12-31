using WebSocketSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClientNetwork : MonoBehaviour
{
    private User user;
    WebSocket ws;
    private string urlPlant = "ws://localhost:3000";
    Message mesajClass;

    public List<SpecialChatData> specialList = new List<SpecialChatData>();

    //GELEN MESAJ
    [Serializable]
    public struct MessagPlant
    { 
        public int valueWater;
        public int valueSun;
        public int valueBlower;
        public int valueHealth;
        public string message;
        
        public string chatUsername;
        public string specialChatUsername;

        public string chatMessage;
        public string specialChatMessage;
    }
    MessagPlant messag;


    //GİDEN MESAJ
    [Serializable]
    public struct MessagChat
    {
        public string message;
        public string username;
        public string chatMessage;
    }
    MessagChat messagChat;
    
    [Serializable]
    public struct MessagSpecialChat
    {
        public string message;
        public string username;
        public string chatSpecialMessage;
        public string userId;
    }
    MessagSpecialChat messagSpecialChat;

    [Serializable]
    public struct MessagData
    {
        public string message;
    }
    MessagData messagData;
    // Start is called before the first frame update
    void Start()
    {
        specialList.Clear();
        user = User.Instance;
        mesajClass = Message.Instance;
        incomingSocketMessage();
        ws.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if (ws == null)
        {
            return;
        }
    }

    public void sendData(string data)
    {
        messagData.message = data;
        string json = JsonUtility.ToJson(messagData);
        ws.Send(json);
    }
    public void sendChat(string mesaj)
    {
        messagChat.message = "Chat";
        messagChat.username = user.Username;
        messagChat.chatMessage = mesaj;

        string json = JsonUtility.ToJson(messagChat);
        ws.Send(json);
    }
    public void sendSpecialChat(string mesaj)
    {
        messagSpecialChat.userId = user.UserId;
        messagSpecialChat.message = "SpecialChat";
        messagSpecialChat.username = user.Username;
        messagSpecialChat.chatSpecialMessage = mesaj;

        string json = JsonUtility.ToJson(messagSpecialChat);
        ws.Send(json);
    }
    public void incomingSocketMessage()
    {     
        ws = new WebSocket(urlPlant);
        ws.OnMessage += (sender, mes) =>
        {
            Debug.Log(sender.ToString());
            string json = mes.Data;
            messag = JsonUtility.FromJson<MessagPlant>(json);
            Debug.Log("burdayım");
            processSocketPlant();
        };
    }

    public void processSocketPlant()
    {
        if (messag.message.Equals("Chat"))
        {
            mesajClass.Chat = messag.chatMessage;
            mesajClass.OtherUsername = messag.chatUsername;
        }
        else if (messag.message.Equals("SpecialChat"))
        {
            SpecialChatData sp = new SpecialChatData(messag.specialChatUsername, messag.specialChatMessage);
            specialList.Add(sp);
        }
        else if (messag.message.Equals("plant"))
        {
            mesajClass.ValueWater = messag.valueWater;
            mesajClass.ValueSun = messag.valueSun;
            mesajClass.ValueBlower = messag.valueBlower;
            mesajClass.Health = messag.valueHealth;
            mesajClass.Messag = messag.message;
        }        
    }

}


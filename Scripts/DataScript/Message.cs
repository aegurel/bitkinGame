using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : Singleton<Message>
{
    private string messag;
    private int valueWater;
    private int valueSun;
    private int valueBlower;
    private int health;
    private string chat;
    private string otherUsername;
    private string specialUsername;
    private string specialChat;

    public int ValueWater { get => valueWater; set => valueWater = value; }
    public int ValueSun { get => valueSun; set => valueSun = value; }
    public int ValueBlower { get => valueBlower; set => valueBlower = value; }
    public string Messag { get => messag; set => messag = value; }
    public int Health { get => health; set => health = value; }
    public string Chat { get => chat; set => chat = value; }
    public string OtherUsername { get => otherUsername; set => otherUsername = value; }
    public string SpecialUsername { get => specialUsername; set => specialUsername = value; }
    public string SpecialChat { get => specialChat; set => specialChat = value; }
}
   

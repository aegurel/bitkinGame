using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    ///Main Scene Script

    private DateTime clickTime;

    public Text usernameText;
    public Text papelText;
    public Text sendingText;
    public Text noticeText;
    public Text experimentTimeText;
    public Text deneySonucText;
    public Text waterLevel;
    public Text sunLevel;
    public Text moistureLevel;
    public Text healthLevel;
    public InputField inputChat;
    public Button waterButon;
    public Button sunButon;
    public Button blowButon;
    public Button rewardedButon;

    public GameObject chatArea;
    public GameObject scroll;
    public GameObject specialText;
    public GameObject waterBar;
    public GameObject moistureBar;
    public GameObject sunBar;
    public GameObject healthBar;
    public GameObject DeneyCanvas;
    public GameObject DeneyBittiCanvas;
    public GameObject ConnectionPanel;
    public GameObject loadingAnim;
    public GameObject loadingTransform;

    public Animator specialAnim;
    public Animator noticeAnim;
    public Animator waterAnim;
    public Animator ruzgarAnim;
    public Animator gunesAnim;
    //public Animator loadingAnim;

    private string water = "water";
    private string blower = "blow";
    private string sun = "sun";
    private string mesaj;

    private string urlUser = "http://localhost:3000/careme/api/info/papel";
    private string urlKotu = "http://localhost:3000/careme/api/info/badslist/update/";
    private string urlIyi = "http://localhost:3000/careme/api/info/goodslist/update/";
    private string urlSonuc = "http://localhost:3000/careme/api/info/app/sonuc";
    private string urlChat = "http://localhost:3000/careme/api/info/app/chat";

    private bool loadOver = false;
    public static bool IEWork = false;

    private int specialPrice = 100;
    public static float sure=0;

    private ClientNetwork cn;
    private Message mesajClass;
    private AdManagers ad;
    private User user;
    private Plant plant;


    [Serializable]
    public struct UserInfo
    {
        public int papel;
    }
    UserInfo[] userInfo;
    
    [Serializable]
    public struct DeneyInfo
    {
        public string sonuc;
    }
    DeneyInfo[] deneyInfo;
    
    [Serializable]
    public struct SentChat
    {
        public string username;
        public string chatMesajj;
    }
    SentChat[] sentChat;

    // Start is called before the first frame update
    private void Awake()
    {
        ConnectionPanel.SetActive(false);
        DeneyBittiCanvas.SetActive(false);    
        IEWork = false;
        plant = Plant.Instance;
        user = User.Instance;
        mesajClass = Message.Instance;

    }
    void Start()
    {
        ad = GameObject.FindObjectOfType<AdManagers>();
        //StartCoroutine(HasInternetConnection());              
    }

    // Update is called once per frame
    void Update()
    {
        if(mesajClass.Messag != null)
        {
            barEditor();
        }
        if(mesajClass.Chat != null)
        {
            chatEditor();
        }

        if (loadOver)
        {
            if (cn.specialList.Count > 0)
            {                
                StartCoroutine(specialChatEditor());
            }
        }
        TimeShower();
    }
    public void sendWater()
    {
        StartCoroutine(ButonTimer("w"));
        if (waterBar.GetComponent<Slider>().value > 550)
        {                        
            StartCoroutine(pushGoodAndBad(urlKotu+user.UserId));
            cn.sendData(water);
        }
        else
        {
            Debug.Log(urlIyi + user.UserId);
            StartCoroutine(pushGoodAndBad(urlIyi+user.UserId));
            cn.sendData(water);
        }

    }
    public void sendBlower()
    {
        StartCoroutine(ButonTimer("b"));
        if (moistureBar.GetComponent<Slider>().value > 550 )
        {
            StartCoroutine(pushGoodAndBad(urlKotu + user.UserId));
            cn.sendData(blower);
        }
        else
        {
            StartCoroutine(pushGoodAndBad(urlIyi + user.UserId));
            cn.sendData(blower);
        }
       
    }
    public void sendSun()
    {
        StartCoroutine(ButonTimer("s"));
        if (sunBar.GetComponent<Slider>().value > 550)
        {
            StartCoroutine(pushGoodAndBad(urlKotu + user.UserId));
            cn.sendData(sun);
        }
        else
        {
            StartCoroutine(pushGoodAndBad(urlIyi + user.UserId));
            cn.sendData(sun);
        }     
    }

    public void sendChat()
    {
        if (sendingText.text.Equals(""))
        {
            noticeText.text = "Lütfen mesaj alanını boş bırakmayın.";
            noticeAnim.SetTrigger("notice");
        }
        else
        {
            string chat = sendingText.text;
            inputChat.text = "";
            cn.sendChat(chat);
        }
    }

    public void sendSpecialChat()
    {
        if (sendingText.text.Equals(""))
        {
            noticeText.text = "Lütfen mesaj alanını boş bırakmayın.";
            noticeAnim.SetTrigger("notice");
        }
        else
        {
            if (user.Papel > 0)
            {
                StartCoroutine(GetUserInfo(urlUser + user.UserId));
            }
            else
            {
                SceneManager.LoadScene("BuyPapel");
            }
        }
    }
    
    public void barEditor()
    {
        int num = UnityEngine.Random.Range(1, 4);
        if (num == 1)
            waterAnim.SetTrigger("waterr");
        else if (num == 2)
            ruzgarAnim.SetTrigger("ruzgarr");
        else if (num == 3)
            gunesAnim.SetTrigger("Guness");

        waterBar.GetComponent<Slider>().value = mesajClass.ValueWater;
        sunBar.GetComponent<Slider>().value = mesajClass.ValueSun;
        moistureBar.GetComponent<Slider>().value = mesajClass.ValueBlower;

        waterLevel.text = mesajClass.ValueWater.ToString();
        sunLevel.text = mesajClass.ValueSun.ToString();
        moistureLevel.text = mesajClass.ValueBlower.ToString();
        healthLevel.text = mesajClass.Health.ToString();

        plant.WaterLevel= mesajClass.ValueWater;
        plant.SunLevel = mesajClass.ValueSun;
        plant.MoistureLevel = mesajClass.ValueBlower;
        plant.HealthLevel = mesajClass.Health;

        mesajClass.Messag = null;
    }
    public void chatEditor()
    {
        chatArea = Instantiate(chatArea, scroll.transform);
        chatArea.transform.GetChild(0).GetComponent<Text>().text = mesajClass.OtherUsername;
        chatArea.transform.GetChild(1).GetComponent<Text>().text = mesajClass.Chat;
        mesajClass.Chat = null;
    }
    IEnumerator pushGoodAndBad(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Errors " + www.error);
        }
    }
    //special chat is same as twitch donate messages. your message is created on top of every users phone  
    IEnumerator specialChatEditor()
    {
        loadOver = false;
        specialText.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = cn.specialList[0].specialUsername;
        specialText.transform.GetChild(1).GetComponent<Text>().text = cn.specialList[0].specialChat;
        specialAnim.SetTrigger("specialChat");
        cn.specialList.RemoveAt(0);
        yield return new WaitForSeconds(10);
        Graphic graphic = inputChat.placeholder;
        ((Text)graphic).text = "Mesajınızı giriniz...";
        loadOver = true;

    }

    IEnumerator GetUserInfo(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Errors " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            userInfo = JsonHelper.GetArray<UserInfo>(json);
            processSpecial();
        }
    }
    IEnumerator GetDeneyInfo(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Errors " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            deneyInfo = JsonHelper.GetArray<DeneyInfo>(json);
            deneySonucText.text = deneyInfo[0].sonuc+" Deney "+plant.ExperimentTime+" GÜN SÜRDÜ.";
        }
    }
    IEnumerator ButonTimer(string m)
    {
        IEWork = true;
        sure = 180f;
        mesaj = m;
        clickTime = DateTime.Now;
        string time = clickTime.ToString();
        PlayerPrefs.SetString("time", time);

        waterButon.interactable = false;
        sunButon.interactable = false;
        blowButon.interactable = false;
        rewardedButon.interactable = true;

        yield return new WaitForSeconds(180);
        rewardedButon.interactable = false;
        ButtonReset();
        PlayerPrefs.DeleteKey("time");
    }
    IEnumerator HasInternetConnection()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com");
        yield return www.SendWebRequest();

        if(www.error != null)
        {
            DestroyLoading();
            ConnectionPanel.SetActive(true);
            DeneyCanvas.SetActive(false);
            DeneyBittiCanvas.SetActive(false);
        }
        else
        {
            DestroyLoading();
            Initialization();
        }
    }
    IEnumerator chatList()
    {
        UnityWebRequest www = UnityWebRequest.Get(urlChat);
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            noticeText.text = "Mesajlar yüklenemedi. Tekrar deneyiniz.";
            noticeAnim.SetTrigger("notice");
        }
        else
        {
            string json = www.downloadHandler.text;
            sentChat = JsonHelper.GetArray<SentChat>(json);
            Debug.Log("sentchat");
            chatInitialization();
        }
    }

    void processSpecial()
    {
        if(userInfo[0].papel >= specialPrice)
        {
            user.Papel -= 100;
            papelText.text = user.Papel.ToString();
            string specialChat = sendingText.text;
            inputChat.text = "";
            Graphic graphic = inputChat.placeholder;
            ((Text)graphic).text = "Mesajınız herkese gönderildi :)";
            cn.sendSpecialChat(specialChat);         
        }
        else
        {
            papelText.text = userInfo[0].papel.ToString();
            noticeText.text = "Yeterli Papel'e sahip değilsiniz.";
            noticeAnim.SetTrigger("notice");
        }
        
    }
    void chatInitialization()
    {
        for(int i = 0; i < sentChat.Length; i++)
        {
            chatArea = Instantiate(chatArea, scroll.transform);
            chatArea.transform.GetChild(0).GetComponent<Text>().text = sentChat[i].username;
            chatArea.transform.GetChild(1).GetComponent<Text>().text = sentChat[i].chatMesajj;
        }
    }
    void Initialization()
    {
        ConnectionPanel.SetActive(false);
        if (plant.Expired)
        {
            DeneyCanvas.SetActive(true);
            DeneyBittiCanvas.SetActive(false);

            TimeManager();

            loadOver = true;
            waterBar.GetComponent<Slider>().value = plant.WaterLevel;
            sunBar.GetComponent<Slider>().value = plant.SunLevel;
            moistureBar.GetComponent<Slider>().value = plant.MoistureLevel;
            healthBar.GetComponent<Slider>().value = plant.HealthLevel;

            experimentTimeText.text = plant.ExperimentTime.ToString();
            usernameText.text = user.Username;
            papelText.text = user.Papel.ToString();

            waterLevel.text = plant.WaterLevel.ToString();
            sunLevel.text = plant.SunLevel.ToString();
            moistureLevel.text = plant.MoistureLevel.ToString();
            healthLevel.text = plant.HealthLevel.ToString();

            cn = GameObject.FindObjectOfType<ClientNetwork>();
            StartCoroutine(chatList());
        }
        else
        {
            DeneyCanvas.SetActive(false);
            DeneyBittiCanvas.SetActive(true);
            StartCoroutine(GetDeneyInfo(urlSonuc));
        }
        
    }
    void TimeManager()
    {
        if (PlayerPrefs.HasKey("time"))
        {
            string time = PlayerPrefs.GetString("time");
            DateTime a = DateTime.Parse(time);
            DateTime now = DateTime.Now;
            TimeSpan timeDifference = now.Subtract(a);

            if ((int)timeDifference.TotalSeconds > 180)
            {
                rewardedButon.interactable = false;
                waterButon.interactable = true;
                sunButon.interactable = true;
                blowButon.interactable = true;
                PlayerPrefs.DeleteKey("time");
            }
            else
            {
                rewardedButon.interactable = true;
                sure = 180-(float)timeDifference.TotalSeconds;
                waterButon.interactable = false;
                sunButon.interactable = false;
                blowButon.interactable = false;
            }
        }
    }
    void TimeShower()
    {
        if (sure > 0)
        {
            rewardedButon.interactable = true;
            sure -= Time.deltaTime;
            if (mesaj != null)
            {
                if (mesaj.Equals("w"))
                {
                    waterButon.transform.GetChild(0).GetComponent<Text>().text = (int)sure + "";
                }
                else if (mesaj.Equals("b"))
                {
                    blowButon.transform.GetChild(0).GetComponent<Text>().text = (int)sure + "";
                }
                else if (mesaj.Equals("s"))
                {
                    sunButon.transform.GetChild(0).GetComponent<Text>().text = (int)sure + "";
                }
            }
            else
            {
                waterButon.transform.GetChild(0).GetComponent<Text>().text = (int)sure + "";
            }
        }
        else
        {
            
            if (!IEWork)
            {
                rewardedButon.interactable = false;
                ButtonReset();
                PlayerPrefs.DeleteKey("Time");
            }
        }
    }
    void ButtonReset()
    {
        waterButon.transform.GetChild(0).GetComponent<Text>().text = "SU";
        blowButon.transform.GetChild(0).GetComponent<Text>().text = "RÜZGAR";
        sunButon.transform.GetChild(0).GetComponent<Text>().text = "GÜNEŞ";
        waterButon.interactable = true;
        sunButon.interactable = true;
        blowButon.interactable = true;
    }

    public void odulluReklamIstek()
    {
        ad.odulluReklam();
    }

    public void listScene()
    {
        if (user.NoAds)
        {
            SceneManager.LoadScene("GoodBadScene");
        }
        else
        {
            if (PlayerPrefs.HasKey("Number"))
            {
                int number = PlayerPrefs.GetInt("Number");
                if (number % 2 == 0)
                {
                    number++;
                    PlayerPrefs.SetInt("Number", number);
                    ad.gecisReklam("GoodBadScene");
                }
                else
                {
                    SceneManager.LoadScene("GoodBadScene");
                }
            }
            else
            {
                PlayerPrefs.SetInt("Number", 1);
                ad.gecisReklam("GoodBadScene");
            }

        }
    }
    public void papelScene()
    {      
        SceneManager.LoadScene("BuyPapel");
    }
    void loadingInst()
    {      
            var loading = Instantiate(loadingAnim, loadingTransform.transform);    
    }void DestroyLoading()
    {
        if (loadingTransform.transform.childCount > 0)
        {
            Destroy(loadingTransform.transform.GetChild(0).gameObject);
        }
    }
}


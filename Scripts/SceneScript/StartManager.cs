using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class StartManager : MonoBehaviour
{
    //before the load main scene, this brings necessary information
    WebSocket ws;

    private string urlUser = "http://localhost:3000/careme/api/info/";
    private string urlPlant = "ws://localhost:3000";
    private string urlVersion = "http://localhost:3000/careme/api/info/app/version";

    private Plant plant;
    private User user;

    public bool loadOver1;
    public bool loadOver2;
    public bool loadOver3;

    public GameObject versionPanel;
    public GameObject ConnectionPanel;

    public GameObject loadingAnim;
    public GameObject loadingTransf;
    bool isLoading = false;

    //public Animator loadingAnim;

    [Serializable]
    public struct PlantInfo
    {
        public bool expired;//hala deney devam ediyormu
        public int experimentTime;
        public int waterInfo;
        public int moistureInfo;
        public int healthInfo;
        public int sunInfo;
    }
    PlantInfo plantInfo;

    [Serializable]
    public struct UserInfo
    {
        public string username;
        public int papel;
        public bool noAds;
    }
    UserInfo[] userInfo;
    
    [Serializable]
    public struct Giris
    {
        public string message;       
    }
    Giris giris;

    [Serializable]
    public struct Version
    {
        public int version;       
    }
    Version[] version;

    private void Awake()
    {
        plant = Plant.Instance;
        user = User.Instance;
        ConnectionPanel.SetActive(false);
        bringVersionInfo();
        loadOver1 = false;
        loadOver2 = false;
        loadOver3 = false;
        //loadingAnim.SetBool("isLoading", true);
        versionPanel.SetActive(false);
        loadingInst();
        StartCoroutine(HasInternetConnection());
    }

    void Start()
    {
        incomingPlant();
        ws.Connect();
        bringUserInfo();
    }

    void Update()
    {
        if (loadOver3)
        {
            //loadingAnim.SetBool("isLoading", true);
            giris.message = "giris";
            string json = JsonUtility.ToJson(giris);
            ws.Send(json);
            loadOver3 = false;
        }
        if (loadOver2 && loadOver1)
        {
            processData();
            loadOver2 = false;
            SceneManager.LoadScene("MainScene");
        }
    }

    public void incomingPlant()
    {
        ws = new WebSocket(urlPlant);
        ws.OnMessage += (sender, mes) =>
        {
            string json = mes.Data;
            plantInfo = JsonUtility.FromJson<PlantInfo>(json);
            Debug.Log("burdayım start");
            loadOver1 = true;
        };
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
            userInfo = JsonHelper.GetArray<UserInfo>(www.downloadHandler.text);
            loadOver2 = true;
            Debug.Log(loadOver2);
        }
    }
    IEnumerator GetVersion(string url)
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
            version = JsonHelper.GetArray<Version>(www.downloadHandler.text);
            Initialization();
        }
    }
    IEnumerator HasInternetConnection()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com");
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            ConnectionPanel.SetActive(true);           
        }
        else
        {
            ConnectionPanel.SetActive(false);
        }
    }
    public void bringUserInfo()
    {
        StartCoroutine(GetUserInfo(urlUser+user.UserId));
    }
    public void bringVersionInfo()
    {
        StartCoroutine(GetVersion(urlVersion));
    }


    public void processData()
    {
        plant.WaterLevel = plantInfo.waterInfo;
        plant.MoistureLevel = plantInfo.moistureInfo;
        plant.SunLevel = plantInfo.sunInfo;
        plant.HealthLevel = plantInfo.healthInfo;
        plant.Expired = plantInfo.expired;
        plant.ExperimentTime = plantInfo.experimentTime;

        user.Username = userInfo[0].username;
        user.Papel = userInfo[0].papel;
        user.NoAds = userInfo[0].noAds;
    }

    void Initialization()
    {
        if (PlayerPrefs.HasKey("version"))
        {
            if(PlayerPrefs.GetInt("version")== 1)
            {
                PlayerPrefs.SetInt("version", 2);
            }
            int localVersion = PlayerPrefs.GetInt("version");
            if (localVersion == version[0].version)
            {
                loadOver3 = true;
            }
            else
            {
                versionPanel.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetInt("version", 2);
            loadOver3 = true;
        }
    }

    void loadingInst()
    {
        if (!isLoading)
        {
            var loading = Instantiate(loadingAnim, loadingTransf.transform);
            isLoading = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

public class GoodAndBad : MonoBehaviour
{
    private string iyiUrl = "http://localhost:3000/careme/api/info/goodslist/info";
    private string kotuUrl = "http://localhost:3000/careme/api/info/badslist/info";
    public GameObject goodCanvas;
    public GameObject badCanvas;
    public GameObject listMember;
    public GameObject ConnectionPanel;

    public GameObject loadingAnim;
    public GameObject loadingTransf;
    bool isLoading = false;


    public GameObject scroll;
    private AdManagers ad;
    private User user;

    [Serializable]
    public struct Liste
    {
        public int isGood;
        public int isBad;
        public string username;
    }
    Liste[] liste;
    private void Awake()
    {
        user = User.Instance;
        goodCanvas.SetActive(true);
        badCanvas.SetActive(false);
        ConnectionPanel.SetActive(false);
    }
    void Start()
    {
        ad = GameObject.FindObjectOfType<AdManagers>();
        loadingInst();
        StartCoroutine(HasInternetConnection());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator getList(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if(www.result!= UnityWebRequest.Result.Success)
        {
            Debug.Log("Error "+www.error);
        }
        else
        {
            liste = JsonHelper.GetArray<Liste>(www.downloadHandler.text);
            DrawList();
            //loadingAnim.SetBool("isLoading", false);
        }
    }
    IEnumerator HasInternetConnection()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com");
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            ConnectionPanel.SetActive(true);
            goodCanvas.SetActive(false);
        }
        else
        {
            Initialization();
        }
    }
    void DrawList()
    {
        if (loadingTransf.transform.childCount > 0)
        {
            Destroy(loadingTransf.transform.GetChild(0).gameObject);
        }    
        for (int i= 0; i < liste.Length; i++)
        {
            if (goodCanvas.activeSelf)
            {
                var llistMember = Instantiate(listMember, scroll.transform);
                llistMember.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = liste[i].username;
                llistMember.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = liste[i].isGood.ToString();
            }else if (badCanvas.activeSelf)
            {
                var llistMember = Instantiate(listMember, scroll.transform);
                llistMember.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = liste[i].username;
                llistMember.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = liste[i].isBad.ToString();
            }
            
        }
    }
    public void goodPanel()
    {
        if (!goodCanvas.activeSelf)
        {
            loadingInst();
            destroyListMember();
            liste = null;
            bringList(iyiUrl);
            goodCanvas.SetActive(true);
            badCanvas.SetActive(false);
        }
    }
    public void badPanel()
    {
        if (!badCanvas.activeSelf)
        {
            loadingInst();
            destroyListMember();
            liste = null;
            bringList(kotuUrl);
            goodCanvas.SetActive(false);
            badCanvas.SetActive(true);                    
        }
    }
    public void GoBack()
    {
        if (user.NoAds)
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            ad.gecisReklam("MainScene");
        }              
    }
    public void destroyListMember()
    {
        int count = scroll.transform.childCount;
        while(count > 0)
        {                    
            Destroy(scroll.transform.GetChild(count - 1).gameObject);
            count--;
        }
       
    }
    public void bringList(string url)
    {
        StartCoroutine(getList(url));
    }
    void Initialization()
    {
        ConnectionPanel.SetActive(false);
        StartCoroutine(getList(iyiUrl));
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

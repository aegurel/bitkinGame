using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//REGISTER SCENE SCRIPT

public class RegisterScript : MonoBehaviour
{
    private string url = "http://localhost:3000/careme/api/info/user/kayit";
    private bool loadOver = false;
    private bool loadOver2 = false;
    private AuthManager auth;
    private User user;

    public InputField emailGirisForm;
    public InputField emailKayitForm;
    public InputField usernameForm;
    public InputField passwordGirisForm;
    public InputField passwordKayitForm;
    public Text mesajText;

    public GameObject girisCanvas;
    public GameObject kayitCanvas;
    public GameObject ConnectionPanel;

    public Animator anim;

    public GameObject loadingAnim;
    public GameObject loadingTransform;
    public bool isLoading = false;


    [Serializable]
    public struct RegisterUser
    {
        public string idFirebase;
        public string username;
        public string email;
    }
    RegisterUser reguser;
    private void Awake()
    {
        
        user = User.Instance;
        auth = AuthManager.Instance;
        ConnectionPanel.SetActive(false);
    }
    void Start()
    {
       StartCoroutine(HasInternetConnection());
    }

    // Update is called once per frame
    void Update()
    {
        if (user.UserInfo != null)
        {         
            mesajShow(user.UserInfo);           
        }
        if (loadOver==true && user.UserId!=null)
        {
            reguser.idFirebase = user.UserId;
            StartCoroutine(userRegister(url));
        }
        if (loadOver2 == true && user.UserId != null)
        {
            SceneManager.LoadScene("LoadScene");
            loadOver2 = false;
        }
    }

    public void Register()
    {
        string username = usernameForm.text;
        string email = emailKayitForm.text;
        string password = passwordKayitForm.text;
            
        if (username != null && email != "" && password != "")
        {
            loadingInst();
            reguser.email = email;
            reguser.username = username;
            loadOver = true;
            auth.SignUp(email, password);            
        }
        else
        {
            mesajText.text = "Lütfen alanları tam doldurunuz";
            anim.SetTrigger("InfoTrig");
        }
    }
    public void Login()
    {
        string email = emailGirisForm.text;
        string password = passwordGirisForm.text;
        if(email != "" && password != "")
        {
            loadingInst();
            loadOver2 = true;
            auth.Login(email, password);          
        }
        else
        {
            mesajText.text = "Lütfen alanları tam doldurunuz";
            anim.SetTrigger("InfoTrig");
        }
        
    }
    public void ResetPass()
    { 
        string email = emailGirisForm.text;
        if (email != "")
        {
            loadingInst();
            auth.ResetPassword(email);
            mesajText.text = "Emailinizi kontrol ediniz";
            anim.SetTrigger("InfoTrig");
        }
        else
        {
            mesajText.text = "Emailinizi yazınız";
            anim.SetTrigger("InfoTrig");
        }
    }
    void AutoLogin()
    {
        if (auth.auth.CurrentUser != null)
        {
            loadingInst();
            auth.AutoLogin(auth.auth.CurrentUser.UserId);
            if (user.UserId != null)
            {
                SceneManager.LoadScene("LoadScene");
            }
        }
        else
        {
            girisCanvas.SetActive(false);
            kayitCanvas.SetActive(true);
        }
    }
    public void girisPanel()
    {
        if (!isLoading)
        {
            girisCanvas.SetActive(true);
            kayitCanvas.SetActive(false);
        }
   
    }
    public void kayitPanel()
    {
        if (!isLoading)
        {
            girisCanvas.SetActive(false);
            kayitCanvas.SetActive(true);
        }     
    }
    IEnumerator userRegister(string url)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(reguser));
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error " + request.error);
        }
        else
        {
            SceneManager.LoadScene("LoadScene");
            loadOver = false;
        }
    }
    IEnumerator HasInternetConnection()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com");
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            ConnectionPanel.SetActive(true);
            girisCanvas.SetActive(false);
            kayitCanvas.SetActive(false);
        }
        else
        {
            ConnectionPanel.SetActive(false);
            AutoLogin();
        }
    }
    void loadingInst()
    {
        if (!isLoading)
        {
            var loading = Instantiate(loadingAnim, loadingTransform.transform);
            isLoading = true;
        }       
    }
    public void mesajShow(string mesaj)
    {
        if (isLoading)
        {
            Destroy(GameObject.FindGameObjectWithTag("loading"));
            mesajText.text = mesaj;
            anim.SetTrigger("InfoTrig");
            user.UserInfo = null;
            isLoading = false;
        }
        
    }
  
}

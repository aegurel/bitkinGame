using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class AdManagers : MonoBehaviour
{
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    string sahne;
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });
        RequestRewarded();
        RequestInterstitial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void RequestRewarded()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId ="ca-app-pub-3940256099942544/5224354917";// test add id
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif
        this.rewardedAd = new RewardedAd(adUnitId);
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";//test add id
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        interstitial.Destroy();
        SceneManager.LoadScene(sahne);
    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        PlayerPrefs.DeleteKey("time");
        MainScript.IEWork = false;
        MainScript.sure = 0; 
    }
    public void gecisReklam(string ysahne)
    {
        sahne = ysahne;
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
        else
        {
            SceneManager.LoadScene(sahne);
        }
    }
    public void odulluReklam()
    {        
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour
{

    private const string interstitialUnitID = "bed1407148fb419d821feb656ec1a2ab";
    private const string rewardUnitID = "58dd2c1519854bc291386fee203e7495";

    private Button InitializeButton;
    private Button LoadInterstitialButton;
    private Button ShowInterstitialButton;
    private Button LoadRewardedButton;
    private Button ShowRewardedButton;
    private Text prompt;
    private Text RewardedAd_Counter;
    private static int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitializeButton = GameObject.Find("InitializeMoPubSDK").GetComponent<Button>();
        if (InitializeButton) InitializeButton.onClick.AddListener(InitializeMoPubSDK);

        LoadInterstitialButton = GameObject.Find("LoadInterstitialAd").GetComponent<Button>();
        LoadInterstitialButton.interactable = false;
        if (LoadInterstitialButton) LoadInterstitialButton.onClick.AddListener(LoadInterstitialAd);

        ShowInterstitialButton = GameObject.Find("ShowInterstitialAd").GetComponent<Button>();
        ShowInterstitialButton.interactable = false;
        if (ShowInterstitialButton) ShowInterstitialButton.onClick.AddListener(ShowInterstitialAd);

        LoadRewardedButton = GameObject.Find("LoadRewardedAd").GetComponent<Button>();
        LoadRewardedButton.interactable = false;
        if (LoadRewardedButton) LoadRewardedButton.onClick.AddListener(LoadRewardedAd);

        ShowRewardedButton = GameObject.Find("ShowRewardedAd").GetComponent<Button>();
        ShowRewardedButton.interactable = false;
        if (ShowRewardedButton) ShowRewardedButton.onClick.AddListener(ShowRewardedAd);


        prompt = GameObject.Find("prompt").GetComponent<Text>();
        prompt.text = "please initialize MoPub SDK first";


        RewardedAd_Counter = GameObject.Find("RewardedAd_Counter").GetComponent<Text>();
        RewardedAd_Counter.text = "counter: " + counter;

    }

    private void InitializeMoPubSDK()
    {
        prompt.text = "initializing SDK.....";
        MoPub.InitializeSdk(new MoPub.SdkConfiguration
        {
            AdUnitId = interstitialUnitID,
            LogLevel = MoPub.LogLevel.Debug,

            MediatedNetworks = new MoPub.MediatedNetwork[]
            {
                new MoPub.SupportedNetwork.Unity
                {
                    NetworkConfiguration = new Dictionary<string,string>
                    {
                        { "gameId", "4144393" },
                    }
                }

            }
        });

        MoPub.LoadInterstitialPluginsForAdUnits(new string[] { interstitialUnitID });
        MoPub.LoadRewardedVideoPluginsForAdUnits(new string[] { rewardUnitID });

        MoPubManager.OnSdkInitializedEvent += OnSdkInitializedEvent;

        MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
        MoPubManager.OnInterstitialShownEvent += OnInterstitialShownEvent;
        MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;

        MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
        MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
        MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
    }

    private void LoadInterstitialAd()
    {

        prompt.text = "loading interstitial Ad.....";
        MoPub.RequestInterstitialAd(interstitialUnitID);
    }

    private void ShowInterstitialAd()
    {
        if (MoPub.IsInterstitialReady(interstitialUnitID))
        {
            MoPub.ShowInterstitialAd(interstitialUnitID);
        }

    }

    private void LoadRewardedAd()
    {
        prompt.text = "loading Rewarded Ad.....";
        MoPub.RequestRewardedVideo(rewardUnitID);
    }


    private void ShowRewardedAd()
    {
        if (MoPub.HasRewardedVideo(rewardUnitID))
        {
            prompt.text = "rewarded Ad shown";
            MoPub.ShowRewardedVideo(rewardUnitID);
        }

    }

    private void OnSdkInitializedEvent(string adUnitId)
    {
        Debug.Log("MoPub SDK initialized ok with adUnitID:  " + adUnitId);
        // The SDK is initialized here. Ready to make ad requests.
        prompt.text = "SDK initialized, ready to load an Ad";
        LoadInterstitialButton.interactable = true;
        LoadRewardedButton.interactable = true;
    }

    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        Debug.Log("MoPub interstitial ad loaded ok with adUnitID:  " + adUnitId);
        prompt.text = "interstitial Ad loaded, ready to show";
        ShowInterstitialButton.interactable = true;
    }

    private void OnInterstitialFailedEvent(string adUnitId, string errorCode)
    {
        Debug.Log("MoPub interstitial ad loaded fail with errorCOde:  " + errorCode);
        prompt.text = "interstitial Ad failed to load, please try again";
    }


    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        Debug.Log("MoPub interstitial ad dismissed with adUnitID: " + adUnitId);
        prompt.text = "interstitial Ad skipped";
    }

    private void OnInterstitialShownEvent(string adUnitId)
    {
        Debug.Log("MoPub interstitial ad shown with adUnitId:  " + adUnitId);
        prompt.text = "interstitial Ad was watched to completion"; //if dismissed, text will change to "skipped"
    }


    private void OnRewardedVideoLoadedEvent(string adUnitId)
    {
        Debug.Log("MoPub rewarded ad loaded ok with adUnitID:  " + adUnitId);
        prompt.text = "rewarded Ad loaded, ready to show";
        ShowRewardedButton.interactable = true;
    }

    private void OnRewardedVideoFailedEvent(string adUnitId, string errorCode)
    {
        Debug.Log("MoPub rewarded ad loaded fail with errorCOde:  " + errorCode);
        prompt.text = "rewarded Ad failed to load, please try again";
    }

    private void OnRewardedVideoReceivedRewardEvent(string adUnitId, string label, float amount)
    {
        Debug.Log("MoPub rewarded ad with adUnitId: " + adUnitId + " was finished watching, reward counter is incremented.");
        RewardedAd_Counter.text = "counter: " + ++counter;
    }


    // Update is called once per frame
    void Update()
    {

    }
}

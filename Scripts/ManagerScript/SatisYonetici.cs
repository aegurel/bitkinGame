using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

//SELL PAPEL AND NO ADS SCRIPT 
namespace CompleteProject
{
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class SatisYonetici : MonoBehaviour, IStoreListener
    {
        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

        private User user;
        public GameObject hataPanel;

        public static string Papel_200 ="papel200";   
        public static string Papel_500 ="papel500";   
        public static string Papel_1000 ="papel1000";   
        public static string Papel_2000 ="papel2000";   
        public static string Papel_4000 ="papel4000";   
        public static string REKLAM_KALDIR = "reklamkaldirma";

        private string url200 = "http://localhost:3000/careme/api/info/two/papel/";
        private string url500 = "http://localhost:3000/careme/api/info/five/papel/";
        private string url1000 = "http://localhost:3000/careme/api/info/thousand/papel/";
        private string url2000 = "http://localhost:3000/careme/api/info/tthousand/papel/";
        private string url4000 = "http://localhost:3000/careme/api/info/fthousand/papel/";
        private string urlreklam = "http://localhost:3000/careme/api/info/app/noads/";

        // Google Play Store-specific product identifier subscription product.
		private static string kProductNameGooglePlaySubscription = "com.yourpackagename";//you have to change this as your package name

        private void Awake()
        {
            m_StoreController = null;
            m_StoreExtensionProvider = null;
        }
        void Start()
        {
            Debug.Log(m_StoreController);
            hataPanel.SetActive(false);
            user = User.Instance;
            if (m_StoreController == null)
            {
                // Satın alma işlemi için bağlantı kur
                InitializePurchasing();
            }
        }

        public void InitializePurchasing() 
        {
            // Satın alma işlemi için bağlantı sağlanmışsa
            if (IsInitialized())
            {
                return;//Devam etme
            }
                
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(Papel_200, ProductType.Consumable);
            builder.AddProduct(Papel_500, ProductType.Consumable);
            builder.AddProduct(Papel_1000, ProductType.Consumable);
            builder.AddProduct(Papel_2000, ProductType.Consumable);
            builder.AddProduct(Papel_4000, ProductType.Consumable);

            builder.AddProduct(REKLAM_KALDIR, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }


        private bool IsInitialized()
        {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }


        public void papel200()
        {
            print("Altın alma işlemi");
            BuyProductID(Papel_200);
        }
        public void papel500()
        {
            print("Altın alma işlemi");
            BuyProductID(Papel_500);
        }
        public void papel1000()
        {
            print("Altın alma işlemi");
            BuyProductID(Papel_1000);
        }
        public void papel2000()
        {
            print("Altın alma işlemi");
            BuyProductID(Papel_2000);
        }
        public void papel4000()
        {
            print("Altın alma işlemi");
            BuyProductID(Papel_4000);
        }


        public void ReklamKaldir()
        {
            print("Reklam kaldırma işlemi");
            BuyProductID(REKLAM_KALDIR);
        }

        void BuyProductID(string productId)
        {
            if (IsInitialized())
            {
                Product product = m_StoreController.products.WithID(productId);

                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Eşzamanlı satın alma işlemi: '{0}'", product.definition.id));
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log("Satın alma işlemi gerçekleştirilemedi.");
                }
            }
            else
            {
                Debug.Log("Satın alma işlemi başlatılamadı.");
            }
        }


        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log(" Başlatma işlemi başarılı.");
            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("Başalatılamadı. Hata Nedeni:" + error);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
        {

            if (String.Equals(args.purchasedProduct.definition.id, Papel_200, StringComparison.Ordinal))
            {
                getParaWorks(url200+user.UserId, 200);
            }
            
            else if (String.Equals(args.purchasedProduct.definition.id, Papel_500, StringComparison.Ordinal))
            {
                getParaWorks(url500 + user.UserId, 500);
            }

            else if (String.Equals(args.purchasedProduct.definition.id, Papel_1000, StringComparison.Ordinal))
            {
                getParaWorks(url1000 + user.UserId, 1000);
            }

            else if (String.Equals(args.purchasedProduct.definition.id, Papel_2000, StringComparison.Ordinal))
            {
                getParaWorks(url2000 + user.UserId, 2000);
                getParaWorks(urlreklam + user.UserId, 0);
            }
            else if (String.Equals(args.purchasedProduct.definition.id, Papel_4000, StringComparison.Ordinal))
            {
                getParaWorks(url4000 + user.UserId, 4000);
                getParaWorks(urlreklam + user.UserId, 0);
            }
            else if (String.Equals(args.purchasedProduct.definition.id, REKLAM_KALDIR, StringComparison.Ordinal))
            {
                getParaWorks(urlreklam + user.UserId,0);
            }

            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            hataPanel.SetActive(true);
        }
        public void closePanel()
        {
            hataPanel.SetActive(false);
        }

        private IEnumerator paraWorks(string url,int papel)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Errors " + www.error);
            }
            else
            {
                user.Papel += papel;
            }
        }
        void getParaWorks(string url,int num)
        {
            StartCoroutine(paraWorks(url,num));
        }
        public void goBack()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
using Firebase.Messaging;
using Unity.Notifications.iOS;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private Text descriptionText;
    void Start()
    {
        Init();
        
        ExamineClickAction();
    }

    private void Init()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                FirebaseMessaging.TokenReceived += OnTokenReceived;
                FirebaseMessaging.MessageReceived += OnMessageReceived;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    
    public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from: " + e.Message.From);
    }

    public void ExamineClickAction()
            {
    #if UNITY_IOS
                var n = iOSNotificationCenter.GetLastRespondedNotification();
                if (n != null)
                {
                    if (n.Body.Contains("test"))
                    {
                        Debug.Log("Intent with weekly reward action");
                        descriptionText.text = "Intent with weekly reward action";
                    }
                    else
                    {
                        Debug.Log("Intent with unknown action");
                        descriptionText.text = "Intent with unknown action";
                    }
                }
                else
                {
                    Debug.Log("No intent detected.");
                    descriptionText.text = "No intent detected.";
                    
                    iOSNotificationCenter.RemoveAllDeliveredNotifications();
                }
    #endif
            }
    
    void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("OnApplicationPause, pauseStatus: " + pauseStatus);
        
        if (!pauseStatus)
        {
            ExamineClickAction();
        }
    }
}

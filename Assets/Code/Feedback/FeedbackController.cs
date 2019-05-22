using Localization;
using UnityEngine;
using Zenject;

namespace Feedback
{
    public class FeedbackController : IInitializable
    {
        [Inject] private readonly IDebugService _debugService;
        [Inject] private readonly ILocalizationController _localizationController;

        public const string FeedbackEmailAdress = "support@devgameou.com";

        private const string FEEDBACK_REQUEST_SUCCESS_PREFS_NAME = "Common.FeedbackSuccess";

        private SerializedPrefsBool _feedbackRequestSuccess;

        #region Public Methods

        public bool FeedbackRequestSuccess
        {
            get { return _feedbackRequestSuccess.Value; }
            set { _feedbackRequestSuccess.Value = value; }
        }

        public void Initialize()
        {
            _feedbackRequestSuccess = new SerializedPrefsBool(FEEDBACK_REQUEST_SUCCESS_PREFS_NAME, false);
        }

        public void OpenMailTemplate(string emailAdress, string subject, string body)
        {
            _debugService.Log(DebugServiceTagConsts.FEEDBACK, "Subject: " + subject, Color.green);
            _debugService.Log(DebugServiceTagConsts.FEEDBACK, "Body: " + body, Color.green);

#if UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer && Prime31.EtceteraBinding.isEmailAvailable())
            {
                Prime31.EtceteraBinding.showMailComposer(emailAdress, subject, body, false);
            }
#elif UNITY_ANDROID
		if(Application.platform==RuntimePlatform.Android)
		{
			//TODO:Add android native
			Application.OpenURL(string.Format("mailto:{0}?subject={1}&body={2}", emailAdress, WWW.EscapeURL(subject).Replace("+","%20"), WWW.EscapeURL(body).Replace("+","%20")));
		}
#endif
        }

        public string GetRateFeedbackSubject()
        {
            return string.Format(_localizationController.GetText("Feedback.Mail.Title"),
                _localizationController.GetText("Feedback.Mail.Rate"),
                _localizationController.GetText("Common.AppName"),
                Application.version,
                _localizationController.CurrentLanguageCode.ToUpper());
        }

        public string GetFeedbackSubject()
        {
            return string.Format(_localizationController.GetText("Feedback.Mail.Title"),
                _localizationController.GetText("Feedback.Mail.Feedback"),
                _localizationController.GetText("Common.AppName"),
                Application.version,
                _localizationController.CurrentLanguageCode.ToUpper());
        }

        public string GetFeedbackBody(string userText = "")
        {
            return string.Format(_localizationController.GetText("Feedback.Mail.Body"),
                userText,
                GetDeviceModel(),
                GetNetworkType(),
                GetOsVersion(),
                _localizationController.CurrentLanguageCode.ToUpper(),
                System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss (UTCzz)"));
        }

        #endregion


        #region Private Methods

        private string GetDeviceModel()
        {
#if UNITY_IOS
            return UnityEngine.iOS.Device.generation.ToString();
#else
		return SystemInfo.deviceModel;
#endif
        }

        private string GetOsVersion()
        {
#if UNITY_IOS
            var osVersion = SystemInfo.operatingSystem;
            return osVersion.Replace("iPhone OS", "iOS");
#else
		return SystemInfo.operatingSystem; 
#endif
        }

        private string GetNetworkType()
        {
            switch (Application.internetReachability)
            {
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    return "Wi-Fi";
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    return "3G";
            }

            return string.Empty;
        }

        #endregion
    }
}
using BugiGames.UI;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

namespace BugiGames.Tools
{
    public class RateThisGame : MonoBehaviour
    {
        [Inject] private WaitLoadingSpinnerDots waitLoadingSpinnerDots;

        [SerializeField] private Button[] starsButtons;

        [SerializeField] private Button closeButton;

        [SerializeField] private Color ratedStarColor;
        [SerializeField] private Color unratedStarColor;

        [SerializeField] private CanvasGroup canvasGroup;

        private float fadeDuration = 0.25f;
        private int currentRating;
        private Vector3 originalPosition;

        private const string playerEnterGameCountDataKey = "playerEnterGameCountDataKey";
        private const string HasRatedDataKey = "HasRatedDataKey";

        [ShowInInspector]
        private int PlayerEnterGameCount
        {
            get
            {
                return PlayerPrefs.GetInt(playerEnterGameCountDataKey, 0);
            }
            set
            {
                PlayerPrefs.SetInt(playerEnterGameCountDataKey, value);
            }
        }

        [ShowInInspector]
        private bool HasRated
        {
            get
            {
                return PlayerPrefs.GetInt(HasRatedDataKey, 0) == 1;
            }

            set
            {
                PlayerPrefs.SetInt(HasRatedDataKey, value ? 1 : 0);
            }
        }

        private void Awake()
        {
            PlayerEnterGameCount++;

            InitializeStars();

            closeButton.onClick.AddListener(DisableRateGameScreen);

            transform.localPosition = originalPosition;
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);

            if (HasRated == false)
            {
                if (PlayerEnterGameCount % 10 == 0 || PlayerEnterGameCount == 3 || PlayerEnterGameCount == 5)
                {
                    Invoke(nameof(EnableRateGameScreen), 0.25f);
                }
            }
        }

        private void OpenAppStore()
        {
#if UNITY_ANDROID
            OpenAndroidAppByLink();
#elif UNITY_IOS
            TryOpenIOSAppByLink();
#endif

            HasRated = true;
        }

        private void TryOpenIOSAppByLink()
        {
            StartCoroutine(GetIOSTrackIdRequest());
        }

        private void OpenAndroidAppByLink()
        {
            Application.OpenURL("market://details?id=" + Application.identifier);
            DisableRateGameScreen();
        }

        private void InitializeStars()
        {
            for (int i = 0; i < starsButtons.Length; i++)
            {
                int starIndex = i;
                starsButtons[i].onClick.AddListener(() => RateStar(starIndex));
                starsButtons[i].image.color = unratedStarColor;
            }
        }

        private void RateStar(int starIndex)
        {
            currentRating = starIndex + 1;
            UpdateStarColors();
            Invoke(nameof(OpenAppStore), 0.2f);
        }

        private void UpdateStarColors()
        {
            for (int i = 0; i < starsButtons.Length; i++)
            {
                starsButtons[i].image.color = (i < currentRating) ? ratedStarColor : unratedStarColor;
            }
        }

        private IEnumerator GetIOSTrackIdRequest()
        {
            var uri = $"https://itunes.apple.com/lookup?bundleId={Application.identifier}";

            waitLoadingSpinnerDots.EnableLoadingScreen();

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        JObject json = JObject.Parse(webRequest.downloadHandler.text);

                        if (json["results"] != null && json["results"].HasValues)
                        {
                            var trackIdToken = json["results"][0]["trackId"];

                            if (trackIdToken != null)
                            {
                                string trackId = trackIdToken.Value<string>();
                                Debug.Log("Track ID: " + trackId);
                                Application.OpenURL($"itms-apps://itunes.apple.com/app/id{trackId}");
                            }
                            else
                            {
                                Debug.LogError("Track ID not found in JSON response.");
                            }
                        }
                        else
                        {
                            Debug.LogError("No 'results' data found in JSON response.");
                        }
                        break;
                }

                waitLoadingSpinnerDots.DisableLoadingScreen();
                DisableRateGameScreen();
            }
        }

        [Button]
        private void EnableRateGameScreen()
        {
            ResetRating();

            transform.localPosition = Vector3.zero;
            gameObject.SetActive(true);
            canvasGroup.DOFade(1, fadeDuration);
        }

        [Button]
        private void DisableRateGameScreen()
        {
            canvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
            {
                transform.localPosition = originalPosition;
                gameObject.SetActive(false);
            });
        }

        private void ResetRating()
        {
            for (int i = 0; i < starsButtons.Length; i++)
            {
                starsButtons[i].image.color = unratedStarColor;
            }

            currentRating = 0;
        }

        [Button]
        private void ResetData()
        {
            HasRated = false;
            PlayerEnterGameCount = 0;
        }
    }
}

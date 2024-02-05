using BugiGames.ScriptableObject;
using BugiGames.Tools;
using System;
using UnityEngine;

namespace BugiGames.Ads
{
    public class AdsController
    {
        public void Initialize()
        {

        }

        public void TryShowInterstitial()
        {
            if (IAPData.Value.IsAdsPurchased())
            {
                DebugColor.LogGreen("Ads purchased cannot be displayed.");
            }
            else
            {

            }
        }

        public void TryShowBannerBottom()
        {
            if (IAPData.Value.IsAdsPurchased())
            {
                DebugColor.LogGreen("Ads purchased cannot be displayed.");
            }
            else
            {

            }
        }

        public void HideBannerBottom()
        {
            if (IAPData.Value.IsAdsPurchased())
            {
                DebugColor.LogGreen("Ads purchased cannot be hidden.");
            }
            else
            {

            }
        }

        public bool IsReadyForShowAdBanner()
        {
            return true;
        }
    }
}


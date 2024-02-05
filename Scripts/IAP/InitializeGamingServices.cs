using BugiGames.Tools;
using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace BugiGames.IAP
{
    public class InitializeGamingServices
    {
        private const string productionEnvironment = "production";

        public bool IsSuccessInitedGamingServices { get; private set; }

        public void Initialize()
        {
            Request(OnSuccessMessage, OnErrorMessage);
        }

        private void Request(Action onSuccess, Action<string> onError)
        {
            try
            {
                var options = new InitializationOptions().SetEnvironmentName(productionEnvironment);
                UnityServices.InitializeAsync(options).ContinueWith(task =>
                {
                    onSuccess();
                });
            }
            catch (Exception exception)
            {
                onError(exception.Message);
            }
        }

        private void OnSuccessMessage()
        {
            IsSuccessInitedGamingServices = true;

            var text = "Congratulations! Unity Gaming Services has been successfully initialized.";
            DebugColor.LogGreen(text);
        }

        private void OnErrorMessage(string message)
        {
            IsSuccessInitedGamingServices = false;

            var text = $"Unity Gaming Services failed to initialize with error: {message}.";
            Debug.LogError(text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using UUwatchlistMobile.Services.Interfaces;

namespace UUwatchlistMobile.Services
{
    public class ApiKeyService: IApiKeyService
    {
        private const string ApiKeySettingName = "YoutubeApiKey";

        public bool HasKey => !string.IsNullOrEmpty(GetKey());

        public string GetKey()
        {
            return Preferences.Default.Get(ApiKeySettingName, string.Empty);
        }

        public void SaveKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            Preferences.Default.Set(ApiKeySettingName, key.Trim());
        }

        public void DeleteKey()
        {
            Preferences.Default.Remove(ApiKeySettingName);
        }
    }
}

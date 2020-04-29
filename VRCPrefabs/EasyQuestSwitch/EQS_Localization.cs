using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyQuestSwitch
{
    public class EQS_Localization
    {
        public EQS_LocalizedLanguage Current;
        public EQS_LocalizedLanguage[] AvailableLanguages;

        [Serializable]
        public class EQS_LocalizedLanguage
        {
            public string DisplayName;

            public string SettingsButton;
            public string SettingsLanguage;
            public string SettingsApplyPC;
            public string SettingsApplyQuest;
            public string SettingsRemoveEQS;
            public string SettingsCacheWarning;
            public string SettingsCacheButton;
            public string SettingsExplanation;
            public string SettingsFeedback;
            public string SettingsGithub;
            public string SettingsTwitter;

            public string PopupDeleteWarning;
            public string PopupAccept;
            public string PopupDecline;

            public string ListSetupEQS;
            public string ListExpand;
            public string ListFold;

            public string LogUnsupportedComponent;
            public string LogSwitchSuccess;
            public string LogSwitchFailure;
        }

        public void LoadLanguages()
        {
            UnityEngine.Object[] JSONlanguages = Resources.LoadAll("EQS_Local", typeof(TextAsset));
            AvailableLanguages = new EQS_LocalizedLanguage[JSONlanguages.Length];
            for(int i = 0; i < JSONlanguages.Length; i++)
            {
                AvailableLanguages[i] = JsonUtility.FromJson<EQS_LocalizedLanguage>(JSONlanguages[i].ToString());
            }
            Current = AvailableLanguages[0];
        }

        public string[] GetLanguages()
        {
            string[] languagesArray = new string[AvailableLanguages.Length];
            for(int i = 0; i < AvailableLanguages.Length; i++)
            {
                languagesArray[i] = AvailableLanguages[i].DisplayName;
            }
            return languagesArray;
        }

        public void SetLanguage(int i) => Current = i < AvailableLanguages.Length ? AvailableLanguages[i] : AvailableLanguages[0];

    }
}
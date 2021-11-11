#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyQuestSwitch
{
    public static class EQS_Localization
    {
        private static EQS_LocalizedLanguage _current;
        public static EQS_LocalizedLanguage Current
        {
            set => _current = value;
            get
            {
                if (_current == null)
                {
                    if (AvailableLanguages == null)
                    {
                        LoadLanguages();
                    }
                    SetLanguage(EditorPrefs.GetInt("EQS_Language", 0));
                }
                return _current;
            }
        }
        public static EQS_LocalizedLanguage[] AvailableLanguages;

        [Serializable]
        public class EQS_LocalizedLanguage
        {
            public string DisplayName;

            public string SettingsButton;
            public string SettingsLanguage;
            public string SettingsListFormat;
            public string[] SettingsListFormatArray;
            public string SettingsApplyPC;
            public string SettingsApplyQuest;
            public string SettingsRemoveEQS;
            public string SettingsCacheWarning;
            public string SettingsAssetPipelineV2;
            public string SettingsCacheButton;
            public string SettingsExplanation;
            public string SettingsDebugOptions;
            public string SettingsDebugReveal;
            public string SettingsFeedback;
            public string SettingsGithub;
            public string SettingsTwitter;

            public string PopupDeleteWarning;
            public string PopupTargetChanged;
            public string PopupAccept;
            public string PopupDecline;

            public string ListSetupEQS;
            public string ListExpand;
            public string ListFold;

            public string LogUnsupportedComponent;
            public string LogComponentExists;
            public string LogSwitchMissing;
            public string LogSwitchFailure;
            public string LogSwitchSuccess;
            public string LogSwitchUnsupported;
        }

        public static void LoadLanguages()
        {
            UnityEngine.Object[] JSONlanguages = Resources.LoadAll("EQS_Localizations", typeof(TextAsset));
            AvailableLanguages = new EQS_LocalizedLanguage[JSONlanguages.Length];
            for(int i = 0; i < JSONlanguages.Length; i++)
            {
                AvailableLanguages[i] = JsonUtility.FromJson<EQS_LocalizedLanguage>(JSONlanguages[i].ToString());
            }
            Current = AvailableLanguages[0];
        }

        public static string[] GetLanguages()
        {
            string[] languagesArray = new string[AvailableLanguages.Length];
            for(int i = 0; i < AvailableLanguages.Length; i++)
            {
                languagesArray[i] = AvailableLanguages[i].DisplayName;
            }
            return languagesArray;
        }

        public static void SetLanguage(int i) => Current = i < AvailableLanguages.Length ? AvailableLanguages[i] : AvailableLanguages[0];


    }
}
#endif
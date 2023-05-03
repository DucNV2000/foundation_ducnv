﻿using Pancake.Apex;
using UnityEngine;

namespace Pancake
{
    [EditorIcon("scriptable_setting")]
    public class HeartSettings : ScriptableSettings<HeartSettings>
    {
        [Label("Privacy on FirstOpen")] [SerializeField] private bool enablePrivacyFirstOpen;

        [Indent, ShowIf(nameof(enablePrivacyFirstOpen))] [SerializeField]
        private string privacyUrl;

        [Indent, Label("Title"), ShowIf(nameof(enablePrivacyFirstOpen))] [SerializeField]
        private string privacyTitle;

        [Indent, Label("Message"), TextArea(3, 6), ShowIf(nameof(enablePrivacyFirstOpen))] [SerializeField]
        private string privacyMessage;

        public static bool EnablePrivacyFirstOpen => Instance.enablePrivacyFirstOpen;
        public static string PrivacyUrl => Instance.privacyUrl;
        public static string PrivacyTitle => Instance.privacyTitle;
        public static string PrivacyMessage => Instance.privacyMessage;
    }
}
﻿using Pancake.Scriptable;
using UnityEngine;

namespace Pancake.Monetization
{
    /// <summary>
    /// Wrapper class
    /// </summary>
    [Searchable]
    [CreateAssetMenu(fileName = "ad_banner_unit_wrapper.asset", menuName = "Pancake/Misc/Advertising/Banner Variable Wrapper", order = 1)]
    [EditorIcon("scriptable_bind")]
    public class BannerVariable : ScriptableObject
    {
        [SerializeField] private StringPairVariable remoteConfigUsingAdmob;
        public AdUnitVariable admobBanner;
        public AdUnitVariable applovinBanner;

        public AdUnitVariable Context()
        {
            bool.TryParse(remoteConfigUsingAdmob.Value.value, out bool usingAdmob);
            return usingAdmob ? admobBanner : applovinBanner;
        }
    }
}
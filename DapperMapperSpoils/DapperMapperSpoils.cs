using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using Modding;
using UnityEngine;

namespace DapperMapperSpoils
{
    public class DapperMapperSpoils : Mod
    {
        internal static DapperMapperSpoils instance;
        
        public DapperMapperSpoils() : base(null)
        {
            instance = this;
        }
        
        public override string GetVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }
        
        public override void Initialize()
        {
            Log("Initializing Mod...");

            CondensedSpoilerLogger.AddCategorySafe("Maps", () => true, new()
            {
                ItemNames.Ancient_Basin_Map,
                ItemNames.City_of_Tears_Map,
                ItemNames.Crossroads_Map,
                ItemNames.Crystal_Peak_Map,
                ItemNames.Deepnest_Map,
                ItemNames.Fog_Canyon_Map,
                ItemNames.Fungal_Wastes_Map,
                ItemNames.Greenpath_Map,
                ItemNames.Howling_Cliffs_Map,
                ItemNames.Kingdoms_Edge_Map,
                ItemNames.Queens_Gardens_Map,
                ItemNames.Resting_Grounds_Map,
                ItemNames.Royal_Waterways_Map,
            });
        }
    }
}
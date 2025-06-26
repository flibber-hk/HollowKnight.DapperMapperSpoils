using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using Modding;
using UnityEngine;

namespace DapperMapperSpoils
{
    public class DapperMapperSpoils : Mod, IGlobalSettings<Settings>, IMenuMod
    {
        internal static DapperMapperSpoils instance;

        public static Settings GS { get; set; } = new();
        public void OnLoadGlobal(Settings gs) => GS = gs;
        public Settings OnSaveGlobal() => GS;

        public DapperMapperSpoils() : base(null)
        {
            instance = this;
        }

        bool IMenuMod.ToggleButtonInsideMenu => false;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            return new List<IMenuMod.MenuEntry>()
            {
                new IMenuMod.MenuEntry
                {
                    Name = "Include Collector's Map",
                    Description = string.Empty,
                    Values = new string[]{ "True", "False" },
                    Saver = opt => GS.IncludeGrubMap = opt == 0,
                    Loader = () => GS.IncludeGrubMap ? 0 : 1,
                }
            };
        }

        public override string GetVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }

        public static List<string> MapNames = new()
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
        };

        public override void Initialize()
        {
            Log("Initializing Mod...");

            CondensedSpoilerLogger.AddCategorySafe("Maps", () => !GS.IncludeGrubMap, new(MapNames));
            CondensedSpoilerLogger.AddCategorySafe("Maps", () => GS.IncludeGrubMap, MapNames.Concat(new[] { ItemNames.Collectors_Map }).ToList());

            if (ModHooks.GetMod("FStatsMod") is Mod)
            {
                HookFStats();
            }
        }

        private void HookFStats() => FStatsInterop.Hook();
    }
}
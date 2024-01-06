using FStats;
using FStats.StatControllers;
using FStats.Util;
using ItemChanger;
using Modding;
using System.Collections.Generic;
using System.Linq;

namespace DapperMapperSpoils
{
    internal static class FStatsInterop
    {
        internal static void Hook()
        {
            FStats.API.OnGenerateFile += reg => reg(new MapTimeline());
        }
    }

    internal class MapTimeline : StatController
    {
        private static readonly Dictionary<string, string> BoolNames = new()
        {
            [nameof(PlayerData.mapAbyss)] = ItemNames.Ancient_Basin_Map.Replace('_', ' '),
            [nameof(PlayerData.mapCity)] = ItemNames.City_of_Tears_Map.Replace('_', ' '),
            [nameof(PlayerData.mapCrossroads)] = ItemNames.Crossroads_Map.Replace('_', ' '),
            [nameof(PlayerData.mapMines)] = ItemNames.Crystal_Peak_Map.Replace('_', ' '),
            [nameof(PlayerData.mapDeepnest)] = ItemNames.Deepnest_Map.Replace('_', ' '),
            [nameof(PlayerData.mapFogCanyon)] = ItemNames.Fog_Canyon_Map.Replace('_', ' '),
            [nameof(PlayerData.mapFungalWastes)] = ItemNames.Fungal_Wastes_Map.Replace('_', ' '),
            [nameof(PlayerData.mapGreenpath)] = ItemNames.Greenpath_Map.Replace('_', ' '),
            [nameof(PlayerData.mapCliffs)] = ItemNames.Howling_Cliffs_Map.Replace('_', ' '),
            [nameof(PlayerData.mapOutskirts)] = ItemNames.Kingdoms_Edge_Map.Replace('_', ' '),
            [nameof(PlayerData.mapRoyalGardens)] = ItemNames.Queens_Gardens_Map.Replace('_', ' '),
            [nameof(PlayerData.mapRestingGrounds)] = ItemNames.Resting_Grounds_Map.Replace('_', ' '),
            [nameof(PlayerData.mapWaterways)] = ItemNames.Royal_Waterways_Map.Replace('_', ' '),
        };

        public Dictionary<string, float> MapObtainTimeline = new();

        public override void Initialize()
        {
            ModHooks.SetPlayerBoolHook += RecordPlayerDataBool;
        }
        public override void Unload()
        {
            ModHooks.SetPlayerBoolHook -= RecordPlayerDataBool;
        }
        private bool RecordPlayerDataBool(string name, bool orig)
        {
            if (orig) Record(name);
            return orig;
        }

        private void Record(string s)
        {
            if (BoolNames.ContainsKey(s) && !MapObtainTimeline.ContainsKey(s))
            {
                // Get from the local settings when recording
                MapObtainTimeline[s] = FStatsMod.LS.Get<Common>().CountedTime;
            }
        }

        public override IEnumerable<DisplayInfo> GetDisplayInfos()
        {
            List<string> lines = BoolNames
                .Where(kvp => MapObtainTimeline.ContainsKey(kvp.Key))
                .OrderBy(kvp => MapObtainTimeline[kvp.Key])
                .Select(kvp => $"{kvp.Value}: {MapObtainTimeline[kvp.Key].PlaytimeHHMMSS()}")
                .ToList();

            if (lines.Count == 0)
            {
                return Enumerable.Empty<DisplayInfo>();
            }

            DisplayInfo template = new()
            {
                Title = "Map Timeline",
                MainStat = GetOwningCollection().Get<Common>().TotalTimeString,
                Priority = BuiltinScreenPriorityValues.SkillTimeline + 1_001,
            };

            return ColumnUtility.CreateDisplay(template, lines);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using RapidGUI;
using UnityEngine;
using ValheimTooler.Core.Extensions;
using ValheimTooler.Models;
using ValheimTooler.Utils;

namespace ValheimTooler.Core
{
    public static class EntitiesItemsHacks
    {
        public static bool s_repairallinrange = false;
        public static bool s_refuelallinrange = false;
        public static bool s_extendeddemister = false;
        public static bool s_wardupdateractive = false;
        public static long player_id = 0L;
        public static string player_name = "none";

        private static bool s_allowspawner = false;
        private static string s_entityQuantityText = "1";
        private static int s_entityLevelIdx = 0;
        private static int s_entityPrefabIdx = 0;
        private static string s_entitySearchTerms = "";
        private static string s_previousSearchTerms = "";

        private static float s_updateTimer = 0f;
        private static readonly float s_updateTimerInterval = 1.5f;

        private static readonly List<string> s_entityLevels = new List<string>();

        private static readonly List<string> s_entityPrefabs = new List<string>();
        private static List<string> s_entityPrefabsFiltered = new List<string>();

        private static int NameComparator(string a, string b)
        {
            return string.Compare(a, b, StringComparison.InvariantCultureIgnoreCase);
        }

        public static void Start()
        {
            for (var i = 1; i <= 5; i++)
            {
                s_entityLevels.Add(i.ToString());
            }
        }

        public static void Update()
        {
            if (Time.time >= s_updateTimer && s_entityPrefabs.Count == 0)
            {
                if (ZNetScene.instance)
                {
                    foreach (GameObject prefab in ZNetScene.instance.m_prefabs)
                    {
                        s_entityPrefabs.Add(prefab.name);
                    }

                    s_entityPrefabs.Sort(NameComparator);
                    s_entityPrefabsFiltered = s_entityPrefabs;
                }

                s_updateTimer = Time.time + s_updateTimerInterval;
            }
        }

        public static void DisplayGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(300), GUILayout.MaxWidth(300));
            {
                GUILayout.BeginVertical();
                {

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_entities_item_giver_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        if (GUILayout.Button(EntryPoint.s_showItemGiver ? VTLocalization.instance.Localize("$vt_entities_item_giver_button_hide") : VTLocalization.instance.Localize("$vt_entities_item_giver_button_show")))
                        {
                            EntryPoint.s_showItemGiver = !EntryPoint.s_showItemGiver;
                        }

                        /*
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_entities_ward_updater")))
                        {
                            UpdateWards();
                        }
                        */

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_entities_drops_button")))
                        {
                            RemoveAllDrops();
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_building_manipulations_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_repair_in_range_button : " + (s_repairallinrange ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_repairallinrange = !s_repairallinrange;
                        }

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_refuelallinrange : " + (s_refuelallinrange ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_refuelallinrange = !s_refuelallinrange;
                        }

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_extendeddemister : " + (s_extendeddemister ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_extendeddemister = !s_extendeddemister;
                        }

                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_entities_spawn_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_entities_spawn_activate_confirmation : " + (s_allowspawner ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_allowspawner = !s_allowspawner;
                        }

                        GUILayout.Space(15);

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_entities_spawn_entity_name :"), GUILayout.ExpandWidth(false));
                            s_entityPrefabIdx = RGUI.SearchableSelectionPopup(s_entityPrefabIdx, s_entityPrefabsFiltered.ToArray(), ref s_entitySearchTerms);

                            SearchItem(s_entitySearchTerms);
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_entities_spawn_quantity :"), GUILayout.ExpandWidth(false));
                            s_entityQuantityText = GUILayout.TextField(s_entityQuantityText, GUILayout.ExpandWidth(true));
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_entities_spawn_level :"), GUILayout.ExpandWidth(false));
                            s_entityLevelIdx = RGUI.SelectionPopup(s_entityLevelIdx, s_entityLevels.ToArray());
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_entities_spawn_button")))
                        {
                            if (int.TryParse(s_entityQuantityText, out int entityQuantity) && int.TryParse(s_entityLevels[s_entityLevelIdx], out int entityLevel))
                            {
                                if (entityQuantity <= 100 && s_entityPrefabIdx < s_entityPrefabsFiltered.Count && s_entityPrefabIdx >= 0 && s_allowspawner)
                                {
                                    SpawnEntities(s_entityPrefabsFiltered[s_entityPrefabIdx], entityLevel, entityQuantity);
                                    s_allowspawner = false;
                                }
                            }
                        }
                    }
                    GUILayout.EndVertical();

                }
                GUILayout.EndVertical();

            }
            GUILayout.EndHorizontal();

        }

        private static void SearchItem(string search)
        {
            if (s_previousSearchTerms.Equals(search))
            {
                return;
            }
            if (search.Length == 0)
            {
                s_entityPrefabsFiltered = s_entityPrefabs;
            }
            else
            {
                string searchLower = search.ToLower();
                s_entityPrefabsFiltered = s_entityPrefabs.Where(i => i.ToLower().Contains(searchLower)).ToList();
            }
            s_previousSearchTerms = search;
        }

        private static void SpawnEntity(GameObject entityPrefab, int level)
        {
            if (entityPrefab != null && Player.m_localPlayer != null)
            {
                Vector3 b = UnityEngine.Random.insideUnitSphere * 0.5f;
                Character component2 = UnityEngine.Object.Instantiate<GameObject>(entityPrefab, Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up + b, Quaternion.identity).GetComponent<Character>();

                if (component2 != null)
                {
                    component2.SetLevel(level);
                }
            }
        }

        private static void SpawnEntities(string entityPrefab, int level, int quantity)
        {
            if (ZNetScene.instance == null)
            {
                return;
            }

            GameObject prefab = ZNetScene.instance.GetPrefab(entityPrefab);

            if (prefab == null ||
                entityPrefab.Contains("_eventzone_boss_base") ||
                entityPrefab.Contains("_TerrainCompiler") ||
                entityPrefab.Contains("_ZoneCtrl"))
            {
                return;
            } 

            for (var i = 0; i < quantity; i++)
            {
                SpawnEntity(prefab, level);
            }
        }

        private static void RemoveAllDrops()
        {
            if (Camera.main != null)
            {
                ItemDrop[] itemDrops = UnityEngine.Object.FindObjectsOfType<ItemDrop>();
                foreach (ItemDrop itemDrop in itemDrops)
                {
                    Fish component = itemDrop.gameObject.GetComponent<Fish>();
                    var distance = Vector3.Distance(Camera.main.transform.position, itemDrop.transform.position);

                    if ((!component || component.IsOutOfWater()) && distance < 11)
                    {
                        ZNetView component2 = itemDrop.GetComponent<ZNetView>();
                        if (component2 && component2.IsValid() && component2.IsOwner())
                        {
                            component2.Destroy();
                        }
                    }
                }
            }
        }

        private static void UpdateWards()
        {
            if (Player.m_localPlayer != null)
            {

                foreach (Player player in Player.GetAllPlayers())
                {
                    PrivateArea[] wards = UnityEngine.Object.FindObjectsOfType<PrivateArea>();
                    foreach (PrivateArea ward in wards)
                    {
                        var distance = Vector3.Distance(Camera.main.transform.position, ward.transform.position);
                        if (distance < 51)
                        {
                            ZNetView m_nview = ward.GetFieldValue<ZNetView>("m_nview");
                            EntitiesItemsHacks.s_wardupdateractive = true;
                            EntitiesItemsHacks.player_id = player.GetPlayerID();
                            EntitiesItemsHacks.player_name = player.GetPlayerName();
                            m_nview.InvokeRPC("TogglePermitted", 0L, "none");
                            EntitiesItemsHacks.s_wardupdateractive = false;
                            EntitiesItemsHacks.player_id = 0L;
                            EntitiesItemsHacks.player_name = "none";
                        }
                    }
                }
            }
        }

    }
}

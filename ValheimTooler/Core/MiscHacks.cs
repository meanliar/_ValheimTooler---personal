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
    public static class MiscHacks
    {
        public static bool s_enableAutopinMap = false;

        private static List<Player> s_players = null;

        private static float s_updateTimer = 0f;
        private static readonly float s_updateTimerInterval = 1.5f;

        public static void Start()
        {
            return;
        }

        public static void Update()
        {
            if (Time.time >= s_updateTimer)
            {
                s_players = Player.GetAllPlayers();

                s_updateTimer = Time.time + s_updateTimerInterval;
            }
        }

        public static void DisplayGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(300), GUILayout.MaxWidth(300));
            {
                GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_misc_esp_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                {
                    GUILayout.Space(EntryPoint.s_boxSpacing);

                    if (GUILayout.Button(VTLocalization.instance.Localize("$vt_misc_player_esp_button : " + (ESP.s_showPlayerESP ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                    {
                        ESP.s_showPlayerESP = !ESP.s_showPlayerESP;
                    }

                    if (GUILayout.Button(VTLocalization.instance.Localize("$vt_misc_monster_esp_button : " + (ESP.s_showMonsterESP ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                    {
                        ESP.s_showMonsterESP = !ESP.s_showMonsterESP;
                    }

                    if (GUILayout.Button(VTLocalization.instance.Localize("$vt_misc_dropped_esp_button : " + (ESP.s_showDroppedESP ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                    {
                        ESP.s_showDroppedESP = !ESP.s_showDroppedESP;
                    }

                    if (GUILayout.Button(VTLocalization.instance.Localize("$vt_misc_deposit_esp_button : " + (ESP.s_showDepositESP ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                    {
                        ESP.s_showDepositESP = !ESP.s_showDepositESP;
                    }

                    if (GUILayout.Button(VTLocalization.instance.Localize("$vt_misc_pickable_esp_button : " + (ESP.s_showPickableESP ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                    {
                        ESP.s_showPickableESP = !ESP.s_showPickableESP;
                    }

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_misc_radius_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.BeginHorizontal();
                            {
                                ConfigManager.instance.s_espRadiusEnabled = GUILayout.Toggle(ConfigManager.instance.s_espRadiusEnabled, "");
                                GUILayout.Label(VTLocalization.instance.Localize("$vt_misc_radius_enable"));
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginVertical();
                            {
                                GUILayout.Label("Detection Limit (" + ConfigManager.instance.s_espRadius.ToString("0.0") + "m)", GUILayout.MinWidth(200));
                                ConfigManager.instance.s_espRadius = GUILayout.HorizontalSlider(ConfigManager.instance.s_espRadius, 5f, 500f, GUILayout.ExpandWidth(true));
                            }
                            GUILayout.EndVertical();

                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Space(10);

                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }
}

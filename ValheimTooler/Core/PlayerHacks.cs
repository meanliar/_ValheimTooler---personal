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
    public static class PlayerHacks
    {
        public static bool s_isInfiniteStaminaMe = false;
        public static bool s_inventoryNoWeightLimit = false;
        public static bool s_instantCraft = false;
        public static bool s_doNotConsume = false;
        public static bool s_controlWind = false;
        public static bool s_megadamage = false;

        private static bool s_allupgrades = false;

        public static void Start()
        {
            return;
        }

        public static void Update()
        {
            if (Player.m_localPlayer != null && Player.m_localPlayer.InDebugFlyMode())
            {
                Player.m_debugFlySpeed = (int)ConfigManager.instance.s_flightSpeed;
            }

        }

        public static void DisplayGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(300), GUILayout.MaxWidth(300));
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_general_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        if (GUILayout.Button(VTLocalization.instance.Localize(s_allupgrades ? VTLocalization.s_deactivateAll : VTLocalization.s_activateAll)))
                        {
                            s_allupgrades = !s_allupgrades;
                            Player.m_localPlayer.VTSetGodMode(s_allupgrades);
                            s_isInfiniteStaminaMe = s_allupgrades;
                            s_inventoryNoWeightLimit = s_allupgrades;
                            Player.m_localPlayer.VTSetNoPlacementCost(s_allupgrades);
                            s_doNotConsume = s_allupgrades;
                            s_controlWind = s_allupgrades;
                        }

                        GUILayout.Space(5);

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_god_mode : " + (Player.m_localPlayer.VTInGodMode() ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            Player.m_localPlayer.VTSetGodMode(!Player.m_localPlayer.VTInGodMode());
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_inf_stamina_me : " + (s_isInfiniteStaminaMe ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_isInfiniteStaminaMe = !s_isInfiniteStaminaMe;
                            s_inventoryNoWeightLimit = !s_inventoryNoWeightLimit;
                        }
                        /*
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_infinite_weight : " + (s_inventoryNoWeightLimit ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_inventoryNoWeightLimit = !s_inventoryNoWeightLimit;
                        }
                        */
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_donotconsume : " + (s_doNotConsume ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_doNotConsume = !s_doNotConsume;
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_wind_power : " + (s_controlWind ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_controlWind = !s_controlWind;
                        }
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_noplacement_cost : " + (Player.m_localPlayer.VTIsNoPlacementCost() ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            Player.m_localPlayer.VTSetNoPlacementCost(!Player.m_localPlayer.VTIsNoPlacementCost());
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_flight_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.BeginHorizontal();
                            {
                                if (GUILayout.Button(VTLocalization.instance.Localize("Fly : " + (Player.m_localPlayer.VTInFlyMode() ? VTLocalization.s_flyOn : VTLocalization.s_cheatOff))))
                                {
                                    Player.m_localPlayer.VTSetFlyMode(!Player.m_localPlayer.VTInFlyMode());
                                }
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.Space(10);

                            GUILayout.BeginVertical();
                            {
                                GUILayout.Label("Flight Speed (" + ConfigManager.instance.s_flightSpeed.ToString("0.0") + ")", GUILayout.MinWidth(200));
                                ConfigManager.instance.s_flightSpeed = GUILayout.HorizontalSlider(ConfigManager.instance.s_flightSpeed, 10f, 80f, GUILayout.ExpandWidth(true));
                            }
                            GUILayout.EndVertical();

                        }
                        GUILayout.EndHorizontal();

                        GUILayout.Space(10);

                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_misc_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_megadamage : " + (s_megadamage ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_megadamage = !s_megadamage;
                        }

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_ghost_mode : " + (Player.m_localPlayer.VTInGhostMode() ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            Player.m_localPlayer.VTSetGhostMode(!Player.m_localPlayer.VTInGhostMode());
                        }

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_skill_button")))
                        {
                            foreach (object obj in Enum.GetValues(typeof(Skills.SkillType)))
                            {
                                Skills.SkillType skillType2 = (Skills.SkillType)obj;

                                if (skillType2 == Skills.SkillType.None || skillType2 == Skills.SkillType.All)
                                    continue;

                                Player.m_localPlayer.VTUpdateSkillLevel(skillType2, 100);
                            }
                        }

                        /*
                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_heal_selected_player")))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.VTHeal();
                            }
                        }
                        */

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_tame_creatures")))
                        {
                            Player.m_localPlayer.VTTameNearbyCreatures();
                        }

                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_emotepicker_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        if (GUILayout.Button(EntryPoint.s_showEmotePicker ? VTLocalization.instance.Localize("$vt_entities_emotepicker_button_hide") : VTLocalization.instance.Localize("$vt_entities_emotepicker_button_show")))
                        {
                            EntryPoint.s_showEmotePicker = !EntryPoint.s_showEmotePicker;
                        }

                    }
                    GUILayout.EndVertical();


                }
                GUILayout.EndVertical();

            }
            GUILayout.EndHorizontal();
        }

    }
}

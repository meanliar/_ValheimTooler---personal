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
    public static class TeleportHacks
    {
        private static int s_teleportTargetIdx = -1;
        private static string s_teleportCoordinates = "0,0,0";
        public static bool s_bypassRestrictedTeleportable = false;

        private static float s_updateTimer = 0f;
        private static readonly float s_updateTimerInterval = 1.5f;

        private static List<TPTarget> s_tpTargets = null;
        private static List<Player> s_players = null;

        public static void Start()
        {
            return;
        }

        public static void Update()
        {
            if (Time.time >= s_updateTimer)
            {
                if (ZNet.instance == null || Minimap.instance == null)
                {
                    s_tpTargets = null;
                    //s_teleportSourceIdx = -1;
                    s_teleportTargetIdx = -1;
                }
                else
                {
                    List<TPTarget> targets = new List<TPTarget>();

                    foreach (Player player in Player.GetAllPlayers())
                    {
                        targets.Add(new TPTarget(TPTarget.TargetType.Player, player));
                    }

                    foreach(ZNet.PlayerInfo player in ZNet.instance.GetPlayerList())
                    {
                        if (player.m_characterID == null || !player.m_publicPosition)
                            continue;

                        var result = targets.FirstOrDefault(t => t.targetType == TPTarget.TargetType.Player &&  t.player.GetPlayerName() == player.m_name);

                        if (result == null)
                        {
                                targets.Add(new TPTarget(TPTarget.TargetType.PlayerNet, player));
                        }
                    }

                    foreach (var pin in Minimap.instance.GetFieldValue<List<Minimap.PinData>>("m_pins"))
                    {
                        if (Minimap.instance.GetFieldValue<bool[]>("m_visibleIconTypes")[(int)pin.m_type]
                            && (Minimap.instance.GetFieldValue<float>("m_sharedMapDataFade") > 0f || pin.m_ownerID == 0L))
                        {
                            targets.Add(new TPTarget(TPTarget.TargetType.MapPin, pin));
                        }
                    }

                    s_tpTargets = targets;
                }

                s_players = Player.GetAllPlayers();

                s_updateTimer = Time.time + s_updateTimerInterval;
            }
        }

        public static void DisplayGUI()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(300), GUILayout.MaxWidth(300));
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_teleport_title_target"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_player_teleport_target :"));
                            s_teleportTargetIdx = RGUI.SelectionPopup(s_teleportTargetIdx, s_tpTargets?.Select(t => t.ToString()).ToArray());
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_teleport_button")))
                        {
                            if (s_players != null && Player.m_localPlayer != null)
                                {
                                    if (s_tpTargets != null && s_teleportTargetIdx < s_tpTargets.Count && s_teleportTargetIdx >= 0)
                                {
                                    var source = Player.m_localPlayer;
                                    var targetPosition = s_tpTargets[s_teleportTargetIdx].Position;

                                    if (targetPosition != null && targetPosition is Vector3 targetPositionValue)
                                    {
                                        source.TeleportTo(targetPositionValue, source.transform.rotation, true);
                                    }
                                }
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_teleport_title_coords"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_player_coordinates: ") + GetPlayerCoordinates(), GUILayout.ExpandWidth(false));
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(VTLocalization.instance.Localize("$vt_player_teleport_coordinates :"), GUILayout.ExpandWidth(false));
                            s_teleportCoordinates = GUILayout.TextField(s_teleportCoordinates, GUILayout.ExpandWidth(true));
                        }
                        GUILayout.EndHorizontal();

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_teleport_button")))
                        {
                            var coordinates = s_teleportCoordinates.Split(',');
                            if (Player.m_localPlayer != null && coordinates.Length == 3)
                            {
                                if (int.TryParse(coordinates[0], out int coord_x) && int.TryParse(coordinates[1], out int coord_y) && int.TryParse(coordinates[2], out int coord_z))
                                {
                                    Player.m_localPlayer.TeleportTo(new Vector3(coord_x, coord_y, coord_z), Player.m_localPlayer.transform.rotation, true);
                                }
                            }
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_player_teleport_restricted_title"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        if (GUILayout.Button(VTLocalization.instance.Localize("$vt_player_teleport_restricted : " + (s_bypassRestrictedTeleportable ? VTLocalization.s_cheatOn : VTLocalization.s_cheatOff))))
                        {
                            s_bypassRestrictedTeleportable = !s_bypassRestrictedTeleportable;
                        }
                    }
                    GUILayout.EndVertical();

                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private static string GetPlayerCoordinates()
        {
            Player localPlayer = Player.m_localPlayer;

            if (localPlayer == null)
            {
                return "[None]";
            }

            return localPlayer.transform.position.ToString("F0");
        }
    }
}

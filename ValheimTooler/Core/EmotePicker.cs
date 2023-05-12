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
    public static class EmotePicker
    {
        private static Rect s_emotePickerRect;

        public static void Start()
        {
            var _config = ConfigManager.instance;
            s_emotePickerRect = new Rect(_config.s_emotePickerInitialPosition.x, _config.s_emotePickerInitialPosition.y, 100, 10);

        }

        public static void Update()
        {
            return;
        }
        public static void DisplayGUI()
        {
            s_emotePickerRect = GUILayout.Window(1003, s_emotePickerRect, EmotePickerWindow, VTLocalization.instance.Localize("$vt_emotepicker_title"), GUILayout.Width(120));

            ConfigManager.instance.s_emotePickerInitialPosition = s_emotePickerRect.position;
        }

        public static void EmotePickerWindow(int windowID)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.Space(10);

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_emote_interactions"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        if (GUILayout.Button("Blow Kiss"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("blowkiss");
                            }
                        }

                        if (GUILayout.Button("Bow"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("bow");
                            }
                        }

                        if (GUILayout.Button("Challenge"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("challenge");
                            }
                        }

                        if (GUILayout.Button("Come here"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("comehere");
                            }
                        }

                        if (GUILayout.Button("Kneel"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("kneel");
                            }
                        }

                        if (GUILayout.Button("Point"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("point");
                                Player.m_localPlayer.FaceLookDirection();
                            }
                        }

                        if (GUILayout.Button("Shrug"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("Shrug");
                            }
                        }

                        if (GUILayout.Button("Thumbs Up"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("thumbsup");
                            }
                        }

                        if (GUILayout.Button("Wave"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("wave");
                            }
                        }


                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_emote_actions"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        if (GUILayout.Button("Cheer"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("cheer");
                            }
                        }

                        if (GUILayout.Button("Dance"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("dance");
                            }
                        }

                        if (GUILayout.Button("Flex"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("flex");
                            }
                        }

                        if (GUILayout.Button("Head Bang"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("headbang");
                            }
                        }

                        if (GUILayout.Button("Roar"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("roar");
                            }
                        }

                        if (GUILayout.Button("Sit"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("sit", false);
                            }
                        }

                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical(VTLocalization.instance.Localize("$vt_emote_emotions"), GUI.skin.box, GUILayout.ExpandWidth(false));
                    {
                        GUILayout.Space(EntryPoint.s_boxSpacing);

                        if (GUILayout.Button("Cower"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("cower");
                            }
                        }

                        if (GUILayout.Button("Cry"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("cry");
                            }
                        }

                        if (GUILayout.Button("Despair"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("despair");
                            }
                        }

                        if (GUILayout.Button("Laugh"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("laugh");
                            }
                        }

                        if (GUILayout.Button("No No No"))
                        {
                            if (Player.m_localPlayer != null)
                            {
                                Player.m_localPlayer.StartEmote("nonono");
                            }
                        }

                    }
                    GUILayout.EndVertical();

                }
                GUILayout.EndVertical();

            }
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

    }
}

using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace OwnerChecker
{
    class Utils
    {
        public static void TellInfo(IRocketPlayer caller, CSteamID ownerid, CSteamID group)
        {
            string charname = null;
            UnturnedPlayer owner = UnturnedPlayer.FromCSteamID(ownerid);

            bool isOnline = owner.Player != null ? owner.Player.channel != null ? true : false : false;

            if (!isOnline)
            {
                charname = GetCharName(ownerid);
            }

            if (OwnerChecker.Instance.Configuration.Instance.SayPlayerID)
            {
                UnturnedChat.Say(caller, "Owner ID: " + ownerid.ToString());
            }

            if (OwnerChecker.Instance.Configuration.Instance.SayPlayerCharacterName)
            {
                if (isOnline)
                {
                    UnturnedChat.Say(caller, "Character Name: " + owner.CharacterName);
                }

                else if (charname != null)
                {
                    UnturnedChat.Say(caller, "Character Name: " + charname);
                }

            }

            if (OwnerChecker.Instance.Configuration.Instance.SayPlayerSteamName)
                OwnerChecker.Instance.StartCoroutine(SteamRequest(ownerid.ToString(), caller));

            if (group != CSteamID.Nil)
            {
                if (OwnerChecker.Instance.Configuration.Instance.SayGroupID)
                {
                    if (group.ToString().Length == 18)
                    {
                        UnturnedChat.Say(caller, "Steam Group ID: " + group.ToString());
                    }
                    else
                    {
                            UnturnedChat.Say(caller, "In-Game Group ID: " + group.ToString());
                    }
                }

                if (OwnerChecker.Instance.Configuration.Instance.SayGroupName)
                {
                    if (group.ToString().Length == 18)
                    {
                        OwnerChecker.Instance.StartCoroutine(SteamGroupRequest(group.ToString(), caller));
                    }
                }
            }

            if (OwnerChecker.Instance.Configuration.Instance.OpenSteamProfile)
            {
                UnturnedPlayer player = UnturnedPlayer.FromName(caller.DisplayName);
                player.Player.sendBrowserRequest("SteamProfile", "http://steamcommunity.com/profiles/" + ownerid);
            }
        }

        public static string GetCharName(CSteamID id)
        {
            string dname = null;
            if (OwnerChecker.Instance.Configuration.Instance.usePlayerInfoLib)
            {
                if (OwnerChecker.IsDependencyLoaded("PlayerInfoLib"))
                {
                    PlayerInfoLibrary.PlayerData data = PlayerInfoLibrary.PlayerInfoLib.Database.QueryById(id, true);
                    dname = data.CharacterName;
                }
            }
            return dname;
        }
        
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static IEnumerator SteamRequest(string input, IRocketPlayer caller)
        {
            UnityWebRequest www = UnityWebRequest.Get("http://steamcommunity.com/profiles/" + input + "?xml=1");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                string data = getBetween(result, "<steamID>", "</steamID>").Replace(" ", "");

                data = data.Replace("<![CDATA[", "").Replace("]]>", "");

                UnturnedChat.Say(caller, "Steam Name: " + data.ToString());
            }
        }

        public static IEnumerator SteamGroupRequest(string input, IRocketPlayer caller)
        {
            UnityWebRequest www = UnityWebRequest.Get("http://steamcommunity.com/gid/" + input + "/memberslistxml?xml=1");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

                string data = getBetween(result, "<groupName>", "</groupName>").Replace(" ", "");

                data = data.Replace("<![CDATA[", "").Replace("]]>", "");

                UnturnedChat.Say(caller, "Group Name: " + data.ToString());
            }
        }
    }
}

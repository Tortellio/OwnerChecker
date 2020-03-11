using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OwnerChecker
{
    public class OwnerCheckerConfig : IRocketPluginConfiguration
    {
        [XmlElement("usePlayerInfoLib")]
        public bool usePlayerInfoLib;

        [XmlElement("SayPlayerID")]
        public bool SayPlayerID;

        [XmlElement("SayPlayerCharacterName")]
        public bool SayPlayerCharacterName;

        [XmlElement("SayPlayerSteamName")]
        public bool SayPlayerSteamName;

        [XmlElement("SayGroupID")]
        public bool SayGroupID;

        [XmlElement("SayGroupName")]
        public bool SayGroupName;

        [XmlElement("OpenSteamProfile")]
        public bool OpenSteamProfile;

        public void LoadDefaults()
        {
            usePlayerInfoLib = true;
            SayPlayerID = true;
            SayPlayerCharacterName = true;
            SayPlayerSteamName = true;
            SayGroupID = true;
            SayGroupName = true;
            OpenSteamProfile = false;
        }
    }
}

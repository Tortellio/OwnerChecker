using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OwnerChecker
{
    public class OwnerChecker : RocketPlugin<OwnerCheckerConfig>
    {
        public static OwnerChecker Instance;

        protected override void Load()
        {
            Instance = this;
            Rocket.Core.Logging.Logger.LogWarning("[OwnerChecker] Plugin loaded correctly!");
            if (Configuration.Instance.usePlayerInfoLib)
            {
                Rocket.Core.Logging.Logger.LogWarning("[OwnerChecker] Player Info Lib will be used!");
            }
            else
            {
                Rocket.Core.Logging.Logger.LogWarning("[OwnerChecker] Player Info Lib will not be used!");
            }
        }

        protected override void Unload()
        {
            Rocket.Core.Logging.Logger.LogWarning("[OwnerChecker] Plugin has been unloaded!");
        }
    }
}

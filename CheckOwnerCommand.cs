using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OwnerChecker
{
    class CheckOwnerCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "checkowner";

        public string Help => "Checks the owner of an barricade, vehicle, or structure";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "co" };

        public List<string> Permissions => new List<string> { "checkowner" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            RaycastInfo thingLocated = TraceRay(player, 2048f, RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.BARRICADE_INTERACT | RayMasks.STRUCTURE_INTERACT);

            if (thingLocated.vehicle != null)
            {
                InteractableVehicle vehicle = thingLocated.vehicle;

                if (vehicle.lockedOwner != CSteamID.Nil)
                {
                    Utils.TellInfo(caller, vehicle.lockedOwner, vehicle.lockedGroup);
                    return;
                }
                UnturnedChat.Say(caller, "Vehicle does not have an owner.");
                return;
            }
            else
            {
                if (!(thingLocated.transform != null)) { return; }
                byte x;
                byte y;
                Interactable2 component = thingLocated.transform.GetComponent<Interactable2>();
                if (!(component.transform != null)) { return; }
                CSteamID val2 = (CSteamID)component.owner;
                CSteamID val3 = (CSteamID)component.group;
                Interactable2SalvageBarricade interactable2SalvageBarricade = component as Interactable2SalvageBarricade;
                if (interactable2SalvageBarricade != null)
                {
                    if (!BarricadeManager.tryGetInfo(interactable2SalvageBarricade.root, out x, out y, out ushort _, out ushort index, out BarricadeRegion region))
                    {
                        return;
                    }
                    ItemBarricadeAsset asset = region.barricades[index].barricade.asset;
                    Utils.TellInfo(caller, val2, val3);
                }
                else
                {
                    if (!(component is Interactable2SalvageStructure))
                    {

                        return;
                    }
                    if (!StructureManager.tryGetInfo(thingLocated.transform, out y, out x, out ushort index2, out StructureRegion region2))
                    {

                        return;
                    }
                    ItemStructureAsset asset = region2.structures[index2].structure.asset;
                    Utils.TellInfo(caller, val2, val3);
                }
            }
        }

        public RaycastInfo TraceRay(UnturnedPlayer player, float distance, int masks)
        {
            return DamageTool.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), distance, masks);
        }
    }
}

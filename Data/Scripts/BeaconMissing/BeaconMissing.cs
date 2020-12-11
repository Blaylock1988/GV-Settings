using System;
using System.Collections.Generic;
using VRage.Game.Components;
//using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using VRage.ObjectBuilders;
using VRage.ModAPI;
//using VRage.Game.ModAPI;
using VRageMath;

namespace Guardians.BeaconMissing
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class Class1 : MySessionComponentBase
    {
        private bool init = false;

        public void Init()
        {
            init = true;
            MyAPIGateway.Session.LocalHumanPlayer.Controller.ControlledEntityChanged += OnCockpitEnter;

            //Stop updating
            MyAPIGateway.Utilities.InvokeOnGameThread(() => SetUpdateOrder(MyUpdateOrder.NoUpdate));

        }

        public override void UpdateBeforeSimulation() //Check for init
        {
            if (!init)
            {
                if (MyAPIGateway.Session?.LocalHumanPlayer?.Controller == null)
                    return;

                Init();
            }
        }
        private void OnCockpitEnter(VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg1, VRage.Game.ModAPI.Interfaces.IMyControllableEntity arg2)
        {
            string strRes = "";

            if (arg2 is Sandbox.Game.Entities.MyCockpit)
            {
                IMyTerminalBlock cockpit = arg2 as IMyTerminalBlock;
                List<IMyBeacon> allBeacons = new List<IMyBeacon>();
                List<VRage.Game.ModAPI.IMySlimBlock> blocks = new List<VRage.Game.ModAPI.IMySlimBlock>();
                cockpit.CubeGrid.GetBlocks(blocks);

                if (!blocks.Exists(x => x.FatBlock != null && x.FatBlock.GetType().ToString() == "Sandbox.Game.Entities.Cube.MyBeacon"))
                    strRes += "  * Grid does not have a beacon. It will be deleted on next cleanup.\n";
                if (cockpit.CubeGrid.BigOwners.Count == 0)
                    strRes += "  * Grid does not have an owner. It will be deleted on next cleanup.\n";
                if (cockpit.CubeGrid.DisplayName.Contains("Grid"))
                    strRes += "  * Grid does not have a unique name. Cannot have \"Grid\" in the name.\n";
                if (strRes.Length > 0)
                    strRes = "!!!WARNING!!! Please fix the following issues:\n" + strRes;
                MyAPIGateway.Utilities.ShowNotification(strRes, 6000, "Red");
            }

        }
    }
}
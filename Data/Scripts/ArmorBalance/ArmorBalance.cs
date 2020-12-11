using Sandbox.Definitions;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;

// Code is based on Gauge's Balanced Deformation code, but heavily modified for more control. 
namespace enenra.ArmorBalance
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class ArmorBalance : MySessionComponentBase
    {
        public const float lightArmorLargeDamageMod = 0.8f;
        public const float lightArmorLargeDeformationMod = 0.25f;
        public const float lightArmorSmallDamageMod = 0.5f;
        public const float lightArmorSmallDeformationMod = 0.25f;

        public const float heavyArmorLargeDamageMod = 1f;
        public const float heavyArmorLargeDeformationMod = 0.25f;
        public const float heavyArmorSmallDamageMod = 0.5f;
        public const float heavyArmorSmallDeformationMod = 0.25f;

        public const float turretLargeDamageMod = 0.5f;
        public const float weaponLargeDamageMod = 0.5f;
        public const float turretSmallDamageMod = 0.5f;
        public const float weaponSmallDamageMod = 0.5f;

        private bool isInit = false;

        private void DoWork()
        {
            foreach (MyDefinitionBase def in MyDefinitionManager.Static.GetAllDefinitions())
            {
                MyCubeBlockDefinition blockDef = def as MyCubeBlockDefinition;
				MyLargeTurretBaseDefinition turretDef = def as MyLargeTurretBaseDefinition;
				MyWeaponBlockDefinition weaponDef = def as MyWeaponBlockDefinition;

                if (blockDef != null && (blockDef.EdgeType == "Light" && (blockDef.BlockTopology != MyBlockTopology.TriangleMesh)))
                {
                    if (blockDef.CubeSize == MyCubeSize.Large)
                    {
                        blockDef.GeneralDamageMultiplier = lightArmorLargeDamageMod;
                        blockDef.DeformationRatio = lightArmorLargeDeformationMod;
                    }

                    if (blockDef.CubeSize == MyCubeSize.Small)
                    {
                        blockDef.GeneralDamageMultiplier = lightArmorSmallDamageMod;
                        blockDef.DeformationRatio = lightArmorSmallDeformationMod;
                    }
                }

                if (blockDef != null && (blockDef.EdgeType == "Heavy" && (blockDef.BlockTopology != MyBlockTopology.TriangleMesh)))
                {
                    if (blockDef.CubeSize == MyCubeSize.Large)
                    {
                        blockDef.GeneralDamageMultiplier = heavyArmorLargeDamageMod;
                        blockDef.DeformationRatio = heavyArmorLargeDeformationMod;
                    }

                    if (blockDef.CubeSize == MyCubeSize.Small)
                    {
                        blockDef.GeneralDamageMultiplier = heavyArmorSmallDamageMod;
                        blockDef.DeformationRatio = heavyArmorSmallDeformationMod;
                    }
                }

                if (turretDef != null)
                {
                    if (turretDef.CubeSize == MyCubeSize.Large)
                    {
                        turretDef.GeneralDamageMultiplier = turretLargeDamageMod;
                    }

                    if (turretDef.CubeSize == MyCubeSize.Small)
                    {
                        turretDef.GeneralDamageMultiplier = turretSmallDamageMod;
                    }
                }
                if (weaponDef != null)
                {
                    if (weaponDef.CubeSize == MyCubeSize.Large)
                    {
                        weaponDef.GeneralDamageMultiplier = weaponLargeDamageMod;
                    }

                    if (weaponDef.CubeSize == MyCubeSize.Small)
                    {
                        weaponDef.GeneralDamageMultiplier = weaponSmallDamageMod;
                    }
                }
            }
        }
        
        public override bool UpdatedBeforeInit()
        {
            DoWork();
            return true;
        }

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            DoWork();
        }

        public override void UpdateBeforeSimulation()
        {
            if (!isInit && MyAPIGateway.Session == null)
            {
                DoWork();
                isInit = true;
            }
        }
    }
}

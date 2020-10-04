using System.Collections.Generic;
using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("Loot Multiplier", "Orange", "1.1.2")]
    [Description("Multiply items in all loot containers in the game")]
    public class LootMultiplier : RustPlugin
    {
        #region Oxide Hooks
        
        private void OnLootSpawn(StorageContainer container)
        {
            timer.Once(config.delay, () =>
            {
                Multiply(container);
            });
        }
        
        #endregion

        #region Core

        private void Multiply(StorageContainer container)
        {
            if (container == null)
            {
                return;
            }

            var multiplier = 0;
            if (config.containers.TryGetValue(container.ShortPrefabName, out multiplier) == false)
            {
                return;
            }

            foreach (var item in container.inventory.itemList.ToArray())
            {
                var shortname = item.info.shortname;
                var category = item.info.category.ToString();
                
                if (config.blacklist.Contains(shortname) || config.blacklist.Contains(category))
                {
                    continue;
                }
                
                if (item.hasCondition && config.multiplyItemsWithCondition == false)
                {
                    continue;
                }
                
                item.amount *= multiplier;
            }
        }

        #endregion
        
        #region Configuration
        
        private static ConfigData config;
        
        private class ConfigData
        {
            [JsonProperty(PropertyName = "Shortname -> Multiplier")]
            public Dictionary<string, int> containers = new Dictionary<string, int>();

            [JsonProperty(PropertyName = "Multiply items with condition")]
            public bool multiplyItemsWithCondition;

            [JsonProperty(PropertyName = "Delay after spawning crate to multiply it")]
            public float delay;

            [JsonProperty(PropertyName = "Item Blacklist")]
            public List<string> blacklist = new List<string>();
        }
        
        private ConfigData GetDefaultConfig()
        {
            return new ConfigData 
            {
                multiplyItemsWithCondition = false,
                delay = 1f,
                containers = new Dictionary<string, int>
                {
                    {"loot-barrel-1", 2},
                    {"loot-barrel-2", 2},
                    {"loot_barrel_1", 2},
                    {"loot_barrel_2", 2},
                    {"crate_underwater_basic", 2},
                    {"crate_underwater_advanced", 2},
                    {"foodbox", 2},
                    {"trash-pile-1", 2},
                    {"minecart", 2},
                    {"oil_barrel", 2},
                    {"crate_basic", 2},
                    {"crate_mine", 2},
                    {"crate_tools", 2},
                    {"crate_normal", 2},
                    {"crate_normal_2", 2},
                    {"crate_normal_2_food", 2},
                    {"crate_normal_2_medical", 2},
                    {"crate_elite", 2},
                    {"codelockedhackablecrate", 2},
                    {"bradley_crate", 2},
                    {"heli_crate", 2},
                    {"supply_drop", 2}
                },
                blacklist = new List<string>
                {
                    "cctv.camera",
                    "targeting.computer",
                    "Weapon"
                }
            };
        }
        
        protected override void LoadConfig()
        {
            base.LoadConfig();
   
            try
            {
                config = Config.ReadObject<ConfigData>();
        
                if (config == null)
                {
                    LoadDefaultConfig();
                }
            }
            catch
            {
                LoadDefaultConfig();
            }

            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            PrintError("Configuration file is corrupt(or not exists), creating new one!");
            config = GetDefaultConfig();
        }

        protected override void SaveConfig()
        {
            Config.WriteObject(config);
        }
        
        #endregion
    }
}
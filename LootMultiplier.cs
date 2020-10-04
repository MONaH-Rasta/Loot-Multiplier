using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Oxide.Plugins
{
    [Info("Loot Multiplier", "Orange", "1.2.0")]
    [Description("Multiply items in all loot containers in the game")]
    public class LootMultiplier : RustPlugin
    {
        #region Configuration
        
        private static ConfigData configData;
        
        private class ConfigData
        {
            [JsonProperty(PropertyName = "Global settings")]
            public GlobalSettings globalS = new GlobalSettings();

            [JsonProperty(PropertyName = "Item settings")]
            public ItemSettings itemS = new ItemSettings();

            public class GlobalSettings
            {
                [JsonProperty(PropertyName = "Default Multiplier for new containers")]
                public int defaultContainerMultiplier = 1;

                [JsonProperty(PropertyName = "Default Multiplier for new Categories")]
                public int defaultCategoryMultiplier = 1;

                [JsonProperty(PropertyName = "Multiply items with condition")]
                public bool multiplyItemsWithCondition = false;

                [JsonProperty(PropertyName = "Delay after spawning container to multiply it")]
                public float delay = 1f;
            }

            public class ItemSettings
            {
                [JsonProperty(PropertyName = "Containers list (shortPrefabName: multiplier)")]
                public Dictionary<string, int> containers = new Dictionary<string, int>()
                {
                    {"bradley_crate", 1},
                    {"codelockedhackablecrate", 1},
                    {"codelockedhackablecrate_oilrig", 1},
                    {"crate_basic", 1},
                    {"crate_elite", 1},
                    {"crate_mine", 1},
                    {"crate_normal", 1},
                    {"crate_normal_2", 1},
                    {"crate_normal_2_food", 1},
                    {"crate_normal_2_medical", 1},
                    {"crate_tools", 1},
                    {"crate_underwater_advanced", 1},
                    {"crate_underwater_basic", 1},
                    {"foodbox", 1},
                    {"heli_crate", 1},
                    {"loot-barrel-1", 1},
                    {"loot-barrel-2", 1},
                    {"loot_barrel_1", 1},
                    {"loot_barrel_2", 1},
                    {"minecart", 1},
                    {"oil_barrel", 1},
                    {"supply_drop", 1},
                    {"trash-pile-1", 1},
                    {"vehicle_parts", 1}
                };

                [JsonProperty(PropertyName = "Categories list (Category: multiplier)")]
                public Dictionary<string, int> categories = new Dictionary<string, int>()
                {
                    {"Ammunition", 1},
                    {"Attire", 1},
                    {"Component", 1},
                    {"Construction", 1},
                    {"Electrical", 1},
                    {"Food", 1},
                    {"Fun", 1},
                    {"Items", 1},
                    {"Medical", 1},
                    {"Misc", 1},
                    {"Resources", 1},
                    {"Tool", 1},
                    {"Traps", 1},
                    {"Weapon", 1}
                };

                [JsonProperty(PropertyName = "Items list (shortname: multiplier)")]
                public Dictionary<string, int> items = new Dictionary<string, int>()
                {
                    {"metalpipe", 1},
                    {"scrap", 1},
                    {"tarp", 1}
                };

                [JsonProperty(PropertyName = "Item | Category Blacklist", ObjectCreationHandling = ObjectCreationHandling.Replace)]
                public List<string> blacklist = new List<string>()
                {
                    {"ammo.rocket.smoke"},
                    {"Attire"},
                    {"Weapon"}
                };
            }
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();

            try
            {
                configData = Config.ReadObject<ConfigData>();

                if (configData == null)
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
            PrintWarning("Creating a new configuration file");
            configData = new ConfigData();
        }

        protected override void SaveConfig()
        {
            Config.WriteObject(configData);
        }

        #endregion

        #region Oxide Hooks

        private void OnLootSpawn(StorageContainer container)
        {
            timer.Once(configData.globalS.delay, () =>
            {
                Multiply(container);
            });
        }
        
        #endregion

        #region Core

        private void Multiply(StorageContainer container)
        {
            if (container == null) return;

            var multiplier = 0;
            var containerName = container.ShortPrefabName;
            if (!configData.itemS.containers.ContainsKey(containerName))
            {
                configData.itemS.containers.Add(containerName, configData.globalS.defaultContainerMultiplier);
                Dictionary<string, int> sorted = new Dictionary<string, int>();
                foreach (KeyValuePair<string, int> cont in configData.itemS.containers.OrderBy(key => key.Key))
                {
                    sorted.Add(cont.Key, cont.Value);
                }
                configData.itemS.containers = sorted;
                SaveConfig();
            }

            multiplier = configData.itemS.containers[containerName];

            foreach (var item in container.inventory.itemList.ToArray())
            {
                var shortname = item.info.shortname;
                var category = item.info.category.ToString();

                if (!configData.itemS.categories.ContainsKey(category))
                {
                    configData.itemS.categories.Add(category, configData.globalS.defaultCategoryMultiplier);;
                    Dictionary<string, int> sorted = new Dictionary<string, int>();
                    foreach (KeyValuePair<string, int> cont in configData.itemS.categories.OrderBy(key => key.Key))
                    {
                        sorted.Add(cont.Key, cont.Value);
                    }
                    configData.itemS.categories = sorted;
                    SaveConfig();
                }

                if (configData.itemS.blacklist.Contains(shortname) || configData.itemS.blacklist.Contains(category))
                {
                    continue;
                }
                
                if (item.hasCondition && configData.globalS.multiplyItemsWithCondition == false)
                {
                    continue;
                }

                var itemMultiplier = configData.itemS.categories[category];
                if (configData.itemS.items.ContainsKey(shortname))
                {
                    itemMultiplier = itemMultiplier * configData.itemS.items[shortname];
                }

                multiplier = multiplier * itemMultiplier;
                item.amount *= (multiplier > 1 ? multiplier : 1);
            }
        }

        #endregion
    }
}
`Loot Multiplier` can multiply any loot from any container in the game. Compatible with all loot plugins.

All multipliers set to 1 by default and has no impact, you can install plugin safely and then edit default config file to your needs.
There are 4 types of lists:
* `Containers list` - is the list of all containers. All new containers automaticly added into this list.
* `Categories list` - is the list of all categories. All new categories automaticly added into this list.
* `Items list` - is the list of items. You can edit this list as you want.
* `Item|Category Blacklist` - is the list of items and categories that will be skipped by this plugin. You can edit this list as you want.

There are 3 multiplier types: Container, Category, Item. Consider that all them 3 will apply to item.
Blacklist overrides all other settings, so if you add item or category to blacklist this plugin will ignore it.


* `Example 1`: you want multiply only scrap in all containers x2.
You need to add `"scrap": 2` to your items list and you're done.

* `Example 2`: you want to multiply all loot in underwater crates x2 and heli crates x4.
You need to set `"crate_underwater_advanced": 2`, `"crate_underwater_basic": 2` and `"heli_crate": 4`.

* `Example 3`: you want to multiply all metal pipes x2, all elite crates loot x2, and all components x4.
You can't do exactly this one. First you set `"crate_elite": 2`, then `"Component": 2`.
This way for all Components in all Elite Crates you will get x2 (Elite Crate multiplier) * x2 (Component multiplier) = x4.
All components in other containers will have x2 multiplier.
Now, if you set `"metalpipe": 4` - you will have x16 multiplier for Metal Pipe in Elite Crates (x2 * x2 * x4).
You will have x8 multiplier for Metal Pipe in other containers (x4 * x2).
Also, if you will add, let's say Tech Trash to blacklist, it will have x1 multiplier no matter of all other settings.


## Configuration

```json
{
  "Global settings": {
    "Default Multiplier for new containers": 1,
    "Default Multiplier for new Categories": 1,
    "Multiply items with condition": false,
    "Delay after spawning container to multiply it": 1.0
  },
  "Item settings": {
    "Containers list (shortPrefabName: multiplier)": {
      "bradley_crate": 1,
      "codelockedhackablecrate": 1,
      "codelockedhackablecrate_oilrig": 1,
      "crate_basic": 1,
      "crate_elite": 1,
      "crate_mine": 1,
      "crate_normal": 1,
      "crate_normal_2": 1,
      "crate_normal_2_food": 1,
      "crate_normal_2_medical": 1,
      "crate_tools": 1,
      "crate_underwater_advanced": 1,
      "crate_underwater_basic": 1,
      "foodbox": 1,
      "heli_crate": 1,
      "loot-barrel-1": 1,
      "loot-barrel-2": 1,
      "loot_barrel_1": 1,
      "loot_barrel_2": 1,
      "minecart": 1,
      "oil_barrel": 1,
      "supply_drop": 1,
      "trash-pile-1": 1,
      "vehicle_parts": 1
    },
    "Categories list (Category: multiplier)": {
      "Ammunition": 1,
      "Attire": 1,
      "Component": 1,
      "Construction": 1,
      "Electrical": 1,
      "Food": 1,
      "Fun": 1,
      "Items": 1,
      "Medical": 1,
      "Misc": 1,
      "Resources": 1,
      "Tool": 1,
      "Traps": 1,
      "Weapon": 1
    },
    "Items list (shortname: multiplier)": {
      "metalpipe": 1,
      "scrap": 1,
      "tarp": 1
    },
    "Item | Category Blacklist": [
      "ammo.rocket.smoke",
      "Attire",
      "Weapon"
    ]
  }
}
```

## Demonstration

[Youtube video](https://youtu.be/NtRMNa8ebb0)
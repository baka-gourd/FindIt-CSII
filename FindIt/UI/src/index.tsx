import { ModRegistrar } from "cs2/modding";
import { VanillaComponentResolver } from "mods/VanillaComponentResolver/VanillaComponentResolver";
import { PrefabSelectionComponent } from "mods/PrefabSelection/PrefabSelection";
import { TopBarComponent } from "mods/TopBar/TopBar";

import mod from "../mod.json";
import { FindItIconComponent } from "mods/FindItIcon/FindItIcon";
import { RemoveVanillaTopBarComponent } from "mods/RemoveVanillaTopBar/RemoveVanillaTopBar";
import { RemoveVanillaAssetGridComponent } from "mods/RemoveVanillaAssetGrid/RemoveVanillaAssetGrid";
import { FindItMainContainerComponent, } from "mods/MainContainer/MainContainer";

const register: ModRegistrar = (moduleRegistry) => {
  // The vanilla component resolver is a singleton that helps extrant and maintain components from game that were not specifically exposed.
  VanillaComponentResolver.setRegistry(moduleRegistry);

  // This repalaces the asset grid.
  moduleRegistry.extend(
    "game-ui/game/components/asset-menu/asset-grid/asset-grid.tsx",
    "AssetGrid",
    RemoveVanillaAssetGridComponent
  );

  // This repalaces the asset category top bar
  moduleRegistry.extend(
    "game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.tsx",
    "AssetCategoryTabBar",
    RemoveVanillaTopBarComponent
  );

  // This appends an absolute position button for Find It. Shoul disappear with photo mode.
  moduleRegistry.append(
    'Game', 
    FindItIconComponent
  );

  // This wraps prefab selection and top bar components.
  moduleRegistry.append(
    'Game', 
    FindItMainContainerComponent
  );
  
  // This is just to verify using UI console that all the component registriations was completed.
  console.log(mod.id + " UI module registrations completed.");
};

export default register;

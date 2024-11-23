﻿using FindIt.Domain.Enums;
using FindIt.Domain.Utilities;
using FindIt.Systems;

using Game.Prefabs;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FindIt.Domain
{
	public class Filters
	{
		public string CurrentSearch { get; set; }
		public int SelectedDlc { get; set; } = int.MinValue;
		public ZoneTypeFilter SelectedZoneType { get; set; } = ZoneTypeFilter.Any;
		public int LotWidthFilter { get; set; }
		public int LotDepthFilter { get; set; }
		public ThemePrefab SelectedTheme { get; set; }
		public AssetPackPrefab SelectedAssetPack { get; set; }
		public bool SelectedThemeNone { get; set; }
		public bool HideAds { get; set; }
		public bool HideRandoms { get; set; }
		public bool HideVanilla { get; set; }
		public int BuildingLevelFilter { get; set; }
		public BuildingCornerFilter SelectedBuildingCorner { get; set; }
		public bool OnlyPlaced { get; set; }

		public IEnumerable<Func<PrefabIndex, bool>> GetFilterList()
		{
			if (HideAds)
			{
				yield return DoAdFilter;
			}

			if (HideRandoms)
			{
				yield return DoRandomFilter;
			}

			if (HideVanilla)
			{
				yield return DoNoVanillaFilter;
			}

			if (OnlyPlaced)
			{
				yield return DoOnlyPlacedFilter;
			}

			if (SelectedDlc != int.MinValue)
			{
				yield return DoDlcFilter;
			}

			if (SelectedZoneType != ZoneTypeFilter.Any)
			{
				yield return DoZoneTypeFilter;
			}

			if (SelectedBuildingCorner != BuildingCornerFilter.Any)
			{
				yield return DoBuildingCornerFilter;
			}

			if (BuildingLevelFilter != 0)
			{
				yield return DoBuildingLevelFilter;
			}

			if (LotDepthFilter != 0)
			{
				yield return DoLotDepthFilter;
			}

			if (LotWidthFilter != 0)
			{
				yield return DoLotWidthFilter;
			}

			if (SelectedThemeNone)
			{
				yield return DoNoThemeFilter;
			}
			else if (SelectedTheme != null)
			{
				yield return DoThemeFilter;
			}

			if (SelectedAssetPack != null)
			{
				yield return DoAssetPackFilter;
			}

			if (!string.IsNullOrWhiteSpace(CurrentSearch))
			{
				yield return Mod.Settings.StrictSearch ? DoStrictSearchFilter : DoSearchFilter;
			}
		}

		private bool DoSearchFilter(PrefabIndex prefab)
		{
			return CurrentSearch.SearchCheck(prefab.Name)
				|| CurrentSearch.SearchCheck(prefab.PrefabName);
			//|| prefab.Tags.Any(DoTagSearch);
		}

		private bool DoStrictSearchFilter(PrefabIndex prefab)
		{
			return prefab.Name.IndexOf(CurrentSearch, StringComparison.InvariantCultureIgnoreCase) >= 0
				|| prefab.PrefabName.IndexOf(CurrentSearch, StringComparison.InvariantCultureIgnoreCase) >= 0;
			//|| prefab.Tags.Any(DoTagSearch);
		}

		private bool DoTagSearch(string tag)
		{
			return tag.IndexOf(CurrentSearch, StringComparison.InvariantCultureIgnoreCase) >= 0;
		}

		private bool DoDlcFilter(PrefabIndex prefab)
		{
			return prefab.DlcId.id == SelectedDlc;
		}

		private bool DoBuildingCornerFilter(PrefabIndex prefab)
		{
			return (prefab.CornerType & SelectedBuildingCorner) == SelectedBuildingCorner;
		}

		private bool DoZoneTypeFilter(PrefabIndex prefab)
		{
			return prefab.ZoneType == SelectedZoneType;
		}

		private bool DoBuildingLevelFilter(PrefabIndex prefab)
		{
			return prefab.BuildingLevel == BuildingLevelFilter;
		}

		private bool DoLotWidthFilter(PrefabIndex prefab)
		{
			return (LotWidthFilter == 10 && prefab.LotSize.x >= 10) || prefab.LotSize.x == LotWidthFilter;
		}

		private bool DoLotDepthFilter(PrefabIndex prefab)
		{
			return (LotDepthFilter == 10 && prefab.LotSize.y >= 10) || prefab.LotSize.y == LotDepthFilter;
		}

		private bool DoAdFilter(PrefabIndex prefab)
		{
			return prefab.SubCategory != PrefabSubCategory.Props_Branding;
		}

		private bool DoRandomFilter(PrefabIndex prefab)
		{
			return !prefab.IsRandom;
		}

		private bool DoNoVanillaFilter(PrefabIndex prefab)
		{
			return !prefab.IsVanilla;
		}

		private bool DoOnlyPlacedFilter(PrefabIndex prefab)
		{
			return PrefabTrackingSystem.GetMostUsedCount(prefab) > 0;
		}

		private bool DoNoThemeFilter(PrefabIndex prefab)
		{
			return prefab.Theme == null;
		}

		private bool DoThemeFilter(PrefabIndex prefab)
		{
			return prefab.Theme == SelectedTheme;
		}

		private bool DoAssetPackFilter(PrefabIndex prefab)
		{
			return prefab.AssetPacks?.Any(x => x == SelectedAssetPack) ?? false;
		}
	}
}

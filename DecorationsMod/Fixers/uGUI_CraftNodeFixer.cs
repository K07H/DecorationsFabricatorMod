﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace DecorationsMod.Fixers
{
    public class uGUI_CraftNodeFixer
    {
#if BELOWZERO
        //private void CreateIcon(uGUI_CraftingMenu.Node node, RectTransform canvas, float size, float x, float y)
        public static void CreateIcon_Postfix(uGUI_CraftingMenu __instance, uGUI_CraftingMenu.Node node, RectTransform canvas, float size, float x, float y)
        {
            if (node != null && node.action == TreeAction.Expand)
            {
                if (__instance != null && node.icon != null)
                {
                    // If current node belongs to one of our custom fabricators
                    if (__instance.id == "DecorationsFabricator" && DecorationNodes.Contains(node.id))
                        node.icon.SetBackgroundColors(DNormal, DHover, DPressed);
                    else if (__instance.id == "FloraFabricator" && FloraNodes.Contains(node.id))
                        node.icon.SetBackgroundColors(FNormal, FHover, FPressed);
                }
            }
        }
#else
        private static readonly FieldInfo _view = typeof(uGUI_CraftNode).GetField("view", BindingFlags.NonPublic | BindingFlags.Instance);

        //protected void CreateIcon()
        public static void CreateIcon_Postfix(uGUI_CraftNode __instance)
        {
            if (_view != null && __instance.action == TreeAction.Expand)
            {
                var cm = (uGUI_CraftingMenu)_view.GetValue(__instance);
                if (cm != null && __instance.icon != null)
                {
                    // If current node belongs to one of our custom fabricators
                    if (cm.id == "DecorationsFabricator" && DecorationNodes.Contains(__instance.id))
                        __instance.icon.SetBackgroundColors(DNormal, DHover, DPressed);
                    else if (cm.id == "FloraFabricator" && FloraNodes.Contains(__instance.id))
                        __instance.icon.SetBackgroundColors(FNormal, FHover, FPressed);
                }
            }
        }
#endif

        // Decorations fabricator colors
        private static readonly Color DNormal = new Color(0.76863f, 0.23137f, 0.36863f);
        private static readonly Color DHover = new Color(0.60784f, 0.18039f, 0.60392f);
        private static readonly Color DPressed = new Color(0.76863f, 0.23137f, 0.43137f);

        // Flora fabricator colors
        private static readonly Color FNormal = new Color(0f, 0.56863f, 0.23922f);
        private static readonly Color FHover = new Color(0.2f, 0.70196f, 0f);
        private static readonly Color FPressed = new Color(0.28235f, 1f, 0f);

        /// <summary>Contains crafting node IDs of the decorations fabricator.</summary>
        private static List<string> DecorationNodes = new List<string>(new string[26]
        {
            "LabElements",
            "Electronics",
            "DrinksAndFood",
            "Precursor",
            "EggsTab",
            "LeviathansTab",
            "OfficeSupplies",
            "ToysAndAccessories",
            "NonFunctionalAnalyzers",
            "OpenedGlassContainers",
            "GlassContainers",
            "LabFurnitures",
            "WallMonitors",
            "CircuitBoxes",
            "SeamothFragments",
            "PrecursorWarperParts",
            "PrecursorWeapons",
            "PrecursorRelics",
            "PrecursorKeys",
            "DmgCreatureEggsTab",
            "NonDmgCreatureEggsTab",
            "LeviathanDolls",
            "SkeletonsParts",
            "Toys",
            "Posters",
            "Accessories"
        });

        /// <summary>Contains crafting node IDs of the flora fabricator.</summary>
        private static List<string> FloraNodes = new List<string>(new string[19]
        {
            "AirSeedsTab", // ConfigSwitcher.UseFlatScreenResolution
            "WaterSeedsTab", // ConfigSwitcher.UseFlatScreenResolution
            "PlantAirTab",
            "TreeAirTab",
            "TropicalPlantTab",
            "RegularAirSeedsTab",
            "RegularWaterSeedsTab",
            "PlantWaterTab",
            "TreeWaterTab",
            "AmphibiousPlantsTab",
            "CoralWaterTab",
            "EdibleRegularAirTab",
            "DecorativeBigAirTab",
            "DecorativeSmallAirTab",
            "DecorativeBushesWaterTab",
            "RegularSmallWaterTab",
            "DecorativeBigWaterTab",
            "FunctionalBigWaterTab",
            "RedGrassesTab"
        });
    }
}
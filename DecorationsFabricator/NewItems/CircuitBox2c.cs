﻿using SMLHelper;
using SMLHelper.Patchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DecorationsMod.NewItems
{
    public class CircuitBox2c : DecorationItem
    {
        public CircuitBox2c() // Feeds abstract class
        {
            this.ClassID = "CircuitBox2c"; // 3e4f0d4d-e8c9-4053-bada-980f442d50d1
            this.ResourcePath = "WorldEntities/Doodads/Debris/Wrecks/Decoration/circuit_box_01_02";

            this.GameObject = Resources.Load<GameObject>(this.ResourcePath);

            this.TechType = TechTypePatcher.AddTechType(this.ClassID,
                                                        LanguageHelper.GetFriendlyWord("CircuitBox2Name"),
                                                        LanguageHelper.GetFriendlyWord("CircuitBox2Description"),
                                                        true);

            this.Recipe = new TechDataHelper()
            {
                _craftAmount = 1,
                _ingredients = new List<IngredientHelper>(new IngredientHelper[2]
                    {
                        new IngredientHelper(TechType.Titanium, 1),
                        new IngredientHelper(TechType.Copper, 1)
                    }),
                _techType = this.TechType
            };
        }

        public override void RegisterItem()
        {
            if (this.IsRegistered == false)
            {
                // Remove Cube object to prevent physics
                GameObject cube = this.GameObject.FindChild("Cube");
                if (cube != null)
                    GameObject.DestroyImmediate(cube);

                // Remove rigid body to prevent physics bugs
                var rb = this.GameObject.GetComponent<Rigidbody>();
                if (rb != null)
                    GameObject.DestroyImmediate(rb);

                // Get box collider
                var collider = this.GameObject.GetComponent<BoxCollider>();
                if (collider == null)
                    collider = this.GameObject.AddComponent<BoxCollider>();
                collider.size = new Vector3(0.7f, 0.2f, 0.08f);

                // We can pick this item
                var pickupable = this.GameObject.GetComponent<Pickupable>();
                if (pickupable == null)
                {
                    pickupable = this.GameObject.AddComponent<Pickupable>();
                    pickupable.isPickupable = true;
                    pickupable.randomizeRotationWhenDropped = true;
                }

                // We can place this item
                var placeTool = this.GameObject.GetComponent<PlaceTool>();
                if (placeTool == null)
                {
                    placeTool = this.GameObject.AddComponent<PlaceTool>();
                    placeTool.allowedInBase = true;
                    placeTool.allowedOnBase = true;
                    placeTool.allowedOnCeiling = false;
                    placeTool.allowedOnConstructable = true;
                    placeTool.allowedOnGround = true;
                    placeTool.allowedOnRigidBody = true;
                    placeTool.allowedOnWalls = true;
                    placeTool.allowedOutside = false;
                    placeTool.rotationEnabled = true;
                    placeTool.enabled = true;
                    placeTool.hasAnimations = false;
                    placeTool.hasBashAnimation = false;
                    placeTool.hasFirstUseAnimation = false;
                    placeTool.mainCollider = collider;
                    placeTool.pickupable = pickupable;
                }

                // Add the new TechType to the hand-equipments
                CraftDataPatcher.customEquipmentTypes.Add(this.TechType, EquipmentType.Hand);

                // Set the buildable prefab
                CustomPrefabHandler.customPrefabs.Add(new CustomPrefab(this.ClassID, $"{DecorationItem.DefaultResourcePath}{this.ClassID}", this.TechType, this.GetPrefab));

                // Set the custom sprite
                CustomSpriteHandler.customSprites.Add(new CustomSprite(this.TechType, AssetsHelper.Assets.LoadAsset<Sprite>("circuitbox2c")));

                // Associate recipe to the new TechType
                CraftDataPatcher.customTechData[this.TechType] = this.Recipe;

                this.IsRegistered = true;
            }
        }

        public override GameObject GetPrefab()
        {
            GameObject prefab = GameObject.Instantiate(this.GameObject);

            // Update TechTag
            prefab.GetComponent<TechTag>().type = this.TechType;

            // Rotate
            GameObject model = prefab.FindChild("Starship_circuit_box_01_02");
            model.transform.localEulerAngles = new Vector3(0, -90, 90);

            // Add fabricating animation
            var fabricating = prefab.AddComponent<VFXFabricating>();
            fabricating.localMinY = -0.65f;
            fabricating.localMaxY = 0.8f;
            fabricating.posOffset = new Vector3(0f, 0.63f, 0.04f);
            fabricating.eulerOffset = new Vector3(0f, 0f, 0f);
            fabricating.scaleFactor = 0.6f;

            return prefab;
        }
    }
}

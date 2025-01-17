﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using Il2CppSystem;
using BepInEx.IL2CPP;
using UnityEngine;
using HarmonyLib;
using Archipelago;
using Newtonsoft.Json;
using UnhollowerRuntimeLib;
using Newtonsoft.Json.Bson;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

namespace TunicArchipelago {

    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class TunicArchipelago : BasePlugin {

        public static ManualLogSource Logger;
        public static RandomizerSettings Settings = new RandomizerSettings();

        public static string SettingsPath = Application.dataPath + "/../BepInEx/plugins/Tunic Archipelago/ArchipelagoSettings.json";
        public static string ItemTrackerPath = Application.persistentDataPath + "/Randomizer/ItemTracker.json";
        public static string SpoilerLogPath = Application.persistentDataPath + "/Randomizer/ArchipelagoSpoiler.log";
        public static ItemTracker Tracker;

        public override void Load() {
            Logger = Log;
            Logger.LogInfo(PluginInfo.NAME+ " v" + PluginInfo.VERSION + " loaded!");
            
            Application.runInBackground = true;

            Tracker = new ItemTracker();

            ClassInjector.RegisterTypeInIl2Cpp<Archipelago>();
            ClassInjector.RegisterTypeInIl2Cpp<WaveSpell>();

            ClassInjector.RegisterTypeInIl2Cpp<PaletteEditor>();
            UnityEngine.Object.DontDestroyOnLoad(new GameObject("palette editor gui", new Il2CppSystem.Type[]
            {
                Il2CppType.Of<PaletteEditor>()
            }) {
                hideFlags = HideFlags.HideAndDontSave
            });
            ClassInjector.RegisterTypeInIl2Cpp<QuickSettings>();
            UnityEngine.Object.DontDestroyOnLoad(new GameObject("quick settings gui", new Il2CppSystem.Type[]
            {
                Il2CppType.Of<QuickSettings>()
            }) {
                hideFlags = HideFlags.HideAndDontSave
            });
            
            if (!Directory.Exists(Application.persistentDataPath + "/Randomizer/")) {
                Directory.CreateDirectory(Application.persistentDataPath + "/Randomizer/");
            }

            if (!File.Exists(SettingsPath)) {
                Settings = new RandomizerSettings();
                File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(Settings, Formatting.Indented));
            } else {
                Settings = JsonConvert.DeserializeObject<RandomizerSettings>(File.ReadAllText(SettingsPath));
                Log.LogInfo("Loaded settings from file: " + JsonConvert.DeserializeObject<RandomizerSettings>(File.ReadAllText(SettingsPath)));
            }

            Harmony Harmony = new Harmony(PluginInfo.GUID);

            // Player Character
            Harmony.Patch(AccessTools.Method(typeof(PlayerCharacter), "Update"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "PlayerCharacter_Update_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(PlayerCharacter), "Start"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "PlayerCharacter_Start_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(PlayerCharacter), "creature_Awake"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "PlayerCharacter_creature_Awake_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(MagicSpell), "CheckInput"), null, new HarmonyMethod(AccessTools.Method(typeof(WaveSpell), "MagicSpell_CheckInput_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(PlayerCharacter._Die_d__481), "MoveNext"), null, new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "PlayerCharacter_Die_MoveNext_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(Monster), "IDamageable_ReceiveDamage"), new HarmonyMethod(AccessTools.Method(typeof(PlayerCharacterPatches), "Monster_IDamageable_ReceiveDamage_PrefixPatch")));

            // Scene Loader
            Harmony.Patch(AccessTools.Method(typeof(SceneLoader), "OnSceneLoaded"), new HarmonyMethod(AccessTools.Method(typeof(SceneLoaderPatches), "SceneLoader_OnSceneLoaded_PrefixPatch")), new HarmonyMethod(AccessTools.Method(typeof(SceneLoaderPatches), "SceneLoader_OnSceneLoaded_PostfixPatch")));

            // Options GUI
            Harmony.Patch(AccessTools.Method(typeof(OptionsGUI), "page_root"), new HarmonyMethod(AccessTools.Method(typeof(OptionsGUIPatches), "OptionsGUI_page_root_PrefixPatch")));

            // Items
            Harmony.Patch(AccessTools.Method(typeof(Chest), "IInteractionReceiver_Interact"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "Chest_IInteractionReceiver_Interact_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(Chest), "InterruptOpening"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "Chest_InterruptOpening_PrefixPatch")));

            Harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "moneySprayQuantityFromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "Chest_moneySprayQuantityFromDatabase_GetterPatch")));

            Harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "itemContentsfromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "Chest_itemContentsfromDatabase_GetterPatch")));

            Harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "itemQuantityFromDatabase"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "Chest_itemQuantityFromDatabase_GetterPatch")));

            Harmony.Patch(AccessTools.PropertyGetter(typeof(Chest), "shouldShowAsOpen"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "Chest_shouldShowAsOpen_GetterPatch")));

            Harmony.Patch(AccessTools.Method(typeof(Chest._openSequence_d__35), "MoveNext"), null, new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "Chest_openSequence_MoveNext_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(PagePickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "PagePickup_onGetIt_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(ItemPickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "ItemPickup_onGetIt_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(HeroRelicPickup), "onGetIt"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "HeroRelicPickup_onGetIt_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(ShopItem), "buy"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "ShopItem_buy_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(ShopItem), "IInteractionReceiver_Interact"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "ShopItem_IInteractionReceiver_Interact_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(TrinketWell), "TossedInCoin"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "TrinketWell_TossedInCoin_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(TrinketWell._giveTrinketUpgrade_d__14), "MoveNext"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "TrinketWell_giveTrinketUpgrade_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(PotionCombine), "Show"), null, new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "PotionCombine_Show_PostFixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(FairyCollection), "getFairyCount"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "FairyCollection_getFairyCount_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(UpgradeAltar), "DoOfferingSequence"), null, new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "UpgradeAltar_DoOfferingSequence_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(ButtonAssignableItem), "CheckFreeItemSpell"), null, new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "ButtonAssignableItem_CheckFreeItemSpell_PostfixPatch")));
            
            // Custom Item Behaviors
            Harmony.Patch(AccessTools.Method(typeof(BoneItemBehaviour), "onActionButtonDown"), new HarmonyMethod(AccessTools.Method(typeof(CustomItemBehaviors), "BoneItemBehavior_onActionButtonDown_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(BoneItemBehaviour), "confirmBoneUseCallback"), new HarmonyMethod(AccessTools.Method(typeof(CustomItemBehaviors), "BoneItemBehavior_confirmBoneUseCallback_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(SpearItemBehaviour), "onActionButtonDown"), new HarmonyMethod(AccessTools.Method(typeof(CustomItemBehaviors), "SpearItemBehaviour_onActionButtonDown_PrefixPatch")));

            // Page Display Patches
            Harmony.Patch(AccessTools.Method(typeof(PageDisplay), "ShowPage"), new HarmonyMethod(AccessTools.Method(typeof(PageDisplayPatches), "PageDisplay_Show_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(PageDisplay), "Show"), new HarmonyMethod(AccessTools.Method(typeof(PageDisplayPatches), "PageDisplay_Show_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(PageDisplay), "close"), new HarmonyMethod(AccessTools.Method(typeof(PageDisplayPatches), "PageDisplay_Close_PostfixPatch")));

            // Enemy Randomizer
            Harmony.Patch(AccessTools.Method(typeof(Campfire), "Interact"), new HarmonyMethod(AccessTools.Method(typeof(EnemyRandomizer), "Campfire_Interact_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(Campfire), "RespawnAtLastCampfire"), new HarmonyMethod(AccessTools.Method(typeof(EnemyRandomizer), "Campfire_RespawnAtLastCampfire_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(TunicKnightVoid), "onFlinch"), new HarmonyMethod(AccessTools.Method(typeof(EnemyRandomizer), "TunicKnightVoid_onFlinch_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(Monster._Die_d__77), "MoveNext"), new HarmonyMethod(AccessTools.Method(typeof(EnemyRandomizer), "Monster_Die_MoveNext_PrefixPatch")), new HarmonyMethod(AccessTools.Method(typeof(EnemyRandomizer), "Monster_Die_MoveNext_PostfixPatch")));
            
            // Finish Line
            Harmony.Patch(AccessTools.Method(typeof(SpeedrunFinishlineDisplay), "showFinishline"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "SpeedrunFinishlineDisplay_showFinishline_PrefixPatch")), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "SpeedrunFinishlineDisplay_showFinishline_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(SpeedrunFinishlineDisplay), "HideFinishline"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "SpeedrunFinishlineDisplay_HideFinishline_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(SpeedrunFinishlineDisplay), "addParadeIcon"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "SpeedrunFinishlineDisplay_addParadeIcon_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(SpeedrunFinishlineDisplay), "AndTime"), null, new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "SpeedrunFinishlineDisplay_AndTime_PostfixPatch")));
            
            Harmony.Patch(AccessTools.Method(typeof(GameOverDecision), "__retry"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "GameOverDecision___retry_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(GameOverDecision), "__newgame"), new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "GameOverDecision___newgame_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(GameOverDecision), "Start"), null, new HarmonyMethod(AccessTools.Method(typeof(SpeedrunFinishlineDisplayPatches), "GameOverDecision_Start_PostfixPatch")));

            // Misc
            Harmony.Patch(AccessTools.Method(typeof(FileManagementGUI), "rePopulateList"), null, new HarmonyMethod(AccessTools.Method(typeof(OptionsGUIPatches), "FileManagementGUI_rePopulateList_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(SaveFile), "GetNewSaveFileName"), null, new HarmonyMethod(AccessTools.Method(typeof(OptionsGUIPatches), "SaveFile_GetNewSaveFileName_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(InventoryDisplay), "Update"), new HarmonyMethod(AccessTools.Method(typeof(ItemStatsHUD), "InventoryDisplay_Update_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(PauseMenu), "__button_ReturnToTitle"), null, new HarmonyMethod(AccessTools.Method(typeof(SceneLoaderPatches), "PauseMenu___button_ReturnToTitle_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(InteractionTrigger), "Interact"), new HarmonyMethod(AccessTools.Method(typeof(InteractionPatches), "InteractionTrigger_Interact_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(BloodstainChest), "IInteractionReceiver_Interact"), new HarmonyMethod(AccessTools.Method(typeof(InteractionPatches), "BloodstainChest_IInteractionReceiver_Interact_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(HitReceiver), "ReceiveHit"), new HarmonyMethod(AccessTools.Method(typeof(SwordProgression), "HitReceiver_ReceiveHit_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(ToggleObjectAnimation), "SetToggle"), null, new HarmonyMethod(AccessTools.Method(typeof(EnemyRandomizer), "ToggleObjectAnimation_SetToggle_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(Parser), "findSymbol"), new HarmonyMethod(AccessTools.Method(typeof(TextBuilderPatches), "Parser_findSymbol_PrefixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(SpriteBuilder), "rebuild"), null, new HarmonyMethod(AccessTools.Method(typeof(TextBuilderPatches), "SpriteBuilder_rebuild_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(SpriteBuilder), "rebuild"), null, new HarmonyMethod(AccessTools.Method(typeof(TextBuilderPatches), "SpriteBuilder_rebuild_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(ShopManager._entrySequence_d__14), "MoveNext"), null, new HarmonyMethod(AccessTools.Method(typeof(ModelSwaps), "ShopManager_entrySequence_MoveNext_PostfixPatch")));

            Harmony.Patch(AccessTools.Method(typeof(ItemPresentation), "presentItem"), new HarmonyMethod(AccessTools.Method(typeof(ItemPatches), "ItemPresentation_presentItem_PrefixPatch")));
            
        }
    }
}

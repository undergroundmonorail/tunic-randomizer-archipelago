﻿using Archipelago.MultiClient.Net.Enums;
using BepInEx.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicArchipelago {
    public class SceneLoaderPatches {
        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static string SceneName;
        public static int SceneId;
        public static float TimeOfLastSceneTransition = 0.0f;
        public static bool SpawnedGhosts = false;

        public static bool SceneLoader_OnSceneLoaded_PrefixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {
            TimeOfLastSceneTransition = SaveFile.GetFloat("playtime");
            if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", SaveFile.GetInt("randomizer picked up 19 [Forest Belltower]"));
            }
            if (SceneName == "Sword Cave") {
                SaveFile.SetInt("chest open 19", SaveFile.GetInt("randomizer picked up 19 [Sword Cave]"));
            }
            return true;
        }

        public static void SceneLoader_OnSceneLoaded_PostfixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {

            ModelSwaps.SwappedThisSceneAlready = false;
            SpawnedGhosts = false;

            if (loadingScene.name == "Posterity" && !EnemyRandomizer.Enemies.ContainsKey("Phage")) {
                EnemyRandomizer.InitializeEnemies("Posterity");
                ModelSwaps.CreateOtherWorldItemBlocks();
                SceneLoader.LoadScene("TitleScreen");
                return;
            }
            if (loadingScene.name == "Library Hall" && !EnemyRandomizer.Enemies.ContainsKey("administrator_servant")) {
                EnemyRandomizer.InitializeEnemies("Library Hall");
                SceneLoader.LoadScene("Posterity");
                return;
            }
            if (loadingScene.name == "Cathedral Redux" && !EnemyRandomizer.Enemies.ContainsKey("Voidtouched")) {
                EnemyRandomizer.InitializeEnemies("Cathedral Redux");
                SceneLoader.LoadScene("Library Hall");
                return;
            }
            if (loadingScene.name == "Fortress Main" && !EnemyRandomizer.Enemies.ContainsKey("woodcutter")) {
                EnemyRandomizer.InitializeEnemies("Fortress Main");
                SceneLoader.LoadScene("Cathedral Redux");
                return;
            }
            if (loadingScene.name == "Fortress Reliquary" && !EnemyRandomizer.Enemies.ContainsKey("voidling redux")) {
                EnemyRandomizer.InitializeEnemies("Fortress Reliquary");
                SceneLoader.LoadScene("Fortress Main");
                return;
            }
            if (loadingScene.name == "ziggurat2020_1" && !EnemyRandomizer.Enemies.ContainsKey("administrator")) {
                EnemyRandomizer.InitializeEnemies("ziggurat2020_1");
                SceneLoader.LoadScene("Fortress Reliquary");
                return;
            }
            if (loadingScene.name == "Swamp Redux 2" && !EnemyRandomizer.Enemies.ContainsKey("bomezome_easy")) {
                EnemyRandomizer.InitializeEnemies("Swamp Redux 2");
                SceneLoader.LoadScene("ziggurat2020_1");
                return;
            }
            if (loadingScene.name == "Quarry Redux" && !EnemyRandomizer.Enemies.ContainsKey("Scavenger")) {
                EnemyRandomizer.InitializeEnemies("Quarry Redux");
                SceneLoader.LoadScene("Swamp Redux 2");
                return;
            }
            if (loadingScene.name == "Fortress Basement" && !EnemyRandomizer.Enemies.ContainsKey("Spider Small")) {
                EnemyRandomizer.InitializeEnemies("Fortress Basement");
                SceneLoader.LoadScene("Quarry Redux");
                return;
            }
            if (loadingScene.name == "frog cave main" && !EnemyRandomizer.Enemies.ContainsKey("Frog Small")) {
                EnemyRandomizer.InitializeEnemies("frog cave main");
                SceneLoader.LoadScene("Fortress Basement");
                return;
            }
            if (loadingScene.name == "Atoll Redux" && !EnemyRandomizer.Enemies.ContainsKey("plover")) {
                EnemyRandomizer.InitializeEnemies("Atoll Redux");
                SceneLoader.LoadScene("frog cave main");
                return;
            }
            if (loadingScene.name == "Archipelagos Redux" && ModelSwaps.GlowEffect == null) {
                ModelSwaps.SetupGlowEffect();
                EnemyRandomizer.InitializeEnemies("Archipelagos Redux");
                SceneLoader.LoadScene("Atoll Redux");
                return;
            }
            if (loadingScene.name == "Transit" && !ModelSwaps.Items.ContainsKey("Relic - Hero Sword")) {
                ModelSwaps.InitializeHeroRelics();
                SceneLoader.LoadScene("Archipelagos Redux");
                return;
            }
            if (loadingScene.name == "Spirit Arena" && ModelSwaps.ThirdSword == null) {
                ModelSwaps.InitializeThirdSword();
                SceneLoader.LoadScene("Transit");
                return;
            }
            if (loadingScene.name == "Library Arena" && ModelSwaps.SecondSword == null) {
                ModelSwaps.InitializeSecondSword();
                SceneLoader.LoadScene("Spirit Arena");
                return;
            }
            if (loadingScene.name == "Cathedral Arena" && !ModelSwaps.Chests.ContainsKey("Hyperdash")) {
                ModelSwaps.InitializeChestType("Hyperdash");
                SceneLoader.LoadScene("Library Arena");
                EnemyRandomizer.InitializeEnemies("Cathedral Arena");
                return;
            }
            if (loadingScene.name == "Overworld Redux" && ModelSwaps.Chests.Count == 0) {
                if (GhostHints.GhostFox == null) {
                    GhostHints.InitializeGhostFox();
                }
                ModelSwaps.InitializeItems();
                EnemyRandomizer.InitializeEnemies("Overworld Redux");
                SceneLoader.LoadScene("Cathedral Arena");
                return;
            }
            if (ModelSwaps.Chests.Count == 0 && loadingScene.name == "TitleScreen") {
                GameObject ArchipelagoObject = new GameObject("archipelago");
                Archipelago.instance = ArchipelagoObject.AddComponent<Archipelago>();   
                GameObject.DontDestroyOnLoad(ArchipelagoObject);
                if (Locations.VanillaLocations.Count == 0) {
                    Locations.CreateLocationLookups();
                }
                PaletteEditor.OdinRounded = Resources.FindObjectsOfTypeAll<Font>().Where(Font => Font.name == "Odin Rounded").ToList()[0];
                SceneLoader.LoadScene("Overworld Redux");
                return;
            }

            if (Camera.main != null && Camera.main.gameObject.GetComponentInParent<CycleController>() == null) {
                Camera.main.transform.parent.gameObject.AddComponent<CycleController>();
            }

            if (SceneName == "Overworld Redux" && (StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || 
                StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && SaveFile.GetInt("randomizer died to heir") != 1 && SaveFile.GetInt("randomizer hexagon quest enabled") == 0) {
                PlayerCharacterPatches.ResetDayNightTimer = 0.0f;
                Logger.LogInfo("Resetting time of day to daytime!");
            }

            Logger.LogInfo("Entering scene " + loadingScene.name + " (" + loadingScene.buildIndex + ")");
            SceneName = loadingScene.name;
            SceneId = loadingScene.buildIndex;

            PlayerCharacterPatches.StungByBee = false;
            // Fur, Puff, Details, Tunic, Scarf
            if (TunicArchipelago.Settings.RandomFoxColorsEnabled) {
                PaletteEditor.RandomizeFoxColors();
            }



            if (PlayerCharacterPatches.IsTeleporting) {
                PlayerCharacter.instance.cheapIceParticleSystemEmission.enabled = false;
                PlayerCharacter.instance.damageBoostParticleSystemEmission.enabled = false;
                PlayerCharacter.instance.staminaBoostParticleSystemEmission.enabled = false;
                PlayerCharacter.instance._CompletelyInvulnerableEvenToIFrameIgnoringAttacks_k__BackingField = false;
                PlayerCharacter.instance.ClearPoison();
                PlayerCharacterPatches.IsTeleporting = false;
                GameObject.Destroy(PlayerCharacter.instance.gameObject.GetComponent<Rotate>());
            }


            if (SceneName == "Waterfall") {
                List<string> RandomObtainedFairies = new List<string>();
                foreach (string Key in ItemLookup.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemLookup.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer obtained fairy " + Key) == 1;
                    if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1) {
                        RandomObtainedFairies.Add(Key);
                    }
                }

                StateVariable.GetStateVariableByName("SV_Fairy_5_Waterfall_Opened").BoolValue = SaveFile.GetInt("randomizer opened fairy chest Waterfall-(-47.0, 45.0, 10.0)") == 1;

                StateVariable.GetStateVariableByName("SV_Fairy_00_Enough Fairies Found").BoolValue = true;

                StateVariable.GetStateVariableByName("SV_Fairy_00_All Fairies Found").BoolValue = true;

            } else if (SceneName == "Spirit Arena") {
                for (int i = 0; i < 28; i++) {
                    SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer obtained page " + i) == 1 ? 1 : 0);
                }
                PlayerCharacterPatches.HeirAssistModeDamageValue = Locations.CheckedLocations.Values.ToList().Where(item => item).ToList().Count / 15;
                if (SaveFile.GetInt("randomizer hexagon quest enabled") == 1) {
                    Resources.FindObjectsOfTypeAll<Foxgod>().ToList()[0].gameObject.transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                    Resources.FindObjectsOfTypeAll<Foxgod>().ToList()[0].gameObject.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                }
            } else if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", 0);
            } else if (SceneName == "Overworld Interiors") {
                foreach (string Key in ItemLookup.HeroRelicLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemLookup.HeroRelicLookup[Key].Flag).BoolValue = Inventory.GetItemByName(Key).Quantity == 1;
                }
                GameObject.Destroy(GameObject.Find("_Special/Bed Toggle Trigger/"));
                if ((StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && SaveFile.GetInt("randomizer hexagon quest enabled") == 0) {
                    InteractionPatches.SetupDayNightHourglass();
                }
                if (GameObject.Find("_Offerings/ash group/")) {
                    GameObject.Find("_Offerings/ash group/").transform.position = new Vector3(-24.2824f, 29.8f, -45.4f);
                }
            } else if (SceneName == "TitleScreen") {
                TitleVersion.Initialize();
                if (!Archipelago.instance.integration.connected) {
                    Archipelago.instance.Connect();
                }
            } else if (SceneName == "Temple") {
                if (SaveFile.GetInt("randomizer hexagon quest enabled") == 1) {
                    foreach (GameObject Questagon in Resources.FindObjectsOfTypeAll<GameObject>().Where(Obj => Obj.name == "questagon")) {
                        Questagon.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                        Questagon.GetComponent<MeshRenderer>().receiveShadows = false;
                    }
                }
                if (TunicArchipelago.Settings.HeroPathHintsEnabled && Inventory.GetItemByName("Hyperdash").Quantity == 0) {
                    GameObject HintStatueGlow = GameObject.Instantiate(ModelSwaps.GlowEffect);
                    HintStatueGlow.SetActive(true);
                    HintStatueGlow.transform.position = new Vector3(13f, 0f, 49f);
                }
            } else if (SceneName == "Overworld Redux") {
                GameObject.Find("_Signposts/Signpost (3)/").GetComponent<Signpost>().message.text = $"#is wA too \"West Garden\"\n<#33FF33>[death] bEwAr uhv tArE [death]";
                GameObject.Find("_Environment Special/Door (1)/door/key twist").GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Key (House)"].GetComponent <MeshRenderer>().materials;
                if (TunicArchipelago.Settings.HeroPathHintsEnabled && SaveFile.GetInt($"randomizer picked up {Hints.MailboxHintId}") == 0) {
                    GameObject.Find("_Environment/_Decorations/Mailbox (1)/mailbox flag").transform.rotation = new Quaternion(0.5f, -0.5f, 0.5f, 0.5f);
                }
            } else if (SceneName == "Swamp Redux 2") {
                GhostHints.GhostFox.GetComponent<NPC>().nPCAnimState = NPC.NPCAnimState.GAZE;
                GameObject DoorHint = GameObject.Instantiate(GhostHints.GhostFox);
                DoorHint.transform.position = new Vector3(82.5f, 14f, 143.7f);
                DoorHint.transform.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
                LanguageLine DoorSecret = ScriptableObject.CreateInstance<LanguageLine>();
                DoorSecret.text = $"$$$... dOnt tehl ehnEwuhn, buht #aht \"DOOR\" bahk #Ar\nkahn bE \"OPENED\" fruhm #E \"OUTSIDE...\"";
                DoorHint.GetComponent<NPC>().script = DoorSecret;
                DoorHint.SetActive(true);
            } else if (SceneName == "g_elements") {
                if (SaveFile.GetInt("randomizer sent lost fox home") == 0) {
                    GhostHints.GhostFox.GetComponent<NPC>().nPCAnimState = NPC.NPCAnimState.SIT;
                    GameObject LostFox = GameObject.Instantiate(GhostHints.GhostFox);
                    LostFox.transform.position = new Vector3(-1.4098f, 0.0585f, 12.9491f);
                    LostFox.transform.transform.rotation = new Quaternion(0f, 0f, 0f, 1f);

                    LanguageLine LostFoxScript = ScriptableObject.CreateInstance<LanguageLine>();
                    if (Inventory.GetItemByName("Homeward Bone Statue").Quantity == 0) {
                        LostFoxScript.text = $"I lawst mI mahjik stOn ahnd kahnt gO hOm...---if yoo fInd it, kahn yoo bri^ it too mE?\nitz smawl ahnd grA.";
                    } else {
                        LostFoxScript.text = $"I lawst mI mahjik stOn ahnd kahnt gO hOm...---... wAt, yoo fownd it! plEz, yooz it now!";
                    }
                    LostFox.GetComponent<NPC>().script = LostFoxScript;

                    LostFox.SetActive(true);
                }
            } else if (SceneName == "Posterity") {
                if (SaveFile.GetInt("randomizer sent lost fox home") == 1) {
                    GhostHints.GhostFox.GetComponent<NPC>().nPCAnimState = NPC.NPCAnimState.SIT;
                    GameObject SavedFox = GameObject.Instantiate(GhostHints.GhostFox);
                    SavedFox.transform.position = new Vector3(80.6991f, 15.9245f, 115.0217f);
                    SavedFox.transform.transform.localEulerAngles = new Vector3(0f, 270f, 0f);

                    LanguageLine SavedFoxScript = ScriptableObject.CreateInstance<LanguageLine>();
                    SavedFoxScript.text = $"%ah^k yoo for sehnding mE hOm.---plEz kEp #aht stOn ahs A rEword. it wil tAk yoo\nbahk too yor wurld.";
                    SavedFox.GetComponent<NPC>().script = SavedFoxScript;

                    SavedFox.SetActive(true);
                }
            } else if (SceneName == "Shop") {
                if (new System.Random().Next(100) < 3) {
                    GameObject.Find("merchant").SetActive(false);
                    GameObject.Find("Environment").transform.GetChild(3).gameObject.SetActive(true);
                }
            } else if (SceneName == "ShopSpecial") {
                if (new System.Random().Next(100) < 3) {
                    GameObject.Find("merchant (1)").SetActive(false);
                    GameObject.Find("Environment").transform.GetChild(3).gameObject.SetActive(true);
                }
            } else {
                foreach (string Key in ItemLookup.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemLookup.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer opened fairy chest " + Key) == 1;
                }
                for (int i = 0; i < 28; i++) {
                    SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer picked up page " + i) == 1 ? 1 : 0);
                }
                foreach (string Key in ItemLookup.HeroRelicLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemLookup.HeroRelicLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer picked up " + ItemLookup.HeroRelicLookup[Key].OriginalPickupLocation) == 1;
                }
            }


            if (Archipelago.instance != null && Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                try {
                    if (TunicArchipelago.Settings.UseCustomTexture) {
                        PaletteEditor.LoadCustomTexture();
                    }
                } catch (Exception ex) {
                    Logger.LogError("An error occurred applying custom texture:");
                    Logger.LogError(ex.Message + " " + ex.StackTrace);
                }
                try {
                    if (!ModelSwaps.SwappedThisSceneAlready && (ItemLookup.ItemList.Count > 0 && SaveFile.GetInt("seed") != 0)) {
                        ModelSwaps.SwapItemsInScene();
                    }
                } catch (Exception ex) {
                    Logger.LogError("An error occurred swapping item models in this scene:");
                    Logger.LogError(ex.Message + " " + ex.StackTrace);
                }

                if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetInt("randomizer holy cross unlocked") == 0) {
                    foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                        SpellToggle.gameObject.GetComponent<ToggleObjectBySpell>().enabled = false;
                    }
                }

                try {
                    if (TunicArchipelago.Settings.GhostFoxHintsEnabled && GhostHints.HintGhosts.Count > 0 && SaveFile.GetInt("seed") != 0) {
                        GhostHints.SpawnHintGhosts(SceneName);
                        SpawnedGhosts = true;
                    }
                } catch (Exception ex) {
                    Logger.LogError("An error occurred spawning hint ghost foxes:");
                    Logger.LogError(ex.Message + " " + ex.StackTrace);
                }


                if (TunicArchipelago.Settings.EnemyRandomizerEnabled && EnemyRandomizer.Enemies.Count > 0 && !EnemyRandomizer.ExcludedScenes.Contains(SceneName)) {
                    EnemyRandomizer.SpawnNewEnemies();
                }

                try {
                    FairyTargets.CreateFairyTargets();
                } catch (Exception ex) {
                    Logger.LogError("An error occurred creating new fairy seeker spell targets:");
                    Logger.LogError(ex.Message + " " + ex.StackTrace);
                }

                if (TunicArchipelago.Settings.RealestAlwaysOn) {
                    try {
                        GameObject.FindObjectOfType<RealestSpell>().SpellEffect();
                    } catch (Exception e) {

                    }
                }

            }

            ItemTracker.SaveTrackerFile();
        }

        public static void PauseMenu___button_ReturnToTitle_PostfixPatch(PauseMenu __instance) {

            if (ItemStatsHUD.HexagonQuest != null) {
                ItemStatsHUD.HexagonQuest.SetActive(false);
            }

            SceneName = "TitleScreen";
        }

    }

}
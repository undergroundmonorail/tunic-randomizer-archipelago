﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx.Logging;
using UnhollowerBaseLib;
using UnityEngine.SceneManagement;
using static TunicArchipelago.SaveFlags;

namespace TunicArchipelago {

    public class EnemyRandomizer {

        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static Dictionary<string, GameObject> Enemies = new Dictionary<string, GameObject>() { };

        public static Dictionary<string, List<string>> DefeatedEnemyTracker = new Dictionary<string, List<string>>();

        public static Dictionary<string, string> EnemiesInCurrentScene = new Dictionary<string, string>() { };

        public static List<string> ExcludedEnemies = new List<string>() {
            "tech knight boss",
            "Spidertank",
            "Scavenger Boss"
        };

        public static List<string> ExcludedScenes = new List<string>() {
            "Library Arena",
            "Fortress Arena",
            "Spirit Arena"
        };

        public static Dictionary<string, List<string>> LocationEnemies = new Dictionary<string, List<string>>() {
            {
                "Overworld Redux",
                new List<string>() {
                    "Blob",
                    "BlobBig",
                    "Hedgehog",
                    "HedgehogBig",
                    "Bat",
                    "Skuladot redux",
                    "Skuladot redux_shield",
                    "Skuladot redux Big",
                    "Honourguard",
                    "beefboy",
                    "bomezome big",
                    "Fox enemy zombie",
                    "_Turret",
                }
            },
            {
                "Archipelagos Redux",
                new List<string>() {
                    "crocodoo Voidtouched",
                    "crocodoo",
                    "Fairyprobe Archipelagos (2)",
                    "Fairyprobe Archipelagos (Dmg)",
                    "tunic knight void"
                }
            },
            {
                "Atoll Redux",
                new List<string>() {
                    "plover",
                    "Crow",
                    "Crabbit",
                    "Crabbo (1)",
                    "Crabbit with Shell",
                    "Spinnerbot Corrupted",
                    "Spinnerbot Baby",
                }
            },
            {
                "frog cave main",
                new List<string>() {
                    "Frog Small",
                    "Frog Spear",
                    "Frog (7)",
                    "Wizard_Support"
                }
            },
            {
                "Fortress Basement",
                new List<string>() {
                    "Spider Small",
                    "Spider Big",
                    "Wizard_Sword",
                    "Wizard_Candleabra",
                }
            },
            {
                "Crypt",
                new List<string>() {
                    "Shadowreaper"
                }
            },
            {
                "Quarry",
                new List<string>() {
                    "Scavenger_stunner"
                }
            },
            {
                "Quarry Redux",
                new List<string>() {
                    "Scavenger",
                    "Scavenger_miner",
                    "Scavenger_support"
                }
            },
            {
                "Swamp Redux 2",
                new List<string>() {
                    "bomezome_easy",
                    "bomezome_fencer",
                    "Ghostfox_monster",
                    "Gunslinger",
                    "sewertentacle",
                    "Crow Voidtouched"
                }
            },
            {
                "Cathedral Arena",
                new List<string>() {
                    "tech knight ghost",
                }
            },
            {
                "ziggurat2020_1",
                new List<string>() {
                    "administrator",
                }
            },
            {
                "ziggurat2020_3",
                new List<string>() {
                    "Centipede from egg (Varient)",
                }
            },
            {
                "Fortress Reliquary",
                new List<string> () {
                    "voidling redux"
                }
            },
            {
                "Fortress Main",
                new List<string> () {
                    "woodcutter"
                }
            },
            {
                "Cathedral Redux",
                new List<string> () {
                    "Voidtouched",
                    "Fox enemy"
                }
            },
            {
                "Library Hall",
                new List<string> () {
                    "administrator_servant"
                }
            },
            {
                "Posterity",
                new List<string> () {
                    "Phage",
                    "Ghost Knight"
                }
            }
        };

        public static Dictionary<string, List<string>> EnemyRankings = new Dictionary<string, List<string>>() {
            {
                "Average",
                new List<string>() {
                    "Blob",
                    "Hedgehog",
                    "Skuladot redux",
                    "plover",
                    "Spinnerbot Baby",
                    "Crabbit",
                    "Crabbit with Shell",
                    "Fox enemy zombie",
                    "BlobBig",
                    "BlobBigger",
                    "HedgehogBig",
                    "Bat",
                    "Spider Small",
                    "bomezome_easy",
                    "Fairyprobe Archipelagos",
                    "Fairyprobe Archipelagos (Dmg)",
                    "Skuladot redux_shield",
                    "Crabbo",
                    "Spinnerbot Corrupted",
                    "Turret",
                    "Hedgehog Trap",
                    "administrator_servant",
                    "Phage",
                }
            },
            {
                "Strong",
                new List<string>() {
                    "Wizard_Candleabra",
                    "sewertentacle",
                    "Honourguard",
                    "Skuladot redux Big",
                    "Crow",
                    "crocodoo",
                    "crocodoo Voidtouched",
                    "Scavenger",
                    "Scavenger_miner",
                    "Scavenger_support",
                    "Scavenger_stunner",
                    "bomezome_fencer",
                    "Ghostfox_monster",
                    "voidling redux",
                    "Frog Spear",
                    "Frog",
                    "Frog Small",
                    "Spider Big",
                    "Wizard_Sword",
                    "Wizard_Support",
                    "Crow Voidtouched",
                    "woodcutter",
                    "Fox enemy",
                    "Centipede",
                    "Ghost Knight",
                }
            },
            {
                "Intense",
                new List<string>() {
                    "administrator",
                    "Gunslinger",
                    "beefboy",
                    "bomezome big",
                    "tech knight ghost",
                    "tunic knight void",
                    "Voidtouched",
                    "Shadowreaper",
                }
            }
        };

        public static Dictionary<string, string> ProperEnemyNames = new Dictionary<string, string>() {
            { "Blob", $"\"Blob\"" },
            { "Hedgehog", $"\"Hedgehog\"" },
            { "Skuladot redux", $"\"Rudeling\"" },
            { "plover", $"\"Plover\"" },
            { "Spinnerbot Baby", $"\"Baby Slorm\"" },
            { "Crabbit", $"\"Crabbit\"" },
            { "Crabbit with Shell", $"gAmkyoob \"Crabbit\"" },
            { "Fox enemy zombie", $"yoo...\"?\"" },
            { "BlobBig", $"\"Blob\" (big)" },
            { "BlobBigger", $"\"Blob\" (bigur)" },
            { "HedgehogBig", $"\"Hedgehog\" (big)" },
            { "Bat", $"\"Phrend\"" },
            { "Spider Small", $"\"Spyrite\"" },
            { "bomezome_easy", $"\"Fleemer\"" },
            { "Fairyprobe Archipelagos", $"\"Fairy\" (Is)" },
            { "Fairyprobe Archipelagos (Dmg)", $"\"Fairy\" (fIur)" },
            { "Skuladot redux_shield", $"\"Rudeling\" ($Eld)" },
            { "Crabbo", $"\"Crabbo\"" },
            { "Spinnerbot Corrupted", $"\"Slorm\"" },
            { "Turret", $"\"Autobolt\"" },
            { "Hedgehog Trap", $"\"Laser Turret\"" },
            { "administrator_servant", $"\"Administrator\" (frehnd)" },
            { "Phage", $"slorm...\"?\"" },
            { "Ghost Knight", $"\"???\"" },
            { "Wizard_Candleabra", $"\"Custodian\" (kahnduhlahbruh)" },
            { "sewertentacle", $"\"Tentacle\"" },
            { "Honourguard", $"\"Envoy\"" },
            { "Skuladot redux Big", $"\"Guard Captain\"" },
            { "Crow", $"\"Husher\"" },
            { "crocodoo", $"\"Terry\"" },
            { "crocodoo Voidtouched", $"\"Terry\" (void)" },
            { "Scavenger", $"\"Scavenger\" (snIpur)" },
            { "Scavenger_miner", $"\"Scavenger\" (mInur)" },
            { "Scavenger_support", $"\"Scavenger\" (suhport)" },
            { "Scavenger_stunner", $"\"Scavenger\" (stuhnur)" },
            { "bomezome_fencer", $"\"Fleemer\" (fehnsur)" },
            { "Ghostfox_monster", $"\"Lost Echo\"" },
            { "voidling redux", $"\"Voidling\"" },
            { "Frog Spear", $"\"Frog\" (spEr) [frog]" },
            { "Frog", $"\"Frog\" [frog]" },
            { "Frog Small", $"\"Frog\" (smawl) [frog]" },
            { "Spider Big", $"\"Sappharach\"" },
            { "Wizard_Sword", $"\"Custodian\"" },
            { "Wizard_Support", $"\"Custodian\" (suhport)" },
            { "Crow Voidtouched", $"\"Husher\" (void)" },
            { "woodcutter", $"\"Woodcutter\"" },
            { "Fox enemy", $"\"You...?\"" },
            { "administrator", $"\"Administrator\"" },
            { "Gunslinger", $"\"Gunslinger\"" },
            { "beefboy", $"\"Beefboy\"" },
            { "bomezome big", $"\"Fleemer\" (lRj)" },
            { "tech knight ghost", $"\"Garden Knight...?\"" },
            { "tunic knight void", $"gRdin nIt...\"?\"" },
            { "Voidtouched", $"\"Voidtouched\"" },
            { "Centipede", $"\"Centipede\"" },
            { "Shadowreaper", $"\"Shadowreaper\"" },
        };

        public static void CreateAreaSeeds() {
            System.Random Random = new System.Random(SaveFile.GetInt("seed"));
            foreach (String Scene in Locations.AllScenes) {
                SaveFile.SetInt($"randomizer enemy seed {Scene}", Random.Next());
            }
        }

        public static void InitializeEnemies(string SceneName) {
            List<Monster> Monsters = Resources.FindObjectsOfTypeAll<Monster>().ToList();
            Dictionary<string, string> RenamedEnemies = new Dictionary<string, string>() {
                { "_Turret", "Turret" },
                { "Fairyprobe Archipelagos (2)", "Fairyprobe Archipelagos" },
                { "Crabbo (1)", "Crabbo" },
                { "Frog (7)", "Frog" },
                { "Hedgehog Trap (1)", "Hedgehog Trap" },
                { "Centipede from egg (Varient)", "Centipede" },
            };
            foreach (string LocationEnemy in LocationEnemies[SceneName]) {
                string EnemyName = LocationEnemy;
                if (RenamedEnemies.ContainsKey(EnemyName)) {
                    EnemyName = RenamedEnemies[EnemyName];
                }
                if (EnemyName == "voidling redux") {
                    Enemies[EnemyName] = GameObject.Instantiate(Monsters.Where(Monster => Monster.name == LocationEnemy && Monster.transform.parent.name == "_Night Encounters").ToList()[0].gameObject);
                } else {
                    Enemies[EnemyName] = GameObject.Instantiate(Monsters.Where(Monster => Monster.name == LocationEnemy).ToList()[0].gameObject);
                }
                GameObject.DontDestroyOnLoad(Enemies[EnemyName]);
                Enemies[EnemyName].SetActive(false);

                Enemies[EnemyName].transform.position = new Vector3(-30000f, -30000f, -30000f);
                if (EnemyName == "Blob") {
                    Enemies["BlobBigger"] = GameObject.Instantiate(Monsters.Where(Monster => Monster.name == LocationEnemy).ToList()[0].gameObject);
                    Enemies["BlobBigger"].transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                    GameObject.DontDestroyOnLoad(Enemies["BlobBigger"]);
                    Enemies["BlobBigger"].SetActive(false);
                    Enemies["BlobBigger"].transform.position = new Vector3(-30000f, -30000f, -30000f);
                    Enemies["BlobBigger"].name = "BlobBigger Prefab";
                    Enemies["BlobBigger"].GetComponent<Blob>().attackDistance = 3f;
                }
                if (EnemyName == "Centipede") {
                    Enemies[EnemyName].GetComponent<Centipede>().maxBeamDistance = 10f;
                    Enemies[EnemyName].GetComponent<Centipede>().attackDistance = 5f;
                }

                Enemies[EnemyName].name = EnemyName + " Prefab";
            }
            if (SceneName == "Archipelagos Redux") {
                Enemies["tunic knight void"].GetComponent<ZTarget>().isActive = true;
                Enemies["tunic knight void"].GetComponent<CapsuleCollider>().enabled = true;
                for (int i = 0; i < Enemies["tunic knight void"].transform.childCount - 2; i++) {
                    Enemies["tunic knight void"].transform.GetChild(i).gameObject.SetActive(true);
                }
                Dat.floatDatabase["mpCost_Spear_mp2"] = 40f;
            }
            if (SceneName == "Fortress Basement") {
                Enemies["Hedgehog Trap"] = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<TurretTrap>().ToList()[0].gameObject);
                GameObject.DontDestroyOnLoad(Enemies["Hedgehog Trap"]);
                Enemies["Hedgehog Trap"].SetActive(false);

                Enemies["Hedgehog Trap"].transform.position = new Vector3(-30000f, -30000f, -30000f);
                Enemies["Hedgehog Trap"].name = "Hedgehog Trap Prefab";
            }

        }

        public static void SpawnNewEnemies() {
            EnemiesInCurrentScene.Clear();

            string CurrentScene = SceneLoaderPatches.SceneName;

            List<BombFlask> bombFlasks = Resources.FindObjectsOfTypeAll<BombFlask>().Where(bomb => bomb.name != "Firecracker").ToList();

            System.Random Random;
            if (TunicArchipelago.Settings.EnemyGeneration == RandomizerSettings.EnemyGenerationType.SEEDED) {
                Random = new System.Random(SaveFile.GetInt($"randomizer enemy seed {CurrentScene}"));
            } else {
                Random = new System.Random();
            }
            List<GameObject> Monsters = Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => (Monster.GetComponent<Monster>() != null || Monster.GetComponent<TurretTrap>() != null) && Monster.transform.parent != null && !Monster.transform.parent.name.Contains("split tier") && !ExcludedEnemies.Contains(Monster.name) && !Monster.name.Contains("Prefab")).ToList();
            if (CurrentScene == "Archipelagos Redux") {
                Monsters = Monsters.Where(Monster => Monster.transform.parent.parent == null || Monster.transform.parent.parent.name != "_Environment Prefabs").ToList();
            }
            if (CurrentScene == "Forest Belltower") {
                Monsters = Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => (Monster.GetComponent<Monster>() != null || Monster.GetComponent<TurretTrap>() != null) && !Monster.name.Contains("Prefab")).ToList();
            }
            if (TunicArchipelago.Settings.ExtraEnemiesEnabled && CurrentScene == "Library Hall" && !CycleController.IsNight) {
                GameObject.Find("beefboy statues").SetActive(false);
                GameObject.Find("beefboy statues (2)").SetActive(false);
                foreach (GameObject Monster in Monsters) {
                    Monster.transform.parent = null;
                }
            }
            if (CurrentScene == "Fortress East" || CurrentScene == "Frog Stairs") {
                Monsters = Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => (Monster.GetComponent<Monster>() != null || Monster.GetComponent<TurretTrap>() != null) && !Monster.name.Contains("Prefab")).ToList();
            }
            if (CurrentScene == "Cathedral Redux") {
                Monsters.AddRange(Resources.FindObjectsOfTypeAll<GameObject>().Where(Monster => Monster.GetComponent<Crow>() != null && !Monster.name.Contains("Prefab")).ToList());
            }
            if (CurrentScene == "Forest Boss Room" && GameObject.Find("Skuladot redux") != null) {
                Monsters.Add(GameObject.Find("Skuladot redux"));
            }
            if (TunicArchipelago.Settings.ExtraEnemiesEnabled && CurrentScene == "Monastery") {
                Resources.FindObjectsOfTypeAll<Voidtouched>().ToList()[0].gameObject.transform.parent = null;
            }

            Monsters = Monsters.Where(Monster => Monster.gameObject.scene.name == CurrentScene).ToList();
            
            int i = 0;
            foreach (GameObject Enemy in Monsters) {
                GameObject NewEnemy = null;
                try {
                    List<string> EnemyKeys = Enemies.Keys.ToList();
                    if (CurrentScene == "Cathedral Arena") {
                        EnemyKeys.Remove("administrator_servant");
                        EnemyKeys.Remove("Hedgehog Trap");
                        if (Inventory.GetItemByName("Wand").Quantity == 0) {
                            EnemyKeys.Remove("Crabbit with Shell");
                        }
                        if (Enemy.transform.parent.name.Contains("Wave")) {
                            float x = (float)(Random.NextDouble() * 15) - 15f;

                            float z = (float)(Random.NextDouble() * 23) - 23f;
                            Enemy.transform.position = new Vector3(x, 0, z);
                        }
                    }
                    if (CurrentScene == "ziggurat2020_1" && Enemy.GetComponent<Administrator>() != null) {
                        EnemyKeys.Remove("Hedgehog Trap");
                        EnemyKeys.Remove("administrator_servant");
                    }
                    if (CurrentScene == "Forest Boss Room" && Enemy.GetComponent<BossAnnounceOnAggro>() != null) {
                        EnemyKeys.Remove("administrator_servant");
                        EnemyKeys.Remove("Hedgehog Trap");
                    }
                    if (CurrentScene == "Sewer" && Enemy.name.Contains("Spinnerbot") && !Enemy.name.Contains("Corrupted")) {
                        Enemy.name = Enemy.name.Replace("Spinnerbot", "Spinnerbot Corrupted");
                    }
                    if (TunicArchipelago.Settings.ExtraEnemiesEnabled) {
                        if (Enemy.transform.parent != null && Enemy.transform.parent.name.Contains("NG+")) {
                            Enemy.transform.parent.gameObject.SetActive(true);
                        }
                    }
                    string NewEnemyName = "";
                    if (TunicArchipelago.Settings.EnemyDifficulty == RandomizerSettings.EnemyRandomizationType.RANDOM) {
                        NewEnemy = GameObject.Instantiate(Enemies[EnemyKeys[Random.Next(EnemyKeys.Count)]]);
                    } else if (TunicArchipelago.Settings.EnemyDifficulty == RandomizerSettings.EnemyRandomizationType.BALANCED) {
                        List<string> EnemyTypes = null;
                        foreach (string Key in EnemyRankings.Keys.Reverse()) {
                            List<string> Rank = EnemyRankings[Key];
                            Rank.Sort();
                            Rank.Reverse();
                            foreach (string EnemyName in Rank) {
                                if (Enemy.name.Contains(EnemyName)) {
                                    if (Enemy.name.Contains("Voidtouched")) {
                                        if (Enemy.name.Contains("Crow") || Enemy.name.Contains("crocodoo")) {
                                            EnemyTypes = EnemyRankings["Strong"];
                                        } else {
                                            EnemyTypes = EnemyRankings["Intense"];
                                        }
                                    } else if (Enemy.name.Contains("administrator")) {
                                        EnemyTypes = Enemy.name.Contains("servant") ? EnemyRankings["Average"] : EnemyRankings["Intense"];
                                    } else {
                                        EnemyTypes = Rank;
                                    }
                                }
                            }
                            if (EnemyTypes != null) {
                                break;
                            }
                        }
                        if (EnemyTypes == null) {
                            NewEnemy = GameObject.Instantiate(Enemies[EnemyKeys[Random.Next(EnemyKeys.Count)]]);
                        } else {
                            if (CurrentScene == "Cathedral Arena") {
                                EnemyTypes.Remove("administrator_servant");
                                EnemyTypes.Remove("Hedgehog Trap");
                                if (Inventory.GetItemByName("Wand").Quantity == 0) {
                                    EnemyTypes.Remove("Crabbit with Shell");
                                }
                            }
                            NewEnemy = GameObject.Instantiate(Enemies[EnemyTypes[Random.Next(EnemyTypes.Count)]]);
                        }
                    } else {
                        NewEnemy = GameObject.Instantiate(Enemies[EnemyKeys[Random.Next(EnemyKeys.Count)]]);
                    }

                    NewEnemy.transform.position = Enemy.transform.position;
                    NewEnemy.transform.rotation = Enemy.transform.rotation;
                    NewEnemy.transform.parent = Enemy.transform.parent;
                    NewEnemy.SetActive(true);
                    if (NewEnemy.GetComponent<DefenseTurret>() != null) {
                        NewEnemy.GetComponent<Monster>().onlyAggroViaTrigger = false;
                    }
                    if (NewEnemy.GetComponent<TunicKnightVoid>() != null && NewEnemy.GetComponent<Creature>().defaultStartingMaxHP != null) {
                        NewEnemy.GetComponent<Creature>().defaultStartingMaxHP._value = 200;
                    }
                    if (NewEnemy.name.Contains("BlobBigger") && NewEnemy.GetComponent<Creature>().defaultStartingMaxHP != null) {
                        NewEnemy.GetComponent<Creature>().defaultStartingMaxHP._value = 25;
                    }
                    if (SceneLoaderPatches.SceneName == "ziggurat2020_1" && Enemy.GetComponent<Administrator>() != null) {
                        GameObject.FindObjectOfType<ZigguratAdminGate>().admin = NewEnemy.GetComponent<Monster>();
                    }
                    if (SceneLoaderPatches.SceneName == "Forest Boss Room" && Enemy.GetComponent<BossAnnounceOnAggro>() != null) {
                        NewEnemy.AddComponent<BossAnnounceOnAggro>();
                        LanguageLine TopLine = ScriptableObject.CreateInstance<LanguageLine>();
                        LanguageLine BottomLine = ScriptableObject.CreateInstance<LanguageLine>();
                        TopLine.text = $"\"Enemy\"";
                        foreach (string Key in ProperEnemyNames.Keys) {
                            if (NewEnemy.name.Replace(" Prefab", "").Replace("(Clone)", "") == Key) {
                                TopLine.text = ProperEnemyNames[Key];
                                if (NewEnemy.name.Contains("crocodoo")) {
                                    BottomLine.text = $"#uh wuhn ahnd OnlE";
                                } else if (EnemyRankings["Average"].Contains(Key)) {
                                    BottomLine.text = $"pEs uhv kAk";
                                } else if (EnemyRankings["Strong"].Contains(Key)) {
                                    BottomLine.text = $"A straw^ ehnuhmE";
                                } else if (EnemyRankings["Intense"].Contains(Key)) {
                                    BottomLine.text = $"A formiduhbl fO";
                                } else {
                                    BottomLine.text = $"A formiduhbl fO";
                                }
                            }
                        }
                        NewEnemy.GetComponent<BossAnnounceOnAggro>().bossTitleTopLine = TopLine;
                        NewEnemy.GetComponent<BossAnnounceOnAggro>().bossTitleBottomLine = BottomLine;
                    }

                    // Randomize support scavengers bomb type
                    if (NewEnemy.GetComponent<Scavenger_Support>() != null) {
                        int bombChance = Random.Next(100);
                        Rigidbody randomBomb;
                        if (bombChance < 4) {

                            randomBomb = bombFlasks.Where(bomb => bomb.name == "centipede_detritus_head").ToList()[0].gameObject.GetComponent<Rigidbody>();
                        } else {
                            List<BombFlask> possibleBombs = bombFlasks.Where(bomb => bomb.name != "Firecracker" && bomb.name != "centipede_detritus_head").ToList();
                            randomBomb = possibleBombs[Random.Next(possibleBombs.Count)].gameObject.GetComponent<Rigidbody>();
                        }
                        NewEnemy.GetComponent<Scavenger_Support>().bombPrefab = randomBomb;
                        if (!randomBomb.gameObject.name.Contains("Firecracker")) {
                            NewEnemy.GetComponent<Scavenger_Support>().tossAngle = 15f;
                        }
                        if (randomBomb.gameObject.name.Contains("Ice") || (randomBomb.gameObject.name.Contains("centipede") && bombChance < 2)) {
                            NewEnemy.transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials = Enemies["Scavenger_stunner"].transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials;
                        }
                    }

                    // For ice snipers
                    if (NewEnemy.GetComponent<Scavenger>() != null) {
                        NewEnemy.GetComponent<Scavenger>().useStunBulletPool = NewEnemy.name.Contains("stunner");
                        if (NewEnemy.name.Contains("stunner")) {
                            NewEnemy.GetComponent<Scavenger>().laserEndSphere = Enemies["Scavenger"].GetComponent<Scavenger>().laserEndSphere;
                        }
                    }

                    NewEnemy.name += $" {i}";
                    EnemiesInCurrentScene.Add(NewEnemy.name, NewEnemy.transform.position.ToString());
                    i++;
                    if (DefeatedEnemyTracker.ContainsKey(CurrentScene) && DefeatedEnemyTracker[CurrentScene].Contains(Enemy.transform.position.ToString())) {
                        GameObject.Destroy(NewEnemy);
                    }
                    GameObject.Destroy(Enemy.gameObject);
                } catch (Exception ex) {
                    Logger.LogInfo("An error occurred spawning a new randomized enemy");
                    if (NewEnemy != null) {
                        GameObject.Destroy(NewEnemy);
                        Logger.LogError("An error occurred spawning the following randomized enemy: " + NewEnemy.name);
                        Logger.LogError(ex.Message + " " + ex.StackTrace);
                    }
                }
            }
            try {
                foreach (string Key in Enemies.Keys) {
                    if (Enemies[Key] != null) {
                        Enemies[Key].SetActive(false);
                    }
                }
            } catch (Exception e) {

            }
        }

        public static bool Monster_Die_MoveNext_PrefixPatch(Monster._Die_d__77 __instance, ref bool __result) {
            if (__instance.__4__this.GetComponent<BossAnnounceOnAggro>() != null) {
                if (SceneManager.GetActiveScene().name == "Forest Boss Room") {
                    StateVariable.GetStateVariableByName("SV_Forest Boss Room_Skuladot redux Big").BoolValue = true;
                    Archipelago.instance.UpdateDataStorage("Defeated Guard Captain", true);
                }
                if (__instance.__4__this.GetComponent<Knightbot>() != null) {
                    Archipelago.instance.UpdateDataStorage("Defeated Garden Knight", true);
                }
                if (__instance.__4__this.GetComponent<Spidertank>() != null) {
                    Archipelago.instance.UpdateDataStorage("Defeated Siege Engine", true);
                }
                if (__instance.__4__this.GetComponent<Librarian>() != null) {
                    Archipelago.instance.UpdateDataStorage("Defeated Librarian", true);
                }
                if (__instance.__4__this.GetComponent<ScavengerBoss>() != null) {
                    Archipelago.instance.UpdateDataStorage("Defeated Boss Scavenger", true);
                }
            }

            if (__instance.__4__this.GetComponent<TunicKnightVoid>() != null) {
                CoinSpawner.SpawnCoins(50, __instance.__4__this.transform.position);
                MPPickup.Drop(100f, __instance.__4__this.transform.position);
                GameObject.Destroy(__instance.__4__this.gameObject);
                return false;
            }

            return true;
        }

        public static void Monster_Die_MoveNext_PostfixPatch(Monster._Die_d__77 __instance, ref bool __result) {
            if (!__result) {
                int EnemiesDefeated = SaveFile.GetInt(EnemiesDefeatedCount);
                SaveFile.SetInt(EnemiesDefeatedCount, EnemiesDefeated + 1);

                string SceneName = SceneLoaderPatches.SceneName;
                if (TunicArchipelago.Settings.EnemyRandomizerEnabled && SceneName != "Cathedral Arena") {
                    if (!DefeatedEnemyTracker.ContainsKey(SceneName)) {
                        DefeatedEnemyTracker.Add(SceneName, new List<string>());
                    }
                    if (EnemiesInCurrentScene.ContainsKey(__instance.__4__this.name)) {
                        DefeatedEnemyTracker[SceneName].Add(EnemiesInCurrentScene[__instance.__4__this.name]);
                    }

                }
            }
        }

        public static bool Campfire_RespawnAtLastCampfire_PrefixPatch(Campfire __instance) {
            DefeatedEnemyTracker.Clear();
            return true;
        }

        public static bool Campfire_Interact_PrefixPatch(Campfire __instance) {
            DefeatedEnemyTracker.Clear();
            return true;
        }

        public static bool TunicKnightVoid_onFlinch_PrefixPatch(TunicKnightVoid __instance) {
            return false;
        }

        public static void ToggleObjectAnimation_SetToggle_PostfixPatch(ToggleObjectAnimation __instance, ref bool state) {
            if (SceneManager.GetActiveScene().name == "Cathedral Arena" && __instance.name == "Chest Reveal" && state) {
                Archipelago.instance.UpdateDataStorage("Cleared Cathedral Gauntlet", true);
            }
        }

    }
}

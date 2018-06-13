using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Utility {

    public static class NameDatabase {

        public static string GetCompoundName(ReputationMoralityTypes morality) {
            string[] partOne;
            string[] partTwo;

            switch (morality) {
                default:
                case ReputationMoralityTypes.NEUTRAL:
                    partOne = new string[] {
                        "Stone",
                        "Grass",
                        "Frost",
                        "Onyx",
                        "Ender",
                        "Cunning",
                        "Earth",
                        "Bronze",
                        "Wind",
                        "Steel",
                        "Wood",

                    };
                    partTwo = new string[] {
                        "blade",
                        "drifter",
                        "weaver",
                        "spell",
                        "seeker",
                        "fall",
                        "throne",
                        "hide",
                        "tusk",
                        "scale",
                        "face",
                        "hill"
                    };
                    break;
                case ReputationMoralityTypes.GOOD:
                    partOne = new string[] {
                        "Light",
                        "Pearl",
                        "Ivory",
                        "Spring",
                        "Gold",
                        "Silver",
                        "Solar",
                        "Sun",
                        "Moon",
                        "Bright"
                    };
                    partTwo = new string[] {
                        "blossom",
                        "talon",
                        "wolf",
                        "stream",
                        "bringer",
                        "stride",
                        "dreamer",
                        "shield",
                        "mace",
                        "caller",
                        "reach",
                        "ford",
                        "hand",
                        "peak"
                    };
                    break;
                case ReputationMoralityTypes.EVIL:
                    partOne = new string[] {
                        "Curse",
                        "Fallen",
                        "Dark",
                        "Red",
                        "Char",
                        "Night",
                        "Chaos",
                        "Hollow",
                        "Shatter",
                        "Doom"
                        };
                    partTwo = new string[] {
                        "hunter",
                        "splitter",
                        "seeker",
                        "slayer",
                        "shade",
                        "stalker",
                        "caller",
                        "blood",
                        "dagger",
                        "mark"
                    };
                    break;
            }


            return partOne[Random.Range(0, partOne.Length)] + partTwo[Random.Range(0, partTwo.Length)];
        }

        public static string GetTerritoryName() {
            int type = Random.Range(0, 1);
            if(type == 0) {
                string[] partOne = new string[] {
                    "Swan",
                    "Rock",
                    "Bear",
                    "Frey",
                    "Honey",
                    "Last",
                    "Wolf",
                    "Silk",
                    "Wind",
                    "Sly",
                    "Bone",
                    "East",
                    "North",
                    "West",
                    "South",
                    "Snow",
                    "Rose"
                };

                string[] partTwo = new string[] {
                    "burgh",
                    "rock",
                    "town",
                    "brook",
                    "well",
                    "bury",
                    "mond",
                    "gulch",
                    "fall",
                    "point",
                    "hall",
                    "hollow",
                    "wood",
                    "bay",
                    "more",
                    "valley"
                };

                return partOne[Random.Range(0, partOne.Length)] + partTwo[Random.Range(0, partTwo.Length)];
            }else if(type == 1) {

            }

            return "";
        }

        public static string GetPointOfInterestName() {
            string[] partOne = new string[] {
                "Stone",
                "Grass",
                "Frost",
                "Onyx",
                "Ender",
                "Cunning",
                "Earth",
                "Bronze",
                "Wind",
                "Steel",
                "Wood",

             };

            string[] partTwo = new string[] {
                "Estate",
                "Mansion",
                "Residence",
                "Chateau",
                "Manor",
                "Plains",
                "Lands",
                "Wastes",
                "Fields",
                "Range",
                "Wilds",
                "Vault",
                "Haven"
            };
            return "The " + partOne[Random.Range(0, partOne.Length)] + " " + partTwo[Random.Range(0, partTwo.Length)];
        }

        public static string GetGroupName() {
            int type = Random.Range(0, 2);
            if (type == 0) {                        //The Something Somethings
                string[] partOne = new string[] {
                    "Serpent",
                    "Forsaken",
                    "Violet",
                    "Cobalt",
                    "Wild",
                    "Last",
                    "Spider",
                    "Demon",
                    "Emerald",
                    "Flame",
                    "Royal",
                    "Faceless",
                    "United",
                    "Voiceless",
                    "Shadow",
                    "Raven",
                    "White"
                };

                string[] partTwo = new string[] {
                    "Crows",
                    "Whisperers",
                    "Tribe",
                    "Clan",
                    "Brotherhood",
                    "Crew",
                    "Hands",
                    "Dreamers",
                    "Company",
                    "Syndicate",
                    "Lions",
                    "Ones",
                    "Angels",
                    "Stalkers",
                    "Swords",
                    "Hawks"
                };

                return "The " + partOne[Random.Range(0, partOne.Length)] + " " + partTwo[Random.Range(0, partTwo.Length)];
            } else if (type == 1) {                     //The Somethings of Something
                string[] partOne = new string[] {
                    "Serpents",
                    "Seekers",
                    "Voices",
                    "Rangers",
                    "Outsiders",
                    "Shades",
                    "Shields",
                    "Spears",
                    "Swords",
                    "Hands",
                    "Eyes",
                    "Visions",
                    "Hammers",
                    "Hunters",
                    "Sentinels",
                    "Ravens",
                    "Guardians",
                    "Striders",
                    "Vanguard",
                    "Phantoms"
                };

                string[] partTwo = new string[] {
                    "of Virtue",
                    "of Fate",
                    "of the Light",
                    "of the Weak",
                    "of Strength",
                    "of Promise",
                    "of the Lost Age",
                    "of the Void",
                    "of the Hawk",
                    "of the Boar",
                    "of the Stag",
                    "of the Lost",
                    "of Dawn",
                    "of Fury",
                    "of the Serene",
                    "of the World"
                };

                return partOne[Random.Range(0, partOne.Length)] + " " + partTwo[Random.Range(0, partTwo.Length)];
            }

            return "";
        }

    }

}
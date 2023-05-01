using COTL_API.Helpers;
using HarmonyLib;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace COTL_API.CustomSkins;

[HarmonyPatch]
public static partial class CustomSkinManager
{
    internal static readonly Dictionary<string, SpineAtlasAsset> CustomAtlases = new();
    internal static readonly Dictionary<string, Skin?> CustomFollowerSkins = new();
    internal static readonly Dictionary<string, bool> AlwaysUnlockedSkins = new();
    internal static readonly Dictionary<string, Texture2D> SkinTextures = new();
    internal static readonly Dictionary<string, Material> SkinMaterials = new();

    internal static readonly Dictionary<string, CustomPlayerSkin> CustomPlayerSkins = new();

    internal static readonly List<Tuple<int, string>> SkinSlots = new()
    {
        Tuple.Create(0, "Crown_Particle1"),
        Tuple.Create(1, "Crown_Particle2"),
        Tuple.Create(2, "Crown_Particle6"),
        Tuple.Create(3, "effects/Crown_Particle3"),
        Tuple.Create(4, "effects/Crown_Particle4"),
        Tuple.Create(5, "effects/Crown_Particle5"),
        Tuple.Create(9, "sunburst"),
        Tuple.Create(9, "sunburst2"),
        Tuple.Create(11, "Corpse"),
        Tuple.Create(12, "Corpse"),
        Tuple.Create(13, "Halo"),
        Tuple.Create(14, "ARM_LEFT"),
        Tuple.Create(15, "PonchoShoulder"),
        Tuple.Create(16, "Tools/PITCHFORK"),
        Tuple.Create(16, "Tools/SEED_BAG"),
        Tuple.Create(16, "Tools/SPADE"),
        Tuple.Create(16, "Tools/WATERING_CAN"),
        Tuple.Create(16, "Tools/FishingRod"),
        Tuple.Create(16, "Tools/FishingRod2"),
        Tuple.Create(16, "Tools/Mop"),
        Tuple.Create(16, "FishingRod_Front"),
        Tuple.Create(16, "GauntletHeavy"),
        Tuple.Create(16, "GauntletHeavy2"),
        Tuple.Create(16, "images/AttackHand1"),
        Tuple.Create(16, "images/AttackHand2"),
        Tuple.Create(17, "LEG_LEFT"),
        Tuple.Create(18, "LEG_RIGHT"),
        Tuple.Create(19, "Body"),
        Tuple.Create(20, "PonchoLeft"),
        Tuple.Create(20, "PonchoLeft2"),
        Tuple.Create(21, "Weapons/Axe"),
        Tuple.Create(21, "Weapons/Blunderbuss"),
        Tuple.Create(21, "Weapons/Dagger"),
        Tuple.Create(21, "Weapons/Hammer"),
        Tuple.Create(21, "Weapons/Sword"),
        Tuple.Create(21, "DaggerFlipped"),
        Tuple.Create(22, "ARM_RIGHT"),
        Tuple.Create(23, "ArmSpikes"),
        Tuple.Create(24, "PonchoRight"),
        Tuple.Create(24, "PonchoRight2"),
        Tuple.Create(25, "PonchoExtra"),
        Tuple.Create(26, "images/Rope"),
        Tuple.Create(27, "images/RopeTopRight"),
        Tuple.Create(28, "images/RopeTopLeft"),
        Tuple.Create(29, "Bell"),
        Tuple.Create(30, "Antler"),
        Tuple.Create(30, "Antler_RITUAL"),
        Tuple.Create(30, "Antler_SERMON"),
        Tuple.Create(30, "Antler_Horn"),
        Tuple.Create(31, "Antler"),
        Tuple.Create(31, "Antler_SERMON"),
        Tuple.Create(31, "Antler_RITUAL"),
        Tuple.Create(31, "Antler_Horn"),
        Tuple.Create(32, "EAR_LEFT"),
        Tuple.Create(32, "EAR_RITUAL"),
        Tuple.Create(32, "EAR_SERMON"),
        Tuple.Create(33, "CrownGlow"),
        Tuple.Create(34, "images/CrownSpikes"),
        Tuple.Create(35, "CROWN"),
        Tuple.Create(35, "CROWN_RITUAL"),
        Tuple.Create(35, "CROWN_SERMON"),
        Tuple.Create(35, "BigCrown"),
        Tuple.Create(35, "CROWN_WHITE"),
        Tuple.Create(36, "images/CrownEyeShut3"),
        Tuple.Create(36, "images/CrownEyeShut2"),
        Tuple.Create(36, "images/CrownEyeShut"),
        Tuple.Create(36, "CROWN_EYE"),
        Tuple.Create(36, "images/CrownEye_RITUAL"),
        Tuple.Create(36, "images/CrownEye_SERMON"),
        Tuple.Create(36, "images/CrownEyeBig"),
        Tuple.Create(39, "HeadBack"),
        Tuple.Create(39, "HeadBackDown"),
        Tuple.Create(39, "HeadBackDown_RITUAL"),
        Tuple.Create(39, "HeadBackDown_SERMON"),
        Tuple.Create(40, "HeadFront"),
        Tuple.Create(40, "HeadFrontDown"),
        Tuple.Create(42, "EAR_RIGHT"),
        Tuple.Create(42, "EAR_RIGHT_RITUAL"),
        Tuple.Create(42, "EAR_RIGHT_SERMON"),
        Tuple.Create(43, "effects/eye_blood"),
        Tuple.Create(43, "effects/eye_tears"),
        Tuple.Create(44, "effects/eye_blood"),
        Tuple.Create(44, "effects/eye_tears"),
        Tuple.Create(45, "MOUTH_NORMAL"),
        Tuple.Create(45, "Face/MOUTH_CHEEKY"),
        Tuple.Create(45, "Face/MOUTH_CHUBBY"),
        Tuple.Create(45, "Face/MOUTH_DEAD"),
        Tuple.Create(45, "Face/MOUTH_GRUMPY"),
        Tuple.Create(45, "Face/MOUTH_HAPPY"),
        Tuple.Create(45, "Face/MOUTH_INDIFFERENT"),
        Tuple.Create(45, "Face/MOUTH_KAWAII"),
        Tuple.Create(45, "Face/MOUTH_OO"),
        Tuple.Create(45, "Face/MOUTH_OPEN"),
        Tuple.Create(45, "Face/MOUTH_SAD"),
        Tuple.Create(45, "Face/MOUTH_SCARED"),
        Tuple.Create(45, "Face/MOUTH_SLEEP_0"),
        Tuple.Create(45, "Face/MOUTH_SLEEP_1"),
        Tuple.Create(45, "Face/MOUTH_TONGUE"),
        Tuple.Create(45, "Face/MOUTH_UNCONVERTED"),
        Tuple.Create(45, "MOUTH_TALK"),
        Tuple.Create(45, "MOUTH_TALK_HAPPY"),
        Tuple.Create(45, "MOUTH_UNCONVERTED_SPEAK"),
        Tuple.Create(45, "MOUTH_GRIMACE"),
        Tuple.Create(45, "MOUTH_SNARL"),
        Tuple.Create(45, "MOUTH_TALK1"),
        Tuple.Create(45, "MOUTH_TALK2"),
        Tuple.Create(45, "MOUTH_TALK3"),
        Tuple.Create(45, "MOUTH_TALK4"),
        Tuple.Create(45, "MOUTH_TALK5"),
        Tuple.Create(46, "EYE"),
        Tuple.Create(46, "EYE_ANGRY_LEFT"),
        Tuple.Create(46, "EYE_BACK"),
        Tuple.Create(46, "EYE_DETERMINED_DOWN_LEFT"),
        Tuple.Create(46, "EYE_DETERMINED_LEFT"),
        Tuple.Create(46, "EYE_DOWN"),
        Tuple.Create(46, "EYE_HALF_CLOSED"),
        Tuple.Create(46, "EYE_HAPPY"),
        Tuple.Create(46, "EYE_UP"),
        Tuple.Create(46, "EYE_WORRIED_LEFT"),
        Tuple.Create(46, "Face/EYE_CLOSED"),
        Tuple.Create(46, "Face/EYE_DEAD"),
        Tuple.Create(46, "Face/EYE_RED"),
        Tuple.Create(46, "Face/EYE_SHOCKED"),
        Tuple.Create(46, "Face/EYE_SLEEPING"),
        Tuple.Create(46, "Face/EYE_SQUINT"),
        Tuple.Create(46, "Face/EYE_UNCONVERTED"),
        Tuple.Create(46, "Face/EYE_UNCONVERTED_WORRIED"),
        Tuple.Create(46, "EYE_ANGRY_LEFT_UP"),
        Tuple.Create(46, "EYE_WHITE"),
        Tuple.Create(46, "EYE_WEARY_LEFT"),
        Tuple.Create(46, "EYE_GRIMACE"),
        Tuple.Create(46, "EYE_WEARY_LEFT_DOWN"),
        Tuple.Create(46, "EYE_HAPPY2"),
        Tuple.Create(46, "Face/EYE_RED_ANGRY"),
        Tuple.Create(46, "EYE_WHITE_ANGRY"),
        Tuple.Create(47, "EYE"),
        Tuple.Create(47, "EYE_ANGRY_RIGHT"),
        Tuple.Create(47, "EYE_BACK"),
        Tuple.Create(47, "EYE_DETERMINED_DOWN_RIGHT"),
        Tuple.Create(47, "EYE_DETERMINED_RIGHT"),
        Tuple.Create(47, "EYE_DOWN"),
        Tuple.Create(47, "EYE_HALF_CLOSED"),
        Tuple.Create(47, "EYE_HAPPY"),
        Tuple.Create(47, "EYE_UP"),
        Tuple.Create(47, "EYE_WORRIED_RIGHT"),
        Tuple.Create(47, "Face/EYE_CLOSED"),
        Tuple.Create(47, "Face/EYE_DEAD"),
        Tuple.Create(47, "Face/EYE_RED"),
        Tuple.Create(47, "Face/EYE_SHOCKED"),
        Tuple.Create(47, "Face/EYE_SLEEPING"),
        Tuple.Create(47, "Face/EYE_SQUINT"),
        Tuple.Create(47, "Face/EYE_UNCONVERTED"),
        Tuple.Create(47, "Face/EYE_UNCONVERTED_WORRIED"),
        Tuple.Create(47, "EYE_ANGRY_RIGHT_UP"),
        Tuple.Create(47, "EYE_WHITE"),
        Tuple.Create(47, "EYE_WEARY_RIGHT"),
        Tuple.Create(47, "EYE_GRIMACE"),
        Tuple.Create(47, "EYE_WEARY_RIGHT_DOWN"),
        Tuple.Create(47, "EYE_HAPPY2"),
        Tuple.Create(47, "Face/EYE_RED_ANGRY"),
        Tuple.Create(47, "EYE_WHITE_ANGRY"),
        Tuple.Create(48, "HairTuft"),
        Tuple.Create(49, "Tools/Book_open"),
        Tuple.Create(49, "Tools/Book_closed"),
        Tuple.Create(49, "Tools/BookFlipping_3"),
        Tuple.Create(49, "Tools/BookFlipping_2"),
        Tuple.Create(49, "Tools/BookFlipping_1"),
        Tuple.Create(49, "Tools/BookFlipping_4"),
        Tuple.Create(51, "PonchoRightCorner"),
        Tuple.Create(52, "PonchoRightCorner"),
        Tuple.Create(53, "images/CrownMouth"),
        Tuple.Create(53, "images/CrownMouthOpen"),
        Tuple.Create(54, "Tools/Chalice"),
        Tuple.Create(54, "Tools/Chalice_Skull"),
        Tuple.Create(54, "Tools/Chalice_Skull_Drink"),
        Tuple.Create(55, "effects/slam_effect0006"),
        Tuple.Create(55, "effects/slam_effect0005"),
        Tuple.Create(55, "effects/slam_effect0004"),
        Tuple.Create(55, "effects/slam_effect0003"),
        Tuple.Create(55, "effects/slam_effect0002"),
        Tuple.Create(55, "effects/slam_effect0001"),
        Tuple.Create(56, "images/CrownSpikes"),
        Tuple.Create(57, "images/CrownSpikes2"),
        Tuple.Create(58, "images/CrownSpikes"),
        Tuple.Create(59, "images/CrownSpikes2"),
        Tuple.Create(60, "AttackHand1"),
        Tuple.Create(60, "AttackHand2"),
        Tuple.Create(61, "GauntletHeavy"),
        Tuple.Create(61, "GauntletHeavy2"),
        Tuple.Create(62, "Weapons/Sling"),
        Tuple.Create(63, "Weapons/SlingRope"),
        Tuple.Create(64, "SlingHand"),
        Tuple.Create(65, "Arm_frontbit"),
        Tuple.Create(66, "whiteball"),
        Tuple.Create(67, "effects/whiteball"),
        Tuple.Create(68, "Weapons/SlingHand"),
        Tuple.Create(69, "effects/portal_btm"),
        Tuple.Create(70, "effects/portal_top"),
        Tuple.Create(71, "portal_splash"),
        Tuple.Create(72, "GrappleHook"),
        Tuple.Create(73, "Weapons/Lute"),
        Tuple.Create(74, "Weapons/SlingHand"),
        Tuple.Create(75, "images/Crown_half_left"),
        Tuple.Create(76, "images/Crown_half_right"),
        Tuple.Create(80, "Sparks1"),
        Tuple.Create(81, "Sparks1"),
        Tuple.Create(82, "Sparks2"),
        Tuple.Create(83, "Sparks2"),
        Tuple.Create(84, "Weapons/SpecialSword_1"),
        Tuple.Create(84, "Weapons/SpecialSword_2"),
        Tuple.Create(84, "Weapons/SpecialSword_3"),
        Tuple.Create(84, "Weapons/SpecialSword_4"),
        Tuple.Create(85, "MonsterHeart_glow"),
        Tuple.Create(85, "KnowledgeParchment"),
        Tuple.Create(85, "Knowledge_Trinket"),
        Tuple.Create(85, "Knowledge_Curse"),
        Tuple.Create(85, "Knowledge_Decoration"),
        Tuple.Create(85, "Knowledge_Weapon"),
        Tuple.Create(85, "Tools/Woodaxe"),
        Tuple.Create(85, "Tools/Woodaxe2"),
        Tuple.Create(85, "Tools/Pickaxe"),
        Tuple.Create(85, "Tools/Pickaxe2"),
        Tuple.Create(85, "Tools/Hammer"),
        Tuple.Create(85, "Net"),
        Tuple.Create(85, "Items/WebberSkull"),
        Tuple.Create(85, "Tools/Book_open"),
        Tuple.Create(85, "Tools/Book_closed"),
        Tuple.Create(86, "MonsterHeart_glow"),
        Tuple.Create(86, "GiftSmall"),
        Tuple.Create(86, "GiftMedium"),
        Tuple.Create(87, "effects/MonsterBlood1"),
        Tuple.Create(88, "effects/MonsterBlood1"),
        Tuple.Create(89, "MonsterBlood2"),
        Tuple.Create(90, "Tools/CardBack"),
        Tuple.Create(90, "Tools/CardFront"),
        Tuple.Create(91, "Tools/CardBack"),
        Tuple.Create(91, "Tools/CardFront"),
        Tuple.Create(92, "Tools/CardBack"),
        Tuple.Create(92, "Tools/CardFront"),
        Tuple.Create(93, "Tools/CardBack"),
        Tuple.Create(93, "Tools/CardFront"),
        Tuple.Create(94, "Tools/CardBack"),
        Tuple.Create(94, "Tools/CardFront"),
        Tuple.Create(95, "Tools/CardBack"),
        Tuple.Create(95, "Tools/CardFront"),
        Tuple.Create(96, "RitualSymbolHalo"),
        Tuple.Create(97, "RitualSymbol_1"),
        Tuple.Create(97, "RitualSymbol_2"),
        Tuple.Create(98, "effects/RitualRing2"),
        Tuple.Create(98, "effects/SermonRing2"),
        Tuple.Create(99, "AttackSlash1"),
        Tuple.Create(99, "AttackSlash2"),
        Tuple.Create(100, "effects/RitualRing"),
        Tuple.Create(100, "effects/SermonRing"),
        Tuple.Create(101, "CollarPiece1"),
        Tuple.Create(102, "CollarPiece2"),
        Tuple.Create(103, "ChainBit1"),
        Tuple.Create(104, "ChainBit2"),
        Tuple.Create(105, "ChainBit1"),
        Tuple.Create(106, "ChainBit3"),
        Tuple.Create(108, "SwordHeavy"),
        Tuple.Create(108, "Weapons/SwordHeavy_Necromancy"),
        Tuple.Create(108, "Weapons/SwordHeavy_Ice"),
        Tuple.Create(108, "Weapons/SwordHeavy_Charm"),
        Tuple.Create(108, "AxeHeavy"),
        Tuple.Create(108, "HammerHeavy"),
        Tuple.Create(109, "effects/SpawnHeavy_1"),
        Tuple.Create(109, "effects/SpawnHeavy_2"),
        Tuple.Create(109, "effects/SpawnHeavy_3"),
        Tuple.Create(109, "effects/SpawnHeavy_4"),
        Tuple.Create(110, "SpawnHeavy_glow"),
        Tuple.Create(111, "FireSmall_0001"),
        Tuple.Create(111, "FireSmall_0002"),
        Tuple.Create(111, "FireSmall_0003"),
        Tuple.Create(111, "FireSmall_0004"),
        Tuple.Create(111, "FireSmall_0005"),
        Tuple.Create(111, "FireSmall_0006"),
        Tuple.Create(111, "FireSmall_0007"),
        Tuple.Create(112, "FireWild_0001"),
        Tuple.Create(112, "FireWild_0002"),
        Tuple.Create(112, "FireWild_0003"),
        Tuple.Create(112, "FireWild_0004"),
        Tuple.Create(112, "FireWild_0005"),
        Tuple.Create(112, "FireWild_0006"),
        Tuple.Create(112, "FireWild_0007"),
        Tuple.Create(112, "FireWild_0008"),
        Tuple.Create(112, "FireWild_0009"),
        Tuple.Create(113, "effects/chunder_1"),
        Tuple.Create(113, "effects/chunder_2"),
        Tuple.Create(113, "effects/chunder_3"),
        Tuple.Create(114, "Curses/Icon_Curse_Blast"),
        Tuple.Create(114, "Curses/Icon_Curse_Fireball"),
        Tuple.Create(114, "Curses/Icon_Curse_Slash"),
        Tuple.Create(114, "Curses/Icon_Curse_Splatter"),
        Tuple.Create(114, "Curses/Icon_Curse_Tentacle")
    };

    internal static readonly Dictionary<string, Tuple<int, string>> SimplifiedSkinNames = new()
    {
        { "Crown_Particle1", Tuple.Create(0, "Crown_Particle1") },
        { "Crown_Particle2", Tuple.Create(1, "Crown_Particle2") },
        { "Crown_Particle6", Tuple.Create(2, "Crown_Particle6") },
        { "effects/Crown_Particle3", Tuple.Create(3, "effects/Crown_Particle3") },
        { "effects/Crown_Particle4", Tuple.Create(4, "effects/Crown_Particle4") },
        { "effects/Crown_Particle5", Tuple.Create(5, "effects/Crown_Particle5") },
        { "sunburst", Tuple.Create(9, "sunburst") },
        { "sunburst2", Tuple.Create(9, "sunburst2") },
        { "Corpse", Tuple.Create(11, "Corpse") },
        { "Corpse", Tuple.Create(12, "Corpse") },
        { "Halo", Tuple.Create(13, "Halo") },
        { "ARM_LEFT", Tuple.Create(14, "ARM_LEFT") },
        { "PonchoShoulder", Tuple.Create(15, "PonchoShoulder") },
        { "Tools/PITCHFORK", Tuple.Create(16, "Tools/PITCHFORK") },
        { "Tools/SEED_BAG", Tuple.Create(16, "Tools/SEED_BAG") },
        { "Tools/SPADE", Tuple.Create(16, "Tools/SPADE") },
        { "Tools/WATERING_CAN", Tuple.Create(16, "Tools/WATERING_CAN") },
        { "Tools/FishingRod", Tuple.Create(16, "Tools/FishingRod") },
        { "Tools/FishingRod2", Tuple.Create(16, "Tools/FishingRod2") },
        { "Tools/Mop", Tuple.Create(16, "Tools/Mop") },
        { "FishingRod_Front", Tuple.Create(16, "FishingRod_Front") },
        { "GauntletHeavy", Tuple.Create(16, "GauntletHeavy") },
        { "GauntletHeavy2", Tuple.Create(16, "GauntletHeavy2") },
        { "images/AttackHand1", Tuple.Create(16, "images/AttackHand1") },
        { "images/AttackHand2", Tuple.Create(16, "images/AttackHand2") },
        { "LEG_LEFT", Tuple.Create(17, "LEG_LEFT") },
        { "LEG_RIGHT", Tuple.Create(18, "LEG_RIGHT") },
        { "Body", Tuple.Create(19, "Body") },
        { "PonchoLeft", Tuple.Create(20, "PonchoLeft") },
        { "PonchoLeft2", Tuple.Create(20, "PonchoLeft2") },
        { "Weapons/Axe", Tuple.Create(21, "Weapons/Axe") },
        { "Weapons/Blunderbuss", Tuple.Create(21, "Weapons/Blunderbuss") },
        { "Weapons/Dagger", Tuple.Create(21, "Weapons/Dagger") },
        { "Weapons/Hammer", Tuple.Create(21, "Weapons/Hammer") },
        { "Weapons/Sword", Tuple.Create(21, "Weapons/Sword") },
        { "DaggerFlipped", Tuple.Create(21, "DaggerFlipped") },
        { "ARM_RIGHT", Tuple.Create(22, "ARM_RIGHT") },
        { "ArmSpikes", Tuple.Create(23, "ArmSpikes") },
        { "PonchoRight", Tuple.Create(24, "PonchoRight") },
        { "PonchoRight2", Tuple.Create(24, "PonchoRight2") },
        { "PonchoExtra", Tuple.Create(25, "PonchoExtra") },
        { "images/Rope", Tuple.Create(26, "images/Rope") },
        { "images/RopeTopRight", Tuple.Create(27, "images/RopeTopRight") },
        { "images/RopeTopLeft", Tuple.Create(28, "images/RopeTopLeft") },
        { "Bell", Tuple.Create(29, "Bell") },
        { "Antler", Tuple.Create(30, "Antler") },
        { "Antler_RITUAL", Tuple.Create(30, "Antler_RITUAL") },
        { "Antler_SERMON", Tuple.Create(30, "Antler_SERMON") },
        { "Antler_Horn", Tuple.Create(30, "Antler_Horn") },
        { "Antler", Tuple.Create(31, "Antler") },
        { "Antler_SERMON", Tuple.Create(31, "Antler_SERMON") },
        { "Antler_RITUAL", Tuple.Create(31, "Antler_RITUAL") },
        { "Antler_Horn", Tuple.Create(31, "Antler_Horn") },
        { "EAR_LEFT", Tuple.Create(32, "EAR_LEFT") },
        { "EAR_RITUAL", Tuple.Create(32, "EAR_RITUAL") },
        { "EAR_SERMON", Tuple.Create(32, "EAR_SERMON") },
        { "CrownGlow", Tuple.Create(33, "CrownGlow") },
        { "images/CrownSpikes", Tuple.Create(34, "images/CrownSpikes") },
        { "CROWN", Tuple.Create(35, "CROWN") },
        { "CROWN_RITUAL", Tuple.Create(35, "CROWN_RITUAL") },
        { "CROWN_SERMON", Tuple.Create(35, "CROWN_SERMON") },
        { "BigCrown", Tuple.Create(35, "BigCrown") },
        { "CROWN_WHITE", Tuple.Create(35, "CROWN_WHITE") },
        { "images/CrownEyeShut3", Tuple.Create(36, "images/CrownEyeShut3") },
        { "images/CrownEyeShut2", Tuple.Create(36, "images/CrownEyeShut2") },
        { "images/CrownEyeShut", Tuple.Create(36, "images/CrownEyeShut") },
        { "CROWN_EYE", Tuple.Create(36, "CROWN_EYE") },
        { "images/CrownEye_RITUAL", Tuple.Create(36, "images/CrownEye_RITUAL") },
        { "images/CrownEye_SERMON", Tuple.Create(36, "images/CrownEye_SERMON") },
        { "images/CrownEyeBig", Tuple.Create(36, "images/CrownEyeBig") },
        { "HeadBack", Tuple.Create(39, "HeadBack") },
        { "HeadBackDown", Tuple.Create(39, "HeadBackDown") },
        { "HeadBackDown_RITUAL", Tuple.Create(39, "HeadBackDown_RITUAL") },
        { "HeadBackDown_SERMON", Tuple.Create(39, "HeadBackDown_SERMON") },
        { "HeadFront", Tuple.Create(40, "HeadFront") },
        { "HeadFrontDown", Tuple.Create(40, "HeadFrontDown") },
        { "EAR_RIGHT", Tuple.Create(42, "EAR_RIGHT") },
        { "EAR_RIGHT_RITUAL", Tuple.Create(42, "EAR_RIGHT_RITUAL") },
        { "EAR_RIGHT_SERMON", Tuple.Create(42, "EAR_RIGHT_SERMON") },
        { "effects/eye_blood", Tuple.Create(43, "effects/eye_blood") },
        { "effects/eye_tears", Tuple.Create(43, "effects/eye_tears") },
        { "effects/eye_blood", Tuple.Create(44, "effects/eye_blood") },
        { "effects/eye_tears", Tuple.Create(44, "effects/eye_tears") },
        { "MOUTH_NORMAL", Tuple.Create(45, "MOUTH_NORMAL") },
        { "Face/MOUTH_CHEEKY", Tuple.Create(45, "Face/MOUTH_CHEEKY") },
        { "Face/MOUTH_CHUBBY", Tuple.Create(45, "Face/MOUTH_CHUBBY") },
        { "Face/MOUTH_DEAD", Tuple.Create(45, "Face/MOUTH_DEAD") },
        { "Face/MOUTH_GRUMPY", Tuple.Create(45, "Face/MOUTH_GRUMPY") },
        { "Face/MOUTH_HAPPY", Tuple.Create(45, "Face/MOUTH_HAPPY") },
        { "Face/MOUTH_INDIFFERENT", Tuple.Create(45, "Face/MOUTH_INDIFFERENT") },
        { "Face/MOUTH_KAWAII", Tuple.Create(45, "Face/MOUTH_KAWAII") },
        { "Face/MOUTH_OO", Tuple.Create(45, "Face/MOUTH_OO") },
        { "Face/MOUTH_OPEN", Tuple.Create(45, "Face/MOUTH_OPEN") },
        { "Face/MOUTH_SAD", Tuple.Create(45, "Face/MOUTH_SAD") },
        { "Face/MOUTH_SCARED", Tuple.Create(45, "Face/MOUTH_SCARED") },
        { "Face/MOUTH_SLEEP_0", Tuple.Create(45, "Face/MOUTH_SLEEP_0") },
        { "Face/MOUTH_SLEEP_1", Tuple.Create(45, "Face/MOUTH_SLEEP_1") },
        { "Face/MOUTH_TONGUE", Tuple.Create(45, "Face/MOUTH_TONGUE") },
        { "Face/MOUTH_UNCONVERTED", Tuple.Create(45, "Face/MOUTH_UNCONVERTED") },
        { "MOUTH_TALK", Tuple.Create(45, "MOUTH_TALK") },
        { "MOUTH_TALK_HAPPY", Tuple.Create(45, "MOUTH_TALK_HAPPY") },
        { "MOUTH_UNCONVERTED_SPEAK", Tuple.Create(45, "MOUTH_UNCONVERTED_SPEAK") },
        { "MOUTH_GRIMACE", Tuple.Create(45, "MOUTH_GRIMACE") },
        { "MOUTH_SNARL", Tuple.Create(45, "MOUTH_SNARL") },
        { "MOUTH_TALK1", Tuple.Create(45, "MOUTH_TALK1") },
        { "MOUTH_TALK2", Tuple.Create(45, "MOUTH_TALK2") },
        { "MOUTH_TALK3", Tuple.Create(45, "MOUTH_TALK3") },
        { "MOUTH_TALK4", Tuple.Create(45, "MOUTH_TALK4") },
        { "MOUTH_TALK5", Tuple.Create(45, "MOUTH_TALK5") },
        { "EYE", Tuple.Create(46, "EYE") },
        { "EYE_ANGRY_LEFT", Tuple.Create(46, "EYE_ANGRY_LEFT") },
        { "EYE_BACK", Tuple.Create(46, "EYE_BACK") },
        { "EYE_DETERMINED_DOWN_LEFT", Tuple.Create(46, "EYE_DETERMINED_DOWN_LEFT") },
        { "EYE_DETERMINED_LEFT", Tuple.Create(46, "EYE_DETERMINED_LEFT") },
        { "EYE_DOWN", Tuple.Create(46, "EYE_DOWN") },
        { "EYE_HALF_CLOSED", Tuple.Create(46, "EYE_HALF_CLOSED") },
        { "EYE_HAPPY", Tuple.Create(46, "EYE_HAPPY") },
        { "EYE_UP", Tuple.Create(46, "EYE_UP") },
        { "EYE_WORRIED_LEFT", Tuple.Create(46, "EYE_WORRIED_LEFT") },
        { "Face/EYE_CLOSED", Tuple.Create(46, "Face/EYE_CLOSED") },
        { "Face/EYE_DEAD", Tuple.Create(46, "Face/EYE_DEAD") },
        { "Face/EYE_RED", Tuple.Create(46, "Face/EYE_RED") },
        { "Face/EYE_SHOCKED", Tuple.Create(46, "Face/EYE_SHOCKED") },
        { "Face/EYE_SLEEPING", Tuple.Create(46, "Face/EYE_SLEEPING") },
        { "Face/EYE_SQUINT", Tuple.Create(46, "Face/EYE_SQUINT") },
        { "Face/EYE_UNCONVERTED", Tuple.Create(46, "Face/EYE_UNCONVERTED") },
        { "Face/EYE_UNCONVERTED_WORRIED", Tuple.Create(46, "Face/EYE_UNCONVERTED_WORRIED") },
        { "EYE_ANGRY_LEFT_UP", Tuple.Create(46, "EYE_ANGRY_LEFT_UP") },
        { "EYE_WHITE", Tuple.Create(46, "EYE_WHITE") },
        { "EYE_WEARY_LEFT", Tuple.Create(46, "EYE_WEARY_LEFT") },
        { "EYE_GRIMACE", Tuple.Create(46, "EYE_GRIMACE") },
        { "EYE_WEARY_LEFT_DOWN", Tuple.Create(46, "EYE_WEARY_LEFT_DOWN") },
        { "EYE_HAPPY2", Tuple.Create(46, "EYE_HAPPY2") },
        { "Face/EYE_RED_ANGRY", Tuple.Create(46, "Face/EYE_RED_ANGRY") },
        { "EYE_WHITE_ANGRY", Tuple.Create(46, "EYE_WHITE_ANGRY") },
        { "EYE", Tuple.Create(47, "EYE") },
        { "EYE_ANGRY_RIGHT", Tuple.Create(47, "EYE_ANGRY_RIGHT") },
        { "EYE_BACK", Tuple.Create(47, "EYE_BACK") },
        { "EYE_DETERMINED_DOWN_RIGHT", Tuple.Create(47, "EYE_DETERMINED_DOWN_RIGHT") },
        { "EYE_DETERMINED_RIGHT", Tuple.Create(47, "EYE_DETERMINED_RIGHT") },
        { "EYE_DOWN", Tuple.Create(47, "EYE_DOWN") },
        { "EYE_HALF_CLOSED", Tuple.Create(47, "EYE_HALF_CLOSED") },
        { "EYE_HAPPY", Tuple.Create(47, "EYE_HAPPY") },
        { "EYE_UP", Tuple.Create(47, "EYE_UP") },
        { "EYE_WORRIED_RIGHT", Tuple.Create(47, "EYE_WORRIED_RIGHT") },
        { "Face/EYE_CLOSED", Tuple.Create(47, "Face/EYE_CLOSED") },
        { "Face/EYE_DEAD", Tuple.Create(47, "Face/EYE_DEAD") },
        { "Face/EYE_RED", Tuple.Create(47, "Face/EYE_RED") },
        { "Face/EYE_SHOCKED", Tuple.Create(47, "Face/EYE_SHOCKED") },
        { "Face/EYE_SLEEPING", Tuple.Create(47, "Face/EYE_SLEEPING") },
        { "Face/EYE_SQUINT", Tuple.Create(47, "Face/EYE_SQUINT") },
        { "Face/EYE_UNCONVERTED", Tuple.Create(47, "Face/EYE_UNCONVERTED") },
        { "Face/EYE_UNCONVERTED_WORRIED", Tuple.Create(47, "Face/EYE_UNCONVERTED_WORRIED") },
        { "EYE_ANGRY_RIGHT_UP", Tuple.Create(47, "EYE_ANGRY_RIGHT_UP") },
        { "EYE_WHITE", Tuple.Create(47, "EYE_WHITE") },
        { "EYE_WEARY_RIGHT", Tuple.Create(47, "EYE_WEARY_RIGHT") },
        { "EYE_GRIMACE", Tuple.Create(47, "EYE_GRIMACE") },
        { "EYE_WEARY_RIGHT_DOWN", Tuple.Create(47, "EYE_WEARY_RIGHT_DOWN") },
        { "EYE_HAPPY2", Tuple.Create(47, "EYE_HAPPY2") },
        { "Face/EYE_RED_ANGRY", Tuple.Create(47, "Face/EYE_RED_ANGRY") },
        { "EYE_WHITE_ANGRY", Tuple.Create(47, "EYE_WHITE_ANGRY") },
        { "HairTuft", Tuple.Create(48, "HairTuft") },
        { "Tools/Book_open", Tuple.Create(49, "Tools/Book_open") },
        { "Tools/Book_closed", Tuple.Create(49, "Tools/Book_closed") },
        { "Tools/BookFlipping_3", Tuple.Create(49, "Tools/BookFlipping_3") },
        { "Tools/BookFlipping_2", Tuple.Create(49, "Tools/BookFlipping_2") },
        { "Tools/BookFlipping_1", Tuple.Create(49, "Tools/BookFlipping_1") },
        { "Tools/BookFlipping_4", Tuple.Create(49, "Tools/BookFlipping_4") },
        { "PonchoRightCorner", Tuple.Create(51, "PonchoRightCorner") },
        { "PonchoRightCorner", Tuple.Create(52, "PonchoRightCorner") },
        { "images/CrownMouth", Tuple.Create(53, "images/CrownMouth") },
        { "images/CrownMouthOpen", Tuple.Create(53, "images/CrownMouthOpen") },
        { "Tools/Chalice", Tuple.Create(54, "Tools/Chalice") },
        { "Tools/Chalice_Skull", Tuple.Create(54, "Tools/Chalice_Skull") },
        { "Tools/Chalice_Skull_Drink", Tuple.Create(54, "Tools/Chalice_Skull_Drink") },
        { "effects/slam_effect0006", Tuple.Create(55, "effects/slam_effect0006") },
        { "effects/slam_effect0005", Tuple.Create(55, "effects/slam_effect0005") },
        { "effects/slam_effect0004", Tuple.Create(55, "effects/slam_effect0004") },
        { "effects/slam_effect0003", Tuple.Create(55, "effects/slam_effect0003") },
        { "effects/slam_effect0002", Tuple.Create(55, "effects/slam_effect0002") },
        { "effects/slam_effect0001", Tuple.Create(55, "effects/slam_effect0001") },
        { "images/CrownSpikes", Tuple.Create(56, "images/CrownSpikes") },
        { "images/CrownSpikes2", Tuple.Create(57, "images/CrownSpikes2") },
        { "images/CrownSpikes", Tuple.Create(58, "images/CrownSpikes") },
        { "images/CrownSpikes2", Tuple.Create(59, "images/CrownSpikes2") },
        { "AttackHand1", Tuple.Create(60, "AttackHand1") },
        { "AttackHand2", Tuple.Create(60, "AttackHand2") },
        { "GauntletHeavy", Tuple.Create(61, "GauntletHeavy") },
        { "GauntletHeavy2", Tuple.Create(61, "GauntletHeavy2") },
        { "Weapons/Sling", Tuple.Create(62, "Weapons/Sling") },
        { "Weapons/SlingRope", Tuple.Create(63, "Weapons/SlingRope") },
        { "SlingHand", Tuple.Create(64, "SlingHand") },
        { "Arm_frontbit", Tuple.Create(65, "Arm_frontbit") },
        { "whiteball", Tuple.Create(66, "whiteball") },
        { "effects/whiteball", Tuple.Create(67, "effects/whiteball") },
        { "Weapons/SlingHand", Tuple.Create(68, "Weapons/SlingHand") },
        { "effects/portal_btm", Tuple.Create(69, "effects/portal_btm") },
        { "effects/portal_top", Tuple.Create(70, "effects/portal_top") },
        { "portal_splash", Tuple.Create(71, "portal_splash") },
        { "GrappleHook", Tuple.Create(72, "GrappleHook") },
        { "Weapons/Lute", Tuple.Create(73, "Weapons/Lute") },
        { "Weapons/SlingHand", Tuple.Create(74, "Weapons/SlingHand") },
        { "images/Crown_half_left", Tuple.Create(75, "images/Crown_half_left") },
        { "images/Crown_half_right", Tuple.Create(76, "images/Crown_half_right") },
        { "Sparks1", Tuple.Create(80, "Sparks1") },
        { "Sparks1", Tuple.Create(81, "Sparks1") },
        { "Sparks2", Tuple.Create(82, "Sparks2") },
        { "Sparks2", Tuple.Create(83, "Sparks2") },
        { "Weapons/SpecialSword_1", Tuple.Create(84, "Weapons/SpecialSword_1") },
        { "Weapons/SpecialSword_2", Tuple.Create(84, "Weapons/SpecialSword_2") },
        { "Weapons/SpecialSword_3", Tuple.Create(84, "Weapons/SpecialSword_3") },
        { "Weapons/SpecialSword_4", Tuple.Create(84, "Weapons/SpecialSword_4") },
        { "MonsterHeart_glow", Tuple.Create(85, "MonsterHeart_glow") },
        { "KnowledgeParchment", Tuple.Create(85, "KnowledgeParchment") },
        { "Knowledge_Trinket", Tuple.Create(85, "Knowledge_Trinket") },
        { "Knowledge_Curse", Tuple.Create(85, "Knowledge_Curse") },
        { "Knowledge_Decoration", Tuple.Create(85, "Knowledge_Decoration") },
        { "Knowledge_Weapon", Tuple.Create(85, "Knowledge_Weapon") },
        { "Tools/Woodaxe", Tuple.Create(85, "Tools/Woodaxe") },
        { "Tools/Woodaxe2", Tuple.Create(85, "Tools/Woodaxe2") },
        { "Tools/Pickaxe", Tuple.Create(85, "Tools/Pickaxe") },
        { "Tools/Pickaxe2", Tuple.Create(85, "Tools/Pickaxe2") },
        { "Tools/Hammer", Tuple.Create(85, "Tools/Hammer") },
        { "Net", Tuple.Create(85, "Net") },
        { "Items/WebberSkull", Tuple.Create(85, "Items/WebberSkull") },
        { "Tools/Book_open", Tuple.Create(85, "Tools/Book_open") },
        { "Tools/Book_closed", Tuple.Create(85, "Tools/Book_closed") },
        { "MonsterHeart_glow", Tuple.Create(86, "MonsterHeart_glow") },
        { "GiftSmall", Tuple.Create(86, "GiftSmall") },
        { "GiftMedium", Tuple.Create(86, "GiftMedium") },
        { "effects/MonsterBlood1", Tuple.Create(87, "effects/MonsterBlood1") },
        { "effects/MonsterBlood1", Tuple.Create(88, "effects/MonsterBlood1") },
        { "MonsterBlood2", Tuple.Create(89, "MonsterBlood2") },
        { "Tools/CardBack", Tuple.Create(90, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(90, "Tools/CardFront") },
        { "Tools/CardBack", Tuple.Create(91, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(91, "Tools/CardFront") },
        { "Tools/CardBack", Tuple.Create(92, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(92, "Tools/CardFront") },
        { "Tools/CardBack", Tuple.Create(93, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(93, "Tools/CardFront") },
        { "Tools/CardBack", Tuple.Create(94, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(94, "Tools/CardFront") },
        { "Tools/CardBack", Tuple.Create(95, "Tools/CardBack") },
        { "Tools/CardFront", Tuple.Create(95, "Tools/CardFront") },
        { "RitualSymbolHalo", Tuple.Create(96, "RitualSymbolHalo") },
        { "RitualSymbol_1", Tuple.Create(97, "RitualSymbol_1") },
        { "RitualSymbol_2", Tuple.Create(97, "RitualSymbol_2") },
        { "effects/RitualRing2", Tuple.Create(98, "effects/RitualRing2") },
        { "effects/SermonRing2", Tuple.Create(98, "effects/SermonRing2") },
        { "AttackSlash1", Tuple.Create(99, "AttackSlash1") },
        { "AttackSlash2", Tuple.Create(99, "AttackSlash2") },
        { "effects/RitualRing", Tuple.Create(100, "effects/RitualRing") },
        { "effects/SermonRing", Tuple.Create(100, "effects/SermonRing") },
        { "CollarPiece1", Tuple.Create(101, "CollarPiece1") },
        { "CollarPiece2", Tuple.Create(102, "CollarPiece2") },
        { "ChainBit1", Tuple.Create(103, "ChainBit1") },
        { "ChainBit2", Tuple.Create(104, "ChainBit2") },
        { "ChainBit1", Tuple.Create(105, "ChainBit1") },
        { "ChainBit3", Tuple.Create(106, "ChainBit3") },
        { "SwordHeavy", Tuple.Create(108, "SwordHeavy") },
        { "Weapons/SwordHeavy_Necromancy", Tuple.Create(108, "Weapons/SwordHeavy_Necromancy") },
        { "Weapons/SwordHeavy_Ice", Tuple.Create(108, "Weapons/SwordHeavy_Ice") },
        { "Weapons/SwordHeavy_Charm", Tuple.Create(108, "Weapons/SwordHeavy_Charm") },
        { "AxeHeavy", Tuple.Create(108, "AxeHeavy") },
        { "HammerHeavy", Tuple.Create(108, "HammerHeavy") },
        { "effects/SpawnHeavy_1", Tuple.Create(109, "effects/SpawnHeavy_1") },
        { "effects/SpawnHeavy_2", Tuple.Create(109, "effects/SpawnHeavy_2") },
        { "effects/SpawnHeavy_3", Tuple.Create(109, "effects/SpawnHeavy_3") },
        { "effects/SpawnHeavy_4", Tuple.Create(109, "effects/SpawnHeavy_4") },
        { "SpawnHeavy_glow", Tuple.Create(110, "SpawnHeavy_glow") },
        { "FireSmall_0001", Tuple.Create(111, "FireSmall_0001") },
        { "FireSmall_0002", Tuple.Create(111, "FireSmall_0002") },
        { "FireSmall_0003", Tuple.Create(111, "FireSmall_0003") },
        { "FireSmall_0004", Tuple.Create(111, "FireSmall_0004") },
        { "FireSmall_0005", Tuple.Create(111, "FireSmall_0005") },
        { "FireSmall_0006", Tuple.Create(111, "FireSmall_0006") },
        { "FireSmall_0007", Tuple.Create(111, "FireSmall_0007") },
        { "FireWild_0001", Tuple.Create(112, "FireWild_0001") },
        { "FireWild_0002", Tuple.Create(112, "FireWild_0002") },
        { "FireWild_0003", Tuple.Create(112, "FireWild_0003") },
        { "FireWild_0004", Tuple.Create(112, "FireWild_0004") },
        { "FireWild_0005", Tuple.Create(112, "FireWild_0005") },
        { "FireWild_0006", Tuple.Create(112, "FireWild_0006") },
        { "FireWild_0007", Tuple.Create(112, "FireWild_0007") },
        { "FireWild_0008", Tuple.Create(112, "FireWild_0008") },
        { "FireWild_0009", Tuple.Create(112, "FireWild_0009") },
        { "effects/chunder_1", Tuple.Create(113, "effects/chunder_1") },
        { "effects/chunder_2", Tuple.Create(113, "effects/chunder_2") },
        { "effects/chunder_3", Tuple.Create(113, "effects/chunder_3") },
        { "Curses/Icon_Curse_Blast", Tuple.Create(114, "Curses/Icon_Curse_Blast") },
        { "Curses/Icon_Curse_Fireball", Tuple.Create(114, "Curses/Icon_Curse_Fireball") },
        { "Curses/Icon_Curse_Slash", Tuple.Create(114, "Curses/Icon_Curse_Slash") },
        { "Curses/Icon_Curse_Splatter", Tuple.Create(114, "Curses/Icon_Curse_Splatter") },
        { "Curses/Icon_Curse_Tentacle", Tuple.Create(114, "Curses/Icon_Curse_Tentacle") }
    };

    internal static List<Skin?>? PlayerSkinOverride { get; set; }

    public static void AddFollowerSkin(CustomFollowerSkin followerSkin)
    {
        var atlasText = followerSkin.GenerateAtlasText();
        AddFollowerSkin(followerSkin.Name, followerSkin.Texture, atlasText, followerSkin.Colors, followerSkin.Hidden,
            followerSkin.Unlocked, followerSkin.TwitchPremium,
            followerSkin.Invariant);
    }

    public static void AddFollowerSkin(string name, Texture2D sheet, string atlasText,
        List<WorshipperData.SlotsAndColours> colors, bool hidden = false, bool unlocked = true,
        bool twitchPremium = false, bool invariant = false)
    {
        Material mat;
        SpineAtlasAsset atlas;
        var overrides = SkinUtils.CreateSkinAtlas(name, sheet, atlasText, RegionOverrideFunction, out mat, out atlas);
        SkinTextures.Add(name, sheet);
        SkinMaterials.Add(name, mat);
        CustomAtlases.Add(name, atlas);

        CreateNewFollowerType(name, colors, hidden, twitchPremium, invariant);
        CreateSkin(name, overrides, unlocked);
    }

    public static void AddPlayerSkin(CustomPlayerSkin playerSkin)
    {
        if (!CustomPlayerSkins.TryAdd(playerSkin.Name, playerSkin))
        {
            LogHelper.LogWarning($"Duplicate skin name: {playerSkin.Name}");
        }
        
        if (Plugin.SkinSettings != null)
            Plugin.SkinSettings.Options =
                new[] { "Default" }.Concat(CustomPlayerSkins.Keys).ToArray();
    }

    private static List<Tuple<int, string>> RegionOverrideFunction(AtlasRegion region)
    {
        var simpleName = region.name;
        var add = "";
        if (simpleName.Contains("#"))
        {
            var split = simpleName.Split('#');
            add = "#" + split[1];
            simpleName = split[0];
        }

        if (SimplifiedSkinNames.TryGetValue(simpleName, out var simplified))
        {
            region.name = simplified.Item1 + ":" + simplified.Item2 + add;
            return new List<Tuple<int, string>> { simplified };
        }

        if (!simpleName.Contains(":")) return new List<Tuple<int, string>>();

        try
        {
            var rName = simpleName.Split(':')[1];
            var regionIndex = (int)(SkinSlots)Enum.Parse(typeof(SkinSlots), simpleName.Split(':')[0]);
            region.name = regionIndex + ":" + rName + "#" + add;
            return new List<Tuple<int, string>> { Tuple.Create(regionIndex, rName) };
        }
        catch (Exception)
        {
            // ignored
        }

        return new List<Tuple<int, string>>();
    }

    internal static void CreateNewFollowerType(string name, List<WorshipperData.SlotsAndColours> colors,
        bool hidden = false, bool twitchPremium = false, bool invariant = false)
    {
        WorshipperData.Instance.Characters.Add(new WorshipperData.SkinAndData
        {
            Title = name,
            Skin = new List<WorshipperData.CharacterSkin>
            {
                new()
                {
                    Skin = name
                }
            },
            SlotAndColours = colors,
            TwitchPremium = twitchPremium,
            _hidden = hidden,
            _dropLocation = WorshipperData.DropLocation.Other,
            _invariant = invariant
        });
    }

    internal static void CreateSkin(string name, List<Tuple<int, string, float, float, float, float>> overrides,
        bool unlocked)
    {
        void Action()
        {
            Skin skin = new(name);
            var dog = WorshipperData.Instance.SkeletonData.Skeleton.Data.FindSkin("Dog");
            var skin2 = SkinUtils.ApplyAllOverrides(dog, skin, overrides, SkinMaterials[name], CustomAtlases[name]);

           if (!CustomFollowerSkins.TryAdd(name, skin2))
            {
                LogHelper.LogWarning("Failed to add skin " + name + " to CustomFollowerSkins");
            }
           
            if (!AlwaysUnlockedSkins.TryAdd(name, unlocked))
            {
                LogHelper.LogWarning("Failed to add skin " + name + " to AlwaysUnlockedSkins");
            }
        }

        if (Plugin.Started)
            Action();
        else
            Plugin.OnStart += Action;
    }

    public static void SetPlayerSkinOverride(Skin? normalSkin, Skin? hurtSkin = null, Skin? hurtSkin2 = null)
    {
        List<Skin?> skins = new()
        {
            normalSkin,
            hurtSkin,
            hurtSkin2
        };
        if (PlayerSkinOverride != null)
            LogHelper.LogDebug("PlayerSkinOverride already exists. Overwriting.");
        PlayerSkinOverride = skins;
    }

    public static void SetPlayerSkinOverride(CustomPlayerSkin skin)
    {
        skin.Apply();
    }

    public static void ResetPlayerSkin()
    {
        PlayerSkinOverride = null;
        SkinUtils.SkinToLoad = null;
    }
}
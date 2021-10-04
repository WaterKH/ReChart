using MoMMusicAnalysis;
using MoMMusicAnalysis.Song;
using MoMMusicAnalysis.Song.BossBattle;
using MoMMusicAnalysis.Song.FieldBattle;
using MoMMusicAnalysis.Song.MemoryDive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReChart.Logic
{
    public static class Utilities
    {
        //    public static bool IsControlDown()
        //    {
        //        return false;//(Control.ModifierKeys & Keys.Control) == Keys.Control;
        //    }

        public static void ConvertAndCopyFiles()
        {
            var files = Directory.GetFiles($"{Settings.Configurations.MelodyOfMemoryRootFolder}\\KINGDOM HEARTS Melody of Memory_Data\\StreamingAssets\\AssetBundles", "*", SearchOption.AllDirectories);
            var hex = "";
            byte[] bytes;

            foreach (var file in files.Where(x => x.Contains("_decrypted")))
            {
                FileInfo info = new FileInfo(file);

                using (var stream = new StreamReader(info.FullName))
                {
                    hex = stream.ReadToEnd();
                }

                if (hex.Substring(0, 5) == "Unity")
                    continue;

                bytes = Enumerable.Range(0, hex.Length / 2).Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16)).ToArray();

                var originalFile = info.FullName.Replace("_decrypted", "");

                File.Copy(originalFile, $"{originalFile}_original");
                File.WriteAllBytes(originalFile, bytes);
            }
        }


        public static string GetEnumName(FieldModelType type, int noteType, int altModel)
        {
            if (type == FieldModelType.CommonEnemy)
                return "CommonEnemy";
            else if (type == FieldModelType.AerialCommonEnemy)
                return "AerialCommonEnemy";
            else if (type == FieldModelType.UncommonEnemy)
                return "UncommonEnemy";
            else if (type == FieldModelType.AerialUncommonEnemy)
                return "AerialUncommonEnemy";
            else if (type == FieldModelType.MultiHitGroundEnemy)
                return "MultiHitGroundEnemy";
            else if (type == FieldModelType.MultiHitAerialEnemy)
                return "MultiHitAerialEnemy";
            else if (type == FieldModelType.EnemyShooterProjectile && noteType == 2)
                return "EnemyShooterProjectile";
            else if (type == FieldModelType.EnemyShooter && noteType == 0)
                return "EnemyShooter";
            else if (type == FieldModelType.AerialEnemyShooterProjectile && noteType == 2)
                return "AerialEnemyShooterProjectile";
            else if (type == FieldModelType.AerialEnemyShooter && noteType == 0)
                return "AerialEnemyShooter";
            else if (type == FieldModelType.JumpingGroundEnemy)
                return "JumpingGroundEnemy";
            else if (type == FieldModelType.JumpingAerialEnemy)
                return "JumpingAerialEnemy";
            else if (type == FieldModelType.HiddenEnemy)
                return "HiddenEnemy";
            else if (type == FieldModelType.HittableAerialUncommonEnemy)
                return "HittableAerialUncommonEnemy";
            else if (type == FieldModelType.CrystalEnemyLeftRight && noteType == 1)
                return "CrystalEnemyLeftRight";
            else if (type == FieldModelType.CrystalEnemyCenter && noteType == 1)
                return "CrystalEnemyCenter";
            else if (type == FieldModelType.GlideNote && noteType == 3)
                return "GlideNote";
            else if (type == FieldModelType.Barrel && noteType == 4 && altModel == 0)
                return "Barrel";
            else if (type == FieldModelType.Crate && noteType == 4 && altModel == 1)
                return "Crate";
            else if (type == FieldModelType.CrystalTeamHeal && noteType == 1)
                return "CrystalTeamHeal";
            else
                return string.Empty;
        }

        public static string GetImage(FieldModelType type, int noteType, int altModel)
        {
            string modelType;

            if (type == FieldModelType.CommonEnemy)
                modelType = "Shadow";
            else if (type == FieldModelType.AerialCommonEnemy)
                modelType = "RedNocturne";
            else if (type == FieldModelType.UncommonEnemy)
                modelType = "Soldier";
            else if (type == FieldModelType.AerialUncommonEnemy)
                modelType = "AirSoldier";
            else if (type == FieldModelType.MultiHitGroundEnemy)
                modelType = "Large";
            else if (type == FieldModelType.MultiHitAerialEnemy)
                modelType = "DarkBall";
            else if (type == FieldModelType.EnemyShooterProjectile && noteType == 2)
                modelType = "Projectile";
            else if (type == FieldModelType.EnemyShooter && noteType == 0)
                modelType = "CreeperPlant";
            else if (type == FieldModelType.AerialEnemyShooterProjectile && noteType == 2)
                modelType = "Projectile";
            else if (type == FieldModelType.AerialEnemyShooter && noteType == 0)
                modelType = "RedNocturne";
            else if (type == FieldModelType.JumpingGroundEnemy)
                modelType = "Crescendo";
            else if (type == FieldModelType.JumpingAerialEnemy)
                modelType = "Crescendo";
            else if (type == FieldModelType.HiddenEnemy)
                modelType = "HiddenShadow";
            else if (type == FieldModelType.HittableAerialUncommonEnemy)
                modelType = "AirSoldier";
            else if (type == FieldModelType.CrystalEnemyLeftRight && noteType == 1)
                modelType = "Crystal-Base";
            else if (type == FieldModelType.CrystalEnemyCenter && noteType == 1)
                modelType = "Crystal-Base";
            else if (type == FieldModelType.GlideNote && noteType == 3)
                modelType = "GlideNoteAlt";
            else if (type == FieldModelType.Barrel && noteType == 4 && altModel == 0)
                modelType = "Barrel";
            else if (type == FieldModelType.Crate && noteType == 4 && altModel == 1)
                modelType = "Bokusu";
            else if (type == FieldModelType.CrystalTeamHeal && noteType == 1)
                modelType = "Crystal-Base";
            else
                modelType = string.Empty;

            return $"/images/ReChart/FieldNotes/{modelType}.png";
        }

        public static string GetImage(FieldAssetType type)
        {
            string modelType;

            if (type == FieldAssetType.AerialCommonArrow || type == FieldAssetType.AerialUncommonArrow || type == FieldAssetType.MultiHitAerialArrow ||
            type == FieldAssetType.AerialShooterArrow || type == FieldAssetType.JumpingAerialArrow)
                modelType = "AssetJumpAttack";
            else if (type == FieldAssetType.ShooterProjectileArrow || type == FieldAssetType.AerialShooterProjectileArrow)
                modelType = "AssetJumpEvade";
            else if (type == FieldAssetType.GlideArrow)
                modelType = "AssetJumpGlide";
            else if (type == FieldAssetType.CrystalRightLeft)
                modelType = "DarkBall";
            else if (type == FieldAssetType.CrystalCenter)
                modelType = "Neoshadow";
            else
                modelType = string.Empty;

            return $"/images/ReChart/FieldAssets/{modelType}.png";
        }

        public static string GetImage(PerformerType type)
        {
            return $"/images/ReChart/PerformerNotes/{type}.png";
        }

        public static string GetImage(string type)
        {
            return $"/images/ReChart/{type}.png";
        }

        public static FieldAssetType GetFieldAssetEnumForFieldNote(string parentModelType)
        {
            if (parentModelType == "AerialCommonEnemy")
                return FieldAssetType.AerialCommonArrow;
            else if (parentModelType == "AerialUncommonEnemy")
                return FieldAssetType.AerialUncommonArrow;
            else if (parentModelType == "MultiHitAerialEnemy")
                return FieldAssetType.MultiHitAerialArrow;
            else if (parentModelType == "EnemyShooterProjectile")
                return FieldAssetType.ShooterProjectileArrow;
            else if (parentModelType == "AerialEnemyShooterProjectile")
                return FieldAssetType.AerialShooterProjectileArrow;
            else if (parentModelType == "AerialEnemyShooter")
                return FieldAssetType.AerialShooterArrow;
            else if (parentModelType == "JumpingAerialEnemy")
                return FieldAssetType.JumpingAerialArrow;
            else if (parentModelType == "CrystalEnemyLeftRight")
                return FieldAssetType.CrystalRightLeft;
            else if (parentModelType == "CrystalEnemyCenter")
                return FieldAssetType.CrystalCenter;
            else if (parentModelType == "GlideNote")
                return FieldAssetType.GlideArrow;
            else
                return FieldAssetType.None;
        }



        public static string GetImage(BossNoteType type)
        {
            string modelType;

            if (type == BossNoteType.Normal)
                modelType = "Normal";
            else if (type == BossNoteType.Swipe)
                modelType = "Swipe";
            else if (type == BossNoteType.HoldStart || type == BossNoteType.HoldEnd)
                modelType = "Hold";
            else if (type == BossNoteType.Crystal)
                modelType = "Crystal";
            else
                modelType = string.Empty;

            return $"/images/ReChart/BossNotes/{modelType}.png";
        }

        public static string GetImage(MemoryNoteType type)
        {
            string modelType;

            if (type == MemoryNoteType.Normal)
                modelType = "Normal";
            else if (type == MemoryNoteType.Swipe)
                modelType = "Swipe";
            else if (type == MemoryNoteType.HoldStart || type == MemoryNoteType.HoldEnd)
                modelType = "Hold";
            else
                modelType = string.Empty;

            return $"/images/ReChart/MemoryNotes/{modelType}.png";
        }

        public static string GetImageFromSongName(string fileName)
        {
            var imagePath = "ReChart/Mom_icon.png";

            switch (fileName)
            {
                case "music0000000":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000019.png";
                    break;
                case "music0000001":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000005.png";
                    break;
                case "music0000002":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000004.png";
                    break;
                case "music0000003":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000004.png";
                    break;
                case "music0000004":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000010.png";
                    break;
                case "music0000005":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000010.png";
                    break;
                case "music0000006":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000008.png";
                    break;
                case "music0000007":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000008.png";
                    break;
                case "music0000008":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000006.png";
                    break;
                case "music0000009":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000006.png";
                    break;
                case "music0000010":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000011.png";
                    break;
                case "music0000011":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000011.png";
                    break;
                case "music0000012":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000007.png";
                    break;
                case "music0000013":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000007.png";
                    break;
                case "music0000014":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000014.png";
                    break;
                case "music0000015":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000014.png";
                    break;
                case "music0000016":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000028.png";
                    break;
                case "music0000017":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000028.png";
                    break;
                case "music0000018":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000009.png";
                    break;
                case "music0000019":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000009.png";
                    break;
                case "music0000020":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000012.png";
                    break;
                case "music0000021":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000012.png";
                    break;
                case "music0000022":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000022.png";
                    break;
                case "music0000023":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000022.png";
                    break;
                case "music0000024":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000025.png";
                    break;
                case "music0000025":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000025.png";
                    break;
                case "music0000026":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000015.png";
                    break;
                case "music0000027":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000015.png";
                    break;
                case "music0000028":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000026.png";
                    break;
                case "music0000029":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000026.png";
                    break;
                case "music0000030":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000038.png";
                    break;
                case "music0000031":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000038.png";
                    break;
                case "music0000032":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000066.png";
                    break;
                case "music0000033":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000066.png";
                    break;
                case "music0000034":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000030.png";
                    break;
                case "music0000035":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000030.png";
                    break;
                case "music0000036":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000062.png";
                    break;
                case "music0000037":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000062.png";
                    break;
                case "music0000038":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000065.png";
                    break;
                case "music0000039":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000065.png";
                    break;
                case "music0000040":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000036.png";
                    break;
                case "music0000041":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000036.png";
                    break;
                case "music0000042":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000018.png";
                    break;
                case "music0000043":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000018.png";
                    break;
                case "music0000044":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000027.png";
                    break;
                case "music0000045":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000001.png";
                    break;
                case "music0000046":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000001.png";
                    break;
                case "music0000047":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000047.png";
                    break;
                case "music0000048":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000047.png";
                    break;
                case "music0000049":
                    // No Listing in World Trip
                    break;
                case "music0000050":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000063.png";
                    break;
                case "music0000051":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000063.png";
                    break;
                case "music0000052":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000029.png";
                    break;
                case "music0000053":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000029.png";
                    break;
                case "music0000054":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000035.png";
                    break;
                case "music0000055":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000035.png";
                    break;
                case "music0000056":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000039.png";
                    break;
                case "music0000057":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000039.png";
                    break;
                case "music0000058":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000043.png";
                    break;
                case "music0000059":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000043.png";
                    break;
                case "music0000060":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000054.png";
                    break;
                case "music0000061":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000054.png";
                    break;
                case "music0000062":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000057.png";
                    break;
                case "music0000063":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000057.png";
                    break;
                case "music0000064":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000037.png";
                    break;
                case "music0000065":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000037.png";
                    break;
                case "music0000066":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000060.png";
                    break;
                case "music0000067":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000060.png";
                    break;
                case "music0000068":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000061.png";
                    break;
                case "music0000069":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000061.png";
                    break;
                case "music0000070":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000064.png";
                    break;
                case "music0000071":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000064.png";
                    break;
                case "music0000072":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000049.png";
                    break;
                case "music0000073":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000049.png";
                    break;
                case "music0000074":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000048.png";
                    break;
                case "music0000075":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000048.png";
                    break;
                case "music0000076":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000052.png";
                    break;
                case "music0000077":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000052.png";
                    break;
                case "music0000078":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000051.png";
                    break;
                case "music0000079":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000051.png";
                    break;
                case "music0000080":
                case "music0000081":
                    // No Listing in World Trip
                    break;
                case "music0000082":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000046.png";
                    break;
                case "music0000083":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000046.png";
                    break;
                case "music0000084":
                    imagePath = "ReChart/Backgrounds/Background_BG0000000.png";
                    break;
                case "music0000085":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000017.png";
                    break;
                case "music0000086":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000016.png";
                    break;
                case "music0000087":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000020.png";
                    break;
                case "music0000088":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000021.png";
                    break;
                case "music0000089":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000024.png";
                    break;
                case "music0000090":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000042.png";
                    break;
                case "music0000091":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000031.png";
                    break;
                case "music0000092":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000033.png";
                    break;
                case "music0000093":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000040.png";
                    break;
                case "music0000094":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000034.png";
                    break;
                case "music0000095":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000041.png";
                    break;
                case "music0000096":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000045.png";
                    break;
                case "music0000097":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000032.png";
                    break;
                case "music0000098":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000023.png";
                    break;
                case "music0000099":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000044.png";
                    break;
                case "music0000100":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000059.png";
                    break;
                case "music0000101":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000069.png";
                    break;
                case "music0000102":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000067.png";
                    break;
                case "music0000103":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000068.png";
                    break;
                case "music0000104":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000070.png";
                    break;
                case "music0000105":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000003.png";
                    break;
                case "music0000106":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000050.png";
                    break;
                case "music0000107":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000053.png";
                    break;
                case "music0000108":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000055.png";
                    break;
                case "music0000109":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000056.png";
                    break;
                case "music0000110":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000002.png";
                    break;
                case "music0000111":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000111.png";
                    break;
                case "music0000112":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000112.png";
                    break;
                case "music0000113":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000113.png";
                    break;
                case "music0000114":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000114.png";
                    break;
                case "music0000115":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000115.png";
                    break;
                case "music0000116":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000116.png";
                    break;
                case "music0000117":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000117.png";
                    break;
                case "music0000118":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000118.png";
                    break;
                case "music0000119":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000119.png";
                    break;
                case "music0000120":
                    // No Listing
                    break;
                case "music0000121":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000120.png";
                    break;
                case "music0000122":
                    imagePath = "ReChart/Backgrounds/Background_BG0000046.png";
                    break;
                case "music0000123":
                    imagePath = "ReChart/Backgrounds/Background_BG0000073.png";
                    break;
                case "music0000124":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000073.png";
                    break;
                case "music0000125":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000072.png";
                    break;
                case "music0000126":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000020.png";
                    break;
                case "music0000127":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000071.png";
                    break;
                case "music0000128":
                    imagePath = "ReChart/Backgrounds/Background_BG0000002.png";
                    break;
                case "music0000129":
                case "music0000130":
                case "music0000131":
                case "music0000132":
                    break;
                case "music0000133":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000131.png";
                    break;
                case "music0000134":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000132.png";
                    break;
                case "music0000135":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000074.png";
                    break;
                case "music0000136":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000075.png";
                    break;
                case "music0000137":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000078.png";
                    break;
                case "music0000138":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000077.png";
                    break;
                case "music0000139":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000076.png";
                    break;
                case "music0000140":
                    imagePath = "ReChart/Backgrounds/Background_BG0000007.png";
                    break;
                    break;
                case "music0000141":
                case "music0000142":
                    // No Listings
                    break;
                case "music0000143":
                    imagePath = "ReChart/Backgrounds/Background_BG0000025.png";
                    break;
                case "music0000144":
                    imagePath = "ReChart/Backgrounds/Background_BG0000001.png";
                    break;
                case "music0000145":
                    imagePath = "ReChart/Backgrounds/Background_BG0000021.png";
                    break;
                case "music0000146":
                    imagePath = "ReChart/Backgrounds/Background_BG0000000.png";
                    break;
                case "music0000147":
                    imagePath = "ReChart/Backgrounds/Background_BG0000020.png";
                    break;
                case "music0000148":
                    imagePath = "ReChart/Backgrounds/Background_BG0000041.png";
                    break;
                case "music0000149":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000004.png";
                    break;
                case "music0000150":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000001.png";
                    break;
                case "music0000151":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000006.png";
                    break;
                case "music0000152":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000009.png";
                    break;
                case "music0000153":
                    // No Listing
                    break;
                case "music0000154":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000022.png";
                    break;
                case "music0000155":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000051.png";
                    break;
                case "music0000156":
                    imagePath = "ReChart/Backgrounds/Background_BG0000019.png";
                    break;
                case "music0000157":
                    imagePath = "ReChart/Backgrounds/Background_BG0000053.png";
                    break;
                case "music0000158":
                    imagePath = "ReChart/Backgrounds/Background_BG0000073.png";
                    break;
                case "music0000159":
                    imagePath = "ReChart/Backgrounds/Background_BG0000077.png";
                    break;
                case "music0000160":
                    imagePath = "ReChart/Backgrounds/Background_BG0000067.png";
                    break;
                case "music0000161":
                    imagePath = "ReChart/Backgrounds/Background_BG0000072.png";
                    break;
                case "music0000162":
                    imagePath = "ReChart/Backgrounds/Background_BG0000018.png";
                    break;
                case "music0000163":
                    imagePath = "ReChart/Backgrounds/Background_BG0000030.png";
                    break;
                case "music0000164":
                    imagePath = "ReChart/Backgrounds/Background_BG0000053.png";
                    break;
                case "music0000165":
                case "music0000166":
                case "music0000167":
                    // No Listing
                    break;
                case "music0000168":
                    imagePath = "ReChart/Backgrounds/Background_BG0000001.png";
                    break;
                case "music0000169":
                    imagePath = "ReChart/Backgrounds/Background_BG0000020.png";
                    break;
                case "music0000170":
                    imagePath = "ReChart/Backgrounds/Background_BG0000000.png";
                    break;
                case "music0000171":
                    imagePath = "ReChart/Backgrounds/Background_BG0000041.png";
                    break;
                case "music0000172":
                    imagePath = "ReChart/Backgrounds/Background_BG0000001.png";
                    break;
                case "music0000173":
                    // No Listing
                    break;
                case "music0000174":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000079.png";
                    break;
                case "music0000175":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000079.png";
                    break;
                case "music0000176":
                    // No Listing
                    break;
                case "music0000177":
                    imagePath = "ReChart/Backgrounds/Background_MUSIC0000120.png";
                    break;
                case "music0000178":
                case "music0000179":
                    // No Listing
                    break;
                case "music0000180":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000010.png";
                    break;
                case "music0000181":
                    // No Listing
                    break;
                case "music0000182":
                    imagePath = "ReChart/Backgrounds/Background_BG0000019.png";
                    break;
                case "music0000183":
                    // No Listing
                    break;
                case "music0000184":
                    imagePath = "ReChart/Backgrounds/Background_BG0000020.png";
                    break;
                case "music0000185":
                case "music0000186":
                    // No Listing
                    break;
                case "music0000187":
                    imagePath = "ReChart/Backgrounds/Background_BG0000016.png";
                    break;
                case "music0000188":
                    break;
                case "music0000189":
                    imagePath = "ReChart/Backgrounds/Background_BG0000013.png";
                    break;
                case "music0000190":
                    imagePath = "ReChart/Backgrounds/Background_BG0000027.png";
                    break;
                case "music0000191":
                case "music0000192":
                    // No Listing
                    break;
                case "music0000193":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000065.png";
                    break;
                case "music0000194":
                case "music0000195":
                    // No Listing
                    break;
                case "music0000196":
                    imagePath = "ReChart/Backgrounds/Background_BG0000074.png";
                    break;
                case "music0000197":
                case "music0000198":
                    // No Listing
                    break;
                case "music0000199":
                    imagePath = "ReChart/Backgrounds/Background_BG0000000.png";
                    break;
                case "music0000200":
                case "music0000201":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000081.png";
                    break;
                case "music0000202":
                case "music0000203":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000081.png";
                    break;
                case "music0000204":
                    imagePath = "ReChart/Worlds/TRIPWORLD0000009.png";
                    break;
                case "music0000205":
                    break;
                case "music0000206":
                    imagePath = "ReChart/Backgrounds/Background_BG0000023.png";
                    break;
                default:
                    imagePath = "ReChart/Mom_icon.png";
                    break;
            }

            return $"images/{imagePath}";
        }
    }
}
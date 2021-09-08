using MoMMusicAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReChart.Logic
{
    public static class Utilities
    {
        //    public static bool IsControlDown()
        //    {
        //        return false;//(Control.ModifierKeys & Keys.Control) == Keys.Control;
        //    }


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
    }
}
using UnityEngine;

public class GameEntries
{
    public enum Scenes
    {
        loading = 0,
        game = 1,
    }

    [System.Serializable]
    public class Slot
    {
        public GameEntries.SOCKET_TYPE slotType;
        public Transform slotTransform;

    }

    public enum SOCKET_TYPE
    {
        ProjectileSocket, BottomSocket
    }

    public enum GAME_STATE
    {
        LOADING,
        MENU,
        STARTED,
        PAUSED,
        GAME_OVER,
        TOWER_SELECTION
    }

    public enum GAME_EVENTS
    {
        TowerSelected = 0,
        TowerDamaged = 1,
        TowerDestroyed = 2,
        UpdateTowerHealthUI = 3,
        EnemyKilled = 4,
        EnemySpawned = 5,
        TowerLevelUp = 6,
        TowerGotExp = 7,
        RoundStarted = 8,
        TowerHealed = 9,
    }

    #region COMBAT

    public enum ABILITY_MECHANIC_TYPE
    {
        Self,
        Projectile,
        Target_Projectile,
        Target_Instant,
        AreaOfEffect,       
    }

    public enum TARGET_TYPE
    {
        Self,
        RandomEnemy,
        NearestEnemy,
        AllEnemies,
        RandomPosition,
        None,
    }

    public enum EFFECT_TYPE
    {
        InstantDamage,
        InstantHeal,
        Knockback
    }
    #endregion
}

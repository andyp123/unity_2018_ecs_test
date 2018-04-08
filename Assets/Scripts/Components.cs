using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;

namespace DanmakuExample
{
    // Enum style struts
    public struct Factions
    {
        public const int Player = 0;
        public const int Enemy = 1;
    }

    // Tag components
    public struct Player : IComponentData {}
    public struct PlayerShot : IComponentData {}
    public struct Enemy : IComponentData {}
    public struct EnemyShot : IComponentData {}
    public struct Dead : IComponentData {}

    // Data components
    public struct PlayerInput : IComponentData
    {
        public float2 Move;
        public int Shoot;
        public float FireCooldown;

        // Can't have bool as value, or a function, so use delegate...
        public bool Fire => Shoot == 1 && FireCooldown <= 0f;
    }
    
    public struct Health : IComponentData
    {
        public int Value;
    }

    public struct Damaged : IComponentData
    {
        public int Damage;
    }

    public struct Shot : IComponentData
    {
        public int Damage;
        public float Lifetime;
    }

    public struct ShotSpawnData : IComponentData
    {
        public Shot Shot;
        public Position2D Position;
        public Heading2D Heading;
        public int Faction; 
    }

    public struct EnemyShootState : IComponentData
    {
        public float Cooldown;
    }

    public struct EnemySpawnCooldown : IComponentData
    {
        public float Value;
    }

    public struct EnemySpawnSystemState : IComponentData
    {
        public int SpawnedEnemyCount;
        public Random.State RandomState;
    }
}

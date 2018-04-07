using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;

namespace DanmakuExample
{
    // Tag components
    public struct Player : IComponentData {}
    public struct PlayerShot : IComponentData {}
    public struct Enemy : IComponentData {}
    public struct EnemyShot : IComponentData {}
    public struct Dead : IComponentData {}

    // Data components
    public struct Health : IComponentData
    {
        public int Value;
    }

    public struct Damaged : IComponentData
    {
        public int Damage;
        public Position2D ImpactPosition;
    }

    public struct Shot : IComponentData
    {
        public int Damage;
        public Position2D Position;
        public Heading2D Heading;
        public float ExpiryTime;
    }
}

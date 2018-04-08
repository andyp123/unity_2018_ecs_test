using UnityEngine;

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms2D;

using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace DanmakuExample
{
    class ShotDamageSystem : JobComponentSystem
    {
        struct Players
        {
            public int Length;
            public ComponentDataArray<Health> Health;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<Player> PlayerTag;
        }

        [Inject] private Players players;

        struct Enemies
        {
            public int Length;
            public ComponentDataArray<Health> Health;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<Enemy> EnemyTag;
        }

        [Inject] private Enemies enemies;

        struct PlayerShots
        {
            public int Length;
            public ComponentDataArray<Shot> Shot;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<PlayerShot> PlayerShotTag;
        }

        [Inject] private PlayerShots playerShots;

        struct EnemyShots
        {
            public int Length;
            public ComponentDataArray<Shot> Shot;
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<EnemyShot> EnemyShotTag;
        }

        [Inject] private EnemyShots enemyShots;

        [ComputeJobOptimization]
        struct CollisionJob : IJobParallelFor
        {
            public float SqrCollisionRadius;

            public ComponentDataArray<Health> Health;
            [ReadOnly] public ComponentDataArray<Position2D> Positions;

            [NativeDisableParallelForRestriction]
            public ComponentDataArray<Shot> Shots;

            [NativeDisableParallelForRestriction]
            [ReadOnly] public ComponentDataArray<Position2D> ShotPositions;

            public void Execute (int index)
            {
                int damage = 0;
                float2 position = Positions[index].Value;

                for (int si = 0; si < Shots.Length; ++si)
                {
                    float2 shotPosition = ShotPositions[si].Value;
                    float2 delta = shotPosition - position;
                    float sqrDistance = math.dot(delta, delta);

                    if (sqrDistance < SqrCollisionRadius)
                    {
                        var shot = Shots[si];
                        damage += shot.Damage;

                        // This will cause the shot to be destroyed by ShotDestroySystem
                        shot.Lifetime = 0f;
                        Shots[si] = shot;
                    }
                }

                var health = Health[index];
                health.Value = math.max(0, health.Value - damage);
                Health[index] = health;
            }
        }

        protected override JobHandle OnUpdate (JobHandle inputDeps)
        {
            var settings = DanmakuMain.Settings;

            if (settings == null)
            {
                return inputDeps;
            }

            var playerVsEnemyShot = new CollisionJob {
                SqrCollisionRadius = settings.playerSize * settings.playerSize,
                Health = players.Health,
                Positions = players.Position,
                ShotPositions = enemyShots.Position,
                Shots = enemyShots.Shot
            }.Schedule(players.Length, 1, inputDeps);
        
            var enemyVsPlayerShot = new CollisionJob {
                SqrCollisionRadius = settings.enemySize * settings.enemySize,
                Health = enemies.Health,
                Positions = enemies.Position,
                ShotPositions = playerShots.Position,
                Shots = playerShots.Shot
            }.Schedule(enemies.Length, 1, playerVsEnemyShot);

            return enemyVsPlayerShot;
        }
    }
}
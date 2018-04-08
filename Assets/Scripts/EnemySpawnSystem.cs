using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Transforms2D;

namespace DanmakuExample
{
    class EnemySpawnSystem : ComponentSystem
    {
        struct State
        {
            public ComponentDataArray<EnemySpawnCooldown> Cooldown;
            public ComponentDataArray<EnemySpawnSystemState> S;
        }

        [Inject] private State state;

        public static void SetupComponentData (EntityManager entityManager)
        {
            var stateEntity = entityManager.CreateEntity(
                ComponentType.Create<EnemySpawnCooldown>(),
                ComponentType.Create<EnemySpawnSystemState>()
                );
            var oldState = Random.state;
            Random.InitState(0xaf77);
            entityManager.SetComponentData(stateEntity, new EnemySpawnCooldown { Value = 0f} );
            entityManager.SetComponentData(stateEntity, new EnemySpawnSystemState {
                SpawnedEnemyCount = 0,
                RandomState = Random.state
                });
            Random.state = oldState;
        }

        protected override void OnUpdate ()
        {
            float cooldown = state.Cooldown[0].Value;
            cooldown = Mathf.Max(0f, state.Cooldown[0].Value - Time.deltaTime);
            bool spawn = cooldown <= 0f;

            if (spawn)
            {
                cooldown = DanmakuMain.Settings.enemySpawnCooldown;
            }

            state.Cooldown[0] = new EnemySpawnCooldown { Value = cooldown };

            if (spawn)
            {
                SpawnEnemy();
            }
        }

        void SpawnEnemy ()
        {
            var settings = DanmakuMain.Settings;

            var newState = state.S[0];
            var oldState = Random.state;
            Random.state = newState.RandomState;

            float2 spawnPosition = ComputeSpawnLocation();
            newState.SpawnedEnemyCount++;

            PostUpdateCommands.CreateEntity(DanmakuMain.EnemyArchetype);
            PostUpdateCommands.SetComponent(default(Enemy));
            PostUpdateCommands.SetComponent(new Position2D { Value = spawnPosition });
            PostUpdateCommands.SetComponent(new Heading2D { Value = new float2(0f, -1f) });
            PostUpdateCommands.SetComponent(new Health { Value = settings.enemyMaxHealth });
            PostUpdateCommands.SetComponent(new EnemyShootState { Cooldown = settings.enemyFireCooldown });
            PostUpdateCommands.SetComponent(new MoveSpeed { speed = settings.enemyMaxSpeed });
            PostUpdateCommands.AddSharedComponent(DanmakuMain.EnemyRenderer);

            newState.RandomState = Random.state;

            state.S[0] = newState;
            Random.state = oldState;
        }

        float2 ComputeSpawnLocation ()
        {
            var settings = DanmakuMain.Settings;

            float r = Random.value;
            float xMin = settings.playfield.xMin;
            float xMax = settings.playfield.xMax;
            float x = xMin + (xMax - xMin) * r;

            return new float2(x, settings.playfield.yMax);
        }
    }
}
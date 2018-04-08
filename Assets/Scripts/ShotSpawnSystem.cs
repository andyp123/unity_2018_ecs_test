using UnityEngine;

using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace DanmakuExample
{
    public class ShotSpawnBarrier : BarrierSystem {}

    public class ShotSpawnSystem : ComponentSystem
    {
        public struct Data
        {
            public EntityArray Entity;
            [ReadOnly] public ComponentDataArray<ShotSpawnData> SpawnData;
            public int Length;
        }

        [Inject] private Data data;

        protected override void OnUpdate ()
        {
            var em = PostUpdateCommands;

            var playerShotRenderer = DanmakuMain.PlayerShotRenderer;
            var enemyShotRenderer = DanmakuMain.EnemyShotRenderer;
            var settings = DanmakuMain.Settings;

            for (int i = 0; i < data.Length; ++i)
            {
                var spawnData = data.SpawnData[i];
                var shotEntity = data.Entity[i];

                em.RemoveComponent<ShotSpawnData>(shotEntity);
                em.AddSharedComponent(shotEntity, default(MoveForward));
                em.AddComponent(shotEntity, spawnData.Shot);
                em.AddComponent(shotEntity, spawnData.Position);
                em.AddComponent(shotEntity, spawnData.Heading);
                em.AddComponent(shotEntity, default(TransformMatrix));

                if (spawnData.Faction == Factions.Player)
                {
                    em.AddComponent(shotEntity, new PlayerShot());
                    em.AddComponent(shotEntity, new MoveSpeed { speed = settings.playerShotSpeed });
                    em.AddSharedComponent(shotEntity, playerShotRenderer);
                }
                else
                {
                    em.AddComponent(shotEntity, new EnemyShot());
                    em.AddComponent(shotEntity, new MoveSpeed { speed = settings.enemyShotSpeed });
                    em.AddSharedComponent(shotEntity, enemyShotRenderer);
                }
            }
        }
    }
}
using UnityEngine;

using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms2D;

namespace DanmakuExample
{
    class EnemyCullSystem : JobComponentSystem
    {
        public struct BoundaryKillJob : IJobProcessComponentData<Health, Position2D, Enemy>
        {
            public float MinY;
            public float MaxY;

            public void Execute (ref Health health, [ReadOnly] ref Position2D pos, [ReadOnly] ref Enemy enemyTag)
            {
                if (pos.Value.y < MinY || pos.Value.y > MaxY)
                {
                    health.Value = -1;
                }
            }
        }

        protected override JobHandle OnUpdate (JobHandle inputDeps)
        {
            if (DanmakuMain.Settings == null)
            {
                return inputDeps;
            }

            var boundaryKillJob = new BoundaryKillJob {
                MinY = DanmakuMain.Settings.playfield.yMin,
                MaxY = DanmakuMain.Settings.playfield.yMax,  
            };

            return boundaryKillJob.Schedule(this, 64, inputDeps);
        }
    }
}
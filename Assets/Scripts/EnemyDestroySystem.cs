using UnityEngine;

using Unity.Collections;
using Unity.Entities;
using Unity.Transforms2D;

namespace DanmakuExample
{
    [UpdateAfter(typeof(MoveForward2DSystem))]
    [UpdateAfter(typeof(EnemyCullSystem))]
    public class EnemyDestroySystem : ComponentSystem
    {
        public struct Data
        {
            public EntityArray Entities;
            public ComponentDataArray<Health> Health;
            public ComponentDataArray<Enemy> EnemyTag;
            public int Length;
        }

        [Inject] private Data data;

        protected override void OnUpdate ()
        {
            for (int i = 0; i < data.Length; ++i)
            {
                Health health = data.Health[i];

                if (health.Value <= 0f)
                {
                    PostUpdateCommands.DestroyEntity(data.Entities[i]);
                }
            }
        }
    }
}
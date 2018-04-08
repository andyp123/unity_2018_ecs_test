using UnityEngine;

using Unity.Collections;
using Unity.Entities;
using Unity.Transforms2D;

namespace DanmakuExample
{
    [UpdateAfter(typeof(ShotSpawnSystem))]
    [UpdateAfter(typeof(MoveForward2DSystem))]
    public class ShotDestroySystem : ComponentSystem
    {
        public struct Data
        {
            public EntityArray Entities;
            public ComponentDataArray<Shot> Shot;
            public int Length;
        }

        [Inject] private Data data;

        protected override void OnUpdate ()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < data.Length; ++i)
            {
                Shot shot = data.Shot[i];
                shot.Lifetime -= dt;

                if (shot.Lifetime <= 0f)
                {
                    PostUpdateCommands.DestroyEntity(data.Entities[i]);
                }

                data.Shot[i] = shot;
            }
        }
    }
}
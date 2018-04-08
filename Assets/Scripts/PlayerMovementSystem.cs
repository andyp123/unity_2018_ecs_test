using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;
using Unity.Collections; // Needed for [Readonly]

namespace DanmakuExample
{
    public class PlayerMovementSystem : ComponentSystem
    {
        public struct Data
        {
            public ComponentDataArray<Position2D> Position;
            [ReadOnly] public ComponentDataArray<PlayerInput> Input;
            public int Length;
        }

        [Inject] private Data data;

        protected override void OnUpdate ()
        {
            DanmakuSettings settings = DanmakuMain.Settings;
            float dt = Time.deltaTime;

            for (int i = 0; i < data.Length; ++i)
            {
                var position = data.Position[i].Value;
                var move = data.Input[i].Move;

                position += move * settings.playerMaxSpeed * dt;

                data.Position[i] = new Position2D { Value = position };
            }
        }
    }
}
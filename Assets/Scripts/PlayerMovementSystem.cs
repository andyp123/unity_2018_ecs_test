using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;

namespace DanmakuExample
{
    public class PlayerMovementSystem : ComponentSystem
    {
        public struct Data
        {
            public ComponentDataArray<Position2D> Position;
            public ComponentDataArray<PlayerInput> Input;
            public int Length;
        }

        [Inject] private Data data;

        protected override void OnUpdate ()
        {
            float dt = Time.deltaTime;

            // settings
            float moveSpeed = 5f;
            float fireCooldown = 0.125f;

            for (int i = 0; i < data.Length; ++i)
            {
                var position = data.Position[i].Value;
                var playerInput = data.Input[i];

                position += playerInput.Move * moveSpeed * dt;

                if (playerInput.Fire)
                {
                    playerInput.FireCooldown = fireCooldown;
                }

                data.Position[i] = new Position2D { Value = position };
                data.Input[i] = playerInput;
            }
        }
    }
}
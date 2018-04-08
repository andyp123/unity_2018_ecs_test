using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;

namespace DanmakuExample
{
    public class PlayerInputSystem : ComponentSystem
    {
        struct PlayerData
        {
            public ComponentDataArray<PlayerInput> Input;
            public int Length;
        }

        [Inject] private PlayerData players;

        protected override void OnUpdate ()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < players.Length; ++i)
            {
                UpdatePlayerInput(i, dt);
            }
        }

        private void UpdatePlayerInput (int i, float dt)
        {
            PlayerInput playerInput;

            playerInput.Move.x = Input.GetAxis("Horizontal");
            playerInput.Move.y = Input.GetAxis("Vertical");
            // playerInput.Shoot = Input.GetButton("Fire1");

            // if (math.length(playerInput.Move) > 1f)
            // {
            //     playerInput.Move = math.normalize(playerInput.Move);
            // }

            // playerInput.FireCooldown = Mathf.Max(0f, players.Input[i].FireCooldown - dt);
            // playerInput.Fire = playerInput.Shoot && playerInput.FireCooldown <= 0;

            players.Input[i] = playerInput;
        }
    }
}
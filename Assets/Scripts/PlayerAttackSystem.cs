using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;
using Unity.Collections; // Needed for [Readonly]

namespace DanmakuExample
{
    public class PlayerAttackSystem : ComponentSystem
    {
        public struct Data
        {
            [ReadOnly] public ComponentDataArray<Position2D> Position;
            public ComponentDataArray<PlayerInput> Input;
            public int Length;
        }

        [Inject] private Data data;

        protected override void OnUpdate ()
        {
            DanmakuSettings settings = DanmakuMain.Settings;
            float dt = Time.deltaTime;

            for (int i = 0; i < data.Length; ++i)
            {
                var playerInput = data.Input[i];

                if (playerInput.Fire)
                {
                    playerInput.FireCooldown = settings.playerFireCooldown;

                    PostUpdateCommands.CreateEntity(DanmakuMain.ShotSpawnArchetype);
                    PostUpdateCommands.SetComponent(new ShotSpawnData {
                        Shot = new Shot {
                            Damage = settings.playerShotDamage,
                            Lifetime = settings.playerShotLifetime
                        },
                        Position = new Position2D { Value = data.Position[i].Value },
                        Heading = new Heading2D { Value = new float2(0f, 1f) },
                        Faction = Factions.Player
                        });
                }

                data.Input[i] = playerInput;
            }
        }
    }
}
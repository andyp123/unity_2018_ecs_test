using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Transforms2D;
using Unity.Rendering;

namespace DanmakuExample
{
    public sealed class DanmakuMain
    {
        public static EntityArchetype PlayerArchetype;
        public static EntityArchetype EnemyArchetype;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize ()
        {
            Debug.Log("Initialize");
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            // https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/content/ecs_in_detail.md#entityarchetype
            // Can use either of these. Apparently, the latter is a tiny bit more efficient
            // typeof(MyComponentData)
            // ComponentType.Create<MyComponentData>()
            PlayerArchetype = entityManager.CreateArchetype(
                ComponentType.Create<Position2D>(),
                ComponentType.Create<Heading2D>(),
                ComponentType.Create<PlayerInput>(),
                // ComponentType.Create<Health>(),
                ComponentType.Create<TransformMatrix>()
                );

            EnemyArchetype = entityManager.CreateArchetype(
                // ComponentType.Create<Enemy>(),
                ComponentType.Create<Position2D>(),
                ComponentType.Create<Heading2D>(),
                // ComponentType.Create<Health>(),
                ComponentType.Create<TransformMatrix>()
                );
        }

        public static void NewGame ()
        {
            Debug.Log("NewGame");
            var entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeWithScene ()
        {
            Debug.Log("InitializeWithScene");
        }
    }
}
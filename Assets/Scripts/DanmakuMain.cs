using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Transforms2D;
using Unity.Rendering;

// This is all heavily based on the Unity TwinStickShooter pure ECS example
namespace DanmakuExample
{
    public sealed class DanmakuMain
    {
        // Archetypes
        public static EntityArchetype PlayerArchetype;
        public static EntityArchetype EnemyArchetype;
        public static EntityArchetype ShotSpawnArchetype;

        // Renderers
        public static MeshInstanceRenderer PlayerRenderer;
        public static MeshInstanceRenderer PlayerShotRenderer;
        public static MeshInstanceRenderer EnemyRenderer;
        public static MeshInstanceRenderer EnemyShotRenderer;

        public static DanmakuSettings Settings;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize ()
        {
            Debug.Log("DanmakuMain.Initialize");

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            // https://github.com/Unity-Technologies/EntityComponentSystemSamples/blob/master/Documentation/content/ecs_in_detail.md#entityarchetype
            // Can use either of these. Apparently, the latter is a tiny bit more efficient
            // typeof(MyComponentData)
            // ComponentType.Create<MyComponentData>()
            PlayerArchetype = entityManager.CreateArchetype(
                ComponentType.Create<Player>(),
                ComponentType.Create<Position2D>(),
                ComponentType.Create<Heading2D>(),
                ComponentType.Create<PlayerInput>(),
                ComponentType.Create<Health>(),
                ComponentType.Create<TransformMatrix>()
                );

            EnemyArchetype = entityManager.CreateArchetype(
                ComponentType.Create<Enemy>(),
                ComponentType.Create<Position2D>(),
                ComponentType.Create<Heading2D>(),
                ComponentType.Create<Health>(),
                ComponentType.Create<TransformMatrix>()
                );

            ShotSpawnArchetype = entityManager.CreateArchetype(
                ComponentType.Create<ShotSpawnData>());
        }

        public static void NewGame ()
        {
            Debug.Log("DanmakuMain.NewGame");

            var entityManager = World.Active.GetOrCreateManager<EntityManager>();

            // Create player entity from archetype
            Entity player = entityManager.CreateEntity(PlayerArchetype);

            // Set up components
            entityManager.SetComponentData(player, new Position2D { Value = new float2(0f, 0f) } );
            entityManager.SetComponentData(player, new Heading2D { Value = new float2(0f, 1f) } );
            entityManager.SetComponentData(player, new Health { Value = Settings.playerMaxHealth } );

            // Add a shared renderer component
            entityManager.AddSharedComponentData(player, PlayerRenderer);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeWithScene ()
        {
            Debug.Log("DanmakuMain.InitializeWithScene");

            var go = GameObject.Find("DanmakuSettings");
            Settings = go?.GetComponent<DanmakuSettings>();
            if (Settings == null)
            {
                Debug.Log("Could not find game settings.");
                return;
            }

            // This seems to be the closest thing to prefabs we have right now
            PlayerRenderer = GetRendererFromPrototype("PrototypePlayer");
            PlayerShotRenderer = GetRendererFromPrototype("PrototypePlayerShot");
            EnemyRenderer = GetRendererFromPrototype("PrototypeEnemy");
            EnemyShotRenderer = GetRendererFromPrototype("PrototypeEnemyShot");

        }

        private static MeshInstanceRenderer GetRendererFromPrototype (string name)
        {
            var go = GameObject.Find(name);
            if (go == null)
            {
                Debug.Log(string.Format("Could not find prototype with name '{0}'.", name));
            }
            var renderer = go.GetComponent<MeshInstanceRendererComponent>().Value;
            Object.Destroy(go);
            return renderer;
        }
    }
}
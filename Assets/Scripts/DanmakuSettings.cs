using UnityEngine;

namespace DanmakuExample
{
    public class DanmakuSettings : MonoBehaviour
    {
        [Header("Player")]
        public int playerMaxHealth = 100;
        public float playerMaxSpeed = 10f;
        public float playerFireCooldown = 0.125f;
        public float playerShotLifetime = 5f;
        public float playerShotSpeed = 20f;
        public int playerShotDamage = 10;
        public float playerSize = 1f;

        [Header("Enemy")]
        public int enemyMaxHealth = 100;
        public float enemyMaxSpeed = 5f;
        public float enemyFireCooldown = 0.5f;
        public float enemyShotLifetime = 5f;
        public float enemyShotSpeed = 10f;
        public int enemyShotDamage = 10;
        public float enemySize = 1f;

        [Header("Spawning")]
        public float enemySpawnCooldown = 0.5f;
        public Rect playfield = new Rect { x = -30f, y = -30f, width = 60f, height = 60f };

        // This is here just because settings needs to be in the scene, so it
        // is an easy place to call NewGame from.
        void Start ()
        {
            DanmakuMain.NewGame();
        }
    }
}
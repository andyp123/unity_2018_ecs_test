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

        void Start ()
        {
            DanmakuMain.NewGame();
        }
    }
}
using UnityEngine;

namespace DanmakuExample
{
    public class DanmakuSettings : MonoBehaviour
    {
        [Header("Player")]
        public float playerMaxSpeed = 10f;
        public float playerShotDelay = 0.125f;
        public int playerMaxHealth = 100;

        void Start ()
        {
            DanmakuMain.NewGame();
        }
    }
}
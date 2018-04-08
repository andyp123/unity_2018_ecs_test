using UnityEngine;

namespace DanmakuExample
{
    public class DanmakuSettings : MonoBehaviour
    {
        public int playerMaxHealth = 100;

        void Start ()
        {
            DanmakuMain.NewGame();
        }
    }
}
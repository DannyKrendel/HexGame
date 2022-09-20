using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexGame.Gameplay
{
    public class LevelLoader : MonoBehaviour
    {
        public void LoadLevel(int level)
        {
            SceneManager.LoadScene($"Level {level}");
        }
    }
}
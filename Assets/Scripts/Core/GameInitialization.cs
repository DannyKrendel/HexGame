using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexGame
{
    public class GameInitialization : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
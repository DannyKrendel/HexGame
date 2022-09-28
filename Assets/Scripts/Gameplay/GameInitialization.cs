using PolyternityStuff.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HexGame.Gameplay
{
    public class GameInitialization : MonoBehaviour
    {
        [SerializeField] private SceneReference _startScene;
        
        private void Awake()
        {
            Application.targetFrameRate = 60;
            SceneManager.LoadScene(_startScene.ScenePath);
        }
    }
}
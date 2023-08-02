using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace MobileRpg.System
{
    public static class Bootstrapper
    {
        private const int GameSceneIndex = 0;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            if(SceneManager.GetActiveScene().buildIndex == GameSceneIndex)
                Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("GameBehaviour")));
        }
    }
}

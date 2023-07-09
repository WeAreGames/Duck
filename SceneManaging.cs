using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class SceneManaging : MonoBehaviour
    {
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}

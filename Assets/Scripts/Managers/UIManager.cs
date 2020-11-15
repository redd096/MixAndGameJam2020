namespace redd096
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("redd096/MonoBehaviours/UI Manager")]
    public class UIManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject pauseMenu = default;
        [SerializeField] GameObject endMenu = default;

        [Header("Boss")]
        [SerializeField] Slider healthBoss = default;

        void Start()
        {
            PauseMenu(false);
            EndMenu(false);
        }

        public void PauseMenu(bool active)
        {
            pauseMenu.SetActive(active);
        }

        public void EndMenu(bool active)
        {
            endMenu.SetActive(active);

            if (active)
                PauseMenu(false);
        }

        public void ShowHealthBoss(bool active)
        {
            healthBoss.gameObject.SetActive(active);
        }
    }
}
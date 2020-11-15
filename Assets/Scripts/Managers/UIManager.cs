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

        [Header("Timer")]
        [SerializeField] Text timerText = default;

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
            if(healthBoss != null)
                healthBoss.gameObject.SetActive(active);
        }

        public void SetHealthBoss(float value)
        {
            if (healthBoss != null)
                healthBoss.value = value;
        }

        public void ShowTimerText(bool active)
        {
            if (timerText != null)
                timerText.gameObject.SetActive(active);
        }

        public void SetTimerText(string text)
        {
            if (timerText != null)
                timerText.text = text;
        }
    }
}
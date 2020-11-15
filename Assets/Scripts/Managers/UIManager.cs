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

        [Header("PowerUp")]
        [SerializeField] Image[] imagesForPowerUp = default;

        int indexImageForPowerUp = 0;

        void Start()
        {
            PauseMenu(false);
            EndMenu(false);
            ShowHealthBoss(false);
            ShowTimerText(false);
        }

        #region menu

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

        #endregion

        #region boss health

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

        #endregion

        #region timer

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

        #endregion

        public void AddPowerUp(Sprite powerUp)
        {
            if (imagesForPowerUp == null || imagesForPowerUp.Length <= indexImageForPowerUp || imagesForPowerUp[indexImageForPowerUp] == null)
                return;

            //set image
            imagesForPowerUp[indexImageForPowerUp].sprite = powerUp;

            //increase index
            indexImageForPowerUp++;
        }
    }
}
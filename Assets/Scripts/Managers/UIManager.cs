namespace redd096
{
    using UnityEngine;
    using System.Collections.Generic;
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

        Dictionary<PowerUp, Image> powerUps = new Dictionary<PowerUp, Image>();

        void Start()
        {
            PauseMenu(false);
            EndMenu(false);
            ShowHealthBoss(false);
            ShowTimerText(false);

            foreach (Image image in imagesForPowerUp)
                image.gameObject.SetActive(false);
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

        public void AddPowerUp(PowerUp powerUp)
        {
            if (imagesForPowerUp == null || imagesForPowerUp.Length <= powerUps.Count || imagesForPowerUp[powerUps.Count -1] == null)
                return;

            Image image = imagesForPowerUp[powerUps.Count - 1];

            //set image
            image.sprite = powerUp.SpritePowerUp;
            image.gameObject.SetActive(true);

            //add to dictionary
            powerUps.Add(powerUp, image);
        }

        public void RemovePowerUp(PowerUp powerUp)
        {
            if(powerUps.ContainsKey(powerUp))
            {
                Image image = powerUps[powerUp];

                //remove from dictionary
                powerUps.Remove(powerUp);
                image.gameObject.SetActive(false);
            }    
        }
    }
}
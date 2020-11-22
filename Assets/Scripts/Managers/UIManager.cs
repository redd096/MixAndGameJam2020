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
        Dictionary<PowerUp, int> powerUps = new Dictionary<PowerUp, int>(); //power up + index of imagesForPowerUp

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
            //find first image deactivate
            for(int i = 0; i < imagesForPowerUp.Length; i++)
            {
                if (imagesForPowerUp[i].gameObject.activeInHierarchy == false)
                {
                    Image image = imagesForPowerUp[i];

                    //set sprite and active image
                    image.sprite = powerUp.SpritePowerUp;
                    image.gameObject.SetActive(true);

                    //add to dictionary, with index
                    powerUps.Add(powerUp, i);

                    break;
                }
            }
        }

        public void RemovePowerUp(PowerUp powerUp)
        {
            if (powerUps.ContainsKey(powerUp) == false)
                return;

            //find image in the array, using the index saved in dictionary
            int index = powerUps[powerUp];

            //deactivate image and remove from dictionary
            imagesForPowerUp[index].gameObject.SetActive(false);
            powerUps.Remove(powerUp);

            //from this image, check if next one is active
            for (int i = index + 1; i < imagesForPowerUp.Length; i++)
            {
                //if active, replace image and set again active this image and deactive next one
                if (imagesForPowerUp[i].gameObject.activeInHierarchy)
                {
                    imagesForPowerUp[i - 1].sprite = imagesForPowerUp[i].sprite;

                    //then set active
                    imagesForPowerUp[i - 1].gameObject.SetActive(true);
                    imagesForPowerUp[i].gameObject.SetActive(false);

                    //update dictionary
                    UpdateDictionary(i);
                }
                //if next one is not active, stop replace sprites
                else
                {
                    break;
                }
            }

        }

        void UpdateDictionary(int index)
        {
            Dictionary<PowerUp, int> powerCopy = powerUps.CreateCopy();

            //foreach index in the dictionary
            foreach(PowerUp powerUp in powerCopy.Keys)
            {
                //if found index, update and stop
                if(powerCopy[powerUp] == index)
                {
                    powerUps[powerUp] = index - 1;
                    break;
                }
            }
        }
    }
}
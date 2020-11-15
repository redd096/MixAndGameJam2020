namespace redd096
{
    using UnityEngine;

    [AddComponentMenu("redd096/Singletons/Game Manager")]
    public class GameManager : Singleton<GameManager>
    {
        public UIManager uiManager { get; private set; }
        public Player player { get; private set; }

        protected override void SetDefaults()
        {
            //get references
            uiManager = FindObjectOfType<UIManager>();
            player = FindObjectOfType<Player>();
        }

        public void LoadURL(string url)
        {
            Application.OpenURL("https://eventhorizonschool.itch.io/cube-invaders");
        }

        public void SetVolume(float value)
        {
            AudioListener.volume = value;
        }
    }
}
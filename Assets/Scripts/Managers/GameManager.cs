namespace redd096
{
    using UnityEngine;
    using System.Collections;

    [AddComponentMenu("redd096/Singletons/Game Manager")]
    public class GameManager : Singleton<GameManager>
    {
        public UIManager uiManager { get; private set; }
        public Player player { get; private set; }

        Coroutine moveToNextArena;

        protected override void SetDefaults()
        {
            //get references
            uiManager = FindObjectOfType<UIManager>();
            player = FindObjectOfType<Player>();
        }

        public void LoadURL(string url)
        {
            Application.OpenURL(url);
        }

        public void SetVolume(float value)
        {
            AudioListener.volume = value;
        }

        public void EndGame()
        {
            Time.timeScale = 0;
            uiManager.EndMenu(true);
        }

        public void MoveCamera(ArenaManager nextArena)
        {
            //do only if not already running a coroutine
            if (moveToNextArena == null)
            {
                moveToNextArena = StartCoroutine(MoveToNextArena(nextArena));
            }
        }

        IEnumerator MoveToNextArena(ArenaManager nextArena)
        {
            //get camera and start position
            Transform cam = Camera.main.transform;
            Vector3 startPosition = cam.position;

            //animation camera
            float delta = 0;
            while (delta < 1)
            {
                delta += Time.deltaTime / nextArena.TimeToMoveCamera;
                cam.position = Vector3.Lerp(startPosition, nextArena.CameraPosition.position, delta);

                yield return null;
            }

            moveToNextArena = null;
        }
    }
}
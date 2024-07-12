using Platformer.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI
{
    public class LevelEndUIView : MonoBehaviour, IUIView
    {
        private const string WIN_RESULT = "Game Won";
        private const string LOST_RESULT = "Game Lost";

        private LevelEndUIController controller;
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Sprite WinSprite;
        [SerializeField] private Sprite LoseSprite;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button quitButton;

        private void Start() => SubscribeToButtonClicks();

        private void SubscribeToButtonClicks()
        {
            homeButton.onClick.AddListener(controller.OnHomeButtonClicked);
            quitButton.onClick.AddListener(controller.OnQuitButtonClicked);
        }

        public void SetController(IUIController controllerToSet) => controller = controllerToSet as LevelEndUIController;

        public void DisableView() => gameObject.SetActive(false);

        public void EnableView() => gameObject.SetActive(true);
        
        public void EndGame(GameEndType gameEndType){
            switch (gameEndType)
            {
                case GameEndType.WIN:
                    resultText.SetText(WIN_RESULT);
                    GetComponent<Image>().sprite = WinSprite;
                    break;
                case GameEndType.LOSE:
                    resultText.SetText(LOST_RESULT);
                    GetComponent<Image>().sprite = LoseSprite;
                    break;
                default:
                    break;
            }
        }
    }
}
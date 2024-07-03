using Platformer.Main;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.UI
{
    public class LevelSelectionUIController : IUIController
    {
        private LevelSelectionUIView levelSelectionView;
        private LevelButtonView levelButtonPrefab;
        private List<LevelButtonView> levelButtons;
        
        public LevelSelectionUIController(LevelSelectionUIView levelSelectionView, LevelButtonView levelButtonPrefab)
        {
            InitializeController(levelSelectionView);
            InitializeLevelButtons(levelButtonPrefab);
        }

        public void InitializeController(IUIView iuiView)
        {
            levelSelectionView = iuiView as LevelSelectionUIView;
            levelSelectionView.SetController(this);
        }

        private void InitializeLevelButtons(LevelButtonView levelButtonPrefab)
        {
            this.levelButtonPrefab = levelButtonPrefab;
            levelButtons = new List<LevelButtonView>();
        }
        public void CreateAndShowLevelSelection(int levelCount)
        {
            CreateLevelButtons(levelCount);
            Show();
        }
        public void Show()
        {
            levelSelectionView.EnableView();
        }

        public void Hide()
        {
            levelSelectionView.DisableView();
        }

        private void RemoveLevelButtons()
        {
            levelButtons.ForEach(button => Object.Destroy(button.gameObject));
            levelButtons.Clear();
        }

        public void CreateLevelButtons(int levelCount)
        {
            for (int i = 1; i <= levelCount; i++)
            {
                var newButton = levelSelectionView.AddButton(levelButtonPrefab);
                newButton.SetOwner(this);
                newButton.SetLevelID(i);
            }
        }

        public void OnLevelSelected(int levelId)
        {
            GameService.Instance.EventService.OnLevelSelected.InvokeEvent(levelId);
            RemoveLevelButtons();
            Hide();
        }
    }
}
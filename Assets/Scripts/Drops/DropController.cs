using UnityEngine;
using Platformer.Player;
using System;

namespace Platformer.Drop{
    public class DropController 
    {
        private DropScriptableObject dropScriptableObject;
        private DropView dropView;

        public DropController(DropScriptableObject dropScriptableObject, Transform parentTransform = null)
        {
            this.dropScriptableObject = dropScriptableObject;
            InitializeView(parentTransform);
        }

        private void InitializeView(Transform parentTransform)
        {
            CreateDropView();
            Tuple<Vector3, Quaternion> positionAndRotation = DetermineSpawnPositionAndRotation(parentTransform);
            PositionDropView(positionAndRotation.Item1, positionAndRotation.Item2);
            ConfigureDropView();
        }

        private void CreateDropView() => dropView = UnityEngine.Object.Instantiate(dropScriptableObject.dropView);

        private Tuple<Vector3, Quaternion> DetermineSpawnPositionAndRotation(Transform parentTransform)
        {
            if (parentTransform == null)
                return Tuple.Create(dropScriptableObject.spawnPosition, Quaternion.identity);

            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * dropScriptableObject.dropRadius;
            return Tuple.Create(parentTransform.position + new Vector3(randomCircle.x, randomCircle.y, 0), parentTransform.rotation);
        }

        private void PositionDropView(Vector3 position, Quaternion rotation) => dropView.transform.SetPositionAndRotation(position, rotation);

        private void ConfigureDropView()
        {
            dropView.SetController(this);
            dropView.SetDropSprite(dropScriptableObject.dropType);
        }


        public void PlayerHit(PlayerView playerHit){
            switch (dropScriptableObject.dropType)
            {
                case DropType.Coin:
                    playerHit.CollectCoin(dropScriptableObject.coinValue);
                    break;
            }
        }
    }
}
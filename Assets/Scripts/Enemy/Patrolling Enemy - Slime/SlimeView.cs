using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Enemy{
    public class SlimeView : EnemyView, IDamagable
    {
        private void Update() => (Controller as SlimeController).Update();

        #region Patrol Functionality
        public void Move(Vector3 position, float speed, bool isMovingRight){
            SetCharacterSpriteDirection(isMovingRight);
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
        }

        private void SetCharacterSpriteDirection(bool isMovingRight) {
            if (isMovingRight != (transform.localScale.x < 0))
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        #endregion

        #region IDamagable Implementation
        public void TakeDamage(int damageToInflict) => Controller.TakeDamage(damageToInflict);
        #endregion
    }
}
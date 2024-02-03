using System.Collections.Generic;
using System.Linq;
using Platformer.Events;
using Platformer.Main;
using Platformer.Melee;
using Platformer.Player;
using UnityEngine;

namespace Platformer.Enemy{
    public class MushroomHeadController : EnemyController
    {
        private EventService EventService => GameService.Instance.EventService;

        private readonly MushroomHeadView mushroomHeadView;
        public MushroomHeadState MushroomHeadState { get; private set; }

        #region Patrolling variables
        private Vector3 nextPosition;
        #endregion

        private bool isPlayerAlive = true;

        public MushroomHeadController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            mushroomHeadView = enemyView as MushroomHeadView;
            mushroomHeadView.SetController(this);
            nextPosition = Data.PatrollingPoints[0];
            SetMushroomHeadState(MushroomHeadState.PATROLLING);
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => EventService.OnPlayerDied.AddListener(OnPlayerDied);

        private void UnsubscribeToEvents() => EventService.OnPlayerDied.RemoveListener(OnPlayerDied);

        private void SetMushroomHeadState(MushroomHeadState state) => MushroomHeadState = state;

        public void Update() {
            DetectPlayer();
            HandleStateBehaviour();
        }

        private void DetectPlayer(){
            if (!isPlayerAlive){
                PlayerExitedAttackRadius();
                return; 
            }

            var otherColliders = Physics2D.OverlapCircleAll(enemyView.transform.position, Data.RangeRadius)?.ToList();
            var playerCollider = otherColliders?.Find(x => x.GetComponent<PlayerView>() != null && !x.isTrigger);
            if (playerCollider != null) PlayerEnteredAttackRadius();
            else PlayerExitedAttackRadius();
        }

        private void HandleStateBehaviour(){
            switch (MushroomHeadState)
            {
                case MushroomHeadState.PATROLLING:
                    PatrolBehavior();
                    break;
                case MushroomHeadState.ATTACK:
                    AttackBehavior();
                    break;
            }
        }

        #region Patrol Behaviour
        private void PatrolBehavior() {
            var patrollingPoints = enemyScriptableObject.PatrollingPoints;
            if (Vector3.Distance(enemyView.transform.position, nextPosition) < 0.1f) {
                ToggleNextPatrolPoint(patrollingPoints);
            }
            UpdateMovementTowardsNextPosition();
        }

        private void ToggleNextPatrolPoint(List<Vector3> patrollingPoints) {
            nextPosition = nextPosition == patrollingPoints[0] ? patrollingPoints[1] : patrollingPoints[0];
        }

        private void UpdateMovementTowardsNextPosition() {
            var isMovingRight = nextPosition.x > enemyView.transform.position.x;
            mushroomHeadView.Move(nextPosition, Data.PatrollingSpeed, isMovingRight);
            EnemyMoved();
        }
        #endregion

        #region Attack Behaviour
        private  void AttackBehavior() {
            if(mushroomHeadView.MeleeContainer.childCount == 0){
                EnemyService.PlayAttackAnimation(mushroomHeadView.Animator);
                _ = new MeleeController(Data.MeleeSO, mushroomHeadView.MeleeContainer);
            }
        }

        private void OnPlayerDied(){
            UnsubscribeToEvents();
            isPlayerAlive = false;
        }

        private void PlayerEnteredAttackRadius() => SetMushroomHeadState(MushroomHeadState.ATTACK);

        private void PlayerExitedAttackRadius() => SetMushroomHeadState(MushroomHeadState.PATROLLING);
        #endregion
    }

    public enum MushroomHeadState{
        PATROLLING,
        ATTACK
    }
}
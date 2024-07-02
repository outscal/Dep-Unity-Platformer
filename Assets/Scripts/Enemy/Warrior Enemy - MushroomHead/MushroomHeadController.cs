using System.Collections.Generic;
using System.Linq;
using Platformer.Events;
using Platformer.Main;
using Platformer.Melee;
using Platformer.Player;
using UnityEngine;

namespace Platformer.Enemy
{
    public class MushroomHeadController : EnemyController
    {
        private EventService EventService => GameService.Instance.EventService;
        private PlayerService PlayerService => GameService.Instance.PlayerService;

        private MushroomHeadView mushroomHeadView;
        public EnemyState MushroomHeadState { get; private set; }

        #region Patrolling variables
        private Vector3 nextPosition;
        private List<Vector3> patrollingPoints;
        private int currentPatrolIndex;
        #endregion

        public MushroomHeadController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            InitializeView();
            InitializeVariables(enemyScriptableObject);
            SubscribeToEvents();
        }

        protected override void InitializeView()
        {
            base.InitializeView();
            mushroomHeadView = enemyView as MushroomHeadView;
            mushroomHeadView.SetController(this);
        }

        protected override void InitializeVariables(EnemyScriptableObject enemyScriptableObject)
        {
            base.InitializeVariables(enemyScriptableObject);
            patrollingPoints = enemyScriptableObject.PatrollingPoints;
            currentPatrolIndex = 0;
            nextPosition = patrollingPoints[currentPatrolIndex];
            SetMushroomHeadState(EnemyState.PATROLLING);
        }

        private void SubscribeToEvents() => EventService.OnPlayerDied.AddListener(OnPlayerDied);

        private void UnsubscribeToEvents() => EventService.OnPlayerDied.RemoveListener(OnPlayerDied);

        private void SetMushroomHeadState(EnemyState state) => MushroomHeadState = state;

        public void Update()
        {
            DetectPlayer();
            HandleStateBehaviour();
        }
        private bool CanAttackPlayer()
        {
            return PlayerService.PlayerController != null && PlayerService.PlayerController.CurrentHealth > 0;
        }

        private void DetectPlayer()
        {
            if (!CanAttackPlayer())
            {
                PlayerExitedAttackRadius();
                return;
            }

            if (IsPlayerInAttackRadius())
            {
                PlayerEnteredAttackRadius();
            }
            else
            {
                PlayerExitedAttackRadius();
            }
        }

        private bool IsPlayerInAttackRadius()
        {
            var otherColliders = Physics2D.OverlapCircleAll(enemyView.transform.position, Data.AttackRangeRadius);
            return otherColliders?.Any(collider => collider.GetComponent<PlayerView>() != null) ?? false;
        }

        private void HandleStateBehaviour()
        {
            switch (MushroomHeadState)
            {
                case EnemyState.PATROLLING:
                    PatrolBehavior();
                    break;
                case EnemyState.ATTACK:
                    AttackBehavior();
                    break;
            }
        }

        #region Patrol Behaviour
        private void PatrolBehavior()
        {
            if (Vector3.Distance(enemyView.transform.position, nextPosition) < 0.1f)
            {
                ToggleNextPatrolPoint();
            }
            UpdateMovementTowardsNextPosition();
        }

        private void ToggleNextPatrolPoint()
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrollingPoints.Count;
            nextPosition = patrollingPoints[currentPatrolIndex];
        }

        private void UpdateMovementTowardsNextPosition()
        {
            var isMovingRight = nextPosition.x > enemyView.transform.position.x;
            mushroomHeadView.Move(nextPosition, Data.PatrollingSpeed, isMovingRight);
            EnemyMoved();
        }
        #endregion

        #region Attack Behaviour
        private void AttackBehavior()
        {
            if (mushroomHeadView.MeleeContainer.childCount == 1)
            {
                EnemyService.PlayAttackAnimation(mushroomHeadView.Animator);
                _ = new MeleeController(Data.MeleeSO, mushroomHeadView.transform);
            }
        }

        private void OnPlayerDied()
        {
            UnsubscribeToEvents();
        }

        private void PlayerEnteredAttackRadius() => SetMushroomHeadState(EnemyState.ATTACK);

        private void PlayerExitedAttackRadius() => SetMushroomHeadState(EnemyState.PATROLLING);
        #endregion
    }

    
}
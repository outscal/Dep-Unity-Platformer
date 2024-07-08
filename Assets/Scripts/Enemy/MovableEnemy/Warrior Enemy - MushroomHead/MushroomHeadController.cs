using System.Collections.Generic;
using System.Linq;
using Platformer.Events;
using Platformer.Main;
using Platformer.Melee;
using Platformer.Player;
using UnityEngine;

namespace Platformer.Enemy
{
    public class MushroomHeadController : MovableEnemyController
    {
        private EventService EventService => GameService.Instance.EventService;
        private PlayerService PlayerService => GameService.Instance.PlayerService;
        public EnemyState MushroomHeadState { get; private set; }

        public MushroomHeadController(MovableEnemyScriptableObject enemyScriptableObject, EnemySpawnData spawnData) : base(enemyScriptableObject, spawnData)
        {
            InitializeVariables(enemyScriptableObject);
            SubscribeToEvents();
        }

        protected override void InitializeVariables(EnemyScriptableObject enemyScriptableObject)
        {
            base.InitializeVariables(enemyScriptableObject);
            SetMushroomHeadState(EnemyState.PATROLLING);
        }

        private void SubscribeToEvents() => EventService.OnPlayerDied.AddListener(OnPlayerDied);

        private void UnsubscribeToEvents() => EventService.OnPlayerDied.RemoveListener(OnPlayerDied);

        private void SetMushroomHeadState(EnemyState state) => MushroomHeadState = state;

        public override void Update()
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
        
        #region Attack Behaviour
        private void AttackBehavior()
        {
            EnemyService.PlayAttackAnimation(enemyView.Animator);
            _ = new MeleeController(Data.MeleeSO);    
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
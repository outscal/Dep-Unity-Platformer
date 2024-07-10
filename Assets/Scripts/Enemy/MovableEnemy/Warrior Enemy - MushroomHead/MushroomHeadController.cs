using System.Collections.Generic;
using System.Linq;
using Platformer.AnimationSystem;
using Platformer.Events;
using Platformer.Main;
using Platformer.Player;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Enemy
{
    public class MushroomHeadController : MovableEnemyController, IDamagable
    {
        private EventService EventService => GameService.Instance.EventService;
        private PlayerService PlayerService => GameService.Instance.PlayerService;
        public EnemyState MushroomHeadState { get; private set; }
        private AnimationService animationService;
        private Collider2D playerCollider;
        public MushroomHeadController(MovableEnemyScriptableObject enemyScriptableObject, EnemySpawnData spawnData) : base(enemyScriptableObject, spawnData)
        {
            InitializeVariables(enemyScriptableObject);
            InitializeView(spawnData);           
            SubscribeToEvents();
        }

        protected override void InitializeVariables(EnemyScriptableObject enemyScriptableObject)
        {
            base.InitializeVariables(enemyScriptableObject);
            SetMushroomHeadState(EnemyState.PATROLLING);
            animationService = GameService.Instance.AnimationService;
        }
        
        protected override void InitializeView(EnemySpawnData spawnData)
        {
            base.InitializeView(spawnData);
            enemyView.SetController(this);
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

            if (IsPlayerInAttackRadius(out playerCollider))
            {
                if(MushroomHeadState != EnemyState.ATTACKING )
                    PlayerEnteredAttackRadius();
            }
            else 
            {
                PlayerExitedAttackRadius();
            }
        }

        public bool IsPlayerInAttackRadius(out Collider2D playerCollider)
        {
            var otherColliders = Physics2D.OverlapCircleAll(enemyView.transform.position, Data.AttackRangeRadius);
            playerCollider = otherColliders.FirstOrDefault(collider => collider.GetComponent<PlayerView>() != null);
            this.playerCollider = playerCollider;
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
                    AttackBehavior(playerCollider);
                    break;
                case EnemyState.ATTACKING:
                    break;
            }
        }
        
        #region Attack Behaviour
        private void AttackBehavior(Collider2D other)
        {
            if (MushroomHeadState == EnemyState.ATTACKING)
                return;
            Debug.Log("sgag");
            SetMushroomHeadState(EnemyState.ATTACKING);
            enemyCombatController.StartAttackCoroutine(other);
            //animationService.PlayTriggerAnimation(enemyView.Animator, EnemyTriggerAnimationType.Attack.ToString());
            //EnemyService.PlayAttackAnimation(EnemyView.Animator);
        }

        //public override void OnCollisionWithPlayer(Collider2D other)
        //{
        //    enemyCombatController.StartAttackCoroutine(other);
        //}

        private void OnPlayerDied()
        {
            UnsubscribeToEvents();
        }

        private void PlayerEnteredAttackRadius()
        {
            Debug.Log("attack state");
            SetMushroomHeadState(EnemyState.ATTACK);
        }

        private void PlayerExitedAttackRadius()
        {
            if (MushroomHeadState == EnemyState.ATTACKING)
                enemyCombatController.StopAttackCoroutine();
            
            SetMushroomHeadState(EnemyState.PATROLLING);
        }

        public override void OnEnemyAttack(Collider2D other)
        {
            animationService.PlayTriggerAnimation(enemyView.Animator, EnemyTriggerAnimationType.Attack.ToString());
            base.OnEnemyAttack(other);
        }
        public override void OnCollisionWithPlayer(Collider2D other)
        {
            
        }
        #endregion
    }

    
}
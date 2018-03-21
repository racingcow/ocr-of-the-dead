using System;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using Assets.Scripts;
using Racingcow.OcrOfTheDead.Enums;

namespace Racingcow.OcrOfTheDead.Views
{
    public class EnemyView : View
    {
        public Signal clawSignal = new Signal();

        public float attackDistance = 3f;

        private UnityEngine.AI.NavMeshAgent _nav;
        private Transform _player;
        private Animator _animator;
        private ReticleView _reticle; // TODO: should I be here?
        
        private const float WALK_ANIMATION_LENGTH = 1.29f;

        public bool dying = false;
        public bool dead = false;
        public float attackSpeed = 0.5f;
        public int damage = 10;
        public int waypointSequence;

        //public OcrWord OcrWord { get; set; }
        public string WaypointName { get; set; }

        public void Persue()
        {
            Debug.Log(string.Format("Enemy '{0}' just got aggro'd", name));
            _nav.enabled = true;
        }

        public void Die()
        {
            //OcrWord.Word.Text = suggestedWord;
            //Words.AddCorrection(OcrWord);

            dying = true;

            _animator.SetInteger("State", (int)EnemyStates.Die);
            _animator.SetInteger("FallDirection", UnityEngine.Random.Range(0, 3));

            _nav.enabled = false;
            //_reticle.TargetNextEnemy();
        }

        protected override void Awake()
        {
            base.Awake();

            _nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
            _nav.enabled = false;

            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _animator = GetComponent<Animator>();
            _reticle = FindObjectOfType<ReticleView>(); //TODO: Don't globally find me!
            UnityEngine.Random.seed = DateTime.Now.Millisecond;

            var walkOffset = UnityEngine.Random.value*WALK_ANIMATION_LENGTH;
            _animator.SetFloat("WalkOffset", walkOffset);
            _animator.SetFloat("AttackSpeed", attackSpeed);
        }

        void Update()
        {
            if (dead || dying) return;

            if (_nav.enabled) _nav.SetDestination(_player.position);

            var playerPos = FindObjectOfType<Camera>().transform.position;
            var zombiePos = transform.position;
            var curDistance = Vector3.Distance(playerPos, zombiePos);

            var state = (int) EnemyStates.Wait;
            if (_nav.enabled)
            {
                if (curDistance <= attackDistance)
                {
                    state = (int) EnemyStates.Attack;
                }
                else
                {
                    state = (int) EnemyStates.Walk;
                }
            }
            //Debug.Log(string.Format("curDistance = {0}, attackDistance = {1}, state = {2}", curDistance, attackDistance, state));

            _animator.SetInteger("State", state);
        }

        public void OnAttack()
        {
            Debug.Log("Enemy view raising claw signal");
            clawSignal.Dispatch();            
        }

        public void OnDead()
        {
            //Debug.Log(string.Format("{0} is dead", name));
            dying = false;
            dead = true;
        }
    }
}

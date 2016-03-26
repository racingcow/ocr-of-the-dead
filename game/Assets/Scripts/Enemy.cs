﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        enum States
        {
            Wait = 0,
            Walk = 1,
            Attack = 2,
            Dying = 3,
            Dead = 4
        }

        public float attackDistance = 3f;

        private NavMeshAgent _nav;
        private Transform _player;
        private Animator _animator;
        private PlayerAttackBehavior _playerAttack;
        
        private const float WALK_ANIMATION_LENGTH = 1.29f;

        public bool dying = false;
        public bool dead = false;
        public float attackSpeed = 0.5f;
        public int damage = 10;
        
        public OcrWord OcrWord { get; set; }

        public void Die(string suggestedWord)
        {
            OcrWord.Word.Text = suggestedWord;
            Words.AddCorrection(OcrWord);

            dying = true;

            _animator.SetInteger("State", (int)States.Dying);
            _animator.SetInteger("FallDirection", UnityEngine.Random.Range(0, 3));

            _nav.enabled = false;
            _playerAttack.TargetNextEnemy();
        }

        void Awake()
        {
            _nav = GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _animator = GetComponent<Animator>();
            _playerAttack = FindObjectOfType<PlayerAttackBehavior>();
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

            var state = (int) (curDistance <= attackDistance ? States.Attack : States.Walk);
            //Debug.Log(string.Format("curDistance = {0}, attackDistance = {1}, state = {2}", curDistance, attackDistance, state));

            _animator.SetInteger("State", state);
        }

        public void OnAttack()
        {
            var oldHealth = PlayerHealth.Instance.Health;
            var newHealth = oldHealth - damage;
            //Debug.Log(string.Format("{0} attacked! Health went from {1} to {2}", name, oldHealth, newHealth));
            PlayerHealth.Instance.Health = newHealth;

            // show some red tint when health is low
            var blood = GameObject.FindGameObjectWithTag("Blood");
            var img = blood.GetComponent<RawImage>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, Math.Max((1f - (newHealth / 100f)) - .3f, 0f));
        }

        public void OnDead()
        {
            //Debug.Log(string.Format("{0} is dead", name));
            dying = false;
            dead = true;
        }
    }
}

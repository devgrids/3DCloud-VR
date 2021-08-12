using System;
using UnityEngine;


namespace Player
{
    public class PlayerAnimation : MonoBehaviour, IPlayerAnimation
    {
        [SerializeField] private Animator _animator;

        public Animator Animator { get => _animator; set => _animator = value; }

        private void Update()
        {
            Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        public void Move(float x, float y)
        {
            Animator.SetFloat("velocityX", x);
            Animator.SetFloat("velocityY", y);
        }
    }
}
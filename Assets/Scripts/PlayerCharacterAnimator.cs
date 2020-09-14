﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;

    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jumping";
    const string FallState = "Falling";
    const string SprintState = "Sprinting";
    const string CastState = "Casting";

    Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartRunning += OnStartRunning;
        _thirdPersonMovement.Jumping += OnJumping;
        _thirdPersonMovement.Falling += OnFalling;
        _thirdPersonMovement.Sprinting += OnSprinting;
        _thirdPersonMovement.Casting += OnCasting;
    }

    private void OnDisable()
    {
        _thirdPersonMovement.Idle -= OnIdle;
        _thirdPersonMovement.StartRunning -= OnStartRunning;
        _thirdPersonMovement.Jumping -= OnJumping;
        _thirdPersonMovement.Falling -= OnFalling;
        _thirdPersonMovement.Sprinting -= OnSprinting;
        _thirdPersonMovement.Casting -= OnCasting;
    }

    public void OnIdle()
    {
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }

    public void OnCasting()
    {
        _animator.CrossFadeInFixedTime(CastState, .2f);
    }

    public void OnSprinting()
    {
        _animator.CrossFadeInFixedTime(SprintState, .2f);
    }

    public void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);
    }

    public void OnJumping()
    {
        _animator.CrossFadeInFixedTime(JumpState, .2f);
    }

    public void OnFalling()
    {
        _animator.CrossFadeInFixedTime(FallState, .2f);
    }
}

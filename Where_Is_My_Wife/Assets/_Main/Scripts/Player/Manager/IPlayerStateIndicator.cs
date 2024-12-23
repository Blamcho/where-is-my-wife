﻿using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public interface IPlayerStateIndicator
    {
        public bool IsDead { get; }
        public bool IsAccelerating { get; }
        public bool IsRunningRight { get; }
        public bool IsLookingRight { get; }
        public bool IsLookingDown { get; }
        public bool IsJumping { get; }
        public bool IsJumpCut { get; }
        public bool IsJumpFalling { get; }
        public bool IsOnWallHang { get; }
        public bool IsRunFalling { get; }
        public bool IsInHookRange { get; }
        public float DashSpeed { get; }
        public bool IsInQTEWindow { get; }
        public Vector2 HookPosition { get; }
        public Vector2 HookLaunchImpulse { get; }
        public bool IsOnJumpInputBuffer();
        public bool IsFastFalling();
        public bool IsOnGround();
        public bool IsInJumpHang();
        public bool IsIdling();
        public bool CanJump();
        public bool CanJumpCut();
    }
}

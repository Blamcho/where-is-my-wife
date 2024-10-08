﻿using System;
using UnityEngine;

namespace WhereIsMyWife.Controllers
{
    public interface IRespawn
    {
        public void SetRespawnPoint(Vector3 respawnPoint);
        public void TriggerRespawn();
        public Action<Vector3> RespawnAction { get; set; }
    }
}
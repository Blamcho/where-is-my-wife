using System;
using UnityEngine;

namespace WhereIsMyWife.Controllers
{
    public interface IRespawn
    {
        public void SetRespawnPoint(Vector3 respawnPoint);
        public void TriggerRespawnStart();
        public void TriggerDeath();
        public Action DeathAction { get; set; }
        public Action<Vector3> RespawnStartAction { get; set; }
        public Action RespawnCompleteAction { get; set; }
    }
}
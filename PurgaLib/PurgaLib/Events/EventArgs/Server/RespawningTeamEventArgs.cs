using System.Collections.Generic;
using PurgaLib.Events.EventSystem.Interfaces;
using PlayerRoles;
using Respawning.Waves;

namespace PurgaLib.Events.EventArgs.Server
{
    public class RespawningTeamEventArgs : IEventArgs
    {
        public bool IsAllowed { get; set; } = true;

        /// <summary>
        /// Gets or sets the list of players eligible to respawn.
        /// </summary>
        public List<PurgaLib.API.Features.Player> Players { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of players that will respawn.
        /// </summary>
        public int MaximumRespawnAmount { get; set; }

        /// <summary>
        /// Gets or sets the wave that is spawning.
        /// </summary>
        public SpawnableWaveBase Wave { get; set; }

        /// <summary>
        /// Gets or sets the spawn queue of roles.
        /// </summary>
        public Queue<RoleTypeId> SpawnQueue { get; set; }

        public RespawningTeamEventArgs(List<PurgaLib.API.Features.Player> players, int maxWaveSize, SpawnableWaveBase wave, Queue<RoleTypeId> spawnQueue)
        {
            Players = players;
            MaximumRespawnAmount = maxWaveSize;
            Wave = wave;
            SpawnQueue = spawnQueue;
        }
    }
}
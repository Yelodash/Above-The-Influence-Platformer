using Audio;
using UnityEngine;

namespace Npc_s
{
    public class NpcDialogue
    {
        private readonly NpcManager npcManager;

        public NpcDialogue(NpcManager npcManager)
        {
            this.npcManager = npcManager;
        }

        /// <summary>
        ///  Plays a random sound when the NPC collides with the player.
        /// </summary>
        public void Dialogue()
        {
            if (!npcManager.gameObject.CompareTag("Player"))
            {
                AudioManager.Instance.PlaySoundAtPoint(Random.Range(71, 74), npcManager.gameObject.transform.position);
            }
        }
    }
}
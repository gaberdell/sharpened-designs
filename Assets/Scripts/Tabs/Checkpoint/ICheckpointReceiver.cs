using UnityEngine;

namespace Sharp.Tabs.Checkpoint
{
    /// <summary>
    /// Implementation of the 'Checkpoint' tab. 
    /// 
    /// Uses a ListView element to manage checkpoint objects.
    /// </summary>
    public interface ICheckpointReceiver
    {
        /// <summary>
        /// 
        /// </summary>
        void OnCheckpointStart(GameObject checkpoint);
    }
}

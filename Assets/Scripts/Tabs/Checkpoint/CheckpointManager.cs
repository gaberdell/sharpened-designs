using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sharp
{
    /// <summary>
    /// This class stores the data related to checkpoint objects in scene.
    /// 
    /// Handles communication between individual checkpoint objects 
    /// and checkpoint tab ui.
    /// </summary>
    public static class CheckpointManager
    {
        /// <summary>
        /// A list of all checkpoints in the scene.
        /// Can add or remove from list but cannot set to a new list 
        /// </summary>
        private static readonly List<Checkpoint> _checkpoints = new();
        /// <summary>
        /// The same list as above but with more limited access
        /// Cannot add or remove from the public version, 
        /// only read/access the data
        /// </summary>
        public static IReadOnlyList<Checkpoint> checkpoints => _checkpoints;

        /// <summary>
        /// A delegate that allows for the UI tab to be 
        /// updated whenever a checkpoint is added or removed
        /// </summary>
        public static event Action changed;

        /// <summary>
        /// The function to add a newly created checkpoint
        /// object to the list if and only if it does not 
        /// currently exist in the list
        /// </summary>
        public static void Register(Checkpoint c)
        {
            if (_checkpoints.Contains(c)) return;

            _checkpoints.Add(c);
            changed?.Invoke();
        }

        /// <summary>
        /// The function to remove a checkpoint object from the list
        /// </summary>
        public static void Unregister(Checkpoint c)
        {
            if (_checkpoints.Remove(c))
            {
                changed?.Invoke();
            }
        }

        /// <summary>
        /// The function to clear out the list of checkpoints
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Reset()
        {
            _checkpoints.Clear();
            changed = null;
        }
    }
}

using UnityEngine;

namespace Sharp.Tabs.Checkpoint
{
    /// <summary>
    /// Implementation for a singular checkpoint.
    /// Stores all necessary information and communicates with CheckpointManager.
    /// </summary>
    public class Checkpoint : MonoBehaviour
    {
        /// <summary>
        /// The name for the checkpoint that 
        /// the user wants to see in UI tab
        /// </summary>
        [SerializeField] private string _label = "Checkpoint 1";
        /// <summary>
        /// The description for the checkpoint that 
        /// the user wants to see in UI tab
        /// </summary>
        [SerializeField, TextArea] private string _description = "The test checkpoint";

        /// <summary>
        /// Returns the checkpoint name
        /// </summary>
        public string label => _label;
        /// <summary>
        /// Returns the checkpoint description
        /// </summary>
        public string description => _description;
        /// <summary>
        /// Returns the checkpoint position
        /// </summary>
        public Vector3 position => transform.position;
        /// <summary>
        /// Returns the checkpoint rotation
        /// </summary>
        public Quaternion rotation => transform.rotation;

        /// <summary>
        /// Used to update the label and description if dynamially creating checkpoints at runtime.
        /// If creating a checkpoint in the editor, use the inspector to set these values.
        /// </summary>
        public void Init(string label, string description)
        {
            _label = label;
            _description = description;
        }

        /// <summary>
        /// Adding the checkpoint to the overall manager list
        /// </summary>
        private void OnEnable() => CheckpointManager.Register(this);
        /// <summary>
        /// Removing the checkpoint from the overall manager list
        /// </summary>
        private void OnDisable() => CheckpointManager.Unregister(this);
    }
}

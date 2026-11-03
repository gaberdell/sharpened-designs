using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sharp.Tabs.Checkpoint
{
    /// <summary>
    /// Implementation of the 'Checkpoint' tab. 
    /// 
    /// Uses a ListView element to manage checkpoint objects.
    /// </summary>
    public class CheckpointTab : AbstractTab
    {
        /// <summary>
        /// The list view for all the checkpoint rows
        /// </summary>
        private ListView _list;

        [Header("Checkpoint Tab Settings")]
        /// <summary>
        /// UXML template for one row for one checkpoint
        /// containing checkpoint name + description 
        /// and ability to jump to checkpoint
        /// </summary>
        [SerializeField] private VisualTreeAsset _row_uxml;

        private void OnCheckpointClick(ClickEvent evt)
        {
            // In _list.bindItem (initialized in OnInstantiateVisualElement), we ensure that 
            // each button has its user data set to its associated checkpoint
            var button = evt.currentTarget as Button;
            var cp = button.userData as Checkpoint;

            JumpTo(cp);
        }

        protected override void OnInstantiateVisualElement()
        {
            _list = root_ui_element.Q<ListView>("checkpoint-list");
            _list.itemsSource = (IList)CheckpointManager.checkpoints;
            _list.fixedItemHeight = 48;
            _list.selectionType = SelectionType.None;

            _list.makeItem = () => _row_uxml.Instantiate();
            _list.bindItem = (element, i) =>
            {
                var cp = CheckpointManager.checkpoints[i];
                element.Q<Label>("description").text = cp.description;

                var btn = element.Q<Button>("jump");
                btn.text = cp.label;
                btn.RegisterCallback<ClickEvent>(OnCheckpointClick, CallbackOptions.Removable);
                btn.userData = cp;
            };
            _list.unbindItem = (element, i) =>
            {
                var btn = element.Q<Button>("jump");
                btn.UnregisterAllRemovableCallbacks();
            };

            CheckpointManager.changed += Refresh;
            Refresh();
        }

        /// <summary>
        /// Tells the list to look at the item source again
        /// </summary>
        private void Refresh() => _list?.RefreshItems();

        /// <summary>
        /// If tab is destroyed, no need for the manager to
        /// keep calling based on changes
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            CheckpointManager.changed -= Refresh;
        }

        /// <summary>
        /// Moves the player to the checkpoint and matches rotation
        /// </summary>
        private void JumpTo(Checkpoint cp)
        {
            var player = GameObject.FindWithTag("Player").transform;

            var cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;

            player.SetPositionAndRotation(cp.position, cp.rotation);

            if (player.TryGetComponent<ICheckpointReceiver>(out var icr))
            {
                icr.OnCheckpointStart(cp.gameObject);
            }

            if (player.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            if (cc) cc.enabled = true;
        }
    }
}

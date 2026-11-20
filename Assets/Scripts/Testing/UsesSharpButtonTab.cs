using UnityEngine;
using Sharp.Tabs.ObjectButton;

namespace Sharp.Testing
{
    /// <summary>
    /// Used to test if ObjectButton works
    /// </summary>
    public class UsesSharpButtonTab : MonoBehaviour
    {
        [SharpButton]
        public void TestMessage() {
            Debug.Log("Print Message!");
        }
    }
}

using Sharp.Core;
using System.Collections;
using UnityEngine;

namespace Sharp.Testing
{
    /// <summary>
    /// Simple testing script which logs repeatedly.
    /// Useful for testing the 'Console' window.
    /// </summary>
    public class PeriodicLogger : BaseSharpBehaviour
    {
        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(LogCoroutine());
        }

        private IEnumerator LogCoroutine()
        {
            while(true)
            {
                Debug.Log("Something has happened!");
                yield return new WaitForSeconds(2);
                Debug.LogError("Something worse has happened!");
                Debug.Assert(false, "Something is false!");
                yield return new WaitForSeconds(2);
            }
        }
    }
}

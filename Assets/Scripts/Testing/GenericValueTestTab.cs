using Sharp.Core.UXML;
using UnityEngine;

namespace Sharp.Testing
{
    public class GenericValueTestTab : AbstractTab
    {
        private object[] tests;

        protected override void Awake()
        {
            tests = new object[]
            {
                1, 1u, 1L, 1Lu, 1.0f, 1.0d,
                new Vector2(1, 2), new Vector3(1, 2, 3), new Vector4(1, 2, 3, 4),
                new Vector2Int(1, 2), new Vector3Int(1, 2, 3),
                true,
                "Hello, world!",
                new object[] { 1, 1.0f, "Example", new object[] { "Example 2" } },
                this,
                gameObject
            };

            base.Awake();
        }

        protected override void OnInstantiateVisualElement()
        {
            foreach (var test in tests)
            {
                var test_ui = new GenericValueVisualElement();
                root_ui_element.Add(test_ui);

                test_ui.SetValue(test);
            }
        }
    }
}

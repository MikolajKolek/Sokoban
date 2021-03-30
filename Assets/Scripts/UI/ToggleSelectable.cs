using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    /// <summary>
    /// This class inherits from <see cref="Toggle"/> and adds the ability to navigate around a <see cref="RectTransform"/> with a <see cref="ScrollRect"/> containing many <see cref="Toggle"/> elements using arrow keys.
    /// </summary>
    public class ToggleSelectable : Toggle {
        [SerializeField] public ScrollRect levelSelectionScroll;
        [SerializeField] public RectTransform levelSelectionContentRect;

        /// <summary>
        /// Called when the <see cref="ToggleSelectable"/> is selected with arrow keys. Sets the toggle to on and calls <see cref="ScrollToToggle"/>.
        /// </summary>
        public override void OnSelect(BaseEventData eventData)
        {
            isOn = true;
            StartCoroutine(ScrollToToggle(this, levelSelectionScroll, levelSelectionContentRect));
            
            base.OnSelect(eventData);
        }

        /// <summary>
        /// A coroutine that smoothly scrolls to the <c>toggle</c> in the <c>levelSelectionContentRect</c> using the <c>levelSelectionScroll</c>.
        /// </summary>
        /// <param name="toggle">The <see cref="Toggle"/> that is scrolled to</param>
        /// <param name="levelSelectionScroll">The <see cref="ScrollRect"/> that is used to scroll the <c>toggle</c> into view</param>
        /// <param name="levelSelectionContentRect">The <see cref="RectTransform"/> containing the <c>toggle</c></param>
        private static IEnumerator ScrollToToggle(Component toggle, ScrollRect levelSelectionScroll, RectTransform levelSelectionContentRect) {
            var scrollRect = levelSelectionScroll;
            var contentRect = levelSelectionContentRect;
            
            var scrollViewHeight = contentRect.rect.height;
            var scrollCount = 25 / scrollViewHeight / 20;
            
            while (toggle.transform.position.y <= 72.5f) {
                scrollRect.verticalNormalizedPosition -= scrollCount;
                yield return null;
            }
            
            while (toggle.transform.position.y >= Screen.height - 27.5f) {
                scrollRect.verticalNormalizedPosition += scrollCount;
                yield return null;
            }
        }
    }
}
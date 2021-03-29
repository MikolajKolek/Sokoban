using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    public class ToggleSelectable : Toggle {
        [SerializeField] public ScrollRect levelSelectionScroll;
        [SerializeField] public RectTransform levelSelectionContentRect;

        public override void OnSelect(BaseEventData eventData)
        {
            isOn = true;
            StartCoroutine(ScrollToToggle(this, levelSelectionScroll, levelSelectionContentRect));
            
            base.OnSelect(eventData);
        }
        
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
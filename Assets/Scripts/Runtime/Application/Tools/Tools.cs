using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Runtime.Tools
{
    public class Tools
    {
        public static void Shuffle<T>(List<T> list) where T : class
        {
            var count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var item = list[i];
                var randomIndex = UnityEngine.Random.Range(0, count);
                list[i] = list[randomIndex];
                list[randomIndex] = item;
            }
        }

        public static void Shuffle<T>(Stack<T> stack) where T : class
        {
            var list = stack.ToList();
            Shuffle(list);
            stack.Clear();
            foreach (var item in list)
                stack.Push(item);
        }

        public static bool IsPointerOverUIElement()
        {
            List<RaycastResult> eventSystemRaysastResults = GetEventSystemRaycastResults();

            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }
            return false;
        }

        private static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}
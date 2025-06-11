using System;
using System.Collections.Generic;
using UnityEngine;


namespace ODON.GameManager
{
    public static class Manager_outline
    {
        private static Dictionary<GameObject, Outline> objectStates = new();

        public static void EnableOutline(GameObject obj)
        {
            if (objectStates.TryGetValue(obj, out var outline))
            {
                outline.enabled = true;
            }
            else
            {
                Debug.LogWarning($"Outline not found for {obj.name}. Please add an outline first.");
            }
        }

        public static void DisableOutline(GameObject obj)
        {
            if (objectStates.TryGetValue(obj, out var outline))
            {
                outline.enabled = false;
            }
            else
            {
                Debug.LogWarning($"Outline not found for {obj.name}. Please add an outline first.");
            }
        }

        public static void AddOutline(GameObject obj)
        {
            if (!obj.TryGetComponent<Outline>(out var outline))
            {
                Debug.Log($"Adding outline to {obj.name}");
                outline = obj.AddComponent<Outline>();
            }

            outline.enabled = false;

            if (!objectStates.ContainsKey(obj))
            {
                objectStates.Add(obj, outline);
            }
            else
            {
                Debug.LogWarning($"Outline already exists for {obj.name}. Updating properties.");
            }
        }
        
        public static void RemoveOutline(GameObject obj)
        {
            if (objectStates.ContainsKey(obj))
            {
                objectStates.Remove(obj);
            }
        }
    }
}
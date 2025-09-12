using System;
using System.Collections.Generic;
using UnityEngine;

namespace ODON.GameManager
{
    /// <summary>
    /// Static manager class for handling Outline components on GameObjects in the ODON application.
    /// Provides methods to add, enable, disable, and remove outlines, and tracks outline states for objects.
    /// </summary>
    public static class Manager_outline
    {
        /// <summary>
        /// Dictionary mapping GameObjects to their Outline components.
        /// </summary>
        private static Dictionary<GameObject, Outline> objectStates = new();

        /// <summary>
        /// Enables the Outline component for the specified GameObject.
        /// </summary>
        /// <param name="obj">The GameObject whose outline should be enabled.</param>
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

        /// <summary>
        /// Disables the Outline component for the specified GameObject.
        /// </summary>
        /// <param name="obj">The GameObject whose outline should be disabled.</param>
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

        /// <summary>
        /// Adds an Outline component to the specified GameObject and tracks it.
        /// If the GameObject already has an Outline, it is reused.
        /// </summary>
        /// <param name="obj">The GameObject to add an outline to.</param>
        public static void AddOutline(GameObject obj)
        {
            if (!obj.TryGetComponent<Outline>(out var outline))
            {
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
        
        /// <summary>
        /// Removes the Outline component tracking for the specified GameObject.
        /// </summary>
        /// <param name="obj">The GameObject to remove outline tracking from.</param>
        public static void RemoveOutline(GameObject obj)
        {
            if (objectStates.ContainsKey(obj))
            {
                objectStates.Remove(obj);
            }
        }
    }
}
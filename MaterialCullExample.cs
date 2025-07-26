using UnityEngine;
using UnityEngine.Rendering;

namespace ModUtils.Examples
{
    /// <summary>
    /// Example script demonstrating how to use MaterialCullHelper.
    /// This can be attached to a GameObject to test the cull mode functionality.
    /// </summary>
    public class MaterialCullExample : MonoBehaviour
    {
        [Header("Cull Mode Controls")]
        [SerializeField] private bool setCullBack = false;
        [SerializeField] private bool setCullFront = false;
        [SerializeField] private bool setCullOff = false;
        
        [Header("Target Selection")]
        [SerializeField] private bool applyToThisObject = true;
        [SerializeField] private bool applyToChildren = false;
        [SerializeField] private GameObject[] additionalTargets;

        [Header("Information")]
        [SerializeField] private string currentCullMode = "Unknown";

        private void Start()
        {
            UpdateCullModeDisplay();
        }

        private void OnValidate()
        {
            // Ensure only one option is selected at a time
            int selectedCount = (setCullBack ? 1 : 0) + (setCullFront ? 1 : 0) + (setCullOff ? 1 : 0);
            
            if (selectedCount > 1)
            {
                // Reset all if multiple are selected
                setCullBack = setCullFront = setCullOff = false;
            }
            else if (selectedCount == 1)
            {
                ApplyCullMode();
            }

            UpdateCullModeDisplay();
        }

        private void ApplyCullMode()
        {
            CullMode targetMode;
            
            if (setCullBack)
                targetMode = CullMode.Back;
            else if (setCullFront)
                targetMode = CullMode.Front;
            else if (setCullOff)
                targetMode = CullMode.Off;
            else
                return;

            Debug.Log($"Applying cull mode: {targetMode}");

            int totalModified = 0;

            // Apply to this object
            if (applyToThisObject)
            {
                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null)
                {
                    totalModified += MaterialCullHelper.SetRendererCullMode(renderer, targetMode);
                }
            }

            // Apply to children
            if (applyToChildren)
            {
                Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
                totalModified += MaterialCullHelper.SetRenderersCullMode(childRenderers, targetMode);
            }

            // Apply to additional targets
            if (additionalTargets != null)
            {
                foreach (GameObject target in additionalTargets)
                {
                    if (target != null)
                    {
                        Renderer renderer = target.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            totalModified += MaterialCullHelper.SetRendererCullMode(renderer, targetMode);
                        }
                    }
                }
            }

            Debug.Log($"Successfully modified {totalModified} materials");
            
            // Reset flags after applying
            setCullBack = setCullFront = setCullOff = false;
        }

        private void UpdateCullModeDisplay()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null && renderer.material != null)
            {
                CullMode? mode = MaterialCullHelper.GetCullMode(renderer.material);
                currentCullMode = mode?.ToString() ?? "Unknown";
            }
            else
            {
                currentCullMode = "No Renderer/Material";
            }
        }

        [ContextMenu("Set Cull Back")]
        private void SetCullBack()
        {
            setCullBack = true;
            setCullFront = setCullOff = false;
            ApplyCullMode();
        }

        [ContextMenu("Set Cull Front")]
        private void SetCullFront()
        {
            setCullFront = true;
            setCullBack = setCullOff = false;
            ApplyCullMode();
        }

        [ContextMenu("Set Cull Off")]
        private void SetCullOff()
        {
            setCullOff = true;
            setCullBack = setCullFront = false;
            ApplyCullMode();
        }

        [ContextMenu("Log Current Cull Mode")]
        private void LogCurrentCullMode()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (Material material in renderer.materials)
                {
                    CullMode? mode = MaterialCullHelper.GetCullMode(material);
                    Debug.Log($"Material '{material.name}' cull mode: {mode}");
                }
            }
            else
            {
                Debug.LogWarning("No renderer found on this GameObject");
            }
        }
    }
}
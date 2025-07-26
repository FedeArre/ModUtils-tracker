using UnityEngine;
using UnityEngine.Rendering;

namespace ModUtils
{
    /// <summary>
    /// Utility class that provides helper methods to easily change material cull modes.
    /// Supports setting cull modes for single materials or arrays of materials.
    /// </summary>
    public static class MaterialCullHelper
    {
        /// <summary>
        /// Sets the cull mode for a single material.
        /// </summary>
        /// <param name="material">The material to modify</param>
        /// <param name="cullMode">The cull mode to apply (Off, Front, Back)</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public static bool SetCullMode(Material material, CullMode cullMode)
        {
            if (material == null)
            {
                Debug.LogWarning("MaterialCullHelper: Cannot set cull mode on null material");
                return false;
            }

            try
            {
                material.SetInt("_Cull", (int)cullMode);
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"MaterialCullHelper: Failed to set cull mode on material '{material.name}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Sets the cull mode for an array of materials.
        /// </summary>
        /// <param name="materials">Array of materials to modify</param>
        /// <param name="cullMode">The cull mode to apply (Off, Front, Back)</param>
        /// <returns>The number of materials successfully modified</returns>
        public static int SetCullMode(Material[] materials, CullMode cullMode)
        {
            if (materials == null)
            {
                Debug.LogWarning("MaterialCullHelper: Cannot set cull mode on null material array");
                return 0;
            }

            int successCount = 0;
            for (int i = 0; i < materials.Length; i++)
            {
                if (SetCullMode(materials[i], cullMode))
                {
                    successCount++;
                }
            }

            return successCount;
        }

        /// <summary>
        /// Sets the material to cull front faces (show back faces only).
        /// </summary>
        /// <param name="material">The material to modify</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public static bool SetCullFront(Material material)
        {
            return SetCullMode(material, CullMode.Front);
        }

        /// <summary>
        /// Sets the material to cull back faces (show front faces only - default for most materials).
        /// </summary>
        /// <param name="material">The material to modify</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public static bool SetCullBack(Material material)
        {
            return SetCullMode(material, CullMode.Back);
        }

        /// <summary>
        /// Sets the material to not cull any faces (show both front and back faces).
        /// </summary>
        /// <param name="material">The material to modify</param>
        /// <returns>True if the operation was successful, false otherwise</returns>
        public static bool SetCullOff(Material material)
        {
            return SetCullMode(material, CullMode.Off);
        }

        /// <summary>
        /// Sets multiple materials to cull front faces (show back faces only).
        /// </summary>
        /// <param name="materials">Array of materials to modify</param>
        /// <returns>The number of materials successfully modified</returns>
        public static int SetCullFront(Material[] materials)
        {
            return SetCullMode(materials, CullMode.Front);
        }

        /// <summary>
        /// Sets multiple materials to cull back faces (show front faces only - default for most materials).
        /// </summary>
        /// <param name="materials">Array of materials to modify</param>
        /// <returns>The number of materials successfully modified</returns>
        public static int SetCullBack(Material[] materials)
        {
            return SetCullMode(materials, CullMode.Back);
        }

        /// <summary>
        /// Sets multiple materials to not cull any faces (show both front and back faces).
        /// </summary>
        /// <param name="materials">Array of materials to modify</param>
        /// <returns>The number of materials successfully modified</returns>
        public static int SetCullOff(Material[] materials)
        {
            return SetCullMode(materials, CullMode.Off);
        }

        /// <summary>
        /// Gets the current cull mode of a material.
        /// </summary>
        /// <param name="material">The material to check</param>
        /// <returns>The current cull mode, or null if the material is null or doesn't have the _Cull property</returns>
        public static CullMode? GetCullMode(Material material)
        {
            if (material == null)
            {
                Debug.LogWarning("MaterialCullHelper: Cannot get cull mode from null material");
                return null;
            }

            try
            {
                if (material.HasProperty("_Cull"))
                {
                    return (CullMode)material.GetInt("_Cull");
                }
                else
                {
                    Debug.LogWarning($"MaterialCullHelper: Material '{material.name}' does not have a _Cull property");
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"MaterialCullHelper: Failed to get cull mode from material '{material.name}': {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Applies cull mode to all materials on a Renderer component.
        /// </summary>
        /// <param name="renderer">The renderer whose materials to modify</param>
        /// <param name="cullMode">The cull mode to apply</param>
        /// <returns>The number of materials successfully modified</returns>
        public static int SetRendererCullMode(Renderer renderer, CullMode cullMode)
        {
            if (renderer == null)
            {
                Debug.LogWarning("MaterialCullHelper: Cannot set cull mode on null renderer");
                return 0;
            }

            return SetCullMode(renderer.materials, cullMode);
        }

        /// <summary>
        /// Applies cull mode to all materials on multiple Renderer components.
        /// </summary>
        /// <param name="renderers">Array of renderers whose materials to modify</param>
        /// <param name="cullMode">The cull mode to apply</param>
        /// <returns>The total number of materials successfully modified across all renderers</returns>
        public static int SetRenderersCullMode(Renderer[] renderers, CullMode cullMode)
        {
            if (renderers == null)
            {
                Debug.LogWarning("MaterialCullHelper: Cannot set cull mode on null renderer array");
                return 0;
            }

            int totalSuccess = 0;
            foreach (var renderer in renderers)
            {
                totalSuccess += SetRendererCullMode(renderer, cullMode);
            }

            return totalSuccess;
        }
    }
}
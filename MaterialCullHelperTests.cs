using UnityEngine;
using UnityEngine.Rendering;

namespace ModUtils.Tests
{
    /// <summary>
    /// Simple test class for MaterialCullHelper functionality.
    /// This can be used to validate that the helper methods work correctly.
    /// </summary>
    public static class MaterialCullHelperTests
    {
        /// <summary>
        /// Runs basic tests on the MaterialCullHelper functionality.
        /// Call this method to validate the implementation.
        /// </summary>
        public static void RunTests()
        {
            Debug.Log("Starting MaterialCullHelper Tests...");
            
            // Test with null material (should handle gracefully)
            TestNullMaterial();
            
            // Test with a basic material
            TestBasicFunctionality();
            
            // Test with material arrays
            TestMaterialArrays();
            
            Debug.Log("MaterialCullHelper Tests Completed!");
        }

        private static void TestNullMaterial()
        {
            Debug.Log("Testing null material handling...");
            
            // These should all return false/0 and log warnings
            bool result1 = MaterialCullHelper.SetCullMode(null, CullMode.Back);
            bool result2 = MaterialCullHelper.SetCullBack(null);
            int result3 = MaterialCullHelper.SetCullMode((Material[])null, CullMode.Off);
            CullMode? result4 = MaterialCullHelper.GetCullMode(null);
            
            if (!result1 && !result2 && result3 == 0 && !result4.HasValue)
            {
                Debug.Log("✓ Null material handling works correctly");
            }
            else
            {
                Debug.LogError("✗ Null material handling failed");
            }
        }

        private static void TestBasicFunctionality()
        {
            Debug.Log("Testing basic functionality...");
            
            // Create a test material with Standard shader (should have _Cull property)
            Material testMaterial = new Material(Shader.Find("Standard"));
            testMaterial.name = "TestMaterial";
            
            // Test setting different cull modes
            bool success1 = MaterialCullHelper.SetCullBack(testMaterial);
            CullMode? mode1 = MaterialCullHelper.GetCullMode(testMaterial);
            
            bool success2 = MaterialCullHelper.SetCullFront(testMaterial);
            CullMode? mode2 = MaterialCullHelper.GetCullMode(testMaterial);
            
            bool success3 = MaterialCullHelper.SetCullOff(testMaterial);
            CullMode? mode3 = MaterialCullHelper.GetCullMode(testMaterial);
            
            if (success1 && mode1 == CullMode.Back &&
                success2 && mode2 == CullMode.Front &&
                success3 && mode3 == CullMode.Off)
            {
                Debug.Log("✓ Basic functionality works correctly");
            }
            else
            {
                Debug.LogError($"✗ Basic functionality failed. Results: {mode1}, {mode2}, {mode3}");
            }
            
            // Clean up
            if (Application.isPlaying)
            {
                Object.Destroy(testMaterial);
            }
            else
            {
                Object.DestroyImmediate(testMaterial);
            }
        }

        private static void TestMaterialArrays()
        {
            Debug.Log("Testing material array functionality...");
            
            // Create test materials
            Material[] materials = new Material[3];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = new Material(Shader.Find("Standard"));
                materials[i].name = $"TestMaterial{i}";
            }
            
            // Test array operations
            int successCount1 = MaterialCullHelper.SetCullBack(materials);
            int successCount2 = MaterialCullHelper.SetCullFront(materials);
            int successCount3 = MaterialCullHelper.SetCullOff(materials);
            
            // Verify all materials were modified
            bool allBack = true, allFront = true, allOff = true;
            
            // Reset and test each mode
            MaterialCullHelper.SetCullBack(materials);
            foreach (Material mat in materials)
            {
                if (MaterialCullHelper.GetCullMode(mat) != CullMode.Back)
                    allBack = false;
            }
            
            MaterialCullHelper.SetCullFront(materials);
            foreach (Material mat in materials)
            {
                if (MaterialCullHelper.GetCullMode(mat) != CullMode.Front)
                    allFront = false;
            }
            
            MaterialCullHelper.SetCullOff(materials);
            foreach (Material mat in materials)
            {
                if (MaterialCullHelper.GetCullMode(mat) != CullMode.Off)
                    allOff = false;
            }
            
            if (successCount1 == 3 && successCount2 == 3 && successCount3 == 3 &&
                allBack && allFront && allOff)
            {
                Debug.Log("✓ Material array functionality works correctly");
            }
            else
            {
                Debug.LogError($"✗ Material array functionality failed. Success counts: {successCount1}, {successCount2}, {successCount3}");
            }
            
            // Clean up
            foreach (Material mat in materials)
            {
                if (Application.isPlaying)
                {
                    Object.Destroy(mat);
                }
                else
                {
                    Object.DestroyImmediate(mat);
                }
            }
        }

        /// <summary>
        /// Creates a simple test scene with a GameObject that has a renderer.
        /// Returns the created GameObject for testing renderer-based methods.
        /// </summary>
        public static GameObject CreateTestObject()
        {
            GameObject testObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            testObj.name = "MaterialCullTestObject";
            
            // Ensure it has a material
            Renderer renderer = testObj.GetComponent<Renderer>();
            if (renderer != null && renderer.material == null)
            {
                renderer.material = new Material(Shader.Find("Standard"));
            }
            
            Debug.Log("Created test object with renderer for manual testing");
            return testObj;
        }
    }
}
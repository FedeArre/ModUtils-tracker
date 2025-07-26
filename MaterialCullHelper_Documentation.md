# MaterialCullHelper Documentation

The `MaterialCullHelper` is a utility class that provides convenient methods for changing material cull modes in Unity. This is particularly useful for mod developers who need to quickly adjust how materials render faces.

## What is Material Culling?

Material culling determines which faces of a 3D object are rendered:
- **Back Culling (CullMode.Back)**: Only front faces are rendered (default for most materials)
- **Front Culling (CullMode.Front)**: Only back faces are rendered 
- **No Culling (CullMode.Off)**: Both front and back faces are rendered

## Usage Examples

### Basic Usage - Single Material

```csharp
using ModUtils;
using UnityEngine;
using UnityEngine.Rendering;

// Get a material from a renderer
Material material = GetComponent<Renderer>().material;

// Set different cull modes
MaterialCullHelper.SetCullBack(material);   // Show front faces only (default)
MaterialCullHelper.SetCullFront(material);  // Show back faces only
MaterialCullHelper.SetCullOff(material);    // Show both sides

// Or use the generic method
MaterialCullHelper.SetCullMode(material, CullMode.Back);
```

### Working with Multiple Materials

```csharp
// Get all materials from a renderer
Material[] materials = GetComponent<Renderer>().materials;

// Apply cull mode to all materials
int successCount = MaterialCullHelper.SetCullOff(materials);
Debug.Log($"Successfully modified {successCount} materials");

// Or work with an array of specific materials
Material[] myMaterials = { material1, material2, material3 };
MaterialCullHelper.SetCullFront(myMaterials);
```

### Working with Renderers

```csharp
// Apply cull mode to all materials on a renderer
Renderer renderer = GetComponent<Renderer>();
MaterialCullHelper.SetRendererCullMode(renderer, CullMode.Off);

// Apply to multiple renderers at once
Renderer[] renderers = GetComponentsInChildren<Renderer>();
int totalModified = MaterialCullHelper.SetRenderersCullMode(renderers, CullMode.Back);
Debug.Log($"Modified materials on {totalModified} renderers");
```

### Checking Current Cull Mode

```csharp
Material material = GetComponent<Renderer>().material;
CullMode? currentMode = MaterialCullHelper.GetCullMode(material);

if (currentMode.HasValue)
{
    Debug.Log($"Current cull mode: {currentMode.Value}");
}
else
{
    Debug.Log("Could not determine cull mode");
}
```

### Error Handling

The helper methods include built-in error handling and will return success indicators:

```csharp
// Single material - returns bool
bool success = MaterialCullHelper.SetCullOff(material);
if (!success)
{
    Debug.LogError("Failed to set cull mode");
}

// Multiple materials - returns count of successful operations
Material[] materials = GetComponent<Renderer>().materials;
int successCount = MaterialCullHelper.SetCullBack(materials);
if (successCount != materials.Length)
{
    Debug.LogWarning($"Only {successCount} out of {materials.Length} materials were modified");
}
```

## Common Use Cases for Mod Development

### 1. Double-Sided Materials
Perfect for creating windows, glass, or thin objects that should be visible from both sides:

```csharp
// Make a window material visible from both sides
Material windowMaterial = Resources.Load<Material>("WindowMaterial");
MaterialCullHelper.SetCullOff(windowMaterial);
```

### 2. Inside-Out Objects
For objects where you want to see the interior (like viewing from inside a helmet):

```csharp
// Show only the inside faces of a helmet
Material helmetMaterial = helmetRenderer.material;
MaterialCullHelper.SetCullFront(helmetMaterial);
```

### 3. Standard Opaque Objects
Ensure standard rendering for solid objects:

```csharp
// Standard rendering for car parts
Material[] carPartMaterials = carRenderer.materials;
MaterialCullHelper.SetCullBack(carPartMaterials);
```

### 4. Batch Processing
Apply settings to many objects at once:

```csharp
// Make all glass materials in the scene double-sided
GameObject[] glassObjects = GameObject.FindGameObjectsWithTag("Glass");
foreach (GameObject obj in glassObjects)
{
    Renderer renderer = obj.GetComponent<Renderer>();
    if (renderer != null)
    {
        MaterialCullHelper.SetRendererCullMode(renderer, CullMode.Off);
    }
}
```

## Important Notes

1. **Material Instance**: The helper modifies material instances. If you're using shared materials, changes will affect all objects using that material.

2. **Shader Compatibility**: The helper works with materials that have a `_Cull` property. Most Unity shaders support this, but custom shaders must include it.

3. **Performance**: Disabling culling (CullMode.Off) doubles the number of faces rendered, which can impact performance.

4. **Error Logging**: All methods include error handling and will log warnings/errors to the console if issues occur.

## Integration into ModUtils

This helper class is designed to be easily integrated into the main ModUtils project. Simply place the `MaterialCullHelper.cs` file in your project's scripts folder, and it will be available throughout your mod.

The class is in the `ModUtils` namespace to avoid conflicts with other code and follows Unity coding conventions for static utility classes.
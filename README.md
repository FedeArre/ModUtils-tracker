# Material Cull Helpers for ModUtils

This implementation provides helpers to change material cull methods (front, back, both) as requested in issue #12.

## Files Included

1. **MaterialCullHelper.cs** - The main utility class with static helper methods
2. **MaterialCullHelper_Documentation.md** - Comprehensive documentation with usage examples
3. **MaterialCullExample.cs** - Example MonoBehaviour script showing practical usage
4. **MaterialCullHelperTests.cs** - Simple test class to validate functionality
5. **README.md** - This file

## Quick Start

```csharp
using ModUtils;
using UnityEngine.Rendering;

// Basic usage - change a single material
Material material = GetComponent<Renderer>().material;
MaterialCullHelper.SetCullOff(material);  // Show both sides

// Array usage - change multiple materials
Material[] materials = GetComponent<Renderer>().materials;
MaterialCullHelper.SetCullBack(materials);  // Standard culling

// Renderer usage - change all materials on a renderer
Renderer renderer = GetComponent<Renderer>();
MaterialCullHelper.SetRendererCullMode(renderer, CullMode.Front);
```

## Features

### Core Methods
- `SetCullMode()` - Generic method accepting CullMode enum
- `SetCullBack()` - Show front faces only (default)
- `SetCullFront()` - Show back faces only  
- `SetCullOff()` - Show both sides (double-sided)

### Array Support
All methods support both single materials and material arrays.

### Renderer Support
- `SetRendererCullMode()` - Apply to all materials on a renderer
- `SetRenderersCullMode()` - Apply to multiple renderers

### Utility Methods
- `GetCullMode()` - Check current cull mode of a material

### Error Handling
- Null-safe operations with helpful warning messages
- Return values indicate success/failure
- Exception handling with error logging

## Integration

To integrate into the main ModUtils project:

1. Copy `MaterialCullHelper.cs` to your scripts folder
2. The class is in the `ModUtils` namespace
3. Add `using ModUtils;` to use the helpers
4. Optionally include the example and test files for reference

## Testing

Run the included tests to validate functionality:

```csharp
using ModUtils.Tests;

// Run automated tests
MaterialCullHelperTests.RunTests();

// Create a test object for manual testing
GameObject testObj = MaterialCullHelperTests.CreateTestObject();
```

## Unity Compatibility

- Works with Unity's built-in render pipeline
- Compatible with materials that have the `_Cull` property
- Tested with Standard shader and most Unity shaders
- Should work with URP/HDRP materials that support culling

## Performance Notes

- Setting `CullMode.Off` doubles rendering cost (both faces rendered)
- Methods are lightweight with minimal overhead
- Array operations are optimized for batch processing

## Use Cases for My Garage Modding

1. **Windows/Glass** - Use `SetCullOff()` for see-through materials
2. **Interior Views** - Use `SetCullFront()` to see inside objects
3. **Standard Parts** - Use `SetCullBack()` for normal solid objects
4. **Batch Processing** - Apply settings to multiple car parts at once
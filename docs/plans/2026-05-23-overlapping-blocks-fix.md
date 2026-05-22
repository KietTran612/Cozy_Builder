# Overlapping Blocks Fix Implementation Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Resolve the horizontal and vertical overlapping/clashing of KayKit models by introducing a runtime parent GameObject "Wrapper" hierarchy, customized prefab offsets, and decoupled vertical stacking calculations.

**Architecture:** 
1. Expose `PrefabOffsetConfig` settings to allow per-prefab local position, rotation, and scale offsets at Runtime.
2. In `TownGridView.GetPooledBlock`, instantiate blocks inside a newly created parent "Wrapper" GameObject, applying the matching config.
3. Update `ApplyColorAndMaterial` and `EnsureCollider` to search children recursively so they support the new Wrapper hierarchy seamlessly.
4. Add separate serialized parameters for `firstBlockHeightOffset` and `blockHeightStep` in `TownGridView` to cleanly configure terrain and block spacing.
5. Pre-configure robust default offsets in `AutoWirePrefabs` so KayKit models look spectacular and perfectly aligned out-of-the-box.

**Tech Stack:** Unity 6000.3.11f1, VContainer, C# / Unity Engine.

---

### Task 1: Add Prefab Offset Configuration & Stacking Fields

**Files:**
- Modify: [TownGridView.cs](file:///c:/1.SOURCE/Unity/Source/Cozy_Builder/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rendering/TownGridView.cs)

**Step 1: Declare `PrefabOffsetConfig` struct and serialization fields**
Add `PrefabOffsetConfig` struct inside `TownGridView.cs` and serialize fields for offsets and height calibration.

**Step 2: Update `GridToWorld` math**
Implement separate offsets for the first layer vs subsequent stack layers:
`Y = firstBlockHeightOffset + (height - 1) * blockHeightStep` (when height > 0).

**Step 3: Update `AutoWirePrefabs` default presets**
Set up realistic default values in `AutoWirePrefabs` (e.g. scale multipliers `0.85` for houses, spacing adjustments) so they wire automatically.

---

### Task 2: Implement Runtime Wrapper Hierarchy & Dynamic Offsets

**Files:**
- Modify: [TownGridView.cs](file:///c:/1.SOURCE/Unity/Source/Cozy_Builder/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rendering/TownGridView.cs)

**Step 1: Update `GetPooledBlock`**
- Create an empty GameObject wrapper.
- Instantiate the FBX model inside the wrapper.
- Fetch and apply the local position, rotation, and scale offsets from config.
- Ensure collider is added to the model.
- Return the wrapper object to be pooled.

**Step 2: Update `ReturnToPool`**
Ensure wrappers are safely recycled to the block root and deactivated.

---

### Task 3: Adjust Renderer and Collider Adapters for Wrapper Hierarchy

**Files:**
- Modify: [TownGridView.cs](file:///c:/1.SOURCE/Unity/Source/Cozy_Builder/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rendering/TownGridView.cs)

**Step 1: Update `ApplyColorAndMaterial` to find `BlockColorAdapter`**
Adapt query logic to check both the root wrapper and the first child:
`blockView.TryGetComponent<BlockColorAdapter>` OR `blockView.transform.GetChild(0).TryGetComponent<BlockColorAdapter>`.

**Step 2: Update `EnsureCollider`**
Ensure it scans the child elements of the wrapper correctly.

---

### Task 4: Align Debug Text Heights

**Files:**
- Modify: [PrototypeTownDebug3D.cs](file:///c:/1.SOURCE/Unity/Source/Cozy_Builder/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Debugging/PrototypeTownDebug3D.cs)

**Step 1: Update Hover Text Position Y Calculation**
Update debug text height math in `UpdateFocusDebug` and `UpdateDirtyHighlights` to read `firstBlockHeightOffset` and `blockHeightStep` from the injected `TownGridView` instance.

---

### Task 5: Verify Project Compiles and Graphify Synced

**Files:**
- Build: Unity Compiler
- Update: Graphify Graph

**Step 1: Build & Compile**
Verify there are no compile errors in Unity.

**Step 2: Run Graphify**
Run `graphify update .` to sync the AST graph.

# HANDOFF

## Purpose
- This file is the cross-device handoff note for the current Chapter 01 setup work.
- Keep this file in Git so the same state is visible on desktop and laptop.

## Active Project
- Project root: `D:\Unity project\Child-of-the-Ghost-Gate`
- Main scene for this task: `Assets/Scenes/Game/Chapter01_Village.unity`
- Current work type: Chapter 01 interaction flow wiring and scene setup

## What Was Implemented
- Extended dialogue flow on top of the existing scene structure instead of replacing it.
- Added dialogue data asset support.
- Added inspectable object flow.
- Added door condition flow.
- Added objective UI support.
- Added checkpoint, death zone, and simple chaser scripts.

## Changed Existing Scripts
- `Assets/Scripts/Game/Chapter01/Dialogue/DialogueTrigger.cs`
- `Assets/Scripts/Game/Chapter01/UI/DialogueUIController.cs`
- `Assets/Scripts/Game/Chapter01/Chapter01FlowController.cs`
- `Assets/Scripts/Game/Chapter01/Interaction/InteractableBase.cs`
- `Assets/Scenes/Game/Chapter01_Village.unity`

## Added New Scripts
- `Assets/Scripts/Game/Chapter01/Dialogue/DialogueData.cs`
- `Assets/Scripts/Game/Chapter01/Interaction/InspectableObject.cs`
- `Assets/Scripts/Game/Chapter01/Interaction/DoorObject.cs`
- `Assets/Scripts/Game/Chapter01/UI/ObjectiveUI.cs`
- `Assets/Scripts/Game/Shared/CheckpointManager.cs`
- `Assets/Scripts/Game/Shared/CheckpointTrigger.cs`
- `Assets/Scripts/Game/Shared/DeathZone.cs`
- `Assets/Scripts/Game/Shared/SimpleChaser.cs`

## Important Fixes Already Applied
- Fixed the serialized field name collision in the interaction inheritance chain.
- `DoorObject` no longer duplicates the old `promptText` field name.
- `InteractableBase` prompt field was renamed to avoid Unity serialization conflicts.
- `DialogueTrigger` now supports `DialogueData` and tracks completion with `HasTriggered`.
- `DialogueUIController` now supports typewriter text and complete-on-input behavior.

## Important Scene Facts
- `DialogueUIController` is attached to `Canvas`, not `DialoguePanel`.
- When linking `DialogueTrigger.dialogueUI`, drag the `Canvas` object that has `DialogueUIController`.
- `DialoguePanel` is the visual root used by `DialogueUIController.dialogueRoot`.
- Current scene already contains:
  - `NPC_Father_Test`
  - `Canvas`
  - `DialoguePanel`
  - `SpeakerNameText`
  - `DialogueBodyText`

## Confirmed Next Work
1. Set `Door_Object.requiredInspections` size to `1` and assign `Charm_Object`.
2. Set `Door_Object.requiredDialogues` size to `1` and assign `NPC_Father_Test`.
3. Set `Door_Object.lockedDialogue` to `Door_LockedData`.
4. Set `Door_Object.dialogueUI` to the `Canvas` object that has `DialogueUIController`.
5. Add `Door_Object.onDoorOpened -> Chapter01Manager -> Chapter01FlowController.OnDoorOpened`.
6. Create `Outside_Trigger` under scene root or `Interactables`.
7. Add `BoxCollider2D` to `Outside_Trigger`.
8. Enable `Is Trigger` on that collider.
9. Add `CheckpointTrigger` to `Outside_Trigger`.
10. Add `Outside_Trigger.onCheckpointReached -> Chapter01Manager -> Chapter01FlowController.OnGoOutside`.
11. Open `Chapter01FlowController` on `Chapter01Manager`.
12. Set `initialState` to `TalkToFather`.
13. Set `fatherDialogue` to `NPC_Father_Test`.
14. Set `charmObject` to `Charm_Object`.
15. Set `doorObject` to `Door_Object`.
16. Set `outsideTrigger` to `Outside_Trigger`.
17. Leave `motherDialogue` empty for now.

## Optional But Recommended Setup
- Create `Assets/Data/Chapter01/Dialogue` if you want a clean place for dialogue assets.
- Keep `DialogueData` assets there:
  - `Father_DialogueData`
  - `Charm_InspectData`
  - `Door_LockedData`
- If needed, create `ObjectiveUIRoot` under `Canvas` and attach `ObjectiveUI`.

## Test Order
1. Enter Play mode.
2. Walk to `NPC_Father_Test`.
3. Press `E` to begin dialogue.
4. Confirm movement locks during dialogue.
5. Confirm `Space` or `Enter` advances dialogue.
6. Confirm father dialogue completion unlocks the charm step.
7. Interact with `Charm_Object`.
8. Confirm charm inspection unlocks the door step.
9. Interact with `Door_Object`.
10. Confirm player moves to the destination transform.
11. Enter `Outside_Trigger`.
12. Confirm chapter state moves to completed flow.

## Known Constraints
- Background art, object textures, and animation sheets are not required for this test pass.
- This setup should work with text UI, Collider2D, empty objects, and simple square sprites only.
- If Inspector drag-and-drop fails again, check Console errors first before reconnecting fields.

## If Unity Shows A Serialization Error Again
1. Read the exact Console error text.
2. Stop and fix that error first.
3. Do not continue Inspector wiring while red Console errors remain.
4. If a component stays broken, remove it and add it again, then reconnect the fields.

## Git End Of Session
1. `git status`
2. `git add .`
3. `git commit -m "clear message"`
4. `git push`

## Resume Prompt
- "Read HANDOFF.md first, then continue the Chapter 01 Inspector wiring from the Door_Object setup step."

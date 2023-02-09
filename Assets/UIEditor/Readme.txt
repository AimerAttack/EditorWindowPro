UI Editor
Copyright © 2014-2017 Argiris Baltzis

Thank you for buying UI Editor!

If you have any questions, comments or feature requests,
please drop by the forums @ http://forum.unity3d.com/threads/ui-editor-is-now-available.371576/

-------------------------------------------------------------------------------------
Version History
-------------------------------------------------------------------------------------
Version 1.3.1
- Droped textures/sprites from project folder now have the name of the image.
- Fixed a bug with Reset/Size on a text not setting its new height.
- Renamed C# foldet to CSharp.
- Fixed an issue with unity 5.4 where the editor wouldn't render a canvas.
- Disabled UI prefab searching as it introduced a delay, you can now update the toolbox manually.

Version 1.3.0
- Added anchor visual.
- Revamped anchor and bounding box lines system.
- Added edge snaping. Moving an element near a canvas or neighbor element edge it will now snap to it.
- Added resize boxes around the element.
- Fixed a bug with anchor context menu not reporting selected type when using custom stretch.
- Added library control functionality, you cann add the UILibraryControl component on a RectTransform in your project and it will be available in the toolbox.
- Fixed a bug with resizing while holding control not resizing proportionaly.
- Fixed a bug in the toolbar that would not report the correct background color.
- Removed snap to grid option from toolbox and moved it to context menu along with other snap to options.
- Fixed a bug with arrow keys nudging incorrectly moving children.
- Arrow keys now also work for rotation, scaling and resizing same way it worked for movement. The appropriate tool must be selected.
- Highlighted and selected elements now show the name of the gameobject on top left if you keep control pressed.
- Added a stretch all in anchor context menu.
- Fixed a bug when creating new elements not spawning at correct position when a canvas scale is applied.
- Minimum unity version supported is now 5.0
- You can now drag images from the projects directory to the editor window and it will automatically create a UI.Image or UI.RawImage for you.
- You can also drag RectTranform prefabs in the editor from the projects folder.

Version 1.2.3
- Fixed a bug with scale UNDO.

Version 1.2.2
- Toolbox background is now less transparent.
- Added more zoom levels.
- You can now scroll using either middle or right mouse button.
- When the view tool is selected you can also pan the canvas with left mouse button.
- Added pan cursor icon when in scrolling mode.
- Decreased size of anchor lines and bounding boxes.
- Added small cosmetics at the root of anchor lines
- When creating a new control it no longer has a decimal position to improve pixel precision.

Version 1.2.1
- Fixed an exception bug with the context menu when having a non ui object selected.
- Context Menu controls now always has same set of controls as the controls toolbox.
- Removed Canvas control from toolbox.
- Made some cosmetic changes to toolbox;
- Added support for Screen Space - Overlay.
- Added Reset/View in context menu to reset the scrolling on the workspace.

Version 1.2.0
- Renamed history.txt to readme.txt
- Scene Settings is no longer hidden in the hierarcy and only gets created if it needs to be used
- Fixed an issue with InputField not being created on unity 4.6.4 as the menu path was wrong.
- Fixed an issue with detecting mouse button as down when clicking in the toolbar.
- Control drag and drop now only works with left mouse button.
- Added a new control "Mask"
- Droping a control on the toolbox no longer creates a control
- Added a new control "Scroll View"
- Added a new control "Rich Text", this is same as text but it has bold,italic, colored and different size text letters.
- Added Horizontal, Vertical and Grid Layout controls.
- Moved Bring To Front/Send to Back in a submenu Order and added Bring to Front by 1 and Send to Back by 1
- Background Rendering Settings are now stored on a per scene basis.
- Reworked Anchor Menu, now you can select horizontal/vertical anchors seperately.
- Fixed an issue where undo wouldnt work when seting the anchor or pivot.
- Fixed an issue with an object being repositioned wrong after an anchor change.
- Minimum grid size is now 2, minimum draw size is 8, grid snap is by default on
- Added dropdown control for Unity 5.2+
- Added a new control "Container"
- Stretched non-graphic recttransforms are now longer left click selectable.
- Fixed an exception bug when undoing a newly created object. 
- Editor renderer now uses a shader
- Optimized scene settings retrieval
- Draging and droping a control over another control will now always make it a child.
- Imrpoved move tool logic so it works as "expected"

ersion 1.1.0
- Fixed a bug when first importing UI Editor not hiding the scene UI.
- Fixed a bug with move tool not moving to correct axis when objects have rotation.
- Fixed a bug with Input Field not being created through drag & drop and context menu on Unity 4.6
- Improved left click select detection
- Root Canvas is no longer selectable through left click
- You can now select a region rectangle.
- Fixed some issues with mouse up events outside the editor window.
- Fixed an issue with background scene rendering
- Changed ui render filtering from bilinear to point when zoom at 100%. This should give identical render quality to game view when at 100% zoom
- There is a hiden settings per scene that will allow per scene saved data for the editor, as a result the camera background render will always stay on instead of desapearing when changing play mode or restarting untiy.
- Fixed an issue with repaint being called than it should on the editor window.
- Fixed a bug with background scene camera drawing to the editor window during update cycle.
- Fixed a bug where the UI would get double rendered when having a background scene camera.
- Fixed a bug with background camera clearing with solid color and being transparent.
- Control+A on a selected objects selects all other objects with same parent
- Made an optimization while searching for objects under mouse position.
- When enable screen space camera on an overlay the planedistance gets adjusted to cameras nearClipPlane. This will ensure UI is always drawn on top of all 3D.
- Added Show/Hide in context menu, it enables and disables the object.
- Fixed an issue with disabled objects not showing at correct position
- Fixed a transform flicker issue when using a canvas scaler after recompiling
- Fixed an issue with moving/rotating/scaling parent and child objects together that would cause the child to get double transformed
- Context Menu Pivor/Anchor now shows which pivot/anchor is active on the objects.
- Fixed some minor artifacts with a white render window when first booting up and when switching from playmode back to editor
- Fixed a bug with clipping masks not functioning in editor.
- Reworked how the move and resize tool work.
- Fixed a mouse bug with the toolbox triggering a selection and locking scrolling.
- Any canvas found to be using "Default" layer will now be switched to "UI".
- Added Reset/Native Size for images and text to reset to its texture size
- Improved anchor lines
- Fixed a but that would cause resizing not to work on stretched surfaces.
- Added a checker background on the editor window
- Fixed an issue with newly created objects having a rotation and scale when they shouldn't
- Drag & Drop / Context Menu creation controls now have the same code path to make results consistent.

Version 1.0.0
First Release


# Prototyping
## A template Unity project I use for prototyping design ideas, future game jams, and for fun coding challenges!


### Implemented Systems
#### Interactions [(Go to folder)](https://github.com/ElliotHume/project-prototyping/tree/main/Assets/_Prototyping/Scripts/Interactions)
- Interactors + Interactables
- Hovers, Selections & a priority system!
#### Pointer Selection [(Go to folder)](https://github.com/ElliotHume/project-prototyping/tree/main/Assets/_Prototyping/Scripts/PointerSelectables)
- In-world mouse pointer selectables
#### Action-Trigger-Task System [(Go to folder)](https://github.com/ElliotHume/project-prototyping/tree/main/Assets/_Prototyping/Scripts/ActionTriggers)
- Triggers that listen for an event before triggering
- Actions that execute when a linked trigger is activated
- Tasks for Actions to complete
#### Grids [(Go to folder)](https://github.com/ElliotHume/project-prototyping/tree/main/Assets/_Prototyping/Scripts/Grids/Core)
- Grid system, with configurable & dynamic dimensions
- IGrid -> IGridCell -> IHasGridPosition
#### Chess Systems [(Go to folder)](https://github.com/ElliotHume/project-prototyping/tree/main/Assets/_Prototyping/Scripts/Chess)
- Dynamic movement options, with full support for adding any type of movement, and hot-swapping movement on chess pieces.
- Special attacks that use the Action-Trigger-Task system to execute, you want a pawn that fires a laser when it moves? We have that!
- Scheduling director, which handles all of the dynamics of the above two systems and schedules it properly depending on who's turn it is and what needs to be resolved.
#### Inventory System [WIP] [(Go to folder)](https://github.com/ElliotHume/project-prototyping/tree/main/Assets/_Prototyping/Scripts/Inventories)
- Implemented the structure and architecture of an an inventory system that has compartments with a tetris-style storage system (think Escape from Tarkov)
- Still working on the UI and implementation of how to get items in and out in-game.

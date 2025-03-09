# Prototyping
## A template Unity project I use for prototyping design ideas, future game jams, and for fun coding challenges!


### Implemented Systems
#### Interactions
- Interactors + Interactables
- Hovers, Selections & a priority system!
#### Pointer Selection
- In-world mouse pointer selectables
#### Action-Trigger-Task System
- Triggers that listen for an event before triggering
- Actions that execute when a linked trigger is activated
- Tasks for Actions to complete
#### Grids
- Grid system, with configurable & dynamic dimensions
- IGrid -> IGridCell -> IHasGridPosition
#### Chess Systems
- Dynamic movement options, with full support for adding any type of movement, and hot-swapping movement on chess pieces.
- Special attacks that use the Action-Trigger-Task system to execute, you want a pawn that fires a laser when it moves? We have that!
- Scheduling director, which handles all of the dynamics of the above two systems and schedules it properly depending on who's turn it is and what needs to be resolved.
#### Inventory System [WIP]
- Implemented the structure and architecture of an an inventory system that has compartments with a tetris-style storage system (think Escape from Tarkov)
- Still working on the UI and implementation of how to get items in and out in-game.

Tilemap Pathfinding Project – README
===================================

Itch.io link:

https://ori755.itch.io/ex6

This project extends a Unity tile‑based movement system with two major upgrades:
(1) item‑dependent movement rules for the player, and  
(2) an improved enemy pathfinding algorithm using weighted A* instead of BFS.

------------------------------------------------------------
1. Gameplay Modifications (Player Movement & Items)
------------------------------------------------------------

We added new mechanics that change how the player interacts with terrain tiles:

• Boat — If the player picks up a Boat, they can move across water tiles  
  (deep_sea, medium_sea, shallow_sea).

• Goat — If the player picks up a Goat, they gain the ability to climb mountains
  (mountains tile becomes walkable).

• Pickaxe — If the player picks up a Pickaxe, mountains become “breakable”:  
  when the player steps on a mountain tile, it is automatically converted  
  into a grass tile, allowing normal movement afterwards.

These changes were implemented inside **KeyboardMoverByTile.cs**, where
movement permissions now depend on:
(1) the tile type the player is trying to step on, and  
(2) the currently held item.

This creates more dynamic level navigation and adds progression based on item pickup.

------------------------------------------------------------
2. Algorithmic Change (Enemy Movement – Weighted A*)
------------------------------------------------------------

Originally, enemies used **BFS**, which finds the shortest path in number of steps,
but ignores terrain difficulty.

We replaced BFS with a full **A\* pathfinding algorithm** that supports:
• weighted movement costs (e.g., moving on grass is cheap, mountains expensive),  
• heuristics (Manhattan distance),  
• priority‑queue‑based open set.

A* is implemented in **AStar.cs**, using:
• `CostOf(tile)` from TilemapGraph.cs to determine tile difficulty  
• a Manhattan heuristic for efficient path estimation  
• a reconstruction step to return the full optimal path

This allows the enemy to prefer “cheaper” routes, not just shortest‑in‑steps ones.
For example:
• grass → fast and cheap  
• mountains or hills → slower and more expensive  
• water → completely forbidden  
• blocked tiles → ignored by the graph

The result is realistic movement where enemies intelligently choose efficient paths.

------------------------------------------------------------
Testing
------------------------------------------------------------

We created a small standalone C# solution containing:
• AStarLib – the core A* implementation  
• AStarTests – NUnit test project validating the algorithm  
• AStarConsole – optional manual testing project

Tests include:
✓ simple graph traversal  
✓ correct path reconstruction  
✓ cost‑weighted decisions  
✓ obstacle avoidance

All tests passed successfully.


------------------------------------------------------------
How to Build (External A* Solution)
------------------------------------------------------------

From terminal inside the repository:

    dotnet build AStarSolution.sln

To run tests:

    dotnet test AStarSolution.sln

All dependencies are local and no external packages are required.

------------------------------------------------------------
How to Use in Unity
------------------------------------------------------------

• Drop AStar.cs and TilemapGraph.cs into Scripts/3-enemies/  
• Enemy scripts call AStar.FindPath() instead of BFS.FindPath()  
• Ensure your TilemapGraph correctly assigns movement costs  
• EnemyController uses the computed path to move step‑by‑step

------------------------------------------------------------
End of README
------------------------------------------------------------

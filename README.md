# DataTableEcs

⚠️ Disclaimer: this is a hobby project I made for fun and for using in MonoGame. However, I would highly suggest using a more fully-featured and battle-tested ECS or implementing your own ECS.

- This is a Entity Component System that uses DataTables for storing components.
- You can create systems by querying an Entity Manager's Entities DataTable. Each type of component has its own column.</p>

For example, a player entity with the position, sprite, and health components would look like this:

|  ID|Position|Name  |Health|
|----|--------|------|------|
|   0|(0, 1)  |Player|10/10 |
|   1|(2, 3)  |Orc   |3/6   |
|   2|(-1, 4) |Troll |18/18 |

# RGB, Y?
A classic top-down shooter game where the player has to switch between four different coloured characters to defeat enemies and puzzles.

## TODO
This will evolve constantly for the next few weeks, but here's a rough list of what I want to do next:
- [ ] Add a proper README
- [ ] Add a proper LICENSE
- [x] Parse settings from ini file
- [x] Fullscreen mode
- [x] Okay - having learned about (Drawable)GameComponents, I now want to stop using them :) - I need more control over where and how things are drawn. This'll be particularly important for screen resolution & scaling stuff (RenderTarget->Draw)
- [x] Resolution configuration in ini file
- [ ] Different projectiles for each character
- [ ] Add a simple enemy with a simple AI that tries to attack any of the player characters
- [ ] Enable collisions between projectiles and enemies so we can kill them
- [ ] Add simple floor switch that does an action - switches are activated when a character of the same colour stands on it
- [x] Tiled map editor integration
- [x] Collisions with tiled map tileset collision objects
- [x] Fix bug where circular collision objects are not in the correct Y position
- [	] Investigate pixel-perfect collisions with tiled map instead of using collision objects
- [x] Set starting positions for the player characters in the Tiled map
- [ ] Player characters need health, which should be displayed on the HUD
- [x] Orthoganal camera that follows the player
- [	] Should player characters be forced to stay together? (limited by camera bounds I guess)
- [x] Add a TODO list to the README :)
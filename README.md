# ITU-CS-464-LAB-06-BSCS24074

CS464 Game Development, Lab 06. A 5-level 3D arena game built in Unity, based on Lecture 07
(Enemies, Obstacles & Power-ups). You roll a ball around an arena, dodge patrolling guards,
spikes, crushers and sweepers, grab power-ups, and finish each level's objective.

**Unity version:** <fill in, e.g. 6000.0.xx>  **Open:** Unity Hub -> Add -> select this folder -> open `Assets/Scenes/Level1`

## Controls
| Key | Action |
|---|---|
| WASD / Arrow keys | Roll the ball |

## Levels
| # | Name | Objective | What makes it harder |
|---|---|---|---|
| 1 | Warm Up | Collect 5 coins | Open room, one spike strip, no enemies, no timer |
| 2 | The Patrol | Collect 8 coins | S-shaped corridors, a patrolling guard, spikes |
| 3 | Beat the Clock | Reach the exit in 30 s | Zig-zag maze, a crusher, spikes, countdown timer |
| 4 | Survive | Stay alive for 40 s | Sweepers, 2 crushers, 2 guards, pillars, no coins |
| 5 | Final Exam | Collect 10 coins, then reach the exit in 75 s | Four rooms, chasing guard, sweeper, crusher, guard, spikes |

## Mechanics (from Lecture 07)
- **Enemies that patrol:** `EnemyPatrol` (loop / ping-pong / random) and `EnemyGuard` (patrol -> chase -> return)
- **Hazards:** `Hazard` (spikes, guards, telegraphed crusher), `MovingHazard` (sweeper)
- **Power-ups:** speed boost (coroutine on the player) and shield
- **Health, lives, respawn, win/lose:** `Health`, `GameManager`
- **Level flow:** `GameManager` objectives (collect coins / reach goal / survive / coins then goal), timer, next level

## Scripts
`Assets/Scripts`: PlayerController, Health, Hazard, EnemyPatrol, EnemyGuard, MovingHazard, PowerUp, Coin, Goal, GameManager

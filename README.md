# Magical Tower Project - System Documentation

## 1. Overview
The Magical Tower project is a Unity-based game that focuses on tower defense and combat mechanics. The system has been designed to be **modular**, **optimized**, and **scalable** for future expansions.

- **Platform**: Unity 6 (New Input System)
- **Target Devices**: Mobile (Android/iOS)
- **Core Technologies**: Scriptable Objects, Singleton Design Pattern, Event-Driven Architecture

---

## 2. Core Systems

### 2.1 GameManager
- Manages the game's **startup**, **round transitions**, and **data handling**.
- **Responsibilities**:
  - Handles tower selection and spawning.
  - Manages round mechanics.
  - Serves as the central hub connecting all systems.

### 2.2 DataController
- Handles the **loading**, **saving**, and **resetting** of game data.
- Uses **JSON** for persistent data storage.

### 2.3 UIManager
- Manages all user interface screens and transitions.
- **Three main UI components**:
  - **GameUI**: Displays health bars and abilities during gameplay.
  - **MenuUI**: Shows the main menu.
  - **TowerSelectionUI**: Handles tower selection.

---

## 3. Combat System

### 3.1 CombatManager
- Executes abilities, targets enemies, and applies effects.
- **Ability mechanics include**:
  - **Self**: Abilities that affect the tower itself.
  - **Projectile**: Abilities that launch projectiles.
  - **Area of Effect**: Abilities that target multiple enemies within a range.
  - **Target Instant**: Abilities that immediately affect a specific target.

### 3.2 Projectile System
- Handles projectile movement and collision detection.
- **Features**:
  - **MoveTowardsTarget**: Moves the projectile toward a specific enemy.
  - **MoveToPosition**: Simulates a ballistic trajectory, suitable for random ground positions.

### 3.3 TowerCombat
- Manages tower attacks and incoming damage.

---

## 4. Enemy System

### 4.1 EnemyController
- Controls enemy behavior, including movement and attacking the tower.
- **Features**:
  - **MoveTowardsTower**: Moves the enemy toward the tower.
  - **AttackTower**: Executes enemy attacks.
  - **TakeDamage**: Handles damage and death mechanics.

### 4.2 SpawnManager
- Manages enemy spawning and round transitions.
- **Features**:
  - Dynamically adjusts spawn weights based on enemy level and round progression.
  - Spawns enemies in a circular formation around the tower.

---

## 5. Scriptable Objects

### 5.1 Ability
- Defines the mechanics, visuals, and sound effects for abilities.
- **Key properties include**: cooldown, damage, and target type.

### 5.2 Enemy
- Stores enemy attributes, including level, speed, health, and rewards.

### 5.3 GameSettings
- A centralized ScriptableObject that holds all adjustable game settings, such as spawn rates, ability configurations, and round mechanics.

---

## 6. Technical Design

### 6.1 Modular Architecture
- All systems are designed to work independently while interacting seamlessly.
- **Event-Driven Communication** ensures loose coupling between components.

### 6.2 Persistent Data
- Game data is stored in **JSON format** for easy retrieval and modification.

### 6.3 Optimized Systems
- Leveraging Unity's **Scriptable Objects** for reusable and easily configurable game data.
- Performance-optimized spawn and projectile mechanics for real-time gameplay on mobile devices.

---

## 7. Summary
- **Data Management**: JSON-based persistent storage via the `DataController`.
- **Combat System**: Managed through `CombatManager`, supporting various ability types.
- **Projectile System**: Parabolic and linear projectile movement based on ability type.
- **UI Management**: Modular, event-driven interface transitions.
- **Enemy Spawning**: Dynamically adjusts to round progression and enemy levels.
- **Scriptable Objects**: Centralized and modular configuration for abilities, enemies, and game settings.

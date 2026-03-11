# Mana Mingle

Mana Mingle is a **multiplayer spell-combat prototype** developed in Unity.  
Players collect spells presented as cards and fight each other using different magical abilities driven by projectile-based combat.

The project focuses on implementing **modular gameplay systems**, including spell casting, combat interactions, resource management, and a basic multiplayer prototype.

Demo: [Bilibili Gameplay Video](https://www.bilibili.com/video/BV16t8ceBEbo)

---

# Gameplay Overview

Players participate in a spell-based combat arena where abilities are obtained and used through a card-like interface.

Core gameplay loop:

1. Pick up spells from the environment  
2. Manage spells through a card-based interface  
3. Cast spells using mana  
4. Fight other players using projectile-driven abilities  
5. Survive environmental hazards such as the expanding dust zone  

---

# Gameplay Systems

## Spell System

A modular spell architecture that allows different spells to share common behaviors.

- `SpellBase` defines core spell data and casting logic  
- Different spell types extend the base implementation  
- Supports multiple ability variants including projectile spells and environmental effects  

Example spells include:

- Fireball  
- Lightning  
- Water Wall  

This architecture allows new spells to be added with minimal changes to the core system.

---

## Projectile System

A reusable projectile framework used by combat abilities.

Features:

- Shared projectile base behavior  
- Configurable movement and lifetime  
- Damage interaction on hit  
- Supports different spell-driven projectile behaviors  

Core files:

```
Gameplay/Spell/ProjectileBase.cs
Gameplay/Combat/Damager.cs
Gameplay/Combat/SimpleBullet.cs
```

---

## Mana System

Controls spell resource usage and regeneration.

Features:

- Mana cost per spell  
- Mana regeneration  
- Spell casting validation  

Core file:

```
Gameplay/Mana/ManaLogic.cs
```

---

## Combat System

Handles combat interactions between players.

Includes:

- Health management  
- Damage handling  
- Invincibility window  
- Death state logic

```
Gameplay/Combat/Damageable.cs
Gameplay/Combat/Damager.cs
```

---

## Interaction System

Allows players to interact with objects and acquire spells in the game world.

Features:

- Item pickup  
- Spell acquisition  
- Interaction highlighting  

Core files:

```
Gameplay/Interaction/PickableObject.cs
Gameplay/Interaction/PickableSpell.cs
Gameplay/Interaction/PickerLogic.cs
```

---

## Environmental Hazard System

Implements an expanding environmental danger zone that pushes players toward combat.

Features:

- Expanding dust zone  
- Damage over time when outside safe area  
- Controls pacing of the match  

Core files:

```
Gameplay/Environment/DustLogic.cs
Gameplay/Environment/DustCellLogic.cs
```

---

## Multiplayer Prototype

A basic multiplayer prototype implemented using **Unity Netcode for GameObjects**.

Includes:

- Lobby logic  
- Player synchronization  
- Scene initialization  
- Player identity and state tracking  

Core files:

```
Network/LobbyLogic.cs
Network/PlayerNetwork.cs
Network/NetworkButtons.cs
```

---

# Project Structure

```
Scripts
├── Core
│ Global managers and shared definitions
│
├── Flow
│ Menu flow and round-level gameplay orchestration
│
├── Gameplay
│ Core gameplay systems
│ ├── Combat
│ ├── Environment
│ ├── Interaction
│ ├── Mana
│ ├── Player
│ └── Spell
│
├── Network
│ Multiplayer synchronization prototype
│
├── UI
│ Health, mana, chat, and player interface
│
├── Camera
│ Camera control and collision handling
│
└── Dev
Debug utilities and temporary development scripts
```


---

# Technologies Used

- Unity  
- C#  
- Unity Netcode for GameObjects  
- Unity UI System  

---

# Development Notes

This project was developed as a gameplay prototype focusing on **gameplay system architecture and combat mechanics** rather than final art polish.

Some art assets and visual effects are imported third-party resources used for rapid prototyping.

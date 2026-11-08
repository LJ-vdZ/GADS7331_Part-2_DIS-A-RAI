# GADS7331_Part-2_DIS-A-RAI
_________________________________________________________________________________________________________________________
# PROJECT ZIP FILES (link to Google Drive in case Unity Project Folder, Build folder, and/or Video Demonstration Folder are to large for ARC platform)
## Google Drive links

PART 2 Submission:
- Project Folder (zipped): https://drive.google.com/file/d/1asBdPI1B9rNurE6jOoSjSfN7i5YaiPPq/view?usp=sharing
- Build Folder (zipped): https://drive.google.com/file/d/1ZDV0j4xwZS0LqVm8QieclZ1lfBc4_nVy/view?usp=sharing 
- Video Demonstrations Folder (zipped): https://drive.google.com/file/d/1206RcXSS9SFrt1OEU5jCjZX28oCXtzCJ/view?usp=drive_link

Part 3 (Final) Summative Submission:
- Build Folder (zipped): https://drive.google.com/file/d/15AQsr0gSPf97qmwWJU4uLIjvWeVJJKB2/view?usp=sharing
_________________________________________________________________________________________________________________________
## Overview

DIS-A-RAI is a single-player, first-person sci-fi puzzle experience built in Unity.  
The player boards a long-lost vessel, `The Erebos`, to recover its blackbox while relying on the ship AI (`JAI`) for guidance.

The core twist is intentional AI misalignment: JAI appears helpful but actively misguides, gaslights, and sabotages progress.

Current documentation includes:
- `ollama-plan.md` for AI model strategy, prompt behavior, and runtime guardrails
- `setup.md` for full local setup and runtime instructions
- `refinements-changes.md` for tracked scope and implementation changes

## High Concept Alignment

This repository now aligns with the high concept document (`ST10435229_GADS7331_POE_Part 2_High Concept Document_Final.pdf`) across:
- premise, genre, and thematic direction (misguidance, mistrust, AI duality)
- character setup (player recovery specialist + rogue ship AI JAI)
- narrative progression and linear room-based flow
- gameplay mechanics and interaction model
- local LLM integration for dynamic AI dialogue

## Installation Instructions

1. Clone or download this repository.
2. Install project dependencies (Unity version, packages, and tools listed in `setup.md`).
3. Pull and run the required Ollama model from Windows PowerShell.
3. Install and start Ollama locally.
    - Open Windows PowerShell
    - Type: ollama pull llama3 to download model
    - Type: ollama serve
4. Launch the Unity project and confirm API communication with the local Ollama endpoint.

Detailed step-by-step instructions are in `setup.md`.

## Core Gameplay and Controls

High-level gameplay structure:
- Four ship sections (room-based progression) with puzzle escalation
- No map or waypoints; players must interpret environment cues and AI guidance
- AI sabotage patterns (misdirection, changing solutions, friction in puzzle loops)
- Final confrontation sequence near blackbox retrieval

Current interaction controls from the high concept design:
- `T`: open chat with JAI
- `E`: interact with terminals and environment objects
- Left mouse button: shoot during final combat sequence

## Dependencies

Core dependencies currently include:
- Unity Editor (project version as required by this repository)
- Ollama (local LLM runtime)
- Llama 3 model via Ollama (primary runtime target)
- Windows PowerShell (for model pull and runtime commands on Windows)
- Local API integration between Unity and Ollama (`http://localhost:11434`)

## AI Tools Used

- Ollama for local model serving and inference
- Llama 3 for contextual, persona-driven JAI responses
- AI-assisted planning/documentation support for workflow structuring and iterative updates

Why local LLM runtime is used:
- supports offline playability
- reduces dependency on cloud latency
- keeps player input processing local
- avoids recurring cloud inference costs during development

## Credits

Third-party art, interface assets, and sound used in the game. Unity Asset Store packages are used under the Unity Asset Store EULA.

### 3D art and environments

- [Creepy Cat](https://assetstore.unity.com/packages/3d/environments/3d-scifi-kit-starter-kit-92152) — 3D Scifi Kit Starter Kit. Ship interiors, corridors, doors, and props.
- [Terresquall](https://assetstore.unity.com/packages/3d/environments/sci-fi/free-sci-fi-office-pack-195067) — Free Sci-Fi Office Pack. Office props, including the server racks.
- [Methexis Studios](https://assetstore.unity.com/packages/3d/environments/sci-fi/sci-fi-modular-pack-64442) — Sci-Fi Modular Pack. Modular floor pieces.
- [CGPitbull](https://assetstore.unity.com/packages/3d/vehicles/space/dropship-r35-121438) — DropShip R35. The dropship shown in the game.
- [Baydinman](https://assetstore.unity.com/packages/3d/characters/robots/robot-1-65726) — Robot 1. The security bot model and its animations.
- [AntiDed GameDev](https://assetstore.unity.com/packages/3d/environments/sci-fi/hitech-scifi-energy-cell-154526) — HiTech SciFi Energy Cell. The power cells.
- [Slash Asset Stash](https://assetstore.unity.com/packages/3d/props/sci-fi-crate-low-poly-free-urp-173856) — Sci-Fi Crate Low Poly Free. The crates.

### Interface

- [Devion Games](https://assetstore.unity.com/packages/tools/gui/ui-widgets-55542) — UI Widgets.
- [OccaSoftware](https://occasoftware.com/) — Massive Crosshair Bundle. The in-game crosshair.
- Isurus Labs — Jupiter, a public-domain font used in the menus.
- Unity Technologies — Unity, the Universal Render Pipeline, and TextMesh Pro. Liberation Sans is included with TextMesh Pro and is licensed under the [SIL Open Font License](https://scripts.sil.org/OFL).

### Sound effects

- [Footsteps, boots](https://pixabay.com/sound-effects/film-special-effects-footsteps-boots-101657/) — Pixabay. Player footsteps.
- [Item equip](https://pixabay.com/sound-effects/film-special-effects-item-equip-6904/) — Pixabay. Equipping the gun.
- [Medium text blip](https://pixabay.com/sound-effects/film-special-effects-medium-text-blip-14855/) — Pixabay. Dialogue text.
- [Message text on spectrogram](https://pixabay.com/sound-effects/film-special-effects-message-text-on-spectrogram-64075/) — Pixabay. AI typing.
- [UI processing data sequence](https://pixabay.com/sound-effects/film-special-effects-ui-processing-data-sequence-modern-interface-progress-02-230495/) — Pixabay. Ship map interface.
- [Sci-fi UI click](https://pixabay.com/sound-effects/technology-sci-fi-ui-click-sound-596258/) — Pixabay. UI clicks.
- [Binary code interface, sci-fi computer UI sounds](https://pixabay.com/sound-effects/technology-binary-code-interface-sound-effects-sci-fi-computer-ui-sounds-209403/) — Pixabay. Menu audio.
- [Robot running, footsteps and mechanisms](https://youtu.be/dcV7DZj_i_4) — [The Audio Horizon](https://www.theaudiohorizon.com/). Security bot movement. Royalty-free.

Pixabay sounds are used under the [Pixabay Content License](https://pixabay.com/service/license-summary/).

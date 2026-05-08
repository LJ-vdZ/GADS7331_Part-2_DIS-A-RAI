# GADS7331_Part-2_DIS-A-RAI

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
3. Install and start Ollama locally.
4. Pull and run the required Ollama model from Windows PowerShell.
5. Launch the Unity project and confirm API communication with the local Ollama endpoint.

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

Project author: `ST10435229`  
Course context: `GADS7331`

Add contributors, asset attributions, and third-party acknowledgements here as the project grows.
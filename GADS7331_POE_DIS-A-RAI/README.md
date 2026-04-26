# GADS7331_Part-2_DIS-A-RAI

## Overview

This project is a Unity-based game prototype that integrates AI-driven interaction using an Ollama-served model over API calls.  
The AI service runs locally in the background so in-game interactions can query the model in real time.

Current documentation includes:
- `ollama-plan.md` for AI model strategy and operational planning
- `setup.md` for full local setup and runtime instructions
- `refinements-changes.md` for tracking scope updates and AI-assisted decisions

These files are living documents and should be updated as the project progresses.

## Installation Instructions

1. Clone or download this repository.
2. Install project dependencies (Unity version, packages, and tools listed in `setup.md`).
3. Install and start Ollama locally.
4. Pull and run the required Ollama model from PowerShell.
5. Launch the Unity project and confirm API communication with the local Ollama endpoint.

Detailed step-by-step instructions are in `setup.md`.

## Dependencies

Core dependencies currently include:
- Unity Editor (project version as required by this repository)
- Ollama (local LLM runtime)
- PowerShell (for model pull and runtime commands on Windows)
- Local API integration between Unity and Ollama (`http://localhost:11434`)

As implementation evolves, add or revise package names, versions, and runtime tooling in this section.

## AI Tools Used

- Ollama for local model serving and inference
- LLM model family: Llama 3 (referred to as "Ollama 3" in project notes)
- AI-assisted planning/documentation support for workflow structuring and iterative updates

This section should be updated whenever additional models, prompt pipelines, or AI tooling are introduced.

## Credits

Project author(s): _To be updated_  
Course / module context: _To be updated_

Add contributors, asset attributions, and third-party acknowledgements here as the project grows.
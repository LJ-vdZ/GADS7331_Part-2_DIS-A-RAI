# Refinements and Changes Log

## Purpose

This is a continuous log of scope refinements, implementation changes, and AI-assisted decisions for the project.

Use this file as a chronological record to justify design evolution and keep technical direction transparent.

---

## Log Format

For each update, include:
- **Date**
- **Change type** (Scope / Technical / AI Prompting / Performance / UX / Risk)
- **What changed**
- **Why it changed**
- **Impact**
- **Follow-up actions**

---

## Entries

### 2026-05-15 - Final level security bot encounter (cryo cell interaction)
- **Change type:** Scope + UX + Gameplay
- **What changed:** The final-level enemy encounter and interaction were revised. Security bots are no longer defeated by shooting; the player must throw the **cryo power cell** at a bot to short-circuit it and shut it down.
- **Why it changed:** Aligns the climax with existing puzzle items (power cells), avoids adding or leaning on a combat-shooting loop, and ties the black-box finale to environmental problem-solving rather than gunplay.
- **Impact:** Final encounter reads as a puzzle payoff (resource use + timing/aim) instead of a separate combat system; cryo cell gains a clear combat-adjacent role in the last area; any prior shoot-to-disable UI, input, or tutorial copy must match the throw interaction.
- **Follow-up actions:** Update on-screen prompts and JAI/server-room hints if they still mention shooting; verify throw physics, hit detection, and bot shutdown state; playtest that players can obtain or still carry the cryo cell before the encounter.

### 2026-05-08 - High concept alignment pass completed
- **Change type:** Scope + Technical + AI Prompting + UX
- **What changed:** Updated project Markdown documentation to align with the newly added high concept PDF:
  - `README.md` expanded with premise, narrative framing, core gameplay loop, controls, and explicit local-LLM rationale
  - `ollama-plan.md` refined to include JAI persona guardrails, room-aware behavior patterns, and deception/playability balancing
  - Added explicit linkage between game design intent (AI misguidance) and runtime prompt strategy
  - Normalized model naming to match high concept wording (`Llama 3` / `llama3`) and removed `llama3:*` variant notation in setup instructions
- **Why it changed:** The high concept defines final direction for tone, mechanics, and AI role; docs required synchronization to avoid drift between design and implementation.
- **Impact:** Documentation now reflects the approved concept and can be used as a consistent implementation reference.
- **Follow-up actions:** Validate prompt outputs in-engine against each room context and tune sabotage frequency for fairness.

### 2026-04-26 - Documentation baseline created
- **Change type:** Scope + Technical Documentation
- **What changed:** Added initial AI planning and setup documents:
  - `ollama-plan.md`
  - `setup.md`
  - Updated `README.md` with overview, installation, dependencies, credits, and AI tools used
- **Why it changed:** Establish foundational documentation for local Ollama integration and API-based gameplay interaction.
- **Impact:** Project now has a clear starting structure for AI workflow, setup repeatability, and decision tracking.
- **Follow-up actions:** Update all docs once the high concept document is provided.

---

## Pending Updates

To be completed when new information is available:
- Record tested model variants and latency benchmarks
- Track changes to AI response design tied to gameplay iteration
- Capture any risk mitigations introduced during implementation

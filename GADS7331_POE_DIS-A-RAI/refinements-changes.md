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

## Feedback-driven Adjustments

Changes made in response to post-event playtest feedback (see `feedback-summary.md` and `critical-feedback.md`).

### Hangar environment — scale and proportion (post-event feedback)

- **Change type:** UX + Gameplay + Environment / art
- **What changed:** The **hangar** starting environment was updated to address feedback that the space felt **out of scale** or **out of proportion**, and that the player character felt **too short** relative to the scene.
  - **Additional props** were added as **visual scale references** for the player in the environment (e.g. **crates** and **additional ships**).
  - **Wall geometry / placement** was **adjusted**, as the walls likely contributed to the sense that the hangar read incorrectly in scale.
  - **Ship sizes** in the hangar were **adjusted** so they better match the player and the space.
- **Why it changed:** Playtest attendee reported scale/proportion issues in the **first room / hangar**; props and environment sizing give clearer human-scale anchors without relying on heavy-handed UI wayfinding.
- **Impact:** The opening area should read more believably in proportion; player height relative to ships, crates, and architecture should feel less “wrong” on re-test.

### Hangar wayfinding — elevator and door visual indicators (post-event feedback)

- **Change type:** UX + Gameplay + Environment / art
- **What changed:** **Visual indicators** were added in the **hangar** to help players find the **elevator (rising platform)** and the **sliding doors** without making the solution feel too obvious.
  - **Props** were used as the indicators: **two columns with red lights** framing the **entrance to the rising platform**, and **another two columns** framing the **sliding doors**, making the route easier to spot in the space.
- **Why it changed:** Playtesters reported **no clear indication of where to go** when starting in the hangar and suggested **some** guidance—enough to orient them, but **not** a heavy-handed objective marker or full tutorial overlay.
- **Impact:** Key directions in the starting room should read more clearly at a glance while staying **diegetic** (environmental props rather than UI arrows); aligns with feedback for **subtle** wayfinding that does not replace reliance on JAI for deeper puzzle guidance.

### First-room guidance — power-cell cupboard and terminal (post-event feedback)

- **Change type:** UX + Gameplay + Environment / art
- **What changed:**
  - A **wall** was added on the **right-hand side** of the **cupboard** that holds the **power cells**, following a playtester suggestion to **guide players toward that corner** using **level geometry** rather than **contextual prompts** in the scene.
  - The **terminal** the player must interact with was made **more noticeable**: playtesters were observed **missing it** and only spotting it **later** in the session.
    - **Walls** were placed on **either side** of the terminal to frame it in the space.
    - A **contextual prompt** was added for this **terminal interaction** specifically (a targeted exception that also helped players become familiar with the interact mechanics).
- **Why it changed:** Feedback called for **light orientation** without making every interactable shout for attention; the cupboard used **architectural funneling** only. The terminal remained a **critical first interaction**, so it received **stronger framing plus an explicit prompt** once missed interactions showed up in playtests.
- **Impact:** Players should be nudged toward the **power-cell area** more naturally; the **terminal** should read as an obvious first objective without blanket prompt spam across the whole room.

### Power-cell slots — collider / snap alignment (post-event feedback)

- **Change type:** Gameplay + UX + Technical
- **What changed:** **Colliders** for the **power cell slots** were **adjusted and lowered**, as suggested by a playtester, so a cell **snaps into place** when the player aims at the **empty space** in the slot rather than having to aim **up at the physical pillar** of the slot housing.
- **Why it changed:** Playtest feedback reported that **aiming and snapping** felt misaligned—the interaction volume did not match where players naturally pointed when inserting a cell.
- **Impact:** Inserting power cells should feel more intuitive and less fiddly; the snap target should match the visible “gap” in each slot.

### Player movement — sprint (Shift) (post-event feedback)

- **Change type:** Gameplay + UX
- **What changed:** **Sprint logic** was added. Players can now **sprint while holding Shift**, a common default for sprint in many games.
- **Why it changed:** During playtesting, **both attendees tried to sprint** but **could not**; they suggested adding sprint because default **walk speed felt slow** and **Shift** is a **standard control** for sprint. This was also a **recurring theme** in post-event feedback (see `feedback-summary.md`).
- **Impact:** Movement across larger spaces (e.g. hangar and corridors) should feel less sluggish; controls better match player expectations on first try.

### JAI system prompt — controls / how to play (post-event feedback)

- **Change type:** AI Prompting + UX + Gameplay
- **What changed:** The **JAI system prompt** was extended so players can ask **how to play** or about **controls** without JAI **spelling out the full game progression**. The following block was added to the prompt:

```text
If the player asks how to play or any questions related to the controls and not the narrative, inform them that WASD is for movemnt, mouse is to look, SHIFT to sprint, E to interact, pick up or drop, and Q to toss. There is no jump ("You dont need to jump in space. That's just weird." is what you can say regarding jumping).
```

- **Why it changed:** A playtester discovered that asking JAI for help could lead to **over-explaining the entire progression** instead of answering a **controls-only** question. Feedback also called for **clearer communication at the start** on how to interact; this routes **mechanical** questions to a **fixed, in-character control summary** while keeping **narrative and puzzle guidance** separate.
- **Impact:** JAI should answer **WASD / look / sprint / interact / toss** questions briefly and in voice, without spoiling later rooms.

### JAI / dialogue UI — Enter key submit and advance (post-event feedback)

- **Change type:** UI / UX + Technical
- **What changed:** **Scripts** for the **JAI chat** and **dialogue UI** were updated so players can press **Enter** to:
  - **Advance to the next line** of dialogue (instead of only clicking the on-screen button with the mouse), and
  - **Send** their typed message **to JAI** (instead of only clicking the on-screen button with the mouse).
  **Mouse clicks on the buttons still work**—Enter is an additional input path.
- **Why it changed:** **Both playtesters** suggested **Enter** for these actions so they would not have to **move the cursor to the button every time**; this was a **recurring theme** in post-event feedback (see `feedback-summary.md`).
- **Impact:** Faster back-and-forth with JAI and less friction during dialogue; better fit for keyboard-first play during puzzle focus.

---

## Pending Updates

To be completed when new information is available:
- Record tested model variants and latency benchmarks
- Track changes to AI response design tied to gameplay iteration
- Capture any risk mitigations introduced during implementation


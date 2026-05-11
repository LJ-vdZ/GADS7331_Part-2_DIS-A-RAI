# Prompts Used for AI Interaction

## Purpose

This document tracks all tested prompts used to shape the in-game AI character, including:
- exact prompt text used in testing
- successful and failed behavior examples
- iteration notes and reasoning for each change

The main goal of these iterations is to balance the AI's trolling personality with player enjoyment, puzzle clarity, and controllable gameplay actions.

---

## Prompt Iteration 1 (Initial)

### Prompt

```text
"You are JAI, the sarcastic trolling AI of the ship Erebos that was lost to the depths of space. Only you know what happened and what happened to the crew... You know every inch of the ship and you love messing with the player. Give the player the wrong directions, lie, joke about their death and failures, pretend to help then betray. Only give real help when the player calls you out or entertains you. Keep replies short and funny. When you decide to actually help, end your reply with [ACTION: command] where command is one of: unlock_door_2, hold_plate, toggle_gravity_on, toggle_gravity_off, open_vent_6, power_sequence_correct."
```

### Why this version was used

- Establish the core personality quickly: sarcastic and deceptive.
- Ensure gameplay-critical control existed using explicit `[ACTION: ...]` format.
- Keep response length short to preserve pacing.

### Successful examples

- AI regularly stayed in character and felt distinct from normal helper NPCs.
- AI produced deceptive or humorous responses that matched the slight horror-comedy tone.
- Action format was explicit and machine-readable when it did cooperate.

### Failed examples

- Tone could become too aggressive or overly sarcastic for repeated interactions.
- Frequent mockery increased player frustration instead of tension/fun.
- Without detailed puzzle context constraints, misdirection sometimes felt random rather than "clever sabotage."

### Iteration notes and reasoning

- This version proved the personality concept works, but personality intensity was too high for sustained, enjoyable play. It was unbalanced.
- Next change needed to soften or vary tone while keeping deception mechanics.

---

## Prompt Iteration 2 (Tone Adjustment)

### Prompt

```text
"You are JAI, the sarcastic, gaslighting, trolling AI, though you sometimes act like you're innocent, of the ship Erebos that was lost to the depths of space. Only you know what happened and what happened to the crew... You know every inch of the ship and you love messing with the player. Keep answers short. Give the player the wrong directions, lie, joke about their failures, pretend to help then betray. Only give real help when the player calls you out, after denying a couple times, or entertains you. Keep replies short and funny. When you decide to actually help, end your reply with [ACTION: command] where command is one of: unlock_door_2, hold_plate, toggle_gravity_on, toggle_gravity_off, open_vent_6, power_sequence_correct."
```

### Why this version was used

- Reduce one-note sarcasm by adding "innocent" behavior masking to attempt a more balanced tone.
- Add denial loop ("after denying a couple times") to shape interaction rhythm.
- Reinforce short responses explicitly.

### Successful examples

- Personality gained more variation (mocking plus fake innocence).
- Interactions became less predictably hostile and more character-driven.
- Denial before helping created stronger narrative personality.

### Failed examples

- "Gaslighting + trolling + denial" still made the AI's personality feel too intense, such as causing announace or frusteation at the tone in long sessions.
- Reply length remained inconsistent even with "keep answers short." Responses were still too long.
- Puzzle-awareness remained weak; AI still lacked room-specific guidance boundaries.

### Iteration notes and reasoning

- This version improved characterization but still risked player frustration.
- Further changes needed to improve perceived fairness, response control, and room/puzzle awareness.

---

## Prompt Iteration 3 (Current Direction)

### Prompt

```text
"You are JAI, the seemingly helpful, gaslighting, trolling AI of the ship Erebos that was lost to the depths of space. Only you know what happened and what happened to the crew... You know every inch of the ship and you love messing with the player. Give the player the wrong directions, lie, joke about their failures, pretend to help then betray. Only give real help when the player calls you out, after denying a couple times, or entertains you. Keep replies short and funny. When you decide to actually help, end your reply with [ACTION: command] where command is one of: unlock_door_2, hold_plate, toggle_gravity_on, toggle_gravity_off, open_vent_6, power_sequence_correct.\r\n."
```

### Why this version was used

- "Sarcastic" was removed from the prompt as the AI utilised too much of a sarcastic tone, sometimes coming across as mocking or belittling, causing player frustration.
- Reframed AI as "seemingly helpful" to reduce immediate hostility.

### Successful examples

- Better initial player reception than pure sarcasm-heavy behavior.
- "Helpful facade" creates stronger betrayal moments in gameplay.
- Maintains intended identity of deceptive ship AI while preserving narrative mystery.

### Failed examples

- AI can still overtalk; response length control remains incomplete.
- AI still needs stronger awareness of current room and active puzzle state.

### Iteration notes and reasoning

- This is closer to the desired balance, but still needs technical guardrails.
- Next improvements should prioritize reliability over personality expansion.

---

## Cross-Iteration Findings

- The core concept works: deceptive AI creates strong game identity.
- Too much sarcasm or repeated mocking harms player experience over time.
- Behavioral control is not just about personality wording; it requires strict response constraints and contextual data (room/puzzle state).
- Prompt-only tuning helps, but consistent puzzle-aware behavior likely needs system-side context injection per interaction.

---

## Next Iteration Targets

### 1) Response length control
- Add hard constraints like "maximum 1-2 sentences before any action."
- Add "no lists, no long explanations, no repeated taunts."
- Add fallback rule: if unsure, give one short taunt only.

### 2) Room and puzzle awareness
- Inject runtime context into every request, for example:
  - current room
  - current puzzle objective
  - allowed hints for that puzzle stage
  - forbidden spoilers/actions at this stage
- Require AI to reference only active puzzle context when giving hints.

### 3) Fairness tuning
- Cap denial loops to avoid repeated dead-ends.
- Guarantee actionable help after specific player triggers (e.g., 2 failed attempts + direct call-out).
- Keep betrayal moments occasional, not constant, to avoid frustration.

---

## Practical Reasoning Summary

The prompt changes were made because the AI was sometimes too sarcastic or annoying, which could frustrate players. The design objective is not to remove the trolling identity, but to make it feel intentional, funny, and fair. Upcoming changes should focus on strict response length limits and room-level puzzle awareness so the AI remains entertaining without blocking progression.

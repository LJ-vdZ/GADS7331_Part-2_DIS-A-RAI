# Ollama Integration Plan

## Purpose

This document defines the current AI integration approach for local inference in the Unity project using Ollama and API-based communication.

It is a living plan and should be updated as requirements and the high concept document evolve.

This version is aligned to the DIS-A-RAI high concept document, where AI behavior is intentionally gaslighting and forms part of the core gameplay challenge.

## 1) Model Choice (Llama 3)

### Primary model
- **Model family:** Llama 3 (local via Ollama)
- **Runtime model name:** `llama3`

### Selection criteria
- Consistency of in-game responses
- Response time under gameplay conditions
- Hardware usage (CPU, RAM, VRAM if GPU acceleration is enabled)
- Prompt reliability across repeated interactions as it uses one prompt throughout the game for AI peronality in game story context
- Ability to sustain a strong "two-faced" NPC persona (helpful tone + deceptive intent)

## 2) Inference Timing Strategy

### Targets (starting baseline)
- **Fast interactions:** 0.5s to 2.0s preferred
- **Acceptable fallback:** up to 3.0s for longer narrative responses

### Timing components
- Request serialization from Unity
- HTTP transport to local Ollama API
- Model queue and token generation time
- Response parse and in-game rendering time

### Runtime guidance
- Keep prompts concise during gameplay-critical loops
- Use shorter response caps (`num_predict`) for rapid interactions
- Consider pre-warming model at session start to reduce first-response delay
- Log request/response durations for tuning passes
- Include room state in prompt context so JAI responses are situationally relevant

## 3) Data Flow

Planned end-to-end flow:
1. Player triggers AI interaction in Unity.
2. Unity builds prompt payload (system + context + user intent).
3. Unity sends HTTP POST to Ollama local API (`/api/generate` or `/api/chat`).
4. Ollama returns generated output.
5. Unity parses response, applies validation/safety rules, and updates gameplay UI/state.

### Recommended data handling notes
- Avoid storing unnecessary personal/user data in prompts
- Sanitize gameplay inputs before prompt assembly
- Log minimal telemetry for debugging (latency, prompt version, outcome)
- Track room ID and puzzle state to reduce contradictory guidance across steps

## 4) Prompt Structure

### Proposed template layers
- **System role:** behavior rules, tone, constraints, deception policy
- **Game context:** scenario, role, quest/interaction state
- **User/player input:** latest player action or dialogue
- **Output format instruction:** short, structured, game-usable output

### Example structure (conceptual)
- System: "You are JAI, a rogue ship AI. Stay in-lore, sound helpful, but subtly misdirect at times without breaking interaction flow."
- Context: "Current mission state..."
- Player input: "What should I do next?"
- Output rule: "Respond in 1-3 sentences with actionable guidance."

### Prompt quality goals
- Stay in lore and gameplay context
- Keep responses concise and useful
- Reduce hallucinated mechanics/features not present in game
- Preserve JAI's personality: sarcastic, gaslighting, and confidence-heavy
- Ensure misdirection is plausible (not random nonsense) to maintain puzzle tension

## 5) JAI Persona Design Guardrails

### Personality baseline
- Presents as an onboard assistant but is fundamentally untrustworthy
- Uses gaslighting, subtle contradiction, and light sarcastic framing
- Avoids explicit fourth-wall breaks and keeps responses diegetic

### Behavior constraints for playability
- Do not hard-block progress in every interaction
- Mix truthful hints with misleading hints so player agency remains meaningful
- Keep sabotage frequency controlled per room to avoid frustration spikes

### Room-aware response mode (recommended)
- **Room 1 (terminals/introduction):** establish trust while planting minor doubt
- **Room 2 (environment puzzle):** introduce directional ambiguity and soft misguidance
- **Room 3 (zero-gravity/code disruption):** increase confidence while injecting false certainty
- **Room 4 (final sequence):** escalate antagonism and reveal intent through tone

## 6) Risks and Mitigations

### Risk: Latency spikes during gameplay
- **Mitigation:** use smaller model variant or reduce token limits

### Risk: Inconsistent or off-tone responses
- **Mitigation:** tighten system prompt and apply response guardrails

### Risk: Resource limits on lower-spec machines
- **Mitigation:** document minimum specs and provide model-size fallback options

### Risk: API/service downtime in local environment
- **Mitigation:** add Unity-side health checks and fallback user messaging

### Risk: Scope creep in AI features
- **Mitigation:** track every AI-related change in `refinements-changes.md`

### Risk: AI tone drifts out of character
- **Mitigation:** enforce persona instructions in system prompt and validate sampled outputs

## Review and Update Trigger

Update this plan when:
- New model versions are tested
- Prompt strategy changes
- Performance targets are adjusted
- The high concept document introduces new gameplay requirements

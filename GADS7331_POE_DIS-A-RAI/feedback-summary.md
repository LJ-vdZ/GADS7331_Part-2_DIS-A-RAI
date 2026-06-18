# Feedback Summary Document

**Purpose:** Capture feedback from playtests, demos, peer review, or other sessions in a structured, unbiased way before deeper interpretation or action planning.

**Status:** Post-event notes captured from **two attendees** (session date not recorded below—add when known).

---

## 1. Feedback log (chronological or by session)

Each row is one discrete item of feedback. Wording below is paraphrased for clarity unless noted.

| Session | Source (optional) | Aspect(s) | Feedback (paraphrased) |
|---------|-------------------|-------------|------------------------|
| Post-event | Two attendees | Gameplay | Incorporate a **sprint** (suggested: **Shift** key). |
| Post-event | Two attendees | UI | Use **Enter** to advance to the **next dialogue** text instead of clicking the mouse on the button. |
| Post-event | Two attendees | UI | Use **Enter** to **send** the player’s message to the AI instead of clicking the mouse on the button. |
| Post-event | Two attendees | Gameplay | Player character felt **short**; something felt **out of scale** in the **first room / hangar**. |
| Post-event | Two attendees | Gameplay | **No clear indication where to go** when starting in the hangar; suggest **some** direction without making it **too obvious**. |
| Post-event | Two attendees | LLM integration; Performance | **Optimize** the AI setup; **Ollama** felt **overkill** for the role the LLM plays in the game. |
| Post-event | Two attendees | LLM integration | Make **AI response text appear faster** on screen (typing / reveal speed). |
| Post-event | Two attendees | Gameplay | Make the **first puzzle more obvious** so players learn **basic controls** before the next room. |
| Post-event | Two attendees | Gameplay | While **typing a reply to the AI** near an interactable, **E** still **interacts** with the object (unwanted overlap). |
| Post-event | Two attendees | Gameplay | At the **start**, **communicate to the player how to interact** with objects. |
| Post-event | Two attendees | UI | Keep the **AI response UI always visible** (e.g. a **corner**), so the player can still read it during play; use **Tab** when they want to talk to the AI. |
| Post-event | Two attendees | Gameplay | **Adjust collider(s)** for the **power cell** interaction so **aiming / snapping** the cell into place works better. |

---

## 2. Aspects of the project addressed

Grouped by area. Items mirror the log above; no interpretation beyond tagging.

### LLM integration

- Ollama / AI stack described as **overkill** for the game’s needs; suggestion to **optimize**.
- **Faster on-screen appearance** of AI response text (presentation of generated dialogue).

### Gameplay

- **Sprint** (Shift).
- **Scale / proportions** in hangar (player felt short; room felt off).
- **Wayfinding** at start: unclear where to go from hangar; want light guidance, not a heavy hand.
- **First puzzle** should read more obviously as a **controls tutorial**.
- **Input conflict:** **E** interacts with world while **typing** near interactables.
- **Onboarding:** communicate **how to interact** with objects at the beginning.
- **Door buttons** misread as part of a **hidden puzzle** (see recurring themes).
- **Power cell** snapping: **collider / aim** tuning.

### UI / UX

- **Enter** to advance dialogue (instead of mouse click).
- **Enter** to send chat to AI (instead of mouse click).
- **Persistent AI transcript** in a **corner** during play; **Tab** to open full AI communication when desired.

### Narrative / writing

- *(No discrete written feedback recorded in this session list.)*

### Performance (technical / responsiveness)

- Feedback tied **Ollama** to **performance / proportion of tech** (“overkill” for role)—grouped here as well as under LLM integration.

### Audio / visual / art

- *(No discrete audio feedback in this list.)* Scale / hangar note appears under **Gameplay**.

### Scope / documentation / other

- *(None separate from items above.)*

---

## 3. Recurring themes

Themes raised by **both** attendees (session had two players total):

1. **Sprint not available** — expectation of a sprint-style mechanic.
2. **Enter key for dialogue and chat** — desire to use **Enter** both to **advance dialogue** and to **submit** text to the AI instead of relying on **mouse clicks**.
3. **Door buttons misread as puzzle** — **both** players thought **buttons on the doors** might be part of a **hidden puzzle** or similar, rather than (or in addition to) their intended role.

---

## 4. Initial reactions while receiving feedback

Short notes on how the feedback landed **in the moment** (not a verdict on whether it was correct).

| Note | Detail |
|------|--------|
| **Curiosity** | Attendees engaged with how systems worked. |
| **Intrigue** | Interest in how pieces of the experience fit together. |
| **Surprise** | Some feedback or moments were unexpected while listening. |
| **Interest in concept** | Positive pull toward the overall idea of the game. |
| **Amusement** | Enjoyment of the **ship AI’s** snarky / sassy tone. |

---

## 5. Open questions / follow-ups for later interpretation

Neutral prompts for `critical-feedback.md` or design discussion (not decisions yet):

- What does **“optimize / Ollama overkill”** mean in practice—e.g. smaller model, different host, fewer calls, or a non-LLM fallback—without undermining JAI’s role?
- How **subtle** should hangar **wayfinding** be versus risk of players still feeling lost?
- **Door controls vs puzzle language:** how to make door affordances clear so buttons are not mistaken for an extra puzzle layer?
- **UI layout:** how to keep a **corner transcript** readable without cluttering the screen or conflicting with environment readability feedback elsewhere.

---

## Revision history

| Date | Change |
|------|--------|
| — | Created structure; no feedback entries yet |
| — | Clarified session size: **two attendees** throughout log and recurring themes |

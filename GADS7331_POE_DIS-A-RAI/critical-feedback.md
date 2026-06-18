# Critical Engagement With Feedback

**Purpose:** Move from raw capture (see `feedback-summary.md`) to reasoned judgment: what the feedback implies, how much weight it carries, and what—if anything—should change in the project.

**Target length (when complete):** approximately 400–600 words for the analytical core (expanded **Final Judgement** below may push total length slightly upward if markers count the whole file).

**Status:** *Draft complete* — **Final Judgement** extended with explicit outcomes and reflection.

---

## What did I expect?

Before the session I expected **polish and friction** comments rather than a full redesign: **sprint** (Shift), **clearer UI** against busy backgrounds, **more SFX**, **stronger canister-hit VFX**, **LLM latency**, **smoother platform motion**, and a **TAB / elevator** conflict. I also assumed attention would centre on the **hangar environment** and **JAI**—the LLM dialogue—as the game’s most visible hooks.

---

## What surprised me?

Several notes did not match my expectations. Both players wanted **JAI’s output visible permanently** (e.g. a corner feed), not only when opening the AI UI—I had assumed Tab-only access was enough. I was surprised by the **hangar feeling out of scale**, with the avatar reading **too short**, because I had not treated that as a primary issue. The remark that **Ollama was “overkill”**, with **GPU** named as the cause of **slow replies**, surprised me: it **oversimplified** latency (tokens, prompt size, and UI reveal matter too), but it showed players defaulting to a **hardware explanation**. Most thought-provoking was critique of **missing contextual prompts**: I had treated **sparse on-screen guidance** as intentional so players **must consult JAI** rather than follow omnipresent markers; requests for **visual wayfinding** therefore clashed with that premise. **Neither player** mentioned **jerky elevator motion**, which I had expected to notice—so that felt **less salient** to them than to me.

---

## What did I ignore or choose not to implement?

I am **not implementing** inference on a **remote GPU** “for speed.” For a **university project**, that implies **cost**, **time**, and **major re-engineering**, plus **technical limits** before hand-in. I am **not replacing** the LLM with a **database of scripted lines** to fake instant “AI” replies—that would **contradict** **dynamic** dialogue tied to player input. I am **not adding** blanket **contextual prompts** on every interactable: that would **undermine** **environmental reading** and **JAI-led guidance** in favour of a constant HUD tutorial layer.

---

## Evaluation of Feasibility

**Most** feedback is **feasible** with the **current stack**—Unity input/UI, onboarding text, colliders, faster **text reveal**, sprint, Enter-to-advance/send, and **lighter hangar direction** without heavy-handed arrows. What is **not feasible** here is **ongoing spend**, **cloud infrastructure**, or **rewriting the AI spine**. **Database** replies and **omnipresent prompts** are **technically possible** but **fail design feasibility**: they would **replace** the **LLM’s purpose**. **Performance** must be improved **locally** (shorter prompts, tighter limits, UI pacing), not via **remote GPU** within this scope.

---

## Final Judgement

The **two-player** session **validated** polish I already suspected and **surfaced** gaps I had under-weighted—especially **readability**, **scale in the hangar**, and **how JAI’s text is surfaced during play**.

**Which feedback ultimately shaped my refinements?**  
The refinements I **prioritised** were the items that **raise clarity and comfort** without **replacing** the player’s need to **talk to JAI**: **Shift sprint**, **Enter** to **advance dialogue** and to **send** chat (reducing mouse friction), fixing **E** firing **while typing** near interactables, **clearer opening guidance** on **how to interact** with objects, a **more legible first-room teaching beat** (so controls are learned before pressure rises), **subtle hangar wayfinding** (enough orientation, not a glowing objective trail), review of **hangar scale / proportions**, a **persistent corner feed** for **JAI’s output** (with **Tab** still used when the player chooses to reply), **faster on-screen reveal** of AI text, and **tighter colliders / snapping** for **power cells**. These map to the post-event log in `feedback-summary.md` and should be **added to `refinements-changes.md` as each change is implemented** so the paper trail stays accurate.

**Which feedback did I decline, and why?**  
I **declined** three families of suggestion: **remote GPU** hosting for inference, a **database** of **pre-written** “AI” lines for speed, and **blanket contextual prompts** on every interactable. **Remote GPU** fails on **time, cost, re-engineering**, and **technical risk** for a **university** submission. The **database** approach would **gut** the reason for using an **LLM**—responses would stop being **genuinely responsive** to free-form player input. **Omnipresent prompts** would **collapse** the design premise that **JAI** is the **unreliable guide** players must interrogate. I also did **not** treat “**replace Ollama entirely**” as an automatic action item: the critique flags **latency**, but the proportionate response here is **local tuning** (prompt length, generation limits, UI pacing), not a **new hosting architecture**.

**How did this influence my understanding of critique and iteration in AI-driven development?**  
Critique of an **LLM-driven** character is **not** like reviewing a static script: the same prompt can produce **acceptable** output nine times and a **misleading** tenth, so iteration means **sampling behaviour**, **versioning prompts** (as in `prompts-used.md`), and separating **flavour** (what JAI says) from **truth** (what the level designer guarantees in code). The session showed that **players still want scaffolding** even when the **fiction** says “ask the AI”—so the design task is to add **structure** (UI, onboarding, light environmental signposting) **without** turning JAI into a **decorative** chat box. With only **two** attendees, I also treat the feedback as **directional**, not statistically definitive: it **sharpens** priorities, but it does not **replace** my own **design intent** or **feasibility** limits. Overall, the experience made iteration feel **layered**: **player critique** → **filter through goals and scope** → **prompt and systems changes** → **re-test**—a loop that belongs beside traditional bug-fixing whenever **generative** dialogue is in the **critical path** of play.

---

## Links and traceability

- Raw / unbiased log: `feedback-summary.md`  
- Design and scope changes log: `refinements-changes.md`  

Post-event feedback from **two attendees** (see `feedback-summary.md`).

---

## Revision history

| Date | Change |
|------|--------|
| — | Created document structure; analytical body pending |
| — | Restructured headings: expectations, surprise, ignored/deferred items, feasibility, final judgement |
| — | Populated “What did I expect?” (anticipated feedback + attention areas) |
| — | Expanded **Final Judgement**: refinements adopted, items declined, reflection on critique in AI-driven development |

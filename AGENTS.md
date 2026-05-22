## graphify

This project has a graphify knowledge graph at graphify-out/.

Use Graphify for code navigation, not as the source of truth for project docs.

Graphify rules:
- Before answering code structure questions, read graphify-out/GRAPH_REPORT.md for god nodes and community structure.
- For code-symbol questions, use `graphify explain "<symbol>"`, `graphify path "<A>" "<B>"`, or `graphify query "<question>"` before falling back to grep.
- Graphify is currently maintained with `graphify update .`, which is AST-only and no-cost. It does not semantically understand Markdown docs deeply.
- Graphify is a manual/agent-invoked workflow: use the commands above when the task needs code graph context.
- After modifying code files, always run `graphify update .` before the final response to keep graphify-out current.

Docs reading rules:
- Always read CURRENT_STATUS.md first when resuming project work or checking current status.
- Read HANDOVER.md only when deeper current project context is needed or when CURRENT_STATUS.md is unclear.
- Treat HANDOVER.md as the current handover/status map. Do not automatically read every linked doc from it.
- Read docs/Development_Session_Log.md only when historical session context is explicitly needed.
- When replacing the `Latest Session Update` in HANDOVER.md, append the old latest session to docs/Development_Session_Log.md first. Do not read the whole log for this routine archive step; use a targeted tail/search only if needed to avoid duplicating an entry.
- Read additional docs only when they are relevant to the current task or when a rule below says they are needed.
- If the task is ambiguous, inspect headings or search within docs first, then open only the smallest relevant doc set.
- For architecture, DI, data model, async, or service structure work, read:
  - docs/Architecture_And_Code_Rules.md
  - docs/Unity_URP_Performance_Code_Rules.md
  - docs/Prototype_Core_Scope.md
- For asset, KayKit, import, license, visual foundation, or URP asset-test work, read:
  - docs/Tooling_And_Asset_Strategy.md
  - docs/Asset_Selection_Checklist.md
  - docs/Asset_Pack_Shortlist.md
- For product/gameplay direction, roadmap, or scope decisions, read:
  - docs/Product_Vision_One_Page.md
  - docs/Our_Cozy_Procedural_Town_Builder_Gameplay.md
  - docs/App_Development_Roadmap.md
  - docs/Townscaper_Gameplay_Research.md
- For launch, monetization, or business decisions, read:
  - docs/App_Launch_And_Monetization_Plan.md

Workflow:
- Use Graphify to orient around code symbols and module relationships.
- Use CURRENT_STATUS.md for current status, then use HANDOVER.md and the docs above selectively for decisions, constraints, and project intent.
- Use docs/Development_Session_Log.md only for old session archaeology, not startup context.
- Keep HANDOVER.md focused on the newest session details. Move the previous latest session into docs/Development_Session_Log.md when a newer latest session is written.
- If any `.cs`, `.asmdef`, or other code-structure files changed, run `graphify update .` after edits and mention whether it succeeded.
- Do not rely on Graphify query output alone for Markdown documentation decisions unless semantic extraction has been explicitly run with an API key.

# AGENTS.md — rules for AI agents in the repository

## Main
Before ANY code changes, the agent must read the code style rules and follow them.
Code style = contract. If instructions conflict, the code style wins.

✅ Code Style Rules: [CodeStyle.md](./CodeStyle.md)

---

## ⚠️ Tool restrictions
- **PowerShell scripts (`.ps1`)**: Allowed **ONLY** with explicit user permission.
- Otherwise, use Bash or built-in tools.

---

## 🚫 Forbidden actions
- **Git commits**: The agent does NOT make commits. Only the user.
- **Running tests**: The agent does NOT run and does NOT plan to run tests without an explicit request.

---

## Mandatory workflow for the agent
1) Read: [CodeStyle.md](./CodeStyle.md)
2) Work only within the task (do not do "helpful" refactors without a request).
3) Follow the project architecture and existing patterns.
4) After changes:
   - ensure the code compiles
   - do not add extra files/entities
   - do not change public APIs unless necessary

---

## What to write in the response after completing a task
- Briefly: what was done
- List of changed files
- How to verify (steps/menu/scene/button), if applicable

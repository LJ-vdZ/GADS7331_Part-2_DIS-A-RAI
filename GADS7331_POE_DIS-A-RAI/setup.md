# Technical Setup Guide

## Purpose

This document provides a complete setup path for running Ollama locally on Windows, pulling a model through PowerShell, and enabling API-driven AI interaction inside the Unity project.

## 1) System Requirements

Minimum recommended:
- Windows 10/11 (64-bit)
- 16 GB RAM (8 GB possible for smaller models, with slower performance)
- Multi-core CPU
- At least 15 GB free disk space
- Stable local environment for background runtime

Recommended for smoother inference:
- 32 GB RAM
- Dedicated GPU (if supported by your local Ollama runtime path)
- SSD storage

## 2) Install Ollama (Windows)

1. Download Ollama from the official website: [https://ollama.com](https://ollama.com)
2. Run the installer.
3. Confirm installation in PowerShell:

```powershell
ollama --version
```

If the command is not recognized, restart PowerShell or system session and try again.

## 3) Pull and Run a Model (PowerShell)

### Pull model

```powershell
ollama pull llama3:8b
```

You can replace `llama3:8b` with another approved model variant as needed.

### Start model inference service

Ollama typically serves locally via background runtime. To explicitly start the service:

```powershell
ollama serve
```

Notes:
- This can run continuously in the background so in-game AI remains available.
- The local API endpoint is usually `http://localhost:11434`.

### Quick test request

```powershell
ollama run llama3:8b "Respond with: Ollama is working."
```

If this returns a valid response, model execution is working.

## 4) API Usage Pattern (Unity -> Ollama)

Integration is API-based from Unity to local Ollama runtime.

Common endpoints:
- `POST /api/generate`
- `POST /api/chat`

Typical local base URL:
- `http://localhost:11434`

Implementation guidance:
- Build request payload in Unity (prompt, context, options)
- Send via HTTP client
- Parse response safely before applying in-game behavior/UI

## 5) Background Runtime Expectations

Because gameplay depends on live AI responses:
- Keep Ollama running before launching gameplay sessions
- Add startup checks in Unity to verify endpoint availability
- Handle API failure gracefully with retry or fallback messages

## 6) Suggested Operational Checks

Before testing game AI:
1. Verify `ollama serve` is active
2. Verify model is pulled (`ollama list`)
3. Run a quick inference check
4. Launch Unity and test one interaction loop
5. Confirm latency is acceptable for intended gameplay

## 7) Troubleshooting

### "Command not found" in PowerShell
- Restart terminal/session
- Confirm Ollama installation path and environment variables

### API connection refused
- Ensure Ollama service is running
- Check if another service is conflicting on port `11434`

### Very slow responses
- Reduce model size
- Lower output token limits
- Reduce prompt complexity

### Inconsistent responses
- Improve prompt structure in system/context layers
- Add stricter output format instructions

## 8) Maintenance

Update this file when:
- Model version changes
- Hardware assumptions change
- API route usage changes
- Deployment/runtime workflow changes

# Typing Game

A falling-word typing game for Windows, built with C# and WPF.

## How it plays

Words drop from the top of the canvas, one per second. Type a falling word
exactly to remove it before it hits the bottom.

- **Score:** typing a word of 5 characters or fewer earns 10 points;
  longer words earn 20.
- **Health:** starts at 100. Every word that reaches the bottom costs 10 health.
- **Timer:** each round lasts 20 seconds.
- **Game over:** when the timer runs out or health hits 0, a dialog shows
  your final score and health.

## Run it

Open `topic3/topic3.sln` in Visual Studio, build, and run (Windows only —
this is a WPF desktop app).

## Structure

| File | What it is |
| --- | --- |
| `topic3/topic3/MainWindow.xaml` / `MainWindow.xaml.cs` | Window layout plus the whole game: word spawning, falling animation, scoring, health, timer |
| `topic3/topic3/App.xaml` / `App.xaml.cs` | Application entry point |
| `topic3/topic3/topic3.csproj` | Project file |

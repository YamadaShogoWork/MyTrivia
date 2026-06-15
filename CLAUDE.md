# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

MyTrivia (言語学雑学管理システム) is a personal, single-user ASP.NET Core MVC web app for cataloging
linguistics trivia learned from the YouTube channel「ゆる言語学ラジオ」. No authentication, runs on
localhost only. See `docs/basic_design.md` for the full functional spec and `docs/db_design.md` for
the current DB design (the file `docs/dbDisign.md` is an older, superseded design — `db_design.md`
is authoritative; notably the old design had a separate `Quiz` table, but the current model stores
the quiz question on `Trivia.QuizQuestion` with `Trivia.TriviaBody` as the answer).

## Commands

The actual project lives in `src/MyTrivia` (an `dotnet new webapp`-generated MVC project).

```bash
# run the app (dev)
dotnet run --project src/MyTrivia

# build
dotnet build src/MyTrivia

# EF Core migrations
dotnet ef migrations add <Name> --project src/MyTrivia
dotnet ef database update --project src/MyTrivia
```

There is no test project yet.

## Architecture

### Stack
- ASP.NET Core MVC, .NET 10 (`Nullable` and `ImplicitUsings` enabled)
- EF Core + SQLite (`Microsoft.EntityFrameworkCore.Sqlite`)
- Server-rendered Razor views, no SPA/JS framework — `wwwroot/js/site.js` and `wwwroot/css/site.css`
  are the default template files

### Database
- SQLite file `mytrivia.db` (+ `-shm`/`-wal`) lives at the repo root.
- Connection string is defined in `src/MyTrivia/appsettings.Development.json`
  (`ConnectionStrings:DefaultConnection`) as an **absolute path**
  (`Data Source=C:\Work\MyTrivia\mytrivia.db`). `docs/fileplace_guide.md` intends this file to be
  gitignored, but it is currently committed — be careful about absolute-path assumptions when editing it.

### Data model (`src/MyTrivia/Models`, `Data/AppDbContext.cs`)
- **Trivia**: the core entity. `TriviaTitle`, `TriviaBody` (also serves as the quiz answer),
  optional `Source` (timestamp note into the source video), optional `VideoId`, optional
  `QuizQuestion`, `CreatedAt`.
- **Video**: YouTube video (`VideoTitle`, unique `VideoUrl`, optional `PublishedAt`). One video has
  many Trivia.
- **Tag**: `TagName` (unique). Many-to-many with Trivia via **TriviaTag** (composite PK
  `TriviaId`+`TagId`).
- Delete behaviors configured in `AppDbContext.OnModelCreating`:
  - Deleting a `Video` sets `Trivia.VideoId` to NULL (trivia rows are kept).
  - Deleting a `Tag` cascades and removes the related `TriviaTag` rows.
  - Trivia delete in `TriviaController.Delete` manually removes its `TriviaTag` rows first.

### Controllers / Views — feature status
Three resources follow the same CRUD pattern (`Index`/`Create`/`Edit`/`Delete`, POST actions use
`[ValidateAntiForgeryToken]`), but are at different stages of completion:
- **Trivia** (`TriviaController`): fully implemented — `Index` (with `tagId` filter and `keyword`
  search across title/body), `Detail`, `Create`, `Edit`, `Delete`. Create/Edit pass `ViewBag.Tags`
  and `ViewBag.Videos` for select lists and handle the `TriviaTag` join rows via a
  `selectedTagIds` array.
- **Tag** (`TagController`): `Index`/`Create`/`Edit`/`Delete` actions exist, but
  `Views/Tag/Edit.cshtml` has not been created yet.
- **Video** (`VideoController`): `Index`/`Create`/`Edit`/`Delete` actions exist, but no
  `Views/Video/*` directory/views exist yet — this is the main remaining work.
- **Home** (`HomeController`): dashboard showing counts (`TriviaCount`, `VideoCount`, `TagCount`)
  and the 4 most recent Trivia.

### View conventions
- `Views/Shared/_Layout.cshtml` defines all styling inline in a `<style>` block (no separate
  stylesheet for app UI) — class names like `.nav`, `.toolbar`, `.btn.primary`, `.tag`, `.field`,
  `.tag-checkboxes`, `.field-block` are reused across pages. Match this style/class system when
  adding new views rather than introducing new CSS.
- The nav highlights the active section via `ViewData["ActiveNav"]` (values: `"Home"`, `"Trivia"`,
  `"Video"`, `"Tag"`), set per-view/controller.
- `docs/mockups/*.html` contains static HTML mockups for each screen (`SC-01`..`SC-15`), matching
  the screen list/IDs in `docs/basic_design.md` §4 — useful reference when building out the
  remaining Video and Tag-edit views.

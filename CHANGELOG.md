# Changelog

Format inspired by [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This file serves the repository and the writing of Steam patch notes; RimWorld does not display
it in game.

## [1.0.0] — unreleased

On release: create the `v1.0.0` tag and the matching GitHub release.

First release of the 1.6 update. The mod's own logic is unchanged from Kutake's 1.5 version,
apart from the one fix below.

### Added

- Support for RimWorld 1.6. The assembly is rebuilt against 1.6 and every patch target was
  checked against the 1.6 game files: `RoomStatDef ReadingBonus` still carries a `workerClass`,
  `BookcaseBase` is still the only def declaring `thingClass Building_Bookcase`, and
  `Building_Bookcase.DrawAt` still matches the reimplementation the mod ships.
- A French translation, 11 keys, covering everything the mod displays. The wording is taken from
  the game's own vocabulary rather than translated freely: *bonus de lecture* from
  `RoomStatDef ReadingBonus`, *case* for a map cell, and the seven quality tiers in the
  parenthesised form the game already uses for items.
- `incompatibleWith` on the original `Kutake.ReadingBoundsAndBookshelfPatch`, so the two cannot
  be run together.

### Fixed

- **The "max books per cell" setting never applied on its own.** Bookcases were looked up with
  `d.HasComp(typeof(Building_KTK_BookCase))`, but `HasComp` searches a def's components while
  `Building_KTK_BookCase` is a thingClass, so the predicate was always false and the list always
  empty. The lookup now matches on `thingClass`.
- **The setting is no longer written into the def on every spawn.** `maxItemsInCell` is a def
  value, shared by every shelf of a type: writing it in `SpawnSetup` meant lowering the setting
  mid-game did nothing until a new shelf was placed, and left the def altered until the game was
  restarted. It is now applied once at startup and again whenever it changes.

### Changed

- **The mod settings list now shows the mod's name.** The settings category used the key
  `KTK_BookMod_ReadingBounsChange`, which read "Reading Bonus Logic Modified" — a section title
  in the original, not a mod name — so the mod was hard to find under its own name. That key now
  carries the mod name, identically in all three languages, matching the non-localisable name in
  `About.xml`. The checkbox below it already says what the mod changes.

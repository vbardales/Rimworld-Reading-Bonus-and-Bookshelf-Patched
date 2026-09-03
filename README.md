# Reading Bonus and Bookshelf Patched 1.6

Update of Kutake's **阅读加成与书架patch** (Reading Bonus and Bookshelf Patch) to RimWorld 1.6.

**I am not the author of this mod.** The idea, the design and the original code are entirely
Kutake's — all I did was the work needed to make it run on 1.6, plus one bug fix. Credit goes to
them; mistakes in the update are mine.

Original mod: https://steamcommunity.com/sharedfiles/filedetails/?id=3463926737 (stays on 1.5)

## What the mod does

Vanilla only counts one book per cell towards a room's reading bonus, so a bookcase holding four
books is worth no more than a shelf holding one. This mod replaces the `ReadingBonus` room-stat
worker so that every book in the room counts, which finally makes high-capacity bookcases worth
building.

In the mod settings you can set how much each quality tier is worth, from awful to legendary, and
how many books a single cell may contribute. The original behaviour is one toggle away, and there
is a reset button.

Supported shelves: vanilla bookcases, plus those from Daily Furniture and Gloomy Furniture when
either is installed.

Available in English, Simplified Chinese (Kutake's own) and French.

Safe to add to an ongoing save. Removing it mid-save reverts the reading bonus to vanilla and
leaves bookcases as they were.

## What changed in the 1.6 update

Nothing in the mod's own logic. The assembly compiles against 1.6 unchanged — none of the APIs it
uses has disappeared or changed signature. The work went into verifying that its patches still
land where they used to:

- The `RoomStatDef` **`ReadingBonus`** still exists and still carries a `workerClass`, so the
  replace patch applies. This is the one that would have silently disabled everything.
- The vanilla bookcase patch targets the abstract **`BookcaseBase`**, not `Bookcase` /
  `BookcaseSmall`. Patch operations run on the XML before inheritance is resolved, so one
  operation still covers both vanilla bookcases.
- **`Building_Bookcase.DrawAt` was compared line by line** with Kutake's reimplementation: the
  book placement maths is identical — same 0.155 / 0.16 steps, same `RotOffsets`, same bookend
  handling. Shelves still draw correctly.

## The bug that was fixed

This is why the mod is "Patched" rather than a plain 1.6 update.

**The "max books per cell" setting never applied on its own.** The mod looked for its bookcases
with `d.HasComp(typeof(Building_KTK_BookCase))`, but `HasComp` searches a def's *components* while
`Building_KTK_BookCase` is a *thingClass* — so the test was always false, the list was always
empty, and the routine that applies the setting did nothing.

The author had worked around it by writing the value into the def every time a shelf was built.
Two consequences: lowering the setting mid-game changed nothing until you placed a new shelf, and
since `maxItemsInCell` is a *def* value, and therefore shared, it stayed altered until you
restarted the game — even after disabling the mod.

The lookup now matches on `thingClass`, and the setting is applied once at startup and again
whenever it changes. The per-spawn write is gone.

## Licence and attribution

Kutake's mod declares **no licence**: there is no `LICENSE` file in it, and its Steam description
carries no reuse statement. This repository therefore follows the established Workshop practice
for reviving abandoned mods — explicit credit to the original author, a link to the source mod,
and immediate removal on their request. That is stated in the mod's own description, not only
here.

See [ATTRIBUTION.md](ATTRIBUTION.md) for the full record of what was taken and what was changed.

# Reading Bonus and Bookshelf Patched — attribution

Mise a jour en 1.6 de **阅读加成与书架patch**
(https://steamcommunity.com/sharedfiles/filedetails/?id=3463926737), par
**Kutake**. Reste en 1.5.

`packageId` d'origine : `Kutake.ReadingBoundsAndBookshelfPatch`.

**La conception et le code sont de Kutake.** Ce depot n'ajoute aucune
fonctionnalite : il recompile l'assemblage contre la 1.6, verifie que les points
d'accroche tiennent toujours, et corrige un defaut — d'ou le suffixe « Patched »
plutot qu'un simple « 1.6 ».

## Licence

**Aucune licence declaree** : pas de fichier `LICENSE` dans le mod, et la
description Steam tient en une ligne, sans mention de reutilisation. La
republication suit donc l'usage etabli des reprises de mods abandonnes sur le
Workshop — credit explicite a l'auteur d'origine, lien vers le mod source, et
retrait immediat sur simple demande de sa part. C'est dit noir sur blanc dans
la description du mod, pas seulement ici.

## Ce qui a ete repris

| Element | Source |
| --- | --- |
| `KTK_RoomStatWorker_ReadingBonus`, `Building_KTK_BookCase`, `KTK_BookMod`, `KTK_BookModSettings` | `1.5/Assemblies/KTK_BookPatch.dll`, decompile |
| Les quatre patchs XML | `1.5/Patches/` |
| Clefs anglaises et chinoises simplifiees | `Languages/` |

**L'espace de noms `KTK_BookMod` et le nom d'assemblage `KTK_BookPatch` sont
conserves.** Les patchs XML visent `KTK_BookMod.*`, et RimWorld enregistre les
reglages sous le nom de la classe `Mod` : renommer l'un ou l'autre casserait la
continuite avec la version de Kutake, et perdrait les reglages d'une partie en
cours. Seuls le nom affiche et le `packageId` changent, comme le veut la
convention des reprises.

## Ce que la 1.6 a demande de changer

**Rien.** L'assemblage compile tel quel contre `Krafs.Rimworld.Ref` 1.6 : aucune
des API employees n'a disparu ni change de signature — `Building_Bookcase`,
`CellsFilledPercentage`, `HeldBooks`, `MaximumBooks`, `RotOffsets`,
`VerticalGraphic`, `RoomStatWorker.GetScore`, `SilhouetteUtility.DrawGraphicSilhouette`,
`DrawAt(Vector3, bool)`.

Le travail a donc porte sur la verification, pas sur la reecriture :

- **`RoomStatDef ReadingBonus` existe toujours** et porte toujours un
  `<workerClass>`. Le `PatchOperationReplace` s'applique donc. C'est le point
  qui aurait silencieusement tout desactive s'il avait bouge.
- **Le patch des bibliotheques vanilla vise `BookcaseBase`**, la def abstraite,
  et non `Bookcase` / `BookcaseSmall`. Comme les operations de patch s'executent
  sur le XML avant resolution de l'heritage, une seule operation couvre bien les
  deux bibliotheques vanilla. Verifie : `BookcaseBase` est toujours la seule def
  a declarer `<thingClass>Building_Bookcase</thingClass>`.
- **`Building_Bookcase.DrawAt` de la 1.6 a ete compare ligne a ligne** avec la
  reimplementation de Kutake : le calcul de position des livres est identique
  (meme pas de 0,155 / 0,16, meme `RotOffsets`, meme gestion des serre-livres).
  Le dessin reste donc correct. `Thing.DrawAt`, que `DrawCase` remplace, se
  reduit toujours au meme `Graphic.Draw` suivi de la silhouette : rien n'est
  perdu a le court-circuiter.

## Le defaut corrige

**Le reglage « nombre maximum de livres par case » ne s'appliquait jamais de
lui-meme.** La cause est une confusion entre deux notions du jeu :

```csharp
d.HasComp(typeof(Building_KTK_BookCase))
```

`HasComp` cherche parmi les **composants** d'une def, alors que
`Building_KTK_BookCase` est un **thingClass**. Le predicat etait donc toujours
faux : `LoadedAllBookCase` renvoyait une liste vide, et `ChangeMaxBookInCell()`
ne faisait rien.

L'auteur avait contourne le probleme en ecrivant la valeur dans la def a chaque
apparition d'une etagere, dans `SpawnSetup`. Deux consequences :

- baisser le reglage en cours de partie ne changeait rien tant qu'on ne posait
  pas une nouvelle etagere ;
- `maxItemsInCell` etant une valeur de **def**, donc partagee, elle restait
  modifiee jusqu'au redemarrage du jeu, meme apres desactivation du mod.

**Correction :** la recherche se fait desormais sur `thingClass`, et le reglage
est applique une fois au demarrage puis a chaque changement. L'ecriture a
l'apparition a ete supprimee.

## Les libelles

Deux changements, tous deux dans `Languages/` — aucun code touche.

**Le nom affiche dans les reglages.** `SettingsCategory()` renvoie la clef
`KTK_BookMod_ReadingBounsChange`, qui valait « 阅读加成逻辑改变 » /
« Reading Bonus Logic Modified ». Chez Kutake c'etait un titre de section, pas
un nom de mod : la liste Options → Reglages des mods affichait donc une phrase
introuvable pour qui cherchait le mod sous son nom. La clef porte desormais le
nom du mod, identique dans les trois langues — comme le nom de `About.xml`, qui
n'est pas localisable et s'affiche donc en anglais partout. Rien n'est perdu :
la case a cocher juste en dessous dit deja ce que le mod change.

**Ajout du francais**, `Languages/French/Keyed/KTK_BookMod.Keys.xml`. Le
vocabulaire est repris du jeu, pas traduit librement : `bonus de lecture`
(`RoomStatDef ReadingBonus`), `case` pour une cellule de carte, et les sept
qualites sous la forme parenthesee que le jeu emploie deja pour les objets —
`Livres (merveille)` comme `bibliotheque (merveille)`, avec les termes exacts de
`Keyed/Enums.xml` : horrible, mediocre, normal, bon, excellent, merveille,
legendaire.

Les trois fichiers declarent les memes onze clefs, et ces onze clefs sont
exactement celles que le code appelle.

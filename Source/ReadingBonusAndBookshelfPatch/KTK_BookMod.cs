using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace KTK_BookMod;

public class KTK_BookMod : Mod
{
    public KTK_BookModSettings settings;

    public KTK_BookMod(ModContentPack content)
        : base(content)
    {
        settings = GetSettings<KTK_BookModSettings>();
    }

    // Toutes les bibliotheques que ce mod prend en charge.
    //
    // CORRECTIF. La version d'origine filtrait sur
    //
    //     d.HasComp(typeof(Building_KTK_BookCase))
    //
    // or HasComp cherche parmi les COMPOSANTS d'une def, tandis que
    // Building_KTK_BookCase est un thingClass. Le predicat etait donc toujours
    // faux : cette liste restait vide, et ChangeMaxBookInCell() ne faisait rien.
    // C'est ce qui avait conduit a ecrire la valeur dans la def a chaque
    // apparition d'une etagere — un contournement du bug plutot que sa correction.
    public static IEnumerable<ThingDef> LoadedAllBookCase =>
        DefDatabase<ThingDef>.AllDefsListForReading
            .Where(d => d.thingClass != null
                        && typeof(Building_KTK_BookCase).IsAssignableFrom(d.thingClass));

    public override void DoSettingsWindowContents(Rect inRect)
    {
        KTK_BookModSettings.DoSettingsWindowContents(inRect);
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "KTK_BookMod_ReadingBounsChange".Translate();
    }

    private static bool IsVanillaBookcase(ThingDef def)
    {
        return def.defName == "BookcaseSmall" || def.defName == "Bookcase";
    }

    // Rafraichit les pieces ou se trouvent des etageres, pour que le bonus de
    // lecture soit recalcule sans attendre.
    public static void ChangeReadingBonus()
    {
        if (Current.ProgramState != ProgramState.Playing) return;

        foreach (Map map in Find.Maps)
        {
            var seenRooms = new HashSet<int>();
            foreach (Building_KTK_BookCase shelf in
                     map.listerBuildings.AllBuildingsColonistOfClass<Building_KTK_BookCase>())
            {
                Room room = shelf.GetRoom();
                if (room != null && seenRooms.Add(room.ID)) room.Notify_TerrainChanged();
            }
        }
    }

    // Applique la capacite par case aux etageres non vanilla.
    //
    // maxItemsInCell est une valeur de DEF, donc partagee par toutes les etageres
    // du meme type. On l'ecrit donc une fois, au demarrage et a chaque changement
    // de reglage — jamais a l'apparition d'un batiment, sans quoi le reglage ne
    // prendrait effet qu'en posant une nouvelle etagere.
    public static void ChangeMaxBookInCell()
    {
        foreach (ThingDef def in LoadedAllBookCase)
        {
            if (IsVanillaBookcase(def)) continue;
            if (def.building == null) continue;
            def.building.maxItemsInCell = (int)KTK_BookModSettings.maxBookInCell;
        }
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        ChangeMaxBookInCell();
        ChangeReadingBonus();
    }
}

// Le reglage doit valoir des le chargement, pas seulement apres un passage par
// la fenetre des options.
[StaticConstructorOnStartup]
public static class KTK_BookModStartup
{
    static KTK_BookModStartup()
    {
        KTK_BookMod.ChangeMaxBookInCell();
    }
}

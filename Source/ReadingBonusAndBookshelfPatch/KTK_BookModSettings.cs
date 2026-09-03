using UnityEngine;
using Verse;

namespace KTK_BookMod;

public class KTK_BookModSettings : ModSettings
{
	public static bool changeReadingBonus = true;

	public static float bounsAwful = 0.001f;

	public static float bounsPoor = 0.005f;

	public static float bounsNormal = 0.01f;

	public static float bounsGood = 0.015f;

	public static float bounsExcellent = 0.02f;

	public static float bounsMasterwork = 0.03f;

	public static float bounsLegendary = 0.04f;

	public static float maxBookInCell = 30f;

	public override void ExposeData()
	{
		Scribe_Values.Look(ref changeReadingBonus, "changeReadingBonus", defaultValue: true);
		Scribe_Values.Look(ref bounsAwful, "bounsAwful", 0.001f);
		Scribe_Values.Look(ref bounsPoor, "bounsPoor", 0.005f);
		Scribe_Values.Look(ref bounsNormal, "bounsNormal", 0.01f);
		Scribe_Values.Look(ref bounsGood, "bounsGood", 0.015f);
		Scribe_Values.Look(ref bounsExcellent, "bounsExcellent", 0.02f);
		Scribe_Values.Look(ref bounsMasterwork, "bounsMasterwork", 0.03f);
		Scribe_Values.Look(ref bounsLegendary, "bounsLegendary", 0.04f);
		Scribe_Values.Look(ref maxBookInCell, "maxBookInCell", 30f);
		base.ExposeData();
	}

	private static void DoSliderLabeled(Listing_Standard l, string label, ref float val, float min, float max, string tooltip = null, float roundTo = -1f)
	{
		Rect rect = l.GetRect(30f);
		Text.Anchor = TextAnchor.MiddleLeft;
		Widgets.Label(rect.LeftPart(0.5f), label);
		if (tooltip != null)
		{
			TooltipHandler.TipRegion(rect.LeftPart(0.5f), tooltip);
		}
		Text.Anchor = TextAnchor.UpperLeft;
		float num = Widgets.HorizontalSlider(rect.RightPart(0.5f), val, min, max, middleAlignment: false, null, null, null, roundTo);
		l.Gap(l.verticalSpacing);
		val = num;
	}

	private static string GetPercString(float val)
	{
		return (val * 1000f).ToString("F0") + "‰";
	}

	private static bool IsFurnitureModFinded()
	{
		if (DefDatabase<DesignationCategoryDef>.GetNamed("ADH_A_DESIGNATIONCATEGORY", errorOnFail: false) != null || DefDatabase<DesignationCategoryDef>.GetNamed("OldStyleFurniture", errorOnFail: false) != null)
		{
			return true;
		}
		return false;
	}

	public static void DoSettingsWindowContents(Rect inRect)
	{
		Listing_Standard listing_Standard = new Listing_Standard();
		listing_Standard.Begin(inRect);
		listing_Standard.CheckboxLabeled("KTK_BookMod_changeReadingBouns".Translate(), ref changeReadingBonus);
		if (changeReadingBonus)
		{
			listing_Standard.GapLine();
			DoSliderLabeled(listing_Standard, "KTK_BookMod_bounsAwful".Translate() + "  " + GetPercString(bounsAwful), ref bounsAwful, 0f, 0.1f, null, 0.001f);
			DoSliderLabeled(listing_Standard, "KTK_BookMod_bounsPoor".Translate() + "  " + GetPercString(bounsPoor), ref bounsPoor, 0f, 0.1f, null, 0.001f);
			DoSliderLabeled(listing_Standard, "KTK_BookMod_bounsNormal".Translate() + "  " + GetPercString(bounsNormal), ref bounsNormal, 0f, 0.1f, null, 0.001f);
			DoSliderLabeled(listing_Standard, "KTK_BookMod_bounsGood".Translate() + "  " + GetPercString(bounsGood), ref bounsGood, 0f, 0.1f, null, 0.001f);
			DoSliderLabeled(listing_Standard, "KTK_BookMod_bounsExcellent".Translate() + "  " + GetPercString(bounsExcellent), ref bounsExcellent, 0f, 0.1f, null, 0.001f);
			DoSliderLabeled(listing_Standard, "KTK_BookMod_bounsMasterwork".Translate() + "  " + GetPercString(bounsMasterwork), ref bounsMasterwork, 0f, 0.1f, null, 0.001f);
			DoSliderLabeled(listing_Standard, "KTK_BookMod_bounsLegendary".Translate() + "  " + GetPercString(bounsLegendary), ref bounsLegendary, 0f, 0.1f, null, 0.001f);
			listing_Standard.GapLine();
		}
		if (IsFurnitureModFinded())
		{
			DoSliderLabeled(listing_Standard, "KTK_BookMod_maxBookInCell".Translate() + "  " + maxBookInCell.ToString("F0"), ref maxBookInCell, 5f, 100f, null, 1f);
			listing_Standard.GapLine();
		}
		if (listing_Standard.ButtonText("KTK_BookMod_ResetToDefault".Translate()))
		{
			ResetToDefault();
		}
		listing_Standard.End();
	}

	private static void ResetToDefault()
	{
		changeReadingBonus = true;
		bounsAwful = 0.001f;
		bounsPoor = 0.005f;
		bounsNormal = 0.01f;
		bounsGood = 0.015f;
		bounsExcellent = 0.02f;
		bounsMasterwork = 0.03f;
		bounsLegendary = 0.04f;
		maxBookInCell = 30f;
	}
}

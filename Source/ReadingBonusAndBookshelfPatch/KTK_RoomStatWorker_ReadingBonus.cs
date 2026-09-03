using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace KTK_BookMod;

public class KTK_RoomStatWorker_ReadingBonus : RoomStatWorker
{
	private const float MaxEnhancement = 0.2f;

	private static readonly List<float> CellFilledFactor = new List<float> { 0.04f, 0.02f, 0.01f, 0.005f };

	private float GetScoreVanilla(Room room)
	{
		float num = 0f;
		float num2 = 0f;
		foreach (Building_Bookcase item in room.ContainedThings<Building_Bookcase>())
		{
			foreach (float item2 in item.CellsFilledPercentage)
			{
				num2 += item2;
			}
		}
		int num3 = 0;
		while (num2 > 0f && num < 0.2f)
		{
			float num4 = ((num2 >= 1f) ? 1f : num2);
			num2 -= num4;
			num += num4 * CellFilledFactor[Mathf.Min(num3++, CellFilledFactor.Count - 1)];
		}
		return 1f + Mathf.Min(num, 0.2f);
	}

	private float GetScoreNew(Room room)
	{
		float num = 0f;
		foreach (Building_Bookcase item in room.ContainedThings<Building_Bookcase>())
		{
			foreach (Book heldBook in item.HeldBooks)
			{
				switch (heldBook.TryGetComp<CompQuality>()?.Quality)
				{
				case QualityCategory.Awful:
					num += KTK_BookModSettings.bounsAwful;
					break;
				case QualityCategory.Poor:
					num += KTK_BookModSettings.bounsPoor;
					break;
				case QualityCategory.Normal:
					num += KTK_BookModSettings.bounsNormal;
					break;
				case QualityCategory.Good:
					num += KTK_BookModSettings.bounsGood;
					break;
				case QualityCategory.Excellent:
					num += KTK_BookModSettings.bounsExcellent;
					break;
				case QualityCategory.Masterwork:
					num += KTK_BookModSettings.bounsMasterwork;
					break;
				case QualityCategory.Legendary:
					num += KTK_BookModSettings.bounsLegendary;
					break;
				}
			}
		}
		return 1f + num / 4f;
	}

	public override float GetScore(Room room)
	{
		if (KTK_BookModSettings.changeReadingBonus)
		{
			return GetScoreNew(room);
		}
		return GetScoreVanilla(room);
	}
}

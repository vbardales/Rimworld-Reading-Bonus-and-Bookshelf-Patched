using RimWorld;
using UnityEngine;
using Verse;

namespace KTK_BookMod;

public class Building_KTK_BookCase : Building_Bookcase
{
	private static readonly Vector3 DrawOffset = new Vector3(0f, 1f / 52f, 0f);

	private static readonly Vector3 DrawOffsetBookcaseEnd = new Vector3(0f, 1f / 13f, 0f);

	private Graphic bookendGraphicEastInt;

	private Graphic bookendGraphicNorthInt;

	private Graphic BookendGraphicEast => bookendGraphicEastInt ?? (bookendGraphicEastInt = def.building.bookendGraphicEast.GraphicColoredFor(this));

	private Graphic BookendGraphicNorth => bookendGraphicNorthInt ?? (bookendGraphicNorthInt = def.building.bookendGraphicNorth.GraphicColoredFor(this));

	private Vector3 DrawCase(Vector3 drawLoc, bool flip = false)
	{
		drawLoc -= Altitudes.AltIncVect * 2f;
		if (def.drawerType == DrawerType.RealtimeOnly || !base.Spawned)
		{
			Graphic.Draw(drawLoc, flip ? base.Rotation.Opposite : base.Rotation, this);
		}
		SilhouetteUtility.DrawGraphicSilhouette(this, drawLoc);
		return drawLoc;
	}

	protected void DrawBook(Vector3 drawLoc)
	{
		Rot4 rot = base.Rotation.Rotated(RotationDirection.Counterclockwise);
		float num = ((base.Rotation == Rot4.North || base.Rotation == Rot4.South) ? 0.155f : 0.16f);
		Vector3 vector = rot.FacingCell.ToVector3() * num;
		Vector3 vector2 = rot.FacingCell.ToVector3() * ((float)(-base.MaximumBooks) * num * 0.5f);
		Vector3 vector3 = RotOffsets[base.Rotation.AsInt];
		for (int i = 0; i < base.HeldBooks.Count; i++)
		{
			Book book = base.HeldBooks[i];
			Rot4 opposite = base.Rotation.Opposite;
			if (opposite == Rot4.East || opposite == Rot4.West)
			{
				opposite = opposite.Opposite;
			}
			Vector3 loc = drawLoc + vector2 + vector3 + DrawOffset + vector * i;
			book.VerticalGraphic.Draw(loc, opposite, this);
		}
		if (base.Rotation != Rot4.South)
		{
			if (base.Rotation != Rot4.North && def.building.bookendGraphicEast != null)
			{
				BookendGraphicEast.Draw(drawLoc + DrawOffsetBookcaseEnd, Rot4.North, this);
			}
			else if (base.Rotation == Rot4.North && def.building.bookendGraphicNorth != null)
			{
				BookendGraphicNorth.Draw(drawLoc + DrawOffsetBookcaseEnd, Rot4.North, this);
			}
		}
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		drawLoc = DrawCase(drawLoc, flip);
		if (def.defName == "BookcaseSmall" || def.defName == "Bookcase")
		{
			DrawBook(drawLoc);
		}
	}

	// CORRECTIF : la version d'origine ecrivait ici
	//
	//     def.building.maxItemsInCell = (int)KTK_BookModSettings.maxBookInCell;
	//
	// a chaque apparition d'une etagere. maxItemsInCell est une valeur de DEF,
	// partagee par toutes les etageres du meme type : baisser le reglage en cours
	// de partie ne changeait rien tant qu'on n'en posait pas une nouvelle, et la
	// def restait modifiee jusqu'au redemarrage du jeu.
	//
	// Le reglage est desormais applique une fois, au demarrage et a chaque
	// changement, par KTK_BookMod.ChangeMaxBookInCell().
}

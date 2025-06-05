using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

namespace SpaceShared;

internal class SpaceUtility
{
	public static void iterateAllTerrainFeatures(Func<TerrainFeature, TerrainFeature> action)
	{
		foreach (GameLocation location in Game1.locations)
		{
			_recursiveIterateLocation(location, action);
		}
	}

	protected static void _recursiveIterateLocation(GameLocation l, Func<TerrainFeature, TerrainFeature> action)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		foreach (Building building in l.buildings)
		{
			if (((NetFieldBase<GameLocation, NetRef<GameLocation>>)(object)building.indoors).Value != null)
			{
				_recursiveIterateLocation(((NetFieldBase<GameLocation, NetRef<GameLocation>>)(object)building.indoors).Value, action);
			}
		}
		foreach (Vector2 key in l.objects.Keys)
		{
			Object val = l.objects[key];
			IndoorPot val2 = (IndoorPot)(object)((val is IndoorPot) ? val : null);
			if (val2 != null)
			{
				((NetFieldBase<HoeDirt, NetRef<HoeDirt>>)(object)val2.hoeDirt).Value = (HoeDirt)action((TerrainFeature)(object)((NetFieldBase<HoeDirt, NetRef<HoeDirt>>)(object)val2.hoeDirt).Value);
			}
		}
		List<Vector2> list = new List<Vector2>();
		Enumerator<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>> enumerator3 = ((NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>)(object)l.terrainFeatures).Keys.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				Vector2 current3 = enumerator3.Current;
				TerrainFeature val3 = action(((NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>)(object)l.terrainFeatures)[current3]);
				if (val3 == null)
				{
					list.Add(current3);
				}
				else if (val3 != ((NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>)(object)l.terrainFeatures)[current3])
				{
					((NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>)(object)l.terrainFeatures)[current3] = val3;
				}
			}
		}
		finally
		{
			((IDisposable)enumerator3).Dispose();
		}
		foreach (Vector2 item in list)
		{
			Vector2 val4 = item;
			Console.WriteLine("removing " + ((object)(Vector2)(ref val4)).ToString());
			((NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>)(object)l.terrainFeatures).Remove(item);
		}
		for (int num = l.resourceClumps.Count - 1; num >= 0; num--)
		{
			ResourceClump val5 = (ResourceClump)action((TerrainFeature)(object)l.resourceClumps[num]);
			if (val5 == null)
			{
				l.resourceClumps.RemoveAt(num);
			}
			else
			{
				l.resourceClumps[num] = val5;
			}
		}
	}
}

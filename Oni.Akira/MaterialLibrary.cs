using System;
using System.Collections.Generic;
using Oni.Imaging;

namespace Oni.Akira
{
	internal class MaterialLibrary
	{
		internal class MarkerMaterials
		{
			private Material ghost;

			private Material stairs;

			private Material door;

			private Material danger;

			private Material barrier;

			private Material impassable;

			private Material blackness;

			private Material floor;

			public Material Barrier
			{
				get
				{
					EnsureMaterials();
					return barrier;
				}
			}

			public Material Ghost
			{
				get
				{
					EnsureMaterials();
					return ghost;
				}
			}

			public Material Danger
			{
				get
				{
					EnsureMaterials();
					return danger;
				}
			}

			public Material DoorFrame
			{
				get
				{
					EnsureMaterials();
					return door;
				}
			}

			public Material Stairs
			{
				get
				{
					EnsureMaterials();
					return stairs;
				}
			}

			public Material Floor
			{
				get
				{
					EnsureMaterials();
					return floor;
				}
			}

			public Material Blackness
			{
				get
				{
					EnsureMaterials();
					return blackness;
				}
			}

			public Material GetMarker(string name)
			{
				EnsureMaterials();
				switch (name)
				{
				case "_marker_door":
					return door;
				case "_marker_ghost":
					return ghost;
				case "_marker_stairs":
					return stairs;
				case "_marker_danger":
					return danger;
				case "_marker_barrier":
					return barrier;
				case "_marker_impassable":
					return impassable;
				case "_marker_blackness":
					return blackness;
				case "_marker_floor":
					return floor;
				default:
					return null;
				}
			}

			public Material GetMarker(Polygon polygon)
			{
				EnsureMaterials();
				GunkFlags gunkFlags = (GunkFlags)((uint)polygon.Flags & 0xF9E7FFFFu);
				GunkFlags gunkFlags2 = GunkFlags.Ghost | GunkFlags.StairsUp | GunkFlags.StairsDown;
				if ((gunkFlags & (GunkFlags.DoorFrame | gunkFlags2)) == GunkFlags.DoorFrame)
				{
					return door;
				}
				if ((gunkFlags & gunkFlags2) != GunkFlags.None)
				{
					return ghost;
				}
				if ((gunkFlags & GunkFlags.Invisible) != GunkFlags.None)
				{
					gunkFlags = (GunkFlags)((uint)gunkFlags & 0xFFFFDFFFu);
					if ((gunkFlags & GunkFlags.Danger) != GunkFlags.None)
					{
						return danger;
					}
					if ((gunkFlags & GunkFlags.Stairs) != GunkFlags.None)
					{
						return stairs;
					}
					if ((gunkFlags & (GunkFlags.NoObjectCollision | GunkFlags.NoCharacterCollision)) == GunkFlags.NoObjectCollision)
					{
						return barrier;
					}
					if ((gunkFlags & (GunkFlags.NoCollision | GunkFlags.NoObjectCollision | GunkFlags.NoCharacterCollision)) == 0)
					{
						return impassable;
					}
					if ((gunkFlags & GunkFlags.NoCollision) != GunkFlags.None)
					{
						return null;
					}
					Console.Error.WriteLine("Unknown invisible material, fix tool: {0}", gunkFlags);
				}
				else if ((gunkFlags & (GunkFlags.TwoSided | GunkFlags.NoCollision)) == (GunkFlags.TwoSided | GunkFlags.NoCollision) && polygon.Material != null && polygon.Material.Name == "BLACKNESS")
				{
					return blackness;
				}
				return null;
			}

			private void EnsureMaterials()
			{
				if (ghost == null)
				{
					CreateGhost();
					CreateBarrier();
					CreateDanger();
					CreateDoor();
					CreateStairs();
					CreateImpassable();
					CreateBlackness();
					CreateFloor();
				}
			}

			private void CreateBarrier()
			{
				Surface surface = new Surface(128, 128);
				Color color = new Color(0, 240, 20, 180);
				Color color2 = new Color(240, 20, 0, byte.MaxValue);
				surface.Fill(0, 0, 128, 128, color);
				surface.Fill(0, 0, 128, 1, color2);
				surface.Fill(0, 127, 128, 1, color2);
				surface.Fill(0, 1, 1, 126, color2);
				surface.Fill(127, 1, 1, 126, color2);
				surface.Fill(64, 1, 1, 126, color2);
				surface.Fill(1, 64, 126, 1, color2);
				barrier = new Material("_marker_barrier", true)
				{
					Flags = (GunkFlags.Invisible | GunkFlags.NoObjectCollision),
					Image = surface
				};
			}

			private void CreateImpassable()
			{
				Surface surface = new Surface(128, 128);
				Color color = new Color(240, 0, 20, 180);
				Color color2 = new Color(240, 20, 0, byte.MaxValue);
				surface.Fill(0, 0, 128, 128, color);
				surface.Fill(0, 0, 128, 1, color2);
				surface.Fill(0, 127, 128, 1, color2);
				surface.Fill(0, 1, 1, 126, color2);
				surface.Fill(127, 1, 1, 126, color2);
				surface.Fill(64, 1, 1, 126, color2);
				surface.Fill(1, 64, 126, 1, color2);
				impassable = new Material("_marker_impassable", true)
				{
					Flags = GunkFlags.Invisible,
					Image = surface
				};
			}

			private void CreateGhost()
			{
				Surface surface = new Surface(128, 128);
				Color color = new Color(16, 48, 240, 240);
				Color color2 = new Color(208, 240, 240, 80);
				surface.Fill(0, 0, 128, 128, color2);
				surface.Fill(0, 0, 128, 1, color);
				surface.Fill(0, 127, 128, 1, color);
				surface.Fill(0, 1, 1, 126, color);
				surface.Fill(127, 1, 1, 126, color);
				surface.Fill(64, 1, 1, 126, color);
				surface.Fill(1, 64, 126, 1, color);
				ghost = new Material("_marker_ghost", true);
				ghost.Flags = GunkFlags.Ghost | GunkFlags.Transparent | GunkFlags.TwoSided | GunkFlags.NoCollision;
				ghost.Image = surface;
			}

			private void CreateDoor()
			{
				Surface surface = new Surface(128, 128);
				Color color = new Color(240, 240, 0, 208);
				Color color2 = new Color(0, 0, 240);
				surface.Fill(0, 0, 128, 128, color);
				surface.Fill(1, 1, 126, 1, color2);
				surface.Fill(1, 1, 1, 126, color2);
				surface.Fill(1, 126, 126, 1, color2);
				surface.Fill(126, 1, 1, 126, color2);
				door = new Material("_marker_door", true)
				{
					Flags = (GunkFlags.DoorFrame | GunkFlags.Transparent | GunkFlags.TwoSided | GunkFlags.NoCollision),
					Image = surface
				};
			}

			private void CreateDanger()
			{
				Surface surface = new Surface(128, 128);
				Color color = new Color(byte.MaxValue, 10, 0, 208);
				Color color2 = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				surface.Fill(0, 0, 128, 128, color);
				surface.Fill(52, 16, 24, 64, color2);
				surface.Fill(52, 96, 24, 16, color2);
				danger = new Material("_marker_danger", true)
				{
					Flags = (GunkFlags.NoCollision | GunkFlags.Invisible | GunkFlags.NoOcclusion | GunkFlags.Danger),
					Image = surface
				};
			}

			private void CreateStairs()
			{
				Surface surface = new Surface(128, 128);
				Color color = new Color(40, 240, 0, 180);
				Color color2 = new Color(40, 0, 240, 180);
				surface.Fill(0, 0, 128, 128, color);
				for (int i = 0; i < surface.Height; i += 32)
				{
					surface.Fill(0, i, surface.Width, 16, color2);
				}
				stairs = new Material("_marker_stairs", true)
				{
					Flags = (GunkFlags.Stairs | GunkFlags.TwoSided | GunkFlags.Invisible | GunkFlags.NoObjectCollision),
					Image = surface
				};
			}

			private void CreateBlackness()
			{
				Surface surface = new Surface(16, 16, SurfaceFormat.BGRX);
				surface.Fill(0, 0, 16, 16, Color.Black);
				blackness = new Material("_marker_blackness", true)
				{
					Flags = (GunkFlags.TwoSided | GunkFlags.NoCollision),
					Image = surface
				};
			}

			private void CreateFloor()
			{
				Surface surface = new Surface(256, 256);
				surface.Fill(0, 0, 16, 16, Color.White);
				for (int i = 0; i < 256; i += 4)
				{
					surface.Fill(i, 0, 1, 256, Color.Black);
					surface.Fill(0, i, 256, 1, Color.Black);
				}
				floor = new Material("_marker_floor", true)
				{
					Flags = GunkFlags.NoCollision,
					Image = surface
				};
			}
		}

		private readonly MarkerMaterials markers = new MarkerMaterials();

		private readonly Dictionary<string, Material> materials = new Dictionary<string, Material>(StringComparer.OrdinalIgnoreCase);

		private Material notFound;

		public MarkerMaterials Markers
		{
			get
			{
				return markers;
			}
		}

		public Material NotFound
		{
			get
			{
				if (notFound == null)
				{
					notFound = GetMaterial("notfoundtex");
				}
				return notFound;
			}
		}

		public IEnumerable<Material> All
		{
			get
			{
				return materials.Values;
			}
		}

		public Material GetMaterial(string name)
		{
			Material value;
			if (!materials.TryGetValue(name, out value))
			{
				value = markers.GetMarker(name);
				if (value == null)
				{
					value = new Material(name);
				}
				materials.Add(name, value);
			}
			if (name.StartsWith("lmap_", StringComparison.OrdinalIgnoreCase))
			{
				value.Flags |= GunkFlags.NoCollision | GunkFlags.NoOcclusion | GunkFlags.SoundTransparent;
			}
			return value;
		}
	}
}

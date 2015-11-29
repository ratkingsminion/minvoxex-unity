using UnityEngine;
using System.Collections;

namespace RatKing.MinimalVoxelExample {

	public struct Voxel {
		public readonly ushort templateIndex;
		//
		public Voxel(ushort templateIndex) {
			this.templateIndex = templateIndex;
		}
		//
		public bool isAir {
			get { return Main.voxelTemplates[templateIndex].isAir; }
		}
		public bool HasFullWall(short dir) {
			//int d = dir + 1 - (dir & 1) * 2; // get opposite direction
			//int w = Main.voxelTemplates[templateIndex].walls & (1 << d);
			//int w = Main.voxelTemplates[templateIndex].walls & (1 << dir);
			return Main.voxelTemplates[templateIndex].HasFullWall(dir);
		}
		public void Build(TempMeshData tmd, int i, ref int vc) {
			Main.voxelTemplates[templateIndex].Build(tmd, i, ref vc);
		}
		//
		public static bool operator ==(Voxel a, Voxel b) {
			return a.templateIndex == b.templateIndex;
		}
		public static bool operator !=(Voxel a, Voxel b) {
			return a.templateIndex != b.templateIndex;
		}
		public override bool Equals(object o) { try { return (bool)(this == (Voxel)o); } catch { return false; } }
		public override int GetHashCode() { return base.GetHashCode(); }
		public override string ToString() { return "Voxel " + templateIndex; }
	}

}
using UnityEngine;
using System.Collections;

namespace RatKing.MinimalVoxelExample {

	public enum VoxelType {
		None,
		FullBox,
		Slope,
	}

	public class VoxelTemplate {
		public static float uvFactorX;
		public static float uvFactorY;
		//
		public string name;
		public VoxelType type;
		public bool isAir;
		//
		public VoxelTemplate(string name, bool isAir, VoxelType type) {
			this.name = name;
			this.isAir = isAir;
			this.type = type;
		}
		//
		virtual public void Build(TempMeshData tmd, int i, ref int vc) {
		}
		//
		virtual public bool HasFullWall(short dir) {
			return !isAir;
		}
	}

}
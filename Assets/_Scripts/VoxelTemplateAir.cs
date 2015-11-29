using UnityEngine;
using System.Collections;

namespace RatKing.MinimalVoxelExample {

	public class VoxelTemplateAir : VoxelTemplate {
		public VoxelTemplateAir() : base("Air", true, VoxelType.None) {
		}

		//

		override public bool HasFullWall(short dir) {
			return false;
		}
	}

}
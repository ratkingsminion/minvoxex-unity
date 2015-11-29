using UnityEngine;
using System.Collections;
using RatKing.Base;

namespace RatKing.MinimalVoxelExample {

	public class Chunk {
		public GameObject go;
		public Mesh m;
		public MeshFilter mf;
		public MeshCollider mc;
		public int[] additionalVoxelsDataPosIndices;
		public Voxel[] additionalVoxelsDataBoxIndices;
		public int additionalVoxelsDataNum;
		public Position3 pos;
		public bool built; // internal, for creating chunk in steps
						   //
		public Chunk(Position3 pos) {
			this.pos = pos;
			Main.chunks[pos] = this;
		}
		//
		public override bool Equals(object obj) {
			var other = obj as Chunk;
			if (other == null) {
				return false;
			}
			return go == other.go;
		}
		public override int GetHashCode() { return base.GetHashCode(); }
		public override string ToString() { return "Chunk at " + pos.x + ", " + pos.y + ", " + pos.z; }
	}

}
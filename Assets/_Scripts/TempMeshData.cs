using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.MinimalVoxelExample {

	public class TempMeshData {
		public List<Vector3> vertices;
		public List<Vector3> normals;
		public List<Vector2> uv;
		public List<int> triangles;
		//
		public TempMeshData(int estimatedSize) {
			vertices = new List<Vector3>(estimatedSize);
			normals = new List<Vector3>(estimatedSize);
			uv = new List<Vector2>(estimatedSize);
			triangles = new List<int>(estimatedSize * 2);
		}
		//
		public void Assign(Mesh m) {
			m.vertices = vertices.ToArray();
			m.triangles = triangles.ToArray();
			m.normals = normals.ToArray();
			m.uv = uv.ToArray();
		}
	}

}
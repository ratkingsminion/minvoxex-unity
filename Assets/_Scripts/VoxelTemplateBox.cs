using UnityEngine;
using System.Collections;

namespace RatKing.MinimalVoxelExample {

	public class VoxelTemplateBox : VoxelTemplate {
		Vector2[] _uvs;

		//

		public VoxelTemplateBox(string name, int uvX, int uvY, int uvW, int uvH) : base(name, false, VoxelType.FullBox) {
			float uvX1f = uvX * uvFactorX;
			float uvY1f = 1.0f - uvY * uvFactorY;
			float uvX2f = (uvX + uvW) * uvFactorX;
			float uvY2f = 1.0f - (uvY + uvH) * uvFactorY;

			_uvs = new Vector2[] {
				new Vector2(uvX1f, uvY2f),
				new Vector2(uvX1f, uvY1f),
				new Vector2(uvX2f, uvY1f),
				new Vector2(uvX2f, uvY2f)
			};
		}

		//

		override public void Build(TempMeshData tmd, int i, ref int vc) {
			// create a box! [ ]
			float xf0 = Main.tempVoxelPosRel.x, yf0 = Main.tempVoxelPosRel.y, zf0 = Main.tempVoxelPosRel.z;
			float xf1 = xf0 + 1, yf1 = yf0 + 1, zf1 = zf0 + 1;
			Voxel nv; // neighbour

			//yf0 *= 0.25f;
			//yf1 *= 0.25f;

			// bottom
			nv = Main.tempVoxels[i - Main.chunkSizeA2P2];
			if (nv.isAir || !nv.HasFullWall(1)) { // Main.voxelTemplates[nv].HasFullWall(2)) {
				tmd.vertices.Add(new Vector3(xf0, yf0, zf1));
				tmd.vertices.Add(new Vector3(xf0, yf0, zf0));
				tmd.vertices.Add(new Vector3(xf1, yf0, zf0));
				tmd.vertices.Add(new Vector3(xf1, yf0, zf1));
				tmd.triangles.Add(vc); tmd.triangles.Add(vc + 1); tmd.triangles.Add(vc + 2);
				tmd.triangles.Add(vc + 2); tmd.triangles.Add(vc + 3); tmd.triangles.Add(vc);
				tmd.normals.AddRange(Main.normalsDown4);
				tmd.uv.AddRange(_uvs);
				vc += 4;
			}
			// top
			nv = Main.tempVoxels[i + Main.chunkSizeA2P2];
			if (nv.isAir || !nv.HasFullWall(0)) {
				tmd.vertices.Add(new Vector3(xf0, yf1, zf0));
				tmd.vertices.Add(new Vector3(xf0, yf1, zf1));
				tmd.vertices.Add(new Vector3(xf1, yf1, zf1));
				tmd.vertices.Add(new Vector3(xf1, yf1, zf0));
				tmd.triangles.Add(vc); tmd.triangles.Add(vc + 1); tmd.triangles.Add(vc + 2);
				tmd.triangles.Add(vc + 2); tmd.triangles.Add(vc + 3); tmd.triangles.Add(vc);
				tmd.normals.AddRange(Main.normalsUp4);
				tmd.uv.AddRange(_uvs);
				vc += 4;
			}
			// back
			nv = Main.tempVoxels[i - Main.chunkSizeA2];
			if (nv.isAir || !nv.HasFullWall(3)) {
				tmd.vertices.Add(new Vector3(xf0, yf0, zf0));
				tmd.vertices.Add(new Vector3(xf0, yf1, zf0));
				tmd.vertices.Add(new Vector3(xf1, yf1, zf0));
				tmd.vertices.Add(new Vector3(xf1, yf0, zf0));
				tmd.triangles.Add(vc); tmd.triangles.Add(vc + 1); tmd.triangles.Add(vc + 2);
				tmd.triangles.Add(vc + 2); tmd.triangles.Add(vc + 3); tmd.triangles.Add(vc);
				tmd.normals.AddRange(Main.normalsBack4);
				tmd.uv.AddRange(_uvs);
				vc += 4;
			}
			// front
			nv = Main.tempVoxels[i + Main.chunkSizeA2];
			if (nv.isAir || !nv.HasFullWall(2)) {
				tmd.vertices.Add(new Vector3(xf1, yf0, zf1));
				tmd.vertices.Add(new Vector3(xf1, yf1, zf1));
				tmd.vertices.Add(new Vector3(xf0, yf1, zf1));
				tmd.vertices.Add(new Vector3(xf0, yf0, zf1));
				tmd.triangles.Add(vc); tmd.triangles.Add(vc + 1); tmd.triangles.Add(vc + 2);
				tmd.triangles.Add(vc + 2); tmd.triangles.Add(vc + 3); tmd.triangles.Add(vc);
				tmd.normals.AddRange(Main.normalsForward4);
				tmd.uv.AddRange(_uvs);
				vc += 4;
			}
			// left
			nv = Main.tempVoxels[i - 1];
			if (nv.isAir || !nv.HasFullWall(5)) {
				tmd.vertices.Add(new Vector3(xf0, yf0, zf1));
				tmd.vertices.Add(new Vector3(xf0, yf1, zf1));
				tmd.vertices.Add(new Vector3(xf0, yf1, zf0));
				tmd.vertices.Add(new Vector3(xf0, yf0, zf0));
				tmd.triangles.Add(vc); tmd.triangles.Add(vc + 1); tmd.triangles.Add(vc + 2);
				tmd.triangles.Add(vc + 2); tmd.triangles.Add(vc + 3); tmd.triangles.Add(vc);
				tmd.normals.AddRange(Main.normalsLeft4);
				tmd.uv.AddRange(_uvs);
				vc += 4;
			}
			// right
			nv = Main.tempVoxels[i + 1];
			if (nv.isAir || !nv.HasFullWall(4)) {
				tmd.vertices.Add(new Vector3(xf1, yf0, zf0));
				tmd.vertices.Add(new Vector3(xf1, yf1, zf0));
				tmd.vertices.Add(new Vector3(xf1, yf1, zf1));
				tmd.vertices.Add(new Vector3(xf1, yf0, zf1));
				tmd.triangles.Add(vc); tmd.triangles.Add(vc + 1); tmd.triangles.Add(vc + 2);
				tmd.triangles.Add(vc + 2); tmd.triangles.Add(vc + 3); tmd.triangles.Add(vc);
				tmd.normals.AddRange(Main.normalsRight4);
				tmd.uv.AddRange(_uvs);
				vc += 4;
			}
		}

		//

		override public bool HasFullWall(short dir) {
			return true;
		}
	}

}
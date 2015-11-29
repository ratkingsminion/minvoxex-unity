using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RatKing.Base;

namespace RatKing.MinimalVoxelExample {

	public class Voxels : MonoBehaviour {
		public Material material;
		public int matTilesX = 2;
		public int matTilesY = 2;
		public int chunkSize = 16;
		public Transform marker;
		public GameObject[] randomSpawnObjects;
		//
		public static Dictionary<Position3, Chunk> chunks = new Dictionary<Position3, Chunk>();
		public static VoxelTemplate[] voxelTemplates;
		public static int chunkSizeA0;
		public static int chunkSizeA1; // add 1
		public static int chunkSizeA2; // add 2
		public static int chunkSizeA2P2; // add 2, power 2
		public static int chunkSizeA2M2; // add 2, multiplicate with 2
										 // working on this while building
		public static Voxel[] tempVoxels;
		public static Chunk tempChunk;
		public static Position3 tempVoxelPosRel = new Position3();
		//
		public static HashSet<Chunk> tempChunksToGetUpdated = new HashSet<Chunk>();
		public static float randSeed;
		public static Vector3[] normalsDown5    = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down, Vector3.down };
		public static Vector3[] normalsUp5      = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up };
		public static Vector3[] normalsBack5    = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back, Vector3.back };
		public static Vector3[] normalsForward5 = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
		public static Vector3[] normalsLeft5    = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left, Vector3.left };
		public static Vector3[] normalsRight5   = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right, Vector3.right };
		public static Vector3[] normalsDown4    = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
		public static Vector3[] normalsUp4      = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
		public static Vector3[] normalsBack4    = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
		public static Vector3[] normalsForward4 = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
		public static Vector3[] normalsLeft4    = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
		public static Vector3[] normalsRight4   = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
		public static Vector3[] normalsDown3    = new Vector3[] { Vector3.down, Vector3.down, Vector3.down };
		public static Vector3[] normalsUp3      = new Vector3[] { Vector3.up, Vector3.up, Vector3.up };
		public static Vector3[] normalsBack3    = new Vector3[] { Vector3.back, Vector3.back, Vector3.back };
		public static Vector3[] normalsForward3 = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward };
		public static Vector3[] normalsLeft3    = new Vector3[] { Vector3.left, Vector3.left, Vector3.left };
		public static Vector3[] normalsRight3   = new Vector3[] { Vector3.right, Vector3.right, Vector3.right };
		//
		Transform chunkParent;

		//

		void OnGUI() {
			GUI.Label(new Rect(10, 10, 300, 200), "Pos: " + Position3.FlooredVector(marker.position) + "\nMove with Mouse MB\nCreate with LB\nRemove with RB");
		}

		void Awake() {
			chunkSizeA0 = chunkSize;
			chunkSizeA1 = chunkSize + 1;
			chunkSizeA2 = chunkSize + 2;
			chunkSizeA2P2 = chunkSizeA2 * chunkSizeA2;
			chunkSizeA2M2 = chunkSizeA2 * 2;
			//
			VoxelTemplate.uvFactorX = 1.0f / (float)matTilesX;
			VoxelTemplate.uvFactorY = 1.0f / (float)matTilesY;
			voxelTemplates = new VoxelTemplate[] {
				new VoxelTemplateAir(),
				new VoxelTemplateBox("1", 0, 0, 1, 1),
				new VoxelTemplateBox("2", 1, 0, 1, 1),
				new VoxelTemplateBox("3", 2, 0, 1, 1),
				new VoxelTemplateBox("4", 0, 1, 1, 1),
				new VoxelTemplateBox("5", 1, 1, 1, 1),
				new VoxelTemplateBox("6", 2, 1, 1, 1),
				new VoxelTemplateBox("7", 0, 2, 1, 1),
				new VoxelTemplateBox("8", 1, 2, 1, 1),
				new VoxelTemplateBox("9", 2, 2, 1, 1),
			};
			//
		}

		IEnumerator Start() {
			randSeed = Random.Range(-1000.0f, 1000.0f);
			tempVoxels = new Voxel[chunkSizeA2 * chunkSizeA2P2];
			chunkParent = new GameObject("_CHUNKS").transform;
			//
			for (int x = 0; x < 5; ++x) {
				for (int y = 0; y < 5; ++y) {
					for (int z = 0; z < 5; ++z) {
						CreateChunk(x, y, z);
						yield return null;
					}
				}
			}
		}

		void Update() {
			Vector3 pos;
			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// get mouse position
			if (Physics.Raycast(r, out hit, 10000.0f)) {
				if (!marker.gameObject.activeSelf) {
					marker.gameObject.SetActive(true);
				}
				pos = hit.point - hit.normal * 0.5f;
				bool wantToBuild = Input.GetButtonDown("Fire1");
				bool wantToDelete = Input.GetButtonDown("Fire2");
				bool wantToMakeSlope = Input.GetKeyDown(KeyCode.F);
				// interaction!
				if (wantToDelete) {
					ChangeVoxel(Position3.FlooredVector(pos), new Voxel(0)); // REMOVE
				}
				else if (wantToBuild) {
					ChangeVoxel(Position3.FlooredVector(pos) + Position3.RoundedVector(hit.normal), new Voxel(2)); // ADD
				}
				else if (wantToMakeSlope) {
					ChangeVoxel(Position3.FlooredVector(pos) + Position3.RoundedVector(hit.normal), new Voxel(10)); // ADD
				}
				pos = hit.point + hit.normal * 0.5f;
				pos.x = Mathf.Floor(pos.x) + 0.5f;
				pos.y = Mathf.Floor(pos.y) + 0.5f;
				pos.z = Mathf.Floor(pos.z) + 0.5f;
				marker.position = pos;
			}
			else {
				if (marker.gameObject.activeSelf)
					marker.gameObject.SetActive(false);
			}
		}

		// CREATE OR CHANGE A VOXEL
		void ChangeVoxel(Position3 posAbs, Voxel voxel, bool updateChunk = true, int padX = 0, int padY = 0, int padZ = 0) {
#if UNITY_EDITOR
			if (padX < -1 || padX > 1) { Debug.LogError("Padding (X) too big!"); }
			if (padY < -1 || padY > 1) { Debug.LogError("Padding (Y) too big!"); }
			if (padZ < -1 || padZ > 1) { Debug.LogError("Padding (Z) too big!"); }
#endif
			Position3 chunkPos = new Position3(
				(posAbs.x < 0 ? (posAbs.x - chunkSize + 1) : posAbs.x) / chunkSize - padX, // TODO: >> instead of /?
				(posAbs.y < 0 ? (posAbs.y - chunkSize + 1) : posAbs.y) / chunkSize - padY, // TODO: >> instead of /?
				(posAbs.z < 0 ? (posAbs.z - chunkSize + 1) : posAbs.z) / chunkSize - padZ // TODO: >> instead of /?
			);
			Chunk curChunk;
			if (!chunks.TryGetValue(chunkPos, out curChunk)) {
				curChunk = new Chunk(chunkPos);
			}
			Position3 posRel = posAbs - (chunkPos * chunkSize);
			int posRelIndex = GetVoxelRelPosIndex(posRel);
			List<int> dataPosInd = new List<int>(); // using List<>s for edit mode only?
			List<Voxel> dataVoxel = new List<Voxel>();
			if (curChunk.additionalVoxelsDataNum != 0) { // get the already created add voxel data
				dataPosInd.AddRange(curChunk.additionalVoxelsDataPosIndices);
				dataVoxel.AddRange(curChunk.additionalVoxelsDataBoxIndices);
			}
			if (dataPosInd.Contains(posRelIndex)) { // box was changed already
				Voxel sb = GetStandardVoxel(posAbs.x, posAbs.y, posAbs.z);
				int dpi = dataPosInd.IndexOf(posRelIndex);
				if (voxel == sb) { // is standard box? -> remove this add data
					dataPosInd.RemoveAt(dpi);
					dataVoxel.RemoveAt(dpi);
					curChunk.additionalVoxelsDataNum--;
				}
				else { // is new box type? -> replace it
					dataVoxel[dpi] = voxel;
				}
			}
			else { // voxel was never changed before? -> add it
				dataPosInd.Add(posRelIndex);
				dataVoxel.Add(voxel);
				curChunk.additionalVoxelsDataNum++;
			}
			curChunk.additionalVoxelsDataPosIndices = dataPosInd.ToArray();
			curChunk.additionalVoxelsDataBoxIndices = dataVoxel.ToArray();
			// somewhere at the limits?
			if (padY == 0 && padZ == 0 && posRel.x == 0) ChangeVoxel(posAbs, voxel, updateChunk, 1);
			else if (padY == 0 && padZ == 0 && posRel.x == chunkSize - 1) ChangeVoxel(posAbs, voxel, updateChunk, -1);
			if (padX == 0 && padZ == 0 && posRel.y == 0) ChangeVoxel(posAbs, voxel, updateChunk, 0, 1);
			else if (padX == 0 && padZ == 0 && posRel.y == chunkSize - 1) ChangeVoxel(posAbs, voxel, updateChunk, 0, -1);
			if (padX == 0 && padY == 0 && posRel.z == 0) ChangeVoxel(posAbs, voxel, updateChunk, 0, 0, 1);
			else if (padX == 0 && padY == 0 && posRel.z == chunkSize - 1) ChangeVoxel(posAbs, voxel, updateChunk, 0, 0, -1);
			// update chunk
			if (updateChunk) {
				CreateChunk(curChunk);
			}
			else {
				tempChunksToGetUpdated.Add(curChunk);
			}
		}

		//

		// USE THESE WHEN PROCESSING MANY VOXELS AT ONCE

		void UpdateChunksStart() {
			tempChunksToGetUpdated.Clear();
		}

		void UpdateChunksEnd(bool clear = true) {
			foreach (var chunk in tempChunksToGetUpdated) {
				CreateChunk(chunk);
			}
			if (clear) {
				tempChunksToGetUpdated.Clear();
			}
		}

		//

		// helper method
		int GetVoxelRelPosIndex(Position3 pos) {
			return (pos.y + 1) * chunkSizeA2P2 + (pos.z + 1) * chunkSizeA2 + (pos.x + 1);
		}
		int GetVoxelRelPosIndex(int x, int y, int z) {
			return (y + 1) * chunkSizeA2P2 + (z + 1) * chunkSizeA2 + (x + 1);
		}

		//

		Voxel nv = new Voxel(0);

		// define the standard box data
		Voxel GetStandardVoxel(int x, int y, int z) {
			
			if (Helpers.Randomness.SimplexNoise.NormalizedNoise(x * 0.07f, y * 0.08f, z * 0.06f + randSeed) < 0.45f) {
				return new Voxel((ushort)(1 +
					Mathf.FloorToInt(Helpers.Randomness.SimplexNoise.NormalizedNoise(x * 0.008f + randSeed, y * 0.02f, z * 0.02f) * (voxelTemplates.Length - 1))));
			}
			else if ((x < 3) || (y < 3) || (z < 3) || (x > 5 * chunkSize - 3) || (y > 5 * chunkSize - 3) || (z > 5 * chunkSize - 3)) {
				// return new Voxel(1);
				return new Voxel((ushort)(1 +
					Mathf.FloorToInt(Helpers.Randomness.SimplexNoise.NormalizedNoise(x * 0.008f + randSeed, y * 0.02f, z * 0.02f) * (voxelTemplates.Length - 1))));
			}

			return nv;

			/*
			return
				(Helpers.Randomness.SimplexNoise.NormalizedNoise(x * 0.07f, y * 0.08f, z * 0.06f + randSeed) < 0.45f)
				//x >= 0 && x < chunkSize && z >= 0 && z < chunkSize && y < 3 && y >= 0
				? new Voxel((ushort)(1 + Mathf.FloorToInt(Helpers.Randomness.SimplexNoise.NormalizedNoise(x * 0.03f + randSeed, y * 0.35f, z * 0.02f) * (voxelTemplates.Length - 1))))
				: nv; // new Voxel(0);
			//*/
		}

		//

		// CREATE A CHUNK (OR MODIFY IT)
		Chunk CreateChunk(int posX, int posY, int posZ) {
			return CreateChunk(new Position3(posX, posY, posZ));
		}
		Chunk CreateChunk(Position3 pos) {
			Chunk chunk;
			chunks.TryGetValue(pos, out chunk);
			if (chunk == null) {
				chunk = new Chunk(pos);
			}
			return CreateChunk(chunk);
		}

		Chunk CreateChunk(Chunk chunk) {
#if UNITY_EDITOR
			if (chunk == null) { Debug.LogError("Chunk must not be null!"); }
#endif
			tempChunk = chunk;
			tempVoxelPosRel.Reset();

			// normal chunk data
			// TODO can be optimized?
			// TODO different standard landscapes
			int cx = chunk.pos.x * chunkSize;
			int cy = chunk.pos.y * chunkSize;
			int cz = chunk.pos.z * chunkSize;
			//int noAirVoxelCount = 0;
			for (int i = 0, y = -1; y < chunkSizeA1; ++y) {
				for (int z = -1; z < chunkSizeA1; ++z) {
					for (int x = -1; x < chunkSizeA1; ++x, ++i) {
						tempVoxels[i] = GetStandardVoxel(x + cx, y + cy, z + cz);
						//			if (!tempVoxels[i].isAir) noAirVoxelCount++;
					}
				}
			}

			// load additional voxels data
			for (int i = 0; i < chunk.additionalVoxelsDataNum; ++i) {
				int vi = chunk.additionalVoxelsDataPosIndices[i];
				//	if (!tempVoxels[vi].isAir) noAirVoxelCount--;
				tempVoxels[vi] = chunk.additionalVoxelsDataBoxIndices[i];
				//	if (!tempVoxels[vi].isAir) noAirVoxelCount++;
			}

			/*
			if (noAirVoxelCount == 0)
				return chunk;
			*/

			// build chunk
			if (!chunk.built) {
				chunk.m = new Mesh();
				chunk.go = new GameObject("chunk " + chunk.pos);
				chunk.go.transform.parent = chunkParent;
				chunk.go.transform.position = chunk.pos.ToVector() * chunkSize;
				chunk.mf = chunk.go.AddComponent<MeshFilter>();
				chunk.go.AddComponent<MeshRenderer>().material = material;
				chunk.mc = chunk.go.AddComponent<MeshCollider>();
			}

			if (!chunk.built && randomSpawnObjects != null && randomSpawnObjects.Length > 0) {
				for (int y = 0; y < chunkSize; ++y) {
					for (int z = 0; z < chunkSize; ++z) {
						for (int x = 0; x < chunkSize; ++x) {
							if (Random.value > 0.998f && tempVoxels[GetVoxelRelPosIndex(x, y, z)].isAir) {
								GameObject go = (GameObject)Instantiate(randomSpawnObjects[Random.Range(0, randomSpawnObjects.Length)], new Vector3(x + cx + 0.5f, y + cy + 0.5f, z + cz + 0.5f), Random.rotationUniform);
								go.transform.parent = chunk.go.transform;
							}
						}
					}
				}
			}

			// create chunk mesh data
			TempMeshData tmd = new TempMeshData(chunkSizeA2P2 * 2);
			int vc = 0; // current vertex count
			int start = (chunkSize + 3) * chunkSizeA2 + 1;
			int index = start;
			for (tempVoxelPosRel.y = 0; tempVoxelPosRel.y < chunkSize; ++tempVoxelPosRel.y, index += chunkSizeA2M2) {
				for (tempVoxelPosRel.z = 0; tempVoxelPosRel.z < chunkSize; ++tempVoxelPosRel.z, index+= 2) {
					for (tempVoxelPosRel.x = 0; tempVoxelPosRel.x < chunkSize; ++tempVoxelPosRel.x, ++index) {
						if (!tempVoxels[index].isAir) {
							tempVoxels[index].Build(tmd, index, ref vc);
						}
					}
				}
			}

			// mesh creation
			chunk.m.Clear();
			tmd.Assign(chunk.m);
			chunk.m.Optimize();
			chunk.mf.sharedMesh = chunk.m;
			chunk.mc.sharedMesh = null;
			chunk.mc.sharedMesh = chunk.m;
			chunk.built = true;

			System.GC.Collect();

			return chunk;
		}

#if UNITY_EDITOR
		void OnDrawGizmos() {
			if (tempVoxels == null || tempVoxels.Length == 0)
				return;

			Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit, 10000.0f)) {
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(hit.point, 0.1f);
				Gizmos.DrawLine(hit.point, hit.point + hit.normal);
			}
			/*
			if (boxesTemp != null && boxesTemp.Length > 0) {
				Vector3 size = Vector3.one * 0.5f;
				Gizmos.color = new Color(0.5f, 0, 0, 0.8f);
				int start = (chunkSize + 3) * chunkSizeA2 + 1;
				for (int i = start, y = 0; y < chunkSize; ++y, i += chunkSizeA2M2)
					for (int z = 0; z < chunkSize; ++z, i += 2)
						for (int x = 0; x < chunkSize; ++x, ++i)
							if (boxesTemp[i] >= 0) Gizmos.DrawCube(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), size);
			}
			*/
		}
#endif
	}

}
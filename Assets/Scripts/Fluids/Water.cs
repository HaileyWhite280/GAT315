using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshCollider))]
public class Water : MonoBehaviour
{
	[System.Serializable]
	struct Wave
    {
		[Range(0.0f, 10.0f)] public float amplitude;
		[Range(0.0f, 10.0f)] public float length;
		[Range(0.0f, 10.0f)] public float rate;

	}

	[SerializeField] [Range(1.0f, 90.0f)] float fps = 30;
	[SerializeField] [Range(0.0f, 1.0f)] float damping = 0.04f;

	[Header("Waves")]
	[SerializeField] Wave waveA;
	[SerializeField] Wave waveB;

	[Header("Mesh Generator")]
	[SerializeField] [Range(1.0f, 80.0f)] float xMeshSize = 40.0f;
	[SerializeField] [Range(1.0f, 80.0f)] float zMeshSize = 40.0f;
	[SerializeField] [Range(2, 80)] int xMeshVertexNum = 2;
	[SerializeField] [Range(2, 80)] int zMeshVertexNum = 2;
	
	MeshFilter meshFilter;
	MeshCollider meshCollider;

	Mesh mesh;
	Vector3[] vertices;

	float time;
	int frame;

	float[,] buffer1;
	float[,] buffer2;

	float timeStep { get => 1.0f / fps; }

	float[,] previousBuffer { get => ((frame % 2) == 0) ? buffer1 : buffer2; }
	float[,] currentBuffer  { get => ((frame % 2) == 0) ? buffer2 : buffer1; }

	void Start()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshCollider = GetComponent<MeshCollider>();

		MeshGenerator.Plane(meshFilter, xMeshSize, zMeshSize, xMeshVertexNum, zMeshVertexNum);

		mesh = meshFilter.mesh;
		vertices = mesh.vertices;

		buffer1 = new float[xMeshVertexNum, zMeshVertexNum];
		buffer2 = new float[xMeshVertexNum, zMeshVertexNum];
	}

	void Update()
	{
		time += Time.deltaTime;
		while (time > timeStep)
		{
			frame++;
			UpdateSimulation(previousBuffer, currentBuffer, timeStep);
			//UpdateWave(currentBuffer);

			time -= timeStep;
		}

		// set vertices height from current buffer
		for (int x = 0; x < xMeshVertexNum; x++)
		{
			for (int z = 0; z < zMeshVertexNum; z++)
			{
				vertices[x + z * xMeshVertexNum].y = currentBuffer[x, z];
			}
		}

		// recalculate mesh with new vertices
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		mesh.RecalculateBounds();
		meshCollider.sharedMesh = mesh;
	}

	void UpdateWave(float[,] buffer)
	{
		for (int x = 0; x < xMeshVertexNum; x++)
		{
			for (int z = 0; z < zMeshVertexNum; z++)
			{
				float tA = (z) * waveA.length;
				float vA = Mathf.Sin(tA + (Time.time * waveA.rate)) * waveA.amplitude;

				float tB = (x) * waveB.length;
				float vB = Mathf.Sin(tB + (Time.time * waveB.rate)) * waveB.amplitude;

				buffer[x, z] = vA + vB;
			}
		}
	}

	void UpdateSimulation(float[,] previous, float[,] current, float dt)
	{
		for (int x = 1; x < xMeshVertexNum-1; x++)
		{
			for (int z = 1; z < zMeshVertexNum-1; z++)
			{
				current[x, z] = previous[x, z];
			}
		}
	}

	public void Touch(Ray ray, float offset)
	{
		if (Physics.Raycast(ray, out RaycastHit raycastHit))
		{
			// check if ray cast hit this mesh
			MeshCollider meshCollider = raycastHit.collider as MeshCollider;
			if (meshCollider == this.meshCollider)
			{
				// get hit triangle
				int[] triangles = mesh.triangles;
				// get triangle index hit
				int index = triangles[raycastHit.triangleIndex * 3];
				// get x and z vertex
				int x = index % xMeshVertexNum;
				int z = index / xMeshVertexNum;

				if (x > 1 && x < xMeshVertexNum - 1 && z > 1 && z < zMeshVertexNum - 1)
				{
					currentBuffer[x, z] = offset;
				}
			}
		}
	}
}

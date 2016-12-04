﻿using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]

public class SimpleColliderToMesh : MonoBehaviour {


	PolygonCollider2D pc2 ;
	void Start () {



		pc2 = gameObject.GetComponent<PolygonCollider2D>();
		int pointCount = 0;
		pointCount = pc2.GetTotalPointCount();
		MeshFilter mf = GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		Vector2[] points = pc2.points;
		Vector3[] vertices = new Vector3[pointCount];
		for(int j=0; j<pointCount; j++){
			Vector2 actual = points[j];
			vertices[j] = new Vector3(actual.x, actual.y, 0);
		}
		Triangulator tr = new Triangulator(points);
		int [] triangles = tr.Triangulate();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mf.mesh = mesh;
		//Render thing
		}
	}

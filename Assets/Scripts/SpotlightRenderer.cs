using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightRenderer : MonoBehaviour
{
  [SerializeField]
  private int sides = 6;
  [SerializeField]
  private float radius = 0.5f;
  [SerializeField]
  private LayerMask layerMask;

  private float distanceFromOrigin = 1;
  private Mesh mesh;

  public float MaxDistance {
    get;
    private set;
  } = 100;


  void Start() {
    mesh = new Mesh();

    var meshFilter = GetComponent<MeshFilter>();
    meshFilter.mesh = mesh;
    CalculateMesh(mesh, radius, true);
  }

  void Update() {
    CalculateMesh(mesh, radius, false);
  }

  Vector3 RaycastVertex(Vector3 throughPoint, bool fullLength) {
    var dir = throughPoint - transform.position;

    if (!fullLength) {
      RaycastHit hit;
      if (Physics.Raycast(transform.position, dir, out hit, MaxDistance, layerMask)) {
        if (hit.collider) {
          return hit.point;
        }
      }
    }

    return transform.position + dir * MaxDistance;
  }

  void CalculateMesh(Mesh mesh, float radius, bool initial, bool fullLength = false) {
    var vnum = (sides * 2) + 1;
    Vector3[] vertices = new Vector3[vnum];
    int[] tris = null;
    Vector3[] normals = null;
    Vector2[] uv = null;

    if (initial) {
      tris = new int[sides * 3];
      normals = new Vector3[vnum];
      uv = new Vector2[vnum];
    }

    Vector3 center = transform.position + transform.forward * distanceFromOrigin;
    //Debug.DrawLine(transform.position, center, Color.black);

    // Add the origin of the spotlight
    vertices[0] = Vector3.zero;
    if (initial) {
      normals[0] = -transform.forward;
      uv[0] = Vector2.zero;
    }

    var startCorner = radius * transform.right + center;
    startCorner = RaycastVertex(startCorner, fullLength);
    startCorner = transform.InverseTransformPoint(startCorner);

    // The "previous" corner point, initialised to the starting corner.
    var previousCorner = startCorner;
    var vIndex = 1;
    var tIndex = 0;
    for (int i = 1; i < sides; i++) {
      // Calculate the angle of the corner in radians.
      float cornerAngle = 2f * Mathf.PI / sides * i;

      var currentCorner = Mathf.Cos(cornerAngle) * radius * transform.right + center + Mathf.Sin(cornerAngle) * radius * transform.up;
      currentCorner = RaycastVertex(currentCorner, fullLength);
      currentCorner = transform.InverseTransformPoint(currentCorner);

      vertices[vIndex] = currentCorner;
      vertices[vIndex + 1] = previousCorner;

      if (initial) {
        tris[tIndex] = 0;
        tris[tIndex + 1] = vIndex;
        tris[tIndex + 2] = vIndex + 1;

        var direction = Vector3.Cross(previousCorner - transform.position, currentCorner - transform.position);
        var normal = direction.normalized;

        normals[vIndex] = normal;
        normals[vIndex + 1] = normal;

        uv[vIndex] = Vector2.one;
        uv[vIndex + 1] = Vector2.one;
      }

      // Having used the current corner, it now becomes the previous corner.
      previousCorner = currentCorner;
      vIndex += 2;
      tIndex += 3;
    }

    // Draw the final side by connecting the last corner to the starting corner.
    vertices[vIndex] = startCorner;
    vertices[vIndex + 1] = previousCorner;

    if (initial) {
      tris[tIndex] = 0;
      tris[tIndex + 1] = vIndex;
      tris[tIndex + 2] = vIndex + 1;

      var direction = Vector3.Cross(previousCorner - transform.position, startCorner - transform.position);
      var normal = direction.normalized;

      normals[vIndex] = normal;
      normals[vIndex + 1] = normal;

      uv[vIndex] = Vector2.one;
      uv[vIndex + 1] = Vector2.one;
    }

    mesh.vertices = vertices;
    if (initial) {
      mesh.triangles = tris;
      mesh.normals = normals;
      mesh.uv = uv;
    }
  }
}

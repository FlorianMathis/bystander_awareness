using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

 public static class CommandBufferExtensions
{
    private static List<MeshFilter> _meshFilters = new List<MeshFilter>();
    public static void DrawAllMeshes(this CommandBuffer cmd, GameObject gameObject, Material material, int pass)
    {
        _meshFilters.Clear();
        gameObject.GetComponentsInChildren(_meshFilters);
 
        foreach (MeshFilter meshFilter in _meshFilters)
        {
            // Static objects may use static batching, preventing us from accessing their default mesh
            if (!meshFilter.gameObject.isStatic)
            {
                var mesh = meshFilter.sharedMesh;
                // Render all submeshes
                for (int i = 0; i < mesh.subMeshCount; i++)
                    cmd.DrawMesh(mesh, meshFilter.transform.localToWorldMatrix, material, i, pass);
            }
        }
    }
}
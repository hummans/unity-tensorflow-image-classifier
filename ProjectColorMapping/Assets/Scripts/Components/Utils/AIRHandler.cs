using System.Collections.Generic;
using UnityEngine;

namespace Components.Utils
{
    public static class AIRHandler
    {
        public static float[] CalculateMarkerImageVertex(GameObject cube)
        {
            List<Vector2> vertexList = new List<Vector2>();

            Vector3[] vertices = cube.GetComponent<MeshFilter>().mesh.vertices;
            Vector2[] result = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; ++i)
            {
                result[i] = Camera.main.WorldToScreenPoint(cube.transform.TransformPoint(vertices[i]));
                vertexList.Add(result[i]);
            }

            // Actual Device Size
            int screenHeight = Screen.height;

            // Use mesh bottom vertices
            // 14(LU), 13(RU), 12(RD), 15(LD)
            Vector2 LU = new Vector2();
            Vector2 RU = new Vector2();
            Vector2 RD = new Vector2();
            Vector2 LD = new Vector2();
            for (int i = 0; i < vertexList.Count; i++)
            {
                if (i >= 12 && i <= 15)
                {
                    LU = vertexList[14];
                    RU = vertexList[13];
                    RD = vertexList[12];
                    LD = vertexList[15];
                }
            }

            float[] srcPosition = new float[8];
            srcPosition[0] = LU.x;
            srcPosition[1] = screenHeight - LU.y;

            srcPosition[2] = RU.x;
            srcPosition[3] = screenHeight - RU.y;

            srcPosition[4] = RD.x;
            srcPosition[5] = screenHeight - RD.y;

            srcPosition[6] = LD.x;
            srcPosition[7] = screenHeight - LD.y;

            return srcPosition;
        }

    }
}

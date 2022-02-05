
using System.IO;
using UnityEngine;
using ViewModel;

namespace Components.Utils
{
    public static class Screenshot
    {
        private static string TEMP_FILE_NAME = "colorMappingTempImg.jpg";

        private static Rect rect;
        private static RenderTexture renderTexture;
        private static Texture2D screenShot;

        public static Texture2D GetScreenShot(Camera camera)
        {
            if (renderTexture == null)
            {
                int sceenWidth = Screen.width;
                int sceenHeight = Screen.height;

                rect = new Rect(0, 0, sceenWidth, sceenHeight);
                renderTexture = new RenderTexture(sceenWidth, sceenHeight, 24);
                screenShot = new Texture2D(sceenWidth, sceenHeight, TextureFormat.ARGB32, false);
            }

            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;

            // Normalize screenshoot with scene parameters
            Texture2D tex = screenShot;
            return tex;
        }

        public static Texture2D CropScreenshoot(Texture2D screenShot)
        {
            // Normalize screenshoot with scene parameters
            Texture2D tex = screenShot;

            byte[] texture_bytes = tex.EncodeToPNG();

            string filePath = Path.Combine(Application.persistentDataPath, TEMP_FILE_NAME);
            File.WriteAllBytes(filePath, texture_bytes);

            renderTexture = null;
            
            Debug.Log($"Screen sucess in: {Application.persistentDataPath}");
            return tex;
        }

        public static Texture2D ResampleAndCrop(Texture2D source, int targetWidth, int targetHeight)
        {
            int sourceWidth = source.width;
            int sourceHeight = source.height;
            float sourceAspect = (float)sourceWidth / sourceHeight;
            float targetAspect = (float)targetWidth / targetHeight;
            int xOffset = 0;
            int yOffset = 0;
            float factor = 1;
            if (sourceAspect > targetAspect)
            { // crop width
                factor = (float)targetHeight / sourceHeight;
                xOffset = (int)((sourceWidth - sourceHeight * targetAspect) * 0.5f);
            }
            else
            { // crop height
                factor = (float)targetWidth / sourceWidth;
                yOffset = (int)((sourceHeight - sourceWidth / targetAspect) * 0.5f);
            }
            Color32[] data = source.GetPixels32();
            Color32[] data2 = new Color32[targetWidth * targetHeight];
            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    var p = new Vector2(Mathf.Clamp(xOffset + x / factor, 0, sourceWidth - 1), Mathf.Clamp(yOffset + y / factor, 0, sourceHeight - 1));
                    // bilinear filtering
                    var c11 = data[Mathf.FloorToInt(p.x) + sourceWidth * (Mathf.FloorToInt(p.y))];
                    var c12 = data[Mathf.FloorToInt(p.x) + sourceWidth * (Mathf.CeilToInt(p.y))];
                    var c21 = data[Mathf.CeilToInt(p.x) + sourceWidth * (Mathf.FloorToInt(p.y))];
                    var c22 = data[Mathf.CeilToInt(p.x) + sourceWidth * (Mathf.CeilToInt(p.y))];
                    var f = new Vector2(Mathf.Repeat(p.x, 1f), Mathf.Repeat(p.y, 1f));
                    data2[x + y * targetWidth] = Color.Lerp(Color.Lerp(c11, c12, p.y), Color.Lerp(c21, c22, p.y), p.x);
                }
            }
    
            var tex = new Texture2D(targetWidth, targetHeight);
            tex.SetPixels32(data2);
            tex.Apply(true);
            return tex;
        }
    }
}

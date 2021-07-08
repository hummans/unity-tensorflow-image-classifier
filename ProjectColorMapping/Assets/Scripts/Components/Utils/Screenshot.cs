
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

        public static Texture2D GetScreenShot(Camera camera, DebugConsole debugConsole)
        {
            if (renderTexture == null)
            {
                int sceenWidth = Screen.width;
                int sceenHeight = Screen.height;

                rect = new Rect(0, 0, sceenWidth, sceenHeight);
                renderTexture = new RenderTexture(sceenWidth, sceenHeight, 24);
                screenShot = new Texture2D(sceenWidth, sceenHeight, TextureFormat.RGB24, false);
            }

            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;

            Texture2D tex = screenShot;

            byte[] texture_bytes = tex.EncodeToJPG();

            string filePath = Path.Combine(Application.persistentDataPath, TEMP_FILE_NAME);
            File.WriteAllBytes(filePath, texture_bytes);

            renderTexture = null;
            screenShot = null;
            
            debugConsole.logInput.Value = "Screen sucess in: " +  Application.persistentDataPath;
            return tex;
        }
    }
}

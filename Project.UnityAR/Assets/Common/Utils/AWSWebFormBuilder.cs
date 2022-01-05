 using UnityEngine;
 using System;
 using System.Text;
 using System.Collections;
 using System.Security.Cryptography;
using SimpleJSON;

public static class AWSWebFormBuilder
 { 
    public static WWWForm BuildUploadRequest(JSONNode fields, Texture2D textureScreenshoot, string file_name)
    {
        WWWForm form = new WWWForm();
        string policy = Convert.ToBase64String(Encoding.UTF8.GetBytes(fields["policy"]));

        string encode = "png";
        byte[] image = ImageHandler.GetTextureBytes(textureScreenshoot, encode);
        string date = fields["x-amz-date"].ToString();

        form.AddField("key", fields["key"].ToString());
        form.AddField("x-amz-algorithm", "AWS4-HMAC-SHA256");
        form.AddField("x-amz-credential", fields["x-amz-credential"].ToString());
        form.AddField("x-amz-date", date);
        form.AddField("x-amz-security-token", fields["x-amz-security-token"].ToString());
        form.AddField("policy", policy);
        form.AddField("x-amz-signature", fields["x-amz-signature"].ToString());
        form.AddBinaryData("file", image, file_name, $"image/{encode}");
        
        return form;
    }
 }
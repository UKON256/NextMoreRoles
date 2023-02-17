using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnhollowerBaseLib;

namespace NextMoreRoles.Modules;

class ResourcesManager
{
    public static Dictionary<string, Sprite> CachedSprites = new();
    public static Sprite LoadSpriteFromResources(string path, float pixelsPerUnit) {
        try
        {
            if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;
            Texture2D texture = LoadTextureFromResources(path);
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            return CachedSprites[path + pixelsPerUnit] = sprite;
        } catch {
            Logger.Error("パスからのスプライト生成に失敗しました", "ResourcesManager");
        }
        return null;
    }

    public static unsafe Texture2D LoadTextureFromResources(string path) {
        try {
            Texture2D texture = new(2, 2, TextureFormat.ARGB32, true);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(path);
            var byteTexture = new byte[stream.Length];
            var read = stream.Read(byteTexture, 0, (int)stream.Length);
            LoadImage(texture, byteTexture, false);
            return texture;
        } catch {
            Logger.Error("画像の読み込みに失敗しました。Path: " + path, "ResourcesManager");
        }
        return null;
    }

    public static Dictionary<string, AudioClip> AudioClipCache = new();
    public static AudioClip LoadAudioClipFromResources(string path, string clipName = "UNNAMED_NAME")
    {
        try
        {
            if (AudioClipCache.TryGetValue(path, out AudioClip audio)) return audio;

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(path);
            var byteAudio = new byte[stream.Length];
            var read = stream.Read(byteAudio, 0, (int)stream.Length);
            float[] samples = new float[byteAudio.Length / 4];
            int offset;
            for (int i = 0; i < samples.Length; i++)
            {
                offset = i * 4;
                samples[i] = (float)BitConverter.ToInt32(byteAudio, offset) / int.MaxValue;
            }
            int channels = 2;
            int sampleRate = 48000;
            AudioClip audioClip = AudioClip.Create(clipName, samples.Length, channels, sampleRate, false);
            audioClip.SetData(samples, 0);
            audioClip.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            return AudioClipCache[path] = audioClip;
        } catch {
            Logger.Error("パスからのオーディオの生成に失敗しました", "ResourcesManager");
        }
        return null;
    }


    internal delegate bool d_LoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
    internal static d_LoadImage iCall_LoadImage;
    private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
    {
        if (iCall_LoadImage == null)
        iCall_LoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");
        var il2cppArray = (Il2CppStructArray<byte>)data;
        return iCall_LoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
    }

    public static Texture2D LoadTextureFromDisk(string path) {
    try {
            if (File.Exists(path))     {
            Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
            var byteTexture = Il2CppSystem.IO.File.ReadAllBytes(path);
            ImageConversion.LoadImage(texture, byteTexture, false);
            return texture;
            }
        } catch {
            Logger.Error("テクスチャの読み込みに失敗しました。Path: " + path, "ResourcesManager");
        }
        return null;
    }
}

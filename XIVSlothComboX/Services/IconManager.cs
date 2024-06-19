namespace XIVSlothComboX.Services;

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dalamud;
using Dalamud.Game.Config;
using Dalamud.Interface.Internal;
using Dalamud.Utility;
using ImGuiNET;
using Lumina.Data.Files;
using Action = Lumina.Excel.GeneratedSheets.Action;


public class IconManager : IDisposable {
    private bool disposed;
    private readonly Dictionary<(int, bool), IDalamudTextureWrap> iconTextures = new();
    private readonly Dictionary<uint, ushort> actionCustomIcons = new() {
            
    };


    
    
    public IconManager() {
     
    }

    public void Dispose() {
        disposed = true;
        var c = 0;
        foreach (var texture in iconTextures.Values.Where(texture => texture != null)) {
            c++;
            texture.Dispose();
        }

        iconTextures.Clear();
        
    }
        
    private void LoadIconTexture(int iconId, bool hq = false) {
        Task.Run(() => {
            try {
                var iconTex = GetIcon(iconId, hq);

                var tex = Service.Interface.UiBuilder.LoadImageRaw(iconTex.GetRgbaImageData(), iconTex.Header.Width, iconTex.Header.Height, 4);

                if (tex.ImGuiHandle != IntPtr.Zero) {
                    this.iconTextures[(iconId, hq)] = tex;
                } else {
                    tex.Dispose();
                }
            } catch (Exception ex) {
            }
        });
    }
        
    public TexFile GetIcon(int iconId, bool hq = false) => this.GetIcon(Service.DataManager.Language, iconId, hq);

    /// <summary>
    /// Get a <see cref="T:Lumina.Data.Files.TexFile" /> containing the icon with the given ID, of the given language.
    /// </summary>
    /// <param name="iconLanguage">The requested language.</param>
    /// <param name="iconId">The icon ID.</param>
    /// <returns>The <see cref="T:Lumina.Data.Files.TexFile" /> containing the icon.</returns>
    public TexFile GetIcon(ClientLanguage iconLanguage, int iconId, bool hq = false)
    {
        string type;
        switch (iconLanguage)
        {
            case ClientLanguage.Japanese:
                type = "ja/";
                break;
            case ClientLanguage.English:
                type = "en/";
                break;
            case ClientLanguage.German:
                type = "de/";
                break;
            case ClientLanguage.French:
                type = "fr/";
                break;

            default:
                type = "/";
                break;
        }
        return this.GetIcon(type, iconId, hq);
    }
        
    public TexFile GetIcon(string type, int iconId, bool hq = false)
    {
        if (type == null)
            type = string.Empty;
        if (type.Length > 0 && !type.EndsWith("/"))
            type += "/";
            
        var formatStr = $"ui/icon/{{0:D3}}000/{(hq?"hq/":"")}{{1}}{{2:D6}}.tex";
        TexFile file = Service.DataManager.GetFile<TexFile>(string.Format(formatStr, (object) (iconId / 1000), (object) type, (object) iconId));
        return file != null || type.Length <= 0 ? file : Service.DataManager.GetFile<TexFile>(string.Format(formatStr, (object) (iconId / 1000), (object) string.Empty, (object) iconId));
    }
        

    public IDalamudTextureWrap? GetActionIcon(Action action) {
        return GetIconTexture(actionCustomIcons.ContainsKey(action.RowId) ? actionCustomIcons[action.RowId] : action.Icon);
    }

    public ushort GetActionIconId(Action action) {
        return actionCustomIcons.ContainsKey(action.RowId) ? actionCustomIcons[action.RowId] : action.Icon;
    }

    public IDalamudTextureWrap? GetIconTexture(int iconId, bool hq = false) {
        if (this.disposed) 
            return null;
        if (this.iconTextures.ContainsKey((iconId, hq))) 
            return this.iconTextures[(iconId, hq)];
        this.iconTextures.Add((iconId, hq), null);
        LoadIconTexture(iconId, hq);
        return this.iconTextures[(iconId, hq)];
    }
}
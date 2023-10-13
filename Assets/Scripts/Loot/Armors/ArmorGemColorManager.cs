using UnityEngine;
using System.Collections;

public class ArmorGemColorManager : MonoBehaviour {

    static public ArmorGemColorManager Instance;

    public Color[] None = new Color[2];
    public Color[] Ruby = new Color[2];
    public Color[] Sapphire = new Color[2];
    public Color[] Emerald = new Color[2];
    public Color[] Garnet = new Color[2];
    public Color[] Amethyst = new Color[2];
    public Color[] Malachite = new Color[2];
    public Color[] Obsidian = new Color[2];
    public Color[] Golden = new Color[2];
    public Color[] Jade = new Color[2];

    public Shader NoramlShader;
    public Shader LuxuryShader;

    public Texture LuxuryPuslingTex01;
    public Texture LuxxryPuslingTex02;

    void Awake()
    {
        Instance = this;
    }
}

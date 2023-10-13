using UnityEngine;
using System.Collections;

public class testbundle : MonoBehaviour {

    public string s = "CH_Wolf_Demon";

    void Start()
    {
        StartCoroutine(testBundle(s));
    }

    IEnumerator testBundle(string mapName)
    {
        string _bundlestring = BundlePath.AssetbundleBaseURL + s + ".assetbundle";
        WWW PerfabWWW = new WWW(_bundlestring);
        yield return PerfabWWW;
        GameObject _go = (GameObject)PerfabWWW.assetBundle.mainAsset;
        Instantiate(_go);
    }
}

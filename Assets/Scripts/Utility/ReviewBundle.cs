using UnityEngine;
using System.Collections;

public class ReviewBundle : MonoBehaviour {

    public string BundleName;

    Object _curBundleObj;

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "Reload"))
        {
            if (_curBundleObj)
            {
                Destroy(_curBundleObj);
                _curBundleObj = null;
            }
            StartCoroutine(_downloadBundle(BundlePath.AssetbundleBaseURL + BundleName));
        }
    }

    IEnumerator _downloadBundle(string bundle)
    {
        WWW _bundle = new WWW(bundle);
        Debug.LogError("Loading : " + bundle);
        yield return _bundle;
        Debug.LogError("Download done");
        Instantiate(_bundle.assetBundle.mainAsset);
    }
}

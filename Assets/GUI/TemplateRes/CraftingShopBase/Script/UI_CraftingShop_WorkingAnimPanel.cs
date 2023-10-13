using UnityEngine;
using System.Collections;

public class UI_CraftingShop_WorkingAnimPanel : MonoBehaviour {

    [SerializeField]  UI_CraftingShop_CraftGroup CraftRoot;
    [SerializeField]  float animationTime = 0;
    [SerializeField]  Animation hammerAnim;
    [SerializeField]  string startAnim = "";
    [SerializeField]  string succeedAnim = "";
    [SerializeField]  string failedAnim = "";
    [SerializeField]  BoxCollider HitBlocker;
    [SerializeField]  GameObject continueBtn = null;
    [SerializeField]  UIFilledSprite progressBar = null;
    [SerializeField]  UILabel workingText = null;
    [SerializeField]  Transform craftingSfx = null;
    [SerializeField]  Transform failedSfx = null;
    [SerializeField]  Transform succeedSfx = null;

    public int attrLevel = 0;
    bool isSucceed = false;

    void Start()
    {
        continueBtn.gameObject.SetActive(false);
        HitBlocker.gameObject.SetActive(false);
    }

    public void StartAnim(bool succeed)
    {
        isSucceed = succeed;
        StartCoroutine(CraftingProgress(succeed));
    }

    IEnumerator CraftingProgress(bool succeed)
    {
        yield return null;
        continueBtn.gameObject.SetActive(false);
        HitBlocker.gameObject.SetActive(true);

        workingText.text = "Working...";
        hammerAnim.Play(startAnim);

        SoundCue.PlayPrefabAndDestroy(craftingSfx);

        progressBar.fillAmount = 0;
        progressBar.color = Color.yellow;

        yield return new WaitForSeconds(animationTime);

        progressBar.fillAmount = 1f;
        if (succeed)
        {
            hammerAnim.Play(succeedAnim);
            progressBar.color = Color.green;
            workingText.text = "Success!!";

            SoundCue.PlayPrefabAndDestroy(succeedSfx);
        }
        else
        {
            hammerAnim.Play(failedAnim);
            progressBar.color = Color.red;
            workingText.text = "Failed...Sorry!";

            SoundCue.PlayPrefabAndDestroy(failedSfx);
        }

        continueBtn.gameObject.SetActive(true);
        HitBlocker.gameObject.SetActive(false);

        if (UI_CraftingShop_Manager.Instance != null)
            UI_CraftingShop_Manager.Instance.CraftingAniFinished(succeed);
    }

    void Update()
    {
        if (progressBar.fillAmount < 1f)
        {
            progressBar.fillAmount += 1f / animationTime * Time.deltaTime;
        }
    }
    
}

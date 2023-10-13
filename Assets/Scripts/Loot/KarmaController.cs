using UnityEngine;
using System.Collections;

public class KarmaController : MonoBehaviour {
    // karma state is which controls what the karma could do
    public KarmaGroupManager.EnumKarmaType KarmaType;

    public Transform PickupSoundPrefab;

    public float SeekRadius = 5f;
    public float MoveSpeed = 1f;
    public float RotateSpeed = 3f;

    public KarmaGroupManager Parent;
    
    void Start()
    {
        Vector3[] _path = { transform.position, transform.position + Vector3.up, transform.position, transform.position - Vector3.up, transform.position };
        iTween.MoveTo(gameObject, iTween.Hash("path", _path, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear, "speed", Random.Range(4.0f, 5.0f)));
        iTween.RotateBy(gameObject, iTween.Hash("y", 360, "speed", Random.Range(RotateSpeed * 0.5f, RotateSpeed * 1.5f), "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));
    }

    void Update()
    {
        #region State Control
        if (Parent.KState == KarmaGroupManager.EnumKarmaState.CanChase && Player.Instance && Vector3.Distance(transform.position, Player.Position) < SeekRadius && !Player.Instance.FSM.IsInState(Player.Instance.DS))
        {
            Parent.KState = KarmaGroupManager.EnumKarmaState.Chasing;
        }

        if (Parent.KState == KarmaGroupManager.EnumKarmaState.Chasing && Player.Instance && Vector3.Distance(transform.position, Player.Position) < 1)
        {
            SoundCue.PlayPrefabAndDestroy(PickupSoundPrefab);
            Parent.PlayerGetKarma(this);
            Parent.Target = Player.Instance.transform;
        }
        #endregion

        #region Movement Control
        if (Parent.KState == KarmaGroupManager.EnumKarmaState.Chasing && !Player.Instance.FSM.IsInState(Player.Instance.DS))
        {
            Vector3 diffPos = (Player.Position + Vector3.up * 0.75f - transform.position).normalized;
            transform.position += diffPos * MoveSpeed * Time.deltaTime;
        }
        #endregion
    }

    public void DestroyKarma()
    {
        Destroy(transform.FindChild("GAM_KarmaShard").gameObject);
        DestructAfterTime.DestructGameObjectNow(gameObject);
    }
}

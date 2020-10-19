using UnityEngine;

public class PlayerController : Controller
{
    private Transform camTrans;
    private Vector3 camOffset;
    

    private float targetBlend;
    private float currentBlend;

    public GameObject daggeratk1fx;

    private void Start()
    {
        base.Init();
        if (daggeratk1fx != null)
        {
            Debug.Log("添加技能");
            fxDic.Add(daggeratk1fx.name, daggeratk1fx);
        }
        Init();
    }

    public override void Init()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;   
    }

    private void Update()
    {
        #region Input
        /*
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 _dir = new Vector2(h, v).normalized;
        if (_dir != Vector2.zero)
        {
            Dir = _dir;
            SetBlend(Constants.BlendWalk);
        }
        else
        {
            Dir = Vector2.zero;
            SetBlend(Constants.BlendIdle);
        }
        */
        #endregion

        if (currentBlend != targetBlend)
        {
            UpdateMixBlend();
        }

        if (isMove)
        {
            //设置方向
            SetDir();
            //产生移动
            SetMove();
            //相机跟随
            SetCam();
        }

        if (skillMove)
        {
            SetSkillMove();
            //相机跟随
            SetCam();
        }
    }

    private void SetDir()
    {
        if (camTrans == null)
        {
            Debug.Log("camTrans == null");
        }
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetCam()
    {
        if (camTrans != null)
        {
            camTrans.position = transform.position - camOffset;
        }
    }

    public override void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }
        ani.SetFloat("Blend", currentBlend);
    }

    public override void SetFX(string name, float destroy)
    {
        GameObject go;
        Debug.Log("当前已赋值的技能特效：" + fxDic.Count);
        foreach(var item in fxDic)
        {
            Debug.Log("--------");
            Debug.Log(item.Key);
            Debug.Log(item.Value);
        }

        Debug.Log("name = " +  name);

        if (fxDic.TryGetValue(name, out go))
        {
            go.SetActive(true);
            Debug.Log("显示技能特效");
            timerSvc.AddTimeTask((int tid) => 
            {
                go.SetActive(false);
            }, destroy);
        }
        else
        {
            Debug.Log("技能没赋值");
        }
    }

    private void SetSkillMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * skillMoveSpeed);
    }

}
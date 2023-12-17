using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SSC_PaintGun : MonoBehaviour
{
    [SerializeField] private Brush brush;
    [SerializeField] private SSC_GunState gun;
    [SerializeField] private InputReader input;
    [SerializeField] private Transform checkPos;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform aimTarget;


    [SerializeField] private LayerMask gunLayer = -1;
    [Range(0.1f, 1f)] public float attackSpeed;

    [SerializeField, Range(0, 1)]
    float autoTime = 1f;
    [SerializeField, Range(1, 100)]
    float range = 50f;

    float rangeLimit = 4f;

    float timeCheck = 0f;
    float autotimeCheck = 0f;

    int normalShot = -10;
    int autoShot = -5;
    
    bool fireStart = false;

    private void Awake()
    {
        if (brush.splatTexture == null)
        {
            brush.splatTexture = Resources.Load<Texture2D>("splats");
            brush.splatsX = 4;
            brush.splatsY = 4;
        }
    }

    private void OnDrawGizmos()
    {
        //CheckGizmo();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(PaintTarget.CursorColor());
        }

        if (Input.GetKeyDown(KeyCode.R) && gun.CanReload)
        {
            PaintTarget.ClearAllPaint();
            gun.UpdateState(gun.MaxAmmo, GunState.READY);
        }

        if (!gun.CanFire)
        {
            fireStart = false;
            autotimeCheck = 0f;
            return;
        }

        // ���콺 Ŭ������ ���� ���� ��� ����.
        if(!input.ShootKey)
        {
            fireStart = false;
            autotimeCheck = 0f;
        }

        // �����ð����� ���Ű �Է»��¶�� ������
        else if(autotimeCheck > autoTime && gun.state == GunState.READY)
        {
            AutoFire();
            return;
        }

        // ����� ���� == ���콺��ư ������������
        else if(fireStart == true)
        {
            autotimeCheck += Time.deltaTime;
            return;
        }

        else if(input.ShootKey && gun.state == GunState.READY)
        {            
            NormalFire();
        }

    }

    /// <summary>
    /// �ܹ� �޼ҵ�
    /// </summary>
    private void NormalFire()
    {
        Ray ray = new Ray(GetOriginPos(), CheckDir());
        Ray checkRay = new Ray(checkPos.position, CheckDir());        
        RaycastHit hit;

        // ���� ������Ʈ�� ������ rangeLimit �� �Ÿ� ������ ��
        if(Physics.Raycast(checkRay, out hit, range, gunLayer))
        {
            float checkDistance = Vector3.Distance(checkPos.position, hit.point);

            if (checkDistance <= rangeLimit)
            {
                UsedAmmo(checkRay, normalShot);

                fireStart = true;
                return;
            }
        }

        // �Ϲ����� ��Ȳ�� ���
        if (Physics.Raycast(ray, out hit, range, gunLayer))
        {
            Ray muzzleRay = new Ray(startPoint.position, hit.point - startPoint.position);

            UsedAmmo(muzzleRay, normalShot);

            fireStart = true;
        }
        
    }

    /// <summary>
    /// ���� �޼ҵ�
    /// </summary>
    private void AutoFire()
    {
        timeCheck += Time.deltaTime;

        if (timeCheck >= attackSpeed)
        {
            Ray ray = new Ray(GetOriginPos(), CheckDir());
            Ray checkRay = new Ray(checkPos.position, CheckDir());
            RaycastHit hit;

            if (Physics.Raycast(checkRay, out hit, range, gunLayer))
            {
                float checkDistance = Vector3.Distance(checkPos.position, hit.point);

                if (checkDistance <= rangeLimit)
                {
                    UsedAmmo(checkRay, autoShot);
                    
                    timeCheck = 0f;
                    return;
                }                                

            }

        }

        if (timeCheck >= attackSpeed)
        {
            Ray ray = new Ray(GetOriginPos(), CheckDir());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range, gunLayer))
            {
                Ray muzzleRay = new Ray(startPoint.position, hit.point - startPoint.position);

                UsedAmmo(muzzleRay, autoShot);

                timeCheck = 0f;        
            }
            
        }
    }

    /// <summary>
    /// ī�޶�� �÷��̾��� ���� ���ϼ��� �����ִ� �޼ҵ�
    /// </summary>
    /// <returns>ī�޶��� ���� ���� + �÷��̾��� �������� </returns>
    Vector3 GetOriginPos()
    {
        Vector3 origin = Vector3.zero;

        origin = Camera.main.transform.position +
            Camera.main.transform.forward *
            Vector3.Distance(Camera.main.transform.position, startPoint.position);

        return origin;
    }
    /// <summary>
    /// GetOriginPos()�� ���� ���� �࿡�� ī�޶��� Ray������ �� �޼ҵ�
    /// </summary>
    /// <returns></returns>
    Vector3 CheckDir()
    {
        Vector3 dir = Vector3.zero;
        dir = Camera.main.transform.forward +
            Camera.main.transform.TransformDirection(dir);

        return dir;
    }

    /// <summary>
    /// ���޹��� Ray ��ġ�� PaintTarget.PaintRay() ����
    /// <para>
    /// ���� ���޹��� _ammo����ŭ GunState�� �Ҹ� ��û
    /// </para>
    /// </summary>
    /// <param name="_ray"></param>
    /// <param name="_ammo"></param>
    void UsedAmmo(Ray _ray, int _ammo)
    {
        PaintTarget.PaintRay(_ray, brush, range);

        gun.UpdateState(_ammo);

        if (gun.Ammo <= 0)
        {
            gun.UpdateState(0, GunState.EMPTY);
        }
    }

    void CheckGizmo()
    {
        //Gizmos.color = Color.yellow;
        //Vector3 dir = Vector3.zero;
        //dir = CheckDir();     

        //Gizmos.DrawLine(GetOriginPos(), Camera.main.transform.forward);

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(startPoint.position, Camera.main.transform.position + Camera.main.transform.forward * range);


        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * range);
    }
}

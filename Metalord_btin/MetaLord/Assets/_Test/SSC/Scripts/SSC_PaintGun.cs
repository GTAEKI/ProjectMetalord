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
        //Gizmos.color = Color.yellow;
        //Vector3 dir = Vector3.zero;
        //dir = Camera.main.transform.forward +
        //    Camera.main.transform.TransformDirection(dir);

        //Gizmos.DrawLine(GetOriginPos(), Camera.main.transform.forward);

        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(startPoint.position, Camera.main.transform.position + Camera.main.transform.forward * range);


        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * range);
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
        Vector3 dir = Vector3.zero;
        dir = Camera.main.transform.forward +
            Camera.main.transform.TransformDirection(dir);

        Ray ray = new Ray(GetOriginPos(), dir);
        RaycastHit hit;

        Ray checkRay = new Ray(checkPos.position, dir);
        RaycastHit checkHit;

        if(Physics.Raycast(checkRay, out checkHit, range, gunLayer))
        {
            float checkDistance = Vector3.Distance(checkPos.position, checkHit.point);
            //Debug.Log($"�������� �Ÿ� : {checkDistance}");

            if (checkDistance <= 4f)
            {
                Ray muzzleRay = new Ray(checkPos.position, dir);

                PaintTarget.PaintRay(muzzleRay, brush, range);

                gun.UpdateState(normalShot);

                if (gun.Ammo <= 0)
                {
                    gun.UpdateState(0, GunState.EMPTY);
                }

                fireStart = true;
                return;
            }
        }

        if (Physics.Raycast(ray, out hit, range, gunLayer))
        {
            Ray muzzleRay = new Ray(startPoint.position, hit.point - startPoint.position);

            float rangePos = Vector3.Distance(GetOriginPos(), hit.point);

            PaintTarget.PaintRay(muzzleRay, brush, range);

            gun.UpdateState(normalShot);

            if (gun.Ammo <= 0)
            {
                gun.UpdateState(0, GunState.EMPTY);
            }

            if(hit.transform.GetComponent<NpcBase>() != null)
            {
                hit.transform.GetComponent<NpcBase>().ChangedState(npcState.glued);
            }

            fireStart = true;
        }
    }

    /// <summary>
    /// ���� �޼ҵ�
    /// </summary>
    private void AutoFire()
    {
        timeCheck += Time.deltaTime;

        if(timeCheck >= attackSpeed)
        {
            Vector3 dir = Vector3.zero;
            dir = Camera.main.transform.forward +
                Camera.main.transform.TransformDirection(dir);

            Ray ray = new Ray(GetOriginPos(), dir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range, gunLayer))
            {
                Ray muzzleRay = new Ray(startPoint.position, hit.point - startPoint.position);

                PaintTarget.PaintRay(muzzleRay, brush, range);

                gun.UpdateState(autoShot);

                if (gun.Ammo <= 0)
                {
                    gun.UpdateState(0, GunState.EMPTY);
                }

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

}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class player : MonoBehaviour
{
    public Transform MainCamera;
    // Start is called before the first frame update
    public float Speed;

    // public GameObject[] weapons = new GameObject[3]; // 3개의 무기 오브젝트
    public GameObject[] weapons;
    // public bool[] hasWeapons = new bool[3] { false, false, false }; // 3개의 무기 슬롯
    public bool[] hasWeapons;
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool dDown;

    bool isJump;
    bool isDodge;
    bool isSwap;
    Vector3 moveVec;
    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;
    GameObject equipWeapon;
    int equipWeaponIndex = -1; // 현재 장착된 무기 인덱스

    [SerializeField]
    float maxSlopeAngle = 45f;
    [SerializeField]
    float rayDistance = 1.2f; // Raycast 거리
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
        Swap();
        Interact();
    }
    // void FixedUpdate() {

    // }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interact");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
        dDown = Input.GetButtonDown("Dodge");
    }
    void Move()
    {
        Vector3 forward = MainCamera.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = MainCamera.right;
        right.y = 0;
        right.Normalize();

        // if (isDodge)
        // {
        //     // 대시 중에는 이동 벡터를 무시하고 현재 방향으로만 이동
        //     moveVec = transform.forward;
        // }
        // else if (isJump)
        // {
        //     // 점프 중에는 이동 벡터를 무시하고 현재 방향으로만 이동
        //     moveVec = transform.forward;
        // }
        if (isSwap)
        {
            // 무기 교체 중에는 위치 고정
            moveVec = Vector3.zero;
        }

        // RaycastHit forwardHit;
            // Vector3 rayOrigin = transform.position + Vector3.up * 0.2f;
            // Vector3 rayDir = moveVec; // 이동 입력 방향

            // if (Physics.Raycast(rayOrigin, rayDir, out forwardHit, 0.6f))
            // {
            //     float angle = Vector3.Angle(forwardHit.normal, Vector3.up);
            //     if (angle > maxSlopeAngle)
            //     {
            //         moveVec = Vector3.zero; // 정면에 높은 경사 → 이동 차단
            //     }
            // }

            // 입력에 따라 카메라 기준으로 이동 벡터 변형
            moveVec = (forward * vAxis + right * hAxis).normalized;

        // 2. 발밑(또는 캐릭터 중심 아래)으로 Raycast 쏴서 지형 normal 얻기
        RaycastHit hit;
        Vector3 groundNormal = Vector3.up;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, rayDistance))
        {
            groundNormal = hit.normal;
        }
        // 2. normal 각도 구하기
        float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);

        // 3. 최대 슬로프 각도 초과시 이동 막기
        if (slopeAngle > maxSlopeAngle)
        {
            // 경사 너무 심하면 이동 금지
            moveVec = Vector3.zero;

            // 경사가 너무 가파르면 캐릭터가 뒤로 후퇴(미끄러짐)하도록 설정
            // 일반적으로, 지형의 법선 방향에 -moveVec(즉, 진행하려던 방향의 반대) 벡터를 섞어줌
            // Vector3 backDir = Vector3.ProjectOnPlane(-groundNormal, Vector3.up).normalized;
            // float slideSpeed = 2.0f; // 원하는 후퇴(미끄러짐) 속도를 설정
            // moveVec = backDir * slideSpeed;
        }
        // 3. 이동 벡터를 지형 normal 평면에 투영 (slope 타고 이동)
        Vector3 slopeMoveVec = Vector3.ProjectOnPlane(moveVec, groundNormal).normalized;

        //transform.position += moveVec * Speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        Vector3 targetPosition = rigid.position + slopeMoveVec * Speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        rigid.MovePosition(targetPosition);

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

        /*
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * Speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
        */
    }
    void Turn()
    {
        // if (moveVec != Vector3.zero)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(moveVec);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        // }
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        // if (jDown && !isJump)
        // if (jDown && moveVec == Vector3.zero && !isDodge && !isSwap)
            if (jDown)
            {
                rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
                anim.SetBool("isJump", true);
                anim.SetTrigger("doJump");
                isJump = true;
            }
            /*
            if (jDown && !isJump)
            {
                isJump = true;
                rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            }
            */
        
    }
    void Dodge()
    {
        // if (jDown && !isJump)
        // if (dDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap)
            if (dDown && !isJump && !isDodge && !isSwap)
            {
                isDodge = true;
                // Debug.Log("Dodge");
                // rigid.AddForce(moveVec * Speed * 2, ForceMode.Impulse);
                // transform.position += moveVec * Speed * 2 * Time.deltaTime;
                // transform.position += moveVec * Speed * 2 * Time.deltaTime;
                // rigid.MovePosition(transform.position + moveVec * Speed * 2 * Time.deltaTime);
                // rigid.velocity = moveVec * Speed * 2;

                // 대시 속도 증가
                // rigid.AddForce(moveVec * Speed * 2, ForceMode.Impulse);
            
                Speed *= 1.5f; // 대시 속도 증가
                rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);

                anim.SetTrigger("doDodge");
                isJump = true;

                Invoke("DodgeOut", 0.4f);
            }
            /*
            if (jDown && !isJump)
            {
                isJump = true;
                rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            }
            */
        
    }
    void DodgeOut()
    {
        Speed /= 1.5f; // 대시 속도 원래대로
        isDodge = false;

    }

    void Swap()
    {
        if(sDown1 && (!hasWeapons[0] || equipWeaponIndex ==  0))
        {
            return;
        }
        else if(sDown2 && (!hasWeapons[1] || equipWeaponIndex ==  1))
        {
            return;
        }
        else if(sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
        {
            return;
        }

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        // if (weaponIndex < 0 || weaponIndex >= weapons.Length)
        // {
        //     Debug.LogWarning("Invalid weapon index: " + weaponIndex);
        //     return;
        // }
        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {
            // 무기 교체
            if (equipWeapon != null)
            {
                equipWeapon.SetActive(false);
            }
            // weapons[weaponIndex].SetActive(true);
            // Debug.Log("Swap to weapon " + (weaponIndex + 1));
            // equipWeaponIndex = weaponIndex; // 현재 장착된 무기 인덱스 업데이트
            equipWeapon = weapons[weaponIndex];

            equipWeapon.SetActive(true);

            anim.SetTrigger("doSwap");
            isSwap = true;
            Invoke("SwapOut", 0.4f);
        }
        // if (equipWeapon != null && isSwap) // 이미 교체 중인 경우
        // {
        //     if (equipWeapon != null)
        //         equipWeapon.SetActive(false);
        //     equipWeapon = weapons[weaponIndex];
        //     // Debug.Log("Swap to weapon 1");
        //     equipWeapon.SetActive(true);

        //     anim.SetTrigger("doSwap");
        //     isSwap = true;

        //     Invoke("SwapOut", 0.4f); // 0.4초 후에 SwapOut 호출
        // }
        // if (sDown1 || sDown2 || sDown3)
        // {
        //     weapons[weaponIndex].SetActive(true);
        // }

        
    }

        void SwapOut()
    {
        
        isSwap = false;

    }
    void Interact()
    {
        if (iDown && nearObject != null && !isJump && !isDodge)
        {
            // Debug.Log("Interaction with " + nearObject.name);
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject); // 무기 획득 후 오브젝트 제거
                // 무기와 상호작용
                // Debug.Log("Picked up weapon: " + nearObject.name);
                // 무기 획득 로직 추가
            }
            // else if (nearObject.CompareTag("Item"))
            // {
            //     // 아이템과 상호작용
            //     Debug.Log("Picked up item: " + nearObject.name);
            //     // 아이템 획득 로직 추가
            // }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
        Debug.Log(nearObject.name);
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }

}

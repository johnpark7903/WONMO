using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Use()
    {
        if (type == Type.Melee)
        {
            // Logic for melee weapon usage
            if (meleeArea != null)
            {
                StopCoroutine("Swing");
                StartCoroutine("Swing");
                // Additional melee logic can be added here
            }
        }
        else if (type == Type.Range)
        {
            StartCoroutine("Shot");
            // Logic for ranged weapon usage
            // This could involve shooting a projectile or similar
        }
    }

    IEnumerator Swing()
    {
        //1
        yield return new WaitForSeconds(0.1f); //1프레임 대기
        meleeArea.enabled = true; // 콜라이더 활성화
        trailEffect.enabled = true; // 트레일 이펙트 활성화
        //2
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false; // 콜라이더 비활성화
        //3
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false; // 트레일 이펙트 비활성화

        // if (trailEffect != null)
        // {
        //     trailEffect.Clear();
        //     trailEffect.enabled = true;
        // }
        // Additional logic for swinging the melee weapon can be added here
        yield return null;
    }

    // Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴
    // Use() 메인루틴 + Swing() 코루틴 (Co-Op)

    IEnumerator Shot()
    {
        //#1. 총알발사
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50; // Adjust the speed as necessary
        // if (bulletCase != null && bulletCasePos != null)
        // {
        //     GameObject instantBulletCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        //     // Add logic to handle the bullet case, such as applying force or velocity
        // }
        yield return null;
        //#2. 탄피배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * Random.Range(10, 20), ForceMode.Impulse);





        // Vector3 caseVec = bulletCasePos.right * 10; // Adjust the force as necessary
        // caseRigid.AddForce(bulletCasePos.right * 10, ForceMode.Impulse); // Adjust the force as necessary

        // if (bullet != null && bulletPos != null)
        // {
        //     GameObject newBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        //     // Add logic to handle the bullet's behavior, such as applying force or velocity
        // }
    }

}

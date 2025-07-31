using System.Collections;
using System.Collections.Generic;
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

}

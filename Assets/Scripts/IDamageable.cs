using UnityEngine;

public interface IDamageable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);//데미지를 입었을 때 호출되는 함수, 데미지량과 맞은 위치
}

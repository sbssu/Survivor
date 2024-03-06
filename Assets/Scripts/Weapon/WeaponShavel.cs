using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShavel : WeaponObject
{
    [SerializeField] Melee prefab;
    [SerializeField] Transform rotatePivot;

    Melee[] shavels;

    protected override void Initialize()
    {
        prefab.gameObject.SetActive(false);
        rotatePivot.localRotation = Quaternion.identity;
    }
    protected override IEnumerator IEAttack()
    {
        CreateShaval();

        // ���� �ð� (���� ������ �������� ���ӵǴ°�?)
        float showTime = cooltime;
        while (showTime > 0.0f)
        {
            if(!isPauseObject)
            {
                showTime -= Time.deltaTime;
                rotatePivot.Rotate(Vector3.forward, 360f * Time.deltaTime);  // pivot�� �ʴ� 1������ ���� �Ѵ�.
            }
                                    
            yield return null;
        }

        ResetShavel();
    }

    private void CreateShaval()
    {
        // �� ������ ���� �� ������ ���� ȸ�� �� ����.
        shavels = new Melee[projectileCount];
        for (int i = 0; i < shavels.Length; i++)
        {
            Quaternion rot = Quaternion.AngleAxis(360f / projectileCount * i, Vector3.forward);
            shavels[i] = Instantiate(prefab, rotatePivot);
            shavels[i].transform.localPosition = rot * Vector3.up * 1.35f;
            shavels[i].transform.localRotation = rot;
            shavels[i].Setup(power, knockback);
            shavels[i].gameObject.SetActive(true);
        }
    }
    private void ResetShavel()
    {
        rotatePivot.localRotation = Quaternion.identity;

        for (int i = 0; i < shavels.Length; i++)
            Destroy(shavels[i].gameObject);

        shavels = null;
    }
}

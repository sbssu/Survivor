using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Unit
{
    public static Player Instance { get; private set; }
    public Vector2 direction { get; private set; }

    [SerializeField] float magnetRange;

    SpriteRenderer spriteRenderer;
    LayerMask expMask;

    List<Item> inventory;           // 소지 아이템의 정보.
    List<WeaponObject> equipWeapons;      // 장비 아이템.

    private void Awake()
    {
        Instance = this;
    }
    protected new void Start()
    {
        inventory = new List<Item>();
        equipWeapons = new List<WeaponObject>();
        direction = Vector2.right;

        AddItem(ItemManager.Instance.GetItem("ITWE0002", 1));

        spriteRenderer = GetComponent<SpriteRenderer>();
        expMask = 1 << LayerMask.NameToLayer("Exp");

        base.Start();
        
        UpdateUI();
    }
    private void Update()
    {
        if (!isAlive || isPauseObject)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, magnetRange, expMask);
        foreach(Collider2D collider in colliders)
            collider.GetComponent<ExpObject>().ContactPlayer(transform, AddExp);
    }

    public void OnMovement(Vector2 input)
    {
        if (!isAlive || isPauseObject)
            return;

        // 방향 대입.
        if(input != Vector2.zero)
            direction = input;

        // 입력값을 이동량으로 변환 후 대입.
        Vector3 movement = input;
        Vector3 nextPosition = transform.position + movement * speed * Time.deltaTime;
        transform.position = Background.Instance.InBoundary(nextPosition, spriteRenderer.size);

        if (input.x != 0f)
            spriteRenderer.flipX = input.x < 0f;

        // 입력 값에 따른 애니메이션 처리.
        anim.SetBool("isRun", input != Vector2.zero);
    }

    private int NeedTotalExp(int level)
    {
        if (level <= 0)
            return 0;

        return Mathf.RoundToInt(5000f / 11 * (Mathf.Pow(1.11f, level - 1) - 1));
    }
    private void AddExp(int amount)
    {
        exp += amount;
        if (exp >= NeedTotalExp(level + 1))
            LevelUp();

        UpdateUI();
    }
    private void LevelUp()
    {
        level++;
        Item[] randomItems = ItemManager.Instance.GetRandomItem(inventory);
        GameManager.Instance.SwitchPauseForce(true);
        DrawUI.Instance.ShowDrawUI(randomItems, (select) => {
            AddItem(randomItems[select]);
            GameManager.Instance.SwitchPauseForce(false);
        });
    }
    private void AddItem(Item selectItem)
    {
        // 만약 선택한 아이템이 존재한다면 교체한다. 없다면 새로 대입한다.
        int index = inventory.FindIndex(item => item.id == selectItem.id);
        if (index == -1)
        {
            inventory.Add(selectItem);

            // 새로운 무기를 선택했을 경우 인스턴스 생성.
            if (selectItem is WeaponItem)
            {
                WeaponObject prefab = ItemManager.Instance.GetWeaponPrefab(selectItem.id);
                WeaponObject newWeapon = Instantiate(prefab, transform);
                equipWeapons.Add(newWeapon);
            }
        }
        else
        {
            // selectItem은 내가 가지고 있는 아이템 보다 레벨이 1 높은 새로운 객체다.
            inventory[index] = selectItem;
        }

        UpdateStatus();
    }


    protected override void UpdateStatus()
    {
        var passives = from item in inventory
                       where item is PassiveItem
                       select item as PassiveItem;

        // 실제 적용 스테이터스 계산.
        ResetIncrease();
        foreach (var p in passives)
            AddIncrease(p.status);

        // 스테이터스 계산.
        base.UpdateStatus();

        // 장비 중인 무기 스테이터스 업데이트.
        foreach (WeaponObject weapon in equipWeapons)
            weapon.UpdateWeapon(finalStatus);
    }
    protected override void Dead()
    {
        anim.SetTrigger("onDead");
        enabled = false;
    }


    private void UpdateUI()
    {
        // 현재 exp의 기준이 누적치이기 때문에 레벨 구간에 따른 비율을 계산.
        float current = exp - NeedTotalExp(level);
        float max = NeedTotalExp(level + 1) - NeedTotalExp(level);
        TopUI.Instance.UpdateExp(current, max);
        TopUI.Instance.UpdateLevel(level);
        TopUI.Instance.UpdateKillCount(killCount);
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, magnetRange);
    }
}

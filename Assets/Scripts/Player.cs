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

    List<Item> inventory;           // ���� �������� ����.
    List<WeaponObject> equipWeapons;      // ��� ������.

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

        // ���� ����.
        if(input != Vector2.zero)
            direction = input;

        // �Է°��� �̵������� ��ȯ �� ����.
        Vector3 movement = input;
        Vector3 nextPosition = transform.position + movement * speed * Time.deltaTime;
        transform.position = Background.Instance.InBoundary(nextPosition, spriteRenderer.size);

        if (input.x != 0f)
            spriteRenderer.flipX = input.x < 0f;

        // �Է� ���� ���� �ִϸ��̼� ó��.
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
        // ���� ������ �������� �����Ѵٸ� ��ü�Ѵ�. ���ٸ� ���� �����Ѵ�.
        int index = inventory.FindIndex(item => item.id == selectItem.id);
        if (index == -1)
        {
            inventory.Add(selectItem);

            // ���ο� ���⸦ �������� ��� �ν��Ͻ� ����.
            if (selectItem is WeaponItem)
            {
                WeaponObject prefab = ItemManager.Instance.GetWeaponPrefab(selectItem.id);
                WeaponObject newWeapon = Instantiate(prefab, transform);
                equipWeapons.Add(newWeapon);
            }
        }
        else
        {
            // selectItem�� ���� ������ �ִ� ������ ���� ������ 1 ���� ���ο� ��ü��.
            inventory[index] = selectItem;
        }

        UpdateStatus();
    }


    protected override void UpdateStatus()
    {
        var passives = from item in inventory
                       where item is PassiveItem
                       select item as PassiveItem;

        // ���� ���� �������ͽ� ���.
        ResetIncrease();
        foreach (var p in passives)
            AddIncrease(p.status);

        // �������ͽ� ���.
        base.UpdateStatus();

        // ��� ���� ���� �������ͽ� ������Ʈ.
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
        // ���� exp�� ������ ����ġ�̱� ������ ���� ������ ���� ������ ���.
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Unit
{
    public static Player Instance { get; private set; }
    public Vector2 direction { get; private set; }

    [SerializeField] float magnetRange;
    [SerializeField] Transform hpPivot;
    [SerializeField] HpBar hpBar;

    List<Item> inventory;                   // ���� �������� ����.
    List<WeaponObject> weaponList;          // ���� ������Ʈ.
    SpriteRenderer spriteRenderer;          // ��������Ʈ ������Ʈ.
    LayerMask expMask;                      // ����ġ ����ũ.

    private void Awake()
    {
        Instance = this;
        inventory = new List<Item>();
        weaponList = new List<WeaponObject>();
        direction = Vector2.right;
        spriteRenderer = GetComponent<SpriteRenderer>();
        expMask = 1 << LayerMask.NameToLayer("Exp");
    }
    protected new void Start()
    {
        base.Start();

        UpdateUI();
    }
    private void Update()
    {
        if (!isAlive || isPauseObject)
            return;

        // ����ġ ����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, magnetRange, expMask);
        foreach(Collider2D collider in colliders)
            collider.GetComponent<ExpObject>()?.ContactPlayer(transform, AddExp);

        // ü�¹��� ��ġ�� ���� �����Ѵ�.
        hpBar.UpdateHpBar(hpPivot, hp, maxHp);
       
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
        //transform.position = Background.Instance.InBoundary(nextPosition, spriteRenderer.size);
        transform.position = nextPosition;

        if (input.x != 0f)
            spriteRenderer.flipX = input.x < 0f;

        // �Է� ���� ���� �ִϸ��̼� ó��.
        anim.SetBool("isRun", input != Vector2.zero);
    }

    protected override void AddExp(int amount)
    {
        base.AddExp(amount);
        UpdateUI();
    }
    protected override void LevelUp()
    {
        AudioManager.Instance.PlaySe("levelup");
        GameManager.Instance.SwitchPause(true);
        Item[] randomItems = ItemManager.Instance.GetRandomItem();
        DrawUI.Instance.ShowDrawUI(randomItems, (select) =>
        {
            AddItem(randomItems[select]);
            GameManager.Instance.SwitchPause(false);
        });
    }
    private void AddItem(Item selectItem)
    {
        // ������ �������� ������ 1 ������Ų��.
        selectItem.level += 1;

        // ������ 1�̶�� ���ο� �������̱� ������ �����Ѵ�.
        if (selectItem.level == 1)
        {
            inventory.Add(selectItem);

            // ���ο� ���⸦ �������� ��� �ν��Ͻ� ����.
            if (selectItem is WeaponItem)
            {
                WeaponItem weaponItem = selectItem as WeaponItem;
                WeaponObject newWeapon = Instantiate(weaponItem.weaponPrefab, transform);
                newWeapon.Setup(this, weaponItem);
                weaponList.Add(newWeapon);
            }
        }

        UpdateAbility();
    }

    protected override void UpdateAbility()
    {
        base.UpdateAbility();

        foreach (WeaponObject weaponObject in weaponList)
            weaponObject.UpdateWaeponStatus();
    }
    protected override Ability GetIncrease()
    {
        Ability increase = new Ability();
        foreach(Item item in inventory)
        {
            if (item is PassiveItem)
                increase += (item as PassiveItem).ability;
        }
        return increase;
    }

    protected override void Dead()
    {
        anim.SetTrigger("onDead");
        enabled = false;

        hpBar.gameObject.SetActive(false);
        GameManager.Instance.OnDeadPlayer();
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

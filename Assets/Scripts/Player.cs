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

    List<Item> inventory;                   // 소지 아이템의 정보.
    List<WeaponObject> weaponList;          // 무기 오브젝트.
    SpriteRenderer spriteRenderer;          // 스프라이트 컨포넌트.
    LayerMask expMask;                      // 경험치 마스크.

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

        // 경험치 구슬
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, magnetRange, expMask);
        foreach(Collider2D collider in colliders)
            collider.GetComponent<ExpObject>()?.ContactPlayer(transform, AddExp);

        // 체력바의 위치와 값을 갱신한다.
        hpBar.UpdateHpBar(hpPivot, hp, maxHp);
       
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
        //transform.position = Background.Instance.InBoundary(nextPosition, spriteRenderer.size);
        transform.position = nextPosition;

        if (input.x != 0f)
            spriteRenderer.flipX = input.x < 0f;

        // 입력 값에 따른 애니메이션 처리.
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
        // 선택한 아이템의 레벨을 1 증가시킨다.
        selectItem.level += 1;

        // 레벨이 1이라면 새로운 아이템이기 때문에 대입한다.
        if (selectItem.level == 1)
        {
            inventory.Add(selectItem);

            // 새로운 무기를 선택했을 경우 인스턴스 생성.
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

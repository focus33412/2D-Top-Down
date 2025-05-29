using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
   public MonoBehaviour CurrentActiveWeapon { get; private set; }

   private PlayerControls playerControls;

   private bool attackButtomDown, isAttacking = false;

   protected override void Awake() 
   {
    base.Awake();
    playerControls = new PlayerControls();
   }

   private void OnEnable() 
   {
    playerControls.Enable();
   }

    private void Start() 
    {
        playerControls.Player.Attack.started += _ => StartAttacking();
        playerControls.Player.Attack.canceled += _ => StopAttacking();
   }

    private void Update() 
    {
    Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon){
        CurrentActiveWeapon = newWeapon;
    }

    public void WeaponNull(){
        CurrentActiveWeapon = null;
    }

    public void ToggleIsAttacking(bool value)
    {
      isAttacking = value;
    }

    private void StartAttacking(){
        attackButtomDown = true;
    }

    private void StopAttacking(){
        attackButtomDown = false;
    }

    private void Attack(){
        if(attackButtomDown && !isAttacking){
            isAttacking = true;
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
   

   
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sword : MonoBehaviour
{

    //public bool FacingLeft { get { return facingLeft;} set { facingLeft = value;}}


    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;

    private GameObject slashAnim;
    //private bool facingLeft = false;



    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Player.Attack.started += _ => Attack();
    }

    private void Update(){
        MouseFollowWithOffset();
    }
    
    private void Attack()
    {
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);


        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
    }

    public void DoneAttackingAnimEvent(){
        weaponCollider.gameObject.SetActive(false);
    }   

    public void SwingUpFlipAnimEvent() {
        if (slashAnim == null) return;
        
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        /*if (facingLeft) { 
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }*/
    }

    public void SwingDownFlipAnimEvent() {
        if (slashAnim == null) return;
        
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        /*if (facingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }*/
    }

    private void MouseFollowWithOffset(){

        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        if(mousePosition.x < playerScreenPoint.x){
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
            if (slashAnim != null)
            {
                slashAnim.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            //facingLeft = true;
        } else {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (slashAnim != null)
            {
                slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            //facingLeft = false;
        }
    }
    
}
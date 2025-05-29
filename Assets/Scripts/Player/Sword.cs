using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private float swordAttackCooldown = 0.5f;

    private Transform weaponCollider;
    private Animator myAnimator;
    private GameObject slashAnim;
   
    


    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Start() {
    weaponCollider = PlayerController.Instance.GetWeaponCollider();
    slashAnimSpawnPoint = GameObject.Find("SlashspawnPoint").transform;
}

    private void Update(){
        MouseFollowWithOffset();

    }

    public void Attack()
    {
        
            //isAttacking = true;
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
            StartCoroutine(AttackCooldownRoutine());

    }

    private IEnumerator AttackCooldownRoutine(){
        yield return new WaitForSeconds(swordAttackCooldown);
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }

    public void DoneAttackingAnimEvent(){
        weaponCollider.gameObject.SetActive(false);
    }   

    public void SwingUpFlipAnimEvent() {
        if (slashAnim == null) return;
        
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
    }

    public void SwingDownFlipAnimEvent() {
        if (slashAnim == null) return;
        
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void MouseFollowWithOffset(){

        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        if(mousePosition.x < playerScreenPoint.x){
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
            if (slashAnim != null)
            {
                slashAnim.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
        } else {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (slashAnim != null)
            {
                slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    
}
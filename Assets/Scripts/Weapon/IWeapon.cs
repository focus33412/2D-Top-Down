// Интерфейс для всех типов оружия
interface IWeapon {
    public void Attack();                    // Метод атаки, который должен быть реализован каждым оружием
    public WeaponInfo GetWeaponInfo();       // Получение информации об оружии
}

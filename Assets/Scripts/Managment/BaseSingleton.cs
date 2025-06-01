using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Базовый класс для глобальных синглтонов, которые должны существовать на протяжении всей игры
public class BaseSingleton : Singleton<BaseSingleton>
{
    // Этот класс используется как основа для глобальных менеджеров,
    // которые должны сохраняться между сценами и быть доступны отовсюду
}

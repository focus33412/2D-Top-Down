using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс для управления переходами между сценами
public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName { get; private set; } // Имя текущего перехода между сценами

    // Устанавливает имя перехода для последующей загрузки сцены
    public void SetTransitionName(string sceneTransitionName) {
        this.SceneTransitionName = sceneTransitionName;
    }
}


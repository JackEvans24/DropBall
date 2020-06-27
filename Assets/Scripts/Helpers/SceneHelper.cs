using UnityEngine;

public static class SceneHelper
{
    public static Scenes[] StandardScenes
    {
        get
        {
            return new Scenes[]
            {
                Scenes.BoardOne,
                Scenes.BoardTwo,
                Scenes.BoardThree
            };
        }
    }

    public static Scenes[] SuddenDeathScenes {
        get
        {
            return new Scenes[]
            {
                Scenes.BoardOne_SuddenDeath,
                Scenes.BoardTwo_SuddenDeath,
                Scenes.BoardThree_SuddenDeath
            };
        }
    }

    public static Scenes GetStandardScene() => StandardScenes[Random.Range(0, StandardScenes.Length)];
    public static Scenes GetSuddenDeathScene() => SuddenDeathScenes[Random.Range(0, SuddenDeathScenes.Length)];
}

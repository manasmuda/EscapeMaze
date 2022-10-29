using System.Collections.Generic;
using System.Linq;

public static class ListUtils
{
    public static List<T> RandomizeList<T>(this List<T> list) {
        List<T> random_list = new List<T>(list);
        System.Random random = new System.Random();
        random_list.OrderBy(x => random.Next());
        return random_list;
    }
        
    public static T GetRandomElement<T>(this List<T> list) {
        if(list != null) {
            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        } else {
            return default(T);
        }
    }
}

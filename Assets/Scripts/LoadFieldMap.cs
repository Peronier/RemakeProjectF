using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public class LoadFieldMap : MonoBehaviour
{
    public string mapName;
    public Field field;

    void Start()
    {
        field.Reset();
        Array2D mapDate = ReadMapFile(mapName);
        if(mapDate != null)
        {
            field.Create(mapDate);
        }
    }

    /**
     * TMXファイルからマップデータを取得する
     */
    private Array2D ReadMapFile(string path)
    {
        try
        {
            XDocument xml = XDocument.Load(path);
            XElement map = xml.Element("map");
            Array2D data = null;
            int w = 0, h = 0;
            foreach(var layer in map.Elements("layer"))
            {
                switch (layer.Attribute("id").Value)
                {
                    case "1":
                        string[] sData = (layer.Element("data").Value).Split(',');
                        w = int.Parse(layer.Attribute("width").Value);
                        h = int.Parse(layer.Attribute("height").Value);
                        data = new Array2D(w, h);
                        for (int z = 0; z < h; z++)
                        {
                            for (int x = 0; x < w; x++)
                            {
                                data.Set(x, z, int.Parse(sData[ToMirrorX(x, w) + z * w]) - 1);
                            }
                        }
                        break;
                }
            }
            return data;
        }
        catch(System.Exception i_exception)
        {
            Debug.LogErrorFormat("{0}", i_exception);
        }
        return null;
    }

    /**
     * z軸に対して反対の値を返す
     */
    private int ToMirrorX(int xGrid, int mapWidth)
    {
        return mapWidth - xGrid - 1;
    }
}

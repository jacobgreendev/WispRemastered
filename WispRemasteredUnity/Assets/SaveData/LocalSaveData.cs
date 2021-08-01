[System.Serializable]
public class LocalSaveData
{
    private static LocalSaveData _save;
    public static LocalSaveData save
    {
        get
        {
            if (_save == null)
                _save = new LocalSaveData();

            return _save;
        }

        set
        {
            _save = value;
        }
    }

    public int maxScore;
}

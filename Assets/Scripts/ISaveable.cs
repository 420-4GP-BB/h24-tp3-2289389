using LitJson;


public interface ISaveable
{
    string SaveID { get; }    
    JsonData SavedData { get; }
    public void LoadFromData(JsonData data);
}

public class BaseManager
{
    protected GameMng Mng;
    public BaseManager(GameMng mng)
    {
        this.Mng = mng;
    }
    public virtual void OnInit()
    { }
    public virtual void OnUpdate()
    { }
    public virtual void OnFixedUpdate()
    { }
}